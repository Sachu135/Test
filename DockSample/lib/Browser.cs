using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockSample.lib
{
	#region Browser Interface
	/// <summary>
	/// An interface defining Browser classes (an Explorer-like tree view of a database).
	/// </summary>
	public interface IBrowser
	{
		/// <summary>
		/// Returns the active Database Client object (this should be set in construction)
		/// </summary>
		DbClient DbClient { get; }

		/// <summary>
		/// Returns an array of TreeNodes representing the object hierarchy for the "Explorer" view.
		/// This can return either the entire hierarchy, or for efficiency, just the higher level(s).
		/// </summary>
		TreeNode[] GetObjectHierarchy();

		/// <summary>
		/// Returns an array of TreeNodes representing the object hierarchy below a given node.
		/// This should return null if there is no hierarchy below the given node, or if the hierarchy
		/// is already present.  This method is called whenever the user expands a node.
		/// </summary>
		TreeNode[] GetSubObjectHierarchy(TreeNode node);

		/// <summary>
		/// Returns text suitable for dropping into a query window, for a given node.
		/// </summary>
		string GetDragText(TreeNode node);

		/// <summary>
		/// Returns a list of actions applicable to a node (suitable for a context menu).
		/// Returns null if no actions are applicable.
		/// </summary>
		StringCollection GetActionList(TreeNode node);

		/// <summary>
		/// Returns text suitable for pasting into a query window, given a particular node and action.
		/// GetActionList() should be called first to obtain a list of applicable actions.
		/// </summary>
		/// <param name="actionIndex">One of the action text strings returned by GetActionList()</param>
		string GetActionText(TreeNode node, string action);

		/// <summary>
		/// Returns a list of available databases
		/// </summary>
		string[] GetDatabases();

		/// <summary>
		/// Creates and returns a new browser object, using the supplied database client object.
		/// </summary>
		IBrowser Clone(DbClient newDbClient);
	}
	#endregion

	#region SQL Server Browser
	/// <summary>
	/// An implementation of IBrowser for MS SQL Server.
	/// </summary>
	public class SqlBrowser : IBrowser
	{
		class SqlNode : TreeNode
		{
			internal string type = "";
			internal string name, owner, safeName, dragText;
			public SqlNode(string text) : base(text) { }
		}

		const int timeout = 8;
		DbClient dbClient;

		public SqlBrowser(DbClient dbClient)
		{
			this.dbClient = dbClient;
		}

		public DbClient DbClient
		{
			get { return dbClient; }
		}

		public TreeNode[] GetObjectHierarchy()
		{
			TreeNode[] top = new TreeNode[]
			{
				new TreeNode ("User Tables"),
				new TreeNode ("System Tables"),
				new TreeNode ("Views"),
				new TreeNode ("User Stored Procs"),
				new TreeNode ("MS Stored Procs"),
				new TreeNode ("Functions")
			};

			string version = dbClient.ExecuteScalar("select SERVERPROPERTY('productversion')", timeout) as string;
			string schemaFunc = version != null && (version[0] == '9' || version[0] == '1') ? "schema_name" : "user_name";
			//testdb..
			string query = @"select 
				type,
				ObjectProperty (id, N'IsMSShipped') shipped, 
				object_name(id) object, 
				" + schemaFunc + @"(uid) owner 
			from sysobjects 
			where type in (N'U', N'S', N'V', N'P', N'FN') 
			order by object, owner";
			DataSet ds = dbClient.Execute(query, timeout);
			if (ds == null || ds.Tables.Count == 0) return null;

			foreach (DataRow row in ds.Tables[0].Rows)
			{
				string type = row["type"].ToString().Substring(0, 2).Trim();

				int position;
				if (type == "U") position = 0;                                      // user table
				else if (type == "S") position = 1;                             // system table
				else if (type == "V") position = 2;                             // view
				else if (type == "FN") position = 5;                                // function
				else if ((int)row["shipped"] == 0) position = 3;                // user stored proc
				else position = 4;                                                      // MS stored proc

				string prefix = row["owner"].ToString() == "dbo" ? "" : row["owner"].ToString() + ".";
				SqlNode node = new SqlNode(prefix + row["object"].ToString());
				node.type = type;
				node.name = row["object"].ToString();
				node.owner = row["owner"].ToString();

				// If the object name contains a space, wrap the "safe name" in square brackets.
				if (node.owner.IndexOf(' ') >= 0 || node.name.IndexOf(' ') >= 0)
				{
					node.safeName = "[" + node.name + "]";
					node.dragText = "[" + node.owner + "].[" + node.name + "]";
				}
				else
				{
					node.safeName = node.name;
					node.dragText = node.owner + "." + node.name;
				}
				if (node.owner != "" && node.owner.ToLower() != "dbo")
					node.safeName = node.dragText;

				top[position].Nodes.Add(node);

				// Add a dummy sub-node to user tables and views so they'll have a clickable expand sign
				// allowing us to have GetSubObjectHierarchy called so the user can view the columns
				if (type == "U" || type == "V") node.Nodes.Add(new TreeNode());
			}
			return top;
		}

		public TreeNode[] GetSubObjectHierarchy(TreeNode node)
		{
			// Show the column breakdown for the selected table
			if (node is SqlNode)
			{
				SqlNode sn = (SqlNode)node;
				if (sn.type == "U" || sn.type == "V")                   // break down columns for user tables and views
				{
					DataSet ds = dbClient.Execute("select COLUMN_NAME name, DATA_TYPE type, CHARACTER_MAXIMUM_LENGTH clength, NUMERIC_PRECISION nprecision, NUMERIC_SCALE nscale, IS_NULLABLE nullable  from INFORMATION_SCHEMA.COLUMNS where TABLE_CATALOG = db_name() and TABLE_SCHEMA = '"
						+ sn.owner + "' and TABLE_NAME = '" + sn.name + "' order by ORDINAL_POSITION", timeout);
					if (ds == null || ds.Tables.Count == 0) return null;

					TreeNode[] tn = new SqlNode[ds.Tables[0].Rows.Count];
					int count = 0;

					foreach (DataRow row in ds.Tables[0].Rows)
					{
						string length;
						if (row["clength"].ToString() != "")
							length = "(" + row["clength"].ToString() + ")";
						else if (row["nprecision"].ToString() != "")
							length = "(" + row["nprecision"].ToString() + "," + row["nscale"].ToString() + ")";
						else length = "";

						string nullable = row["nullable"].ToString().StartsWith("Y") ? "null" : "not null";

						SqlNode column = new SqlNode(row["name"].ToString() + " ("
							+ row["type"].ToString() + length + ", " + nullable + ")");
						column.type = "CO";         // column
						column.dragText = row["name"].ToString();
						if (column.dragText.IndexOf(' ') >= 0)
							column.dragText = "[" + column.dragText + "]";
						column.safeName = column.dragText;
						tn[count++] = column;
					}
					return tn;
				}
			}
			return null;
		}

		public string GetDragText(TreeNode node)
		{
			if (node is SqlNode)
				return ((SqlNode)node).dragText;
			else
				return "";
		}

		public StringCollection GetActionList(TreeNode node)
		{
			if (!(node is SqlNode)) return null;

			SqlNode sn = (SqlNode)node;
			StringCollection output = new StringCollection();

			if (sn.type == "U" || sn.type == "S" || sn.type == "V")
			{
				output.Add("select * from " + sn.safeName);
				output.Add("sp_help " + sn.safeName);
				if (sn.type != "V")
				{
					output.Add("sp_helpindex " + sn.safeName);
					output.Add("sp_helpconstraint " + sn.safeName);
					output.Add("sp_helptrigger " + sn.safeName);
				}
				output.Add("(insert all fields)");
				output.Add("(insert all fields, table prefixed)");
			}

			if (sn.type == "V" || sn.type == "P" || sn.type == "FN")
				output.Add("View / Modify " + sn.name);

			if (sn.type == "CO" && ((SqlNode)sn.Parent).type == "U")
				output.Add("Alter column...");

			return output.Count == 0 ? null : output;
		}

		public string GetActionText(TreeNode node, string action)
		{
			if (!(node is SqlNode)) return null;

			SqlNode sn = (SqlNode)node;

			if (action.StartsWith("select * from ") || action.StartsWith("sp_"))
				return action;

			if (action.StartsWith("(insert all fields"))
			{
				StringBuilder sb = new StringBuilder();
				// If the table-prefixed option has been selected, add the table name to all the fields
				string prefix = action == "(insert all fields)" ? "" : sn.safeName + ".";
				int chars = 0;
				foreach (TreeNode subNode in GetSubObjectHierarchy(node))
				{
					if (chars > 50)
					{
						chars = 0;
						sb.Append("\r\n");
					}
					string s = (sb.Length == 0 ? "" : ", ") + prefix + ((SqlNode)subNode).dragText;
					chars += s.Length;
					sb.Append(s);
				}
				return sb.Length == 0 ? null : sb.ToString();
			}

			if (action.StartsWith("View / Modify "))
			{
				DataSet ds = dbClient.Execute("sp_helptext " + sn.safeName, timeout);
				if (ds == null || ds.Tables.Count == 0) return null;

				StringBuilder sb = new StringBuilder();
				bool altered = false;
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					string line = row[0].ToString();
					if (!altered && line.Trim().ToUpper().StartsWith("CREATE"))
					{
						sb.Append("ALTER" + line.Trim().Substring(6, line.Trim().Length - 6) + "\r\n");
						altered = true;
					}
					else
						sb.Append(line);
				}
				return sb.ToString().Trim();
			}

			if (action == "Alter column...")
				return "alter table " + ((SqlNode)sn.Parent).dragText + " alter column " + sn.safeName + " ";

			return null;
		}

		public string[] GetDatabases()
		{
			// cool, but only supported in SQL Server 2000+
			DataSet ds = dbClient.Execute("dbo.sp_MShasdbaccess", timeout);
			// works in SQL Server 7...
			if (ds == null || ds.Tables.Count == 0)
				ds = dbClient.Execute("select name from master.dbo.sysdatabases order by name", timeout);
			if (ds == null || ds.Tables.Count == 0) return null;
			string[] sa = new string[ds.Tables[0].Rows.Count];
			int count = 0;
			foreach (DataRow row in ds.Tables[0].Rows)
				sa[count++] = row[0].ToString().Trim();
			return sa;
		}

		public IBrowser Clone(DbClient newDbClient)
		{
			SqlBrowser sb = new SqlBrowser(newDbClient);
			return sb;
		}
	}
	#endregion

	#region Simple Oracle Browser
	/// <summary>
	/// A simple implementation of IBrowser for Oracle.  No support for SPs, packages, etc
	/// </summary>
	public class OracleBrowser : IBrowser
	{
		class OracleNode : TreeNode
		{
			internal string type = "";
			internal string dragText = "";
			public OracleNode(string text) : base(text) { }
		}

		const int timeout = 8;
		DbClient dbClient;

		public OracleBrowser(DbClient dbClient)
		{
			this.dbClient = dbClient;
		}

		public DbClient DbClient
		{
			get { return dbClient; }
		}

		public TreeNode[] GetObjectHierarchy()
		{
			TreeNode[] top = new TreeNode[]
			{
				new TreeNode ("User Tables"),
				new TreeNode ("User Views"),
			};

			DataSet ds = dbClient.Execute("select TABLE_NAME from USER_TABLES", timeout);
			if (ds == null || ds.Tables.Count == 0) return null;

			foreach (DataRow row in ds.Tables[0].Rows)
			{
				OracleNode node = new OracleNode(row[0].ToString());
				node.type = "T";
				node.dragText = node.Text;
				top[0].Nodes.Add(node);
				// Add a dummy sub-node to user tables and views so they'll have a clickable expand sign
				// allowing us to have GetSubObjectHierarchy called so the user can view the columns
				node.Nodes.Add(new TreeNode());
			}

			ds = dbClient.Execute("select VIEW_NAME from USER_VIEWS", timeout);
			if (ds == null || ds.Tables.Count == 0) return top;

			foreach (DataRow row in ds.Tables[0].Rows)
			{
				OracleNode node = new OracleNode(row[0].ToString());
				node.type = "V";
				node.dragText = node.Text;
				top[1].Nodes.Add(node);
				// Add a dummy sub-node to user tables and views so they'll have a clickable expand sign
				// allowing us to have GetSubObjectHierarchy called so the user can view the columns
				node.Nodes.Add(new TreeNode());
			}

			return top;
		}

		public TreeNode[] GetSubObjectHierarchy(TreeNode node)
		{
			// Show the column breakdown for the selected table

			if (node is OracleNode)
			{
				OracleNode on = (OracleNode)node;
				if (on.type == "T" || on.type == "V")
				{
					DataSet ds = dbClient.Execute("select COLUMN_NAME name, DATA_TYPE type, DATA_LENGTH clength, DATA_PRECISION nprecision, DATA_SCALE nscale, NULLABLE nullable from USER_TAB_COLUMNS where TABLE_NAME = '"
						+ on.Text + "'", timeout);
					if (ds == null || ds.Tables.Count == 0) return null;

					TreeNode[] tn = new OracleNode[ds.Tables[0].Rows.Count];
					int count = 0;

					foreach (DataRow row in ds.Tables[0].Rows)
					{
						string length;
						if (row["clength"].ToString() != "")
							length = "(" + row["clength"].ToString() + ")";
						else if (row["nprecision"].ToString() != "")
							length = "(" + row["nprecision"].ToString() + "," + row["nscale"].ToString() + ")";
						else length = "";

						string nullable = row["nullable"].ToString().StartsWith("Y") ? "null" : "not null";

						OracleNode column = new OracleNode(row["name"].ToString() + " ("
							+ row["type"].ToString() + length + ", " + nullable + ")");

						column.dragText = row["name"].ToString();

						tn[count++] = column;
					}
					return tn;
				}
			}
			return null;
		}

		public string GetDragText(TreeNode node)
		{
			if (node is OracleNode)
				return ((OracleNode)node).dragText;
			else
				return null;
		}

		public StringCollection GetActionList(TreeNode node)
		{
			if (!(node is OracleNode)) return null;

			OracleNode on = (OracleNode)node;
			StringCollection output = new StringCollection();

			if (on.type == "T" || on.type == "V")
			{
				output.Add("select * from " + on.dragText);
			}

			return output.Count == 0 ? null : output;
		}

		public string GetActionText(TreeNode node, string action)
		{
			if (!(node is OracleNode)) return null;
			OracleNode on = (OracleNode)node;
			if (action.StartsWith("select * from "))
				return action;
			else
				return null;
		}

		public string[] GetDatabases()
		{
			return new String[] { dbClient.Database };
		}

		public IBrowser Clone(DbClient newDbClient)
		{
			OracleBrowser ob = new OracleBrowser(newDbClient);
			return ob;
		}
	}
	#endregion

	#region Postgres Server Browser
	/// <summary>
	/// An implementation of IBrowser for MS SQL Server.
	/// </summary>
	public class PostgresBrowser : IBrowser
	{
		class PostgresNode : TreeNode
		{
			internal string type = "";
			internal int noOfArgs = 0;
			internal string name, owner, safeName, dragText;
			public PostgresNode(string text) : base(text) { }
		}

		const int timeout = 8;
		DbClient dbClient;

		public PostgresBrowser(DbClient dbClient)
		{
			this.dbClient = dbClient;
		}

		public DbClient DbClient
		{
			get { return dbClient; }
		}

		public TreeNode[] GetObjectHierarchy()
		{
			TreeNode[] top = new TreeNode[]
			{
				new TreeNode ("User Tables"),
				new TreeNode ("Views"),
				//new TreeNode ("User Stored Procs"),
				new TreeNode ("Functions")
			};
			DataSet ds = dbClient.Execute(@"select nsp.nspname as object_schema,
				   cls.relname as object_name, 
				   rol.rolname as owner, 
				   case cls.relkind
					 when 'r' then 'TABLE'
					 when 'm' then 'MATERIALIZED_VIEW'
					 when 'i' then 'INDEX'
					 when 'S' then 'SEQUENCE'
					 when 'v' then 'VIEW'
					 when 'c' then 'TYPE'
					 else cls.relkind::text
				   end as object_type,
					'' as function_arguments
			from pg_class cls
			  join pg_roles rol on rol.oid = cls.relowner
			  join pg_namespace nsp on nsp.oid = cls.relnamespace
			where nsp.nspname not in ('information_schema', 'pg_catalog')
			  and nsp.nspname not like 'pg_toast%'
			  AND cls.relkind IN ('r', 'v')
			  --and rol.rolname = current_user  
			union
			 select n.nspname as object_schema,
				   p.proname as object_name,
				   '' AS OWNER,
				   'FUNCTION' AS object_type,
					pg_get_function_arguments(p.oid) as function_arguments
			from pg_proc p
			left join pg_namespace n on p.pronamespace = n.oid
			left join pg_language l on p.prolang = l.oid
			left join pg_type t on t.oid = p.prorettype 
			where n.nspname not in ('pg_catalog', 'information_schema')
			order by object_schema, object_name;", timeout);

			if (ds != null && ds.Tables.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					string type = row["object_type"].ToString();

					int position = 0;
					if (type == "TABLE") position = 0;                                      // user table
					else if (type == "VIEW") position = 1;                             // system table
					else if (type == "FUNCTION") position = 2;

					string prefix = row["object_schema"].ToString() == "public" ? "" : row["object_schema"].ToString() + ".";
					PostgresNode node = new PostgresNode(prefix + row["object_name"].ToString());
					node.type = type;
					node.name = row["object_name"].ToString();
					node.owner = row["object_schema"].ToString();
					node.noOfArgs = row["function_arguments"].ToString().Length > 0 ? row["function_arguments"].ToString().Split(',').Length : 0;
					// If the object name contains a space, wrap the "safe name" in square brackets.
					//if (node.owner.IndexOf(' ') >= 0 || node.name.IndexOf(' ') >= 0)
					//{
					//	node.safeName = "\"" + node.name + "\"";
					//	node.dragText = "\"" + node.owner + "\".\"" + node.name + "\"";
					//}
					//else
					//{
					//	node.safeName = node.name;
					//	node.dragText = node.owner + "." + node.name;
					//}
					node.safeName = "\"" + node.owner + "\".\"" + node.name + "\"";
					node.dragText = "\"" + node.owner + "\".\"" + node.name + "\"";
					//if (node.owner != "" && node.owner.ToLower() != "dbo")
					//	node.safeName = node.dragText;

					top[position].Nodes.Add(node);

					// Add a dummy sub-node to user tables and views so they'll have a clickable expand sign
					// allowing us to have GetSubObjectHierarchy called so the user can view the columns
					if (type == "TABLE" || type == "VIEW")
						node.Nodes.Add(new TreeNode());
				}
			}
			if (ds == null || ds.Tables.Count == 0) return null;

			return top;
		}

		public TreeNode[] GetSubObjectHierarchy(TreeNode node)
		{
			// Show the column breakdown for the selected table
			if (node is PostgresNode)
			{
				PostgresNode sn = (PostgresNode)node;
				if (sn.type == "TABLE" || sn.type == "VIEW")                   // break down columns for user tables and views
				{
					var query = @"SELECT column_name AS NAME, data_type AS TYPE, is_nullable AS nullable FROM information_schema.columns WHERE table_schema = '" + sn.owner + "' AND table_name = '" + sn.name + "';";
					DataSet ds = dbClient.Execute(query, timeout);
					if (ds == null || ds.Tables.Count == 0) return null;

					TreeNode[] tn = new PostgresNode[ds.Tables[0].Rows.Count];
					int count = 0;

					foreach (DataRow row in ds.Tables[0].Rows)
					{

						string nullable = row["nullable"].ToString().StartsWith("Y") ? "null" : "not null";

						PostgresNode column = new PostgresNode(row["name"].ToString() + " ("
							+ row["type"].ToString() + ", " + nullable + ")");
						column.type = "CO";         // column
						column.dragText = row["name"].ToString();

						column.dragText = "\"" + column.dragText + "\"";
						//if (column.dragText.IndexOf(' ') >= 0)
						//	column.dragText = "[" + column.dragText + "]";
						column.safeName = column.dragText;
						tn[count++] = column;
					}
					return tn;
				}
			}
			return null;
		}

		public string GetDragText(TreeNode node)
		{
			if (node is PostgresNode)
				return ((PostgresNode)node).dragText;
			else
				return "";
		}

		private string GetArgsText(int noOfArgs)
		{
			string st = "(";
			for (int i = 1; i <= noOfArgs; i++)
			{
				st += (i == noOfArgs ? "?" : "?, ");
			}
			st += ")";
			return st;
		}
		public StringCollection GetActionList(TreeNode node)
		{
			if (!(node is PostgresNode)) return null;

			PostgresNode sn = (PostgresNode)node;
			StringCollection output = new StringCollection();

			if (sn.type == "TABLE" || sn.type == "VIEW" || sn.type == "FUNCTION")
			{
				output.Add("select * from " + sn.safeName + (sn.type == "FUNCTION" ? GetArgsText(sn.noOfArgs) : "") + ";");
				//output.Add("sp_help " + sn.safeName);
				//if (sn.type != "V")
				//{
				//	output.Add("sp_helpindex " + sn.safeName);
				//	output.Add("sp_helpconstraint " + sn.safeName);
				//	output.Add("sp_helptrigger " + sn.safeName);
				//}
				output.Add("(insert all fields)");
				output.Add("(insert all fields, table prefixed)");
			}

			if (sn.type == "VIEW" || sn.type == "FUNCTION") //|| sn.type == "P" || sn.type == "FN"
				output.Add("View/Modify " + sn.name);

			//if (sn.type == "CO" && ((SqlNode)sn.Parent).type == "U")
			//	output.Add("Alter column...");

			return output.Count == 0 ? null : output;
		}

		public string GetActionText(TreeNode node, string action)
		{
			if (!(node is PostgresNode)) return null;

			PostgresNode sn = (PostgresNode)node;

			if (action.StartsWith("select * from ") || action.StartsWith("sp_"))
				return action;

			if (action.StartsWith("(insert all fields"))
			{
				StringBuilder sb = new StringBuilder();
				// If the table-prefixed option has been selected, add the table name to all the fields
				string prefix = action == "(insert all fields)" ? "" : sn.safeName + ".";
				int chars = 0;
				foreach (TreeNode subNode in GetSubObjectHierarchy(node))
				{
					if (chars > 50)
					{
						chars = 0;
						sb.Append("\r\n");
					}
					string s = (sb.Length == 0 ? "" : ", ") + prefix + ((PostgresNode)subNode).dragText;
					chars += s.Length;
					sb.Append(s);
				}
				return sb.Length == 0 ? null : sb.ToString();
			}

			if (action.StartsWith("View/Modify "))
			{
				if (sn.type == "FUNCTION")
				{
					var query = @"select case when l.lanname = 'internal' then p.prosrc
								else pg_get_functiondef(p.oid)
								end as definition
					from pg_proc p
					left join pg_namespace n on p.pronamespace = n.oid
					left join pg_language l on p.prolang = l.oid
					where n.nspname not in ('pg_catalog', 'information_schema')
					AND p.proname = '" + sn.name + "';";
					DataSet ds = dbClient.Execute(query, timeout);
					if (ds == null || ds.Tables.Count == 0) return null;

					StringBuilder sb = new StringBuilder();
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						string line = row[0].ToString();
						sb.Append(line);
					}
					return sb.ToString().Trim();
				}
				else
				{
					DataSet ds = dbClient.Execute(("select definition from pg_views where viewname = '" + sn.name + "';"), timeout);
					if (ds == null || ds.Tables.Count == 0) return null;

					StringBuilder sb = new StringBuilder();
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						string line = row[0].ToString();
						sb.Append("CREATE OR REPLACE VIEW " + "\"" + sn.name + "\"" + "\r\n");
						sb.Append("AS" + "\r\n");
						sb.Append(line);
						//if (!altered && line.Trim().ToUpper().StartsWith("CREATE"))
						//{
						//	sb.Append("ALTER" + line.Trim().Substring(6, line.Trim().Length - 6) + "\r\n");
						//	altered = true;
						//}
						//else
						//	sb.Append(line);
					}
					return sb.ToString().Trim();
				}
			}

			if (action == "Alter column...")
				return "alter table " + ((PostgresNode)sn.Parent).dragText + " alter column " + sn.safeName + " ";

			return null;
		}

		public string[] GetDatabases()
		{

			DataSet ds = dbClient.Execute("SELECT datname AS name FROM pg_database WHERE datistemplate = false order by name", timeout);
			if (ds == null || ds.Tables.Count == 0) return null;
			string[] sa = new string[ds.Tables[0].Rows.Count];
			int count = 0;
			foreach (DataRow row in ds.Tables[0].Rows)
				sa[count++] = row[0].ToString().Trim();
			return sa;
		}

		public IBrowser Clone(DbClient newDbClient)
		{
			SqlBrowser sb = new SqlBrowser(newDbClient);
			return sb;
		}
	}
	#endregion

	#region MySql Server Browser
	/// <summary>
	/// An implementation of IBrowser for MS SQL Server.
	/// </summary>
	public class MySqlBrowser : IBrowser
	{
		class MySqlNode : TreeNode
		{
			internal string type = "";
			internal int noOfArgs = 0;
			internal string name, owner, safeName, dragText; //
			public MySqlNode(string text) : base(text) { }
		}

		const int timeout = 8;
		DbClient dbClient;

		public MySqlBrowser(DbClient dbClient)
		{
			this.dbClient = dbClient;
		}

		public DbClient DbClient
		{
			get { return dbClient; }
		}

		public TreeNode[] GetObjectHierarchy()
		{
			TreeNode[] top = new TreeNode[]
			{
				new TreeNode ("User Tables"),
				new TreeNode ("Views"),
				new TreeNode ("Procedures"),
				new TreeNode ("Functions"),
				//new TreeNode ("User Stored Procs"),
				
			};
			try
			{
				
				DataSet dsTables = dbClient.Execute(string.Format("SELECT table_name, table_type FROM information_schema.tables WHERE  table_schema = '{0}';", DbClient.Database), timeout);
				DataSet dsPro = dbClient.Execute(string.Format("SHOW PROCEDURE STATUS WHERE Db = '{0}';", dbClient.Database), timeout);
				DataSet dsFunc = dbClient.Execute(string.Format("SHOW FUNCTION STATUS WHERE Db = '{0}';", dbClient.Database), timeout);

				var isSystable = dbClient.Database == "information_schema";
				if (dsTables != null && dsTables.Tables.Count > 0)
				{
					foreach (DataRow row in dsTables.Tables[0].Rows)
					{
						MySqlNode node = new MySqlNode(row[0].ToString());

						node.name = row[0].ToString();
						node.owner = dbClient.Database;

						node.safeName = "`" + dbClient.Database + "`.`" + node.name + "`";
						node.dragText = "`" + dbClient.Database + "`.`" + node.name + "`";

						if (row[1].ToString().Equals("BASE TABLE") || isSystable)
						{
							node.type = "TABLE";
							top[0].Nodes.Add(node);
							node.Nodes.Add(new TreeNode());
						}
						else if (row[1].ToString().Equals("VIEW"))
						{
							node.type = "VIEW";
							top[1].Nodes.Add(node);
							node.Nodes.Add(new TreeNode());
						}

					}
				}

				if (dsPro != null && dsPro.Tables.Count > 0)
				{
					foreach (DataRow row in dsPro.Tables[0].Rows)
					{
						if (row["Db"].ToString() != "sys" || isSystable)
						{
							MySqlNode node = new MySqlNode(row["Name"].ToString());
							node.type = "PROC";
							node.name = row["Name"].ToString();
							node.owner = dbClient.Database;

							node.safeName = "`" + dbClient.Database + "`.`" + node.name + "`";
							node.dragText = "`" + dbClient.Database + "`.`" + node.name + "`";

							top[2].Nodes.Add(node);
						}

					}
				}

				if (dsFunc != null && dsFunc.Tables.Count > 0)
				{
					foreach (DataRow row in dsFunc.Tables[0].Rows)
					{
						if (row["Db"].ToString() != "sys" || isSystable)
						{
							MySqlNode node = new MySqlNode(row["Name"].ToString());
							node.type = "FUNC";
							node.name = row["Name"].ToString();
							node.owner = dbClient.Database;

							node.safeName = "`" + dbClient.Database + "`.`" + node.name + "`";
							node.dragText = "`" + dbClient.Database + "`.`" + node.name + "`";

							top[3].Nodes.Add(node);
						}

					}
				}

				if (top[0].Nodes.Count == 0 && top[1].Nodes.Count == 0 && top[2].Nodes.Count == 0 && top[3].Nodes.Count == 0) return null;

				return top;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public TreeNode[] GetSubObjectHierarchy(TreeNode node)
		{
			// Show the column breakdown for the selected table
			if (node is MySqlNode)
			{
				MySqlNode sn = (MySqlNode)node;
				if (sn.type == "TABLE" || sn.type == "VIEW") // break down columns for user tables and views
				{
					var query = @"SELECT column_name AS NAME, data_type AS TYPE, is_nullable AS nullable FROM information_schema.columns WHERE table_schema = '" + sn.owner + "' AND table_name = '" + sn.name + "';";
					DataSet ds = dbClient.Execute(query, timeout);
					if (ds == null || ds.Tables.Count == 0) return null;

					TreeNode[] tn = new MySqlNode[ds.Tables[0].Rows.Count];
					int count = 0;

					foreach (DataRow row in ds.Tables[0].Rows)
					{
						string nullable = row["nullable"].ToString().StartsWith("Y") ? "null" : "not null";

						MySqlNode column = new MySqlNode(row["name"].ToString() + " ("
							+ row["type"].ToString() + ", " + nullable + ")");
						column.type = "CO";         // column
						column.dragText = row["name"].ToString();

						column.dragText = "`" + column.dragText + "`";
						//if (column.dragText.IndexOf(' ') >= 0)
						//	column.dragText = "[" + column.dragText + "]";
						column.safeName = column.dragText;
						tn[count++] = column;
					}
					return tn;
				}
			}
			return null;
		}

		public string GetDragText(TreeNode node)
		{
			if (node is MySqlNode)
				return ((MySqlNode)node).dragText;
			else
				return "";
		}

		private string GetArgsText(int noOfArgs)
		{
			string st = "(";
			for (int i = 1; i <= noOfArgs; i++)
			{
				st += (i == noOfArgs ? "?" : "?, ");
			}
			st += ")";
			return st;
		}
		public StringCollection GetActionList(TreeNode node)
		{
			if (!(node is MySqlNode)) return null;

			MySqlNode sn = (MySqlNode)node;
			StringCollection output = new StringCollection();

			if (sn.type == "TABLE" || sn.type == "VIEW")
			{
				output.Add("select * from " + sn.safeName + (sn.type == "FUNCTION" ? GetArgsText(sn.noOfArgs) : "") + ";");
				if (sn.type == "TABLE")
				{
					output.Add("(insert all fields)");
					output.Add("(insert all fields, table prefixed)");
				}
				
			}
			if (sn.type == "PROC" || sn.type == "FUNC")
			{
				output.Add("Execute " + sn.safeName);
			}
			if (sn.type == "VIEW" || sn.type == "FUNC" || sn.type == "PROC") //|| sn.type == "P" || sn.type == "FN"
				output.Add("View/Modify " + sn.name);

			//if (sn.type == "CO" && ((SqlNode)sn.Parent).type == "U")
			//	output.Add("Alter column...");

			return output.Count == 0 ? null : output;
		}

		public string GetActionText(TreeNode node, string action)
		{
			if (!(node is MySqlNode)) return null;

			MySqlNode sn = (MySqlNode)node;

			if (action.StartsWith("select * from "))
				return action;

			if (action.StartsWith("(insert all fields"))
			{
				StringBuilder sb = new StringBuilder();
				// If the table-prefixed option has been selected, add the table name to all the fields
				string prefix = action == "(insert all fields)" ? "" : sn.safeName + ".";
				int chars = 0;
				foreach (TreeNode subNode in GetSubObjectHierarchy(node))
				{
					if (chars > 50)
					{
						chars = 0;
						sb.Append("\r\n");
					}
					string s = (sb.Length == 0 ? "" : ", ") + prefix + ((MySqlNode)subNode).dragText;
					chars += s.Length;
					sb.Append(s);
				}
				return sb.Length == 0 ? null : sb.ToString();
			}

			if (action.StartsWith("View/Modify "))
			{
				var query = string.Empty;
				var rowNo = 2;
				if (sn.type == "FUNC")
				{
					query = "SHOW CREATE FUNCTION " + sn.name;
				} 
				else if (sn.type == "VIEW")
				{
					query = "SHOW CREATE VIEW " + sn.name;
					rowNo = 1;
				}
				else
				{
					query = "SHOW CREATE PROCEDURE " + sn.name;
				}

				DataSet ds = dbClient.Execute(query, timeout);
				if (ds == null || ds.Tables.Count == 0) return null;

				StringBuilder sb = new StringBuilder();
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					string line = row[rowNo].ToString();
					sb.Append(line);
				}
				return sb.ToString().Trim();
			}

			if (action.StartsWith("Execute "))
			{
				var query = string.Empty;
				if (sn.type == "FUNC")
				{
					query = "SELECT " + sn.safeName + "(...);";
				}
				else if (sn.type == "PROC")
				{
					query = "CALL " + sn.safeName + "(...);";
				}
				return query;
			}

			if (action == "Alter column...")
				return "alter table " + ((MySqlNode)sn.Parent).dragText + " alter column " + sn.safeName + " ";

			return null;
		}

		public string[] GetDatabases()
		{

			DataSet ds = dbClient.Execute("SHOW DATABASES;", timeout);
			if (ds == null || ds.Tables.Count == 0) return null;
			string[] sa = new string[ds.Tables[0].Rows.Count];
			int count = 0;
			foreach (DataRow row in ds.Tables[0].Rows)
				sa[count++] = row[0].ToString().Trim();
			return sa;
		}

		public IBrowser Clone(DbClient newDbClient)
		{
			SqlBrowser sb = new SqlBrowser(newDbClient);
			return sb;
		}
	}
	#endregion
}
