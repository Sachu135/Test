using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DockSample.lib
{
	public class FileUtil
	{
		static string error = "";

		/// <summary> A description of the last error encountered </summary>
		public static string Error
		{
			get { return error; }
		}

		/// <summary> Write a string to a file, returning true if successful </summary>
		/// <param name="fileName">Qualified filename</param>
		/// <param name="data">String data to write</param>
		public static bool WriteToFile(string fileName, string data)
		{
			bool success = true;
			try
			{
				System.IO.StreamWriter w = System.IO.File.CreateText(fileName);
				try
				{
					w.Write(data);
				}
				catch (Exception e)
				{
					error = "Cannot write to file: " + fileName + "\r\n" + e.Message;
					success = false;
				}
				finally
				{
					w.Close();
				}
			}
			catch (Exception e)
			{
				error = "Cannot create file: " + fileName + "\r\n" + e.Message;
				success = false;
			}
			return success;
		}

		/// <summary>
		/// Write a string to a file, returning true if successful.
		/// </summary>
		/// <param name="fileName">Qualified filename</param>
		/// <param name="data">String collection to write</param>
		public static bool WriteToFile(string fileName, StringCollection data)
		{
			bool success = true;
			try
			{
				System.IO.StreamWriter w = System.IO.File.CreateText(fileName);
				try
				{
					foreach (string s in data)
						w.WriteLine(s);
				}
				catch (Exception e)
				{
					error = "Cannot write to file: " + fileName + "\r\n" + e.Message;
					success = false;
				}
				finally { w.Close(); }
			}
			catch (Exception e)
			{
				error = "Cannot create file: " + fileName + "\r\n" + e.Message;
				success = false;
			}
			return success;
		}

		/// <summary>
		/// Reads the contents of a file into a string, returning true if successful.
		/// </summary>
		/// <param name="fileName">Qualified filename</param>
		/// <param name="data">Output data</param>
		public static bool ReadFromFile(string fileName, out string data)
		{
			bool success = true;
			data = "";
			try
			{
				System.IO.StreamReader r = System.IO.File.OpenText(fileName);
				try
				{
					data = r.ReadToEnd();
				}
				catch (Exception e)
				{
					error = "Cannot read from file: " + fileName + "\r\n" + e.Message;
					success = false;
				}
				finally { r.Close(); }
			}
			catch (Exception e)
			{
				error = "Cannot open file: " + fileName + "\r\n" + e.Message;
				success = false;
			}
			return success;
		}

		/// <summary>
		/// Reads the contents of a file into a string, returning true if successful.
		/// </summary>
		/// <param name="fileName">Qualified filename</param>
		/// <param name="data">Output data</param>
		public static bool ReadFromFile(string fileName, out StringCollection data)
		{
			bool success = true;
			data = new StringCollection();
			try
			{
				System.IO.StreamReader r = System.IO.File.OpenText(fileName);
				try
				{
					string s;
					do
					{
						s = r.ReadLine();
						if (s != null) data.Add(s);
					}
					while (s != null);
				}
				catch (Exception e)
				{
					error = "Cannot read from file: " + fileName + "\r\n" + e.Message;
					success = false;
				}
				finally { r.Close(); }
			}
			catch (Exception e)
			{
				error = "Cannot open file: " + fileName + "\r\n" + e.Message;
				success = false;
			}
			return success;
		}



		/// <summary>
		/// method to encrypt the file
		/// </summary>
		/// <param name="inputFile"></param>
		/// <param name="outputFile"></param>
		public static void EncryptFile(string inputFile, string outputFile)
		{
			try
			{
				string password = @"myKey123"; // Your Key Here
				UnicodeEncoding UE = new UnicodeEncoding();
				byte[] key = UE.GetBytes(password);

				string cryptFile = outputFile;
				FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

				RijndaelManaged RMCrypto = new RijndaelManaged();

				CryptoStream cs = new CryptoStream(fsCrypt,
					RMCrypto.CreateEncryptor(key, key),
					CryptoStreamMode.Write);

				FileStream fsIn = new FileStream(inputFile, FileMode.Open);

				int data;
				while ((data = fsIn.ReadByte()) != -1)
					cs.WriteByte((byte)data);


				fsIn.Close();
				cs.Close();
				fsCrypt.Close();
			}
			catch(Exception ex)
			{
				//MessageBox.Show("Encryption failed!", "Error");
			}
		}

	}
}
