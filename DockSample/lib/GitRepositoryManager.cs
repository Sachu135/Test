using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace DockSample.lib
{
    public class GitRepositoryManager
    {
        private readonly string _repoSource;
        private readonly UsernamePasswordCredentials _credentials;
        private readonly DirectoryInfo _localFolder;

        public GitRepositoryManager()
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GitRepositoryManager" /> class.
        /// </summary>
        /// <param name="username">The Git credentials username.</param>
        /// <param name="password">The Git credentials password.</param>
        /// <param name="gitRepoUrl">The Git repo URL.</param>
        /// <param name="localFolder">The full path to local folder.</param>
        public GitRepositoryManager(string username, string password, string gitRepoUrl, string localFolder)
        {
            var folder = new DirectoryInfo(localFolder);

            if (!folder.Exists)
            {
                throw new Exception(string.Format("Source folder '{0}' does not exist.", _localFolder));
            }

            _localFolder = folder;

            _credentials = new UsernamePasswordCredentials
            {
                Username = username,
                Password = password
            };

            _repoSource = gitRepoUrl;
        }


        public void InitRepository(string localFolder)
        {
            if (!Directory.Exists(Path.Combine(localFolder, ".git")))
            {
                string strTempFolder = Path.Combine(localFolder, Guid.NewGuid().ToString());
                if (!Directory.Exists(strTempFolder))
                    Repository.Init(strTempFolder);

                if (Directory.Exists(strTempFolder))
                {
                    //code to copy the all file in parent directory
                    //Now Create all of the directories
                    foreach (string dirPath in Directory.GetDirectories(strTempFolder, "*", SearchOption.AllDirectories))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(dirPath.Replace(strTempFolder, localFolder));
                        di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(strTempFolder, "*.*",
                        SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(strTempFolder, localFolder), true);

                    Directory.Delete(strTempFolder, true);
                }
            }
        }

        /// <summary>
        /// Commits all changes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.Exception"></exception>
        public void CommitAllChanges(string message, string localFolder, string email = "")
        {
            try
            {
                InitRepository(localFolder);
                using (var repo = new Repository(_localFolder.FullName))
                {
                    //Signature author = repo.Config.BuildSignature(DateTimeOffset.Now);
                    var files = _localFolder.GetFiles("*", SearchOption.AllDirectories).Select(f => f.FullName);
                    //repo.Stage(files);
                    Commands.Stage(repo, files);

                    //repo.Commit(message);

                    // Create the committer's signature and commit
                    var author = new Signature(_credentials.Username, email, DateTime.Now);
                    var committer = author;

                    //repo.Commit(message, author);

                    // Commit to the repository
                    var commit = repo.Commit(message, author, committer);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void CommitToBranch(string message, string localFolder, string branchName, string email = "")
        {
            try
            {
                InitRepository(localFolder);
                using (var repo = new Repository(_localFolder.FullName))
                {
                    //Signature author = repo.Config.BuildSignature(DateTimeOffset.Now);
                    var files = _localFolder.GetFiles("*", SearchOption.AllDirectories).Select(f => f.FullName);
                    //repo.Stage(files);
                    Commands.Stage(repo, files);

                    //repo.Commit(message);

                    // Create the committer's signature and commit
                    var author = new Signature(_credentials.Username, email, DateTime.Now);
                    var committer = author;

                    //repo.Commit(message, author);

                    // Commit to the repository
                    //var commit = repo.Commit(message, author, committer);

                    Commands.Checkout(repo, repo.Branches[branchName]);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Pushes all commits.
        /// </summary>
        /// <param name="branchName">Name of the remote branch.</param>
        /// <exception cref="System.Exception"></exception>
        public void PushCommits(string branchName, string localFolder, string email = "")
        {
            try
            {
                InitRepository(localFolder);
                using (var repo = new Repository(_localFolder.FullName))
                {
                    string remoteName = string.Empty;
                    string[] splitUrl = _repoSource.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitUrl.Count() > 0)
                    {
                        remoteName = splitUrl[splitUrl.Length - 1].Contains(".git") ? splitUrl[splitUrl.Length - 1].Replace(".git", string.Empty) : splitUrl[splitUrl.Length - 1];
                    }

                    var remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                    if (remote == null)
                    {
                        repo.Network.Remotes.Add(remoteName, _repoSource);
                        remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                    }

                    var options = new PushOptions
                    {
                        CredentialsProvider = (url, usernameFromUrl, types) => _credentials
                    };

                    var pushRefSpec = string.Format(@"refs/heads/{0}", branchName);

                    // Create the committer's signature and commit
                    var author = new Signature(_credentials.Username, email, DateTime.Now);

                    //repo.Network.Push(remote, pushRefSpec, options, author);
                    repo.Network.Push(remote, pushRefSpec, options);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Pull from repository
        /// </summary>
        /// <param name="branchName">Name of the remote branch</param>
        /// <param name="email">Email address for signature</param>
        public void Pull(string branchName, string localFolder, string email = "")
        {
            try
            {
                InitRepository(localFolder);
                using (var repo = new Repository(_localFolder.FullName))
                {
                    string remoteName = string.Empty;
                    string[] splitUrl = _repoSource.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitUrl.Count() > 0)
                    {
                        remoteName = splitUrl[splitUrl.Length - 1].Contains(".git") ? splitUrl[splitUrl.Length - 1].Replace(".git", string.Empty) : splitUrl[splitUrl.Length - 1];
                    }

                    var remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                    if (remote == null)
                    {
                        repo.Network.Remotes.Add(remoteName, _repoSource);
                    }

                    remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);

                    var fetchOptions = new FetchOptions
                    {
                        CredentialsProvider = (url, user, cred) => _credentials
                    };

                    var mergeOptions = new MergeOptions()
                    {
                        FastForwardStrategy = FastForwardStrategy.Default,
                        FileConflictStrategy = CheckoutFileConflictStrategy.Merge
                    };

                    PullOptions pullOptions = new PullOptions()
                    {
                        FetchOptions = fetchOptions,
                        MergeOptions = mergeOptions
                    };

                    var merger = new Signature(_credentials.Username, email, DateTime.Now);
                    string refSpec = string.Format("refs/heads/{2}:refs/remotes/{0}/{1}",
                        remoteName, branchName, branchName);
                    Commands.Fetch(repo, remote.Name, new string[] { refSpec }, pullOptions.FetchOptions, null);
                    repo.MergeFetchedRefs(merger, pullOptions.MergeOptions);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tuple<bool,bool,Dictionary<string,string>> HasUncommittedChanges()
        {
            bool linitialized = true;
            bool lretval = false;
            Dictionary<string, string> listFiles = new Dictionary<string, string>();
            try
            {
                using (var repo = new Repository(_localFolder.FullName))
                {
                    RepositoryStatus status = repo.RetrieveStatus();
                    if (status.IsDirty)
                    {
                        var files = repo.RetrieveStatus()
                            .Where(s => s.State == FileStatus.ModifiedInWorkdir || s.State == FileStatus.NewInWorkdir || s.State == FileStatus.DeletedFromWorkdir || s.State == FileStatus.Conflicted)
                            //.Select(s => s.FilePath)
                            .ToList();
                        if (files.Count > 0)
                        {
                            foreach(var item in files)
                            {
                                string strValue = string.Empty;
                                switch (item.State)
                                {
                                    case FileStatus.ModifiedInWorkdir:
                                        strValue = "UPDATE";
                                        break;
                                    case FileStatus.NewInWorkdir:
                                        strValue = "NEW";
                                        break;
                                    case FileStatus.DeletedFromWorkdir:
                                        strValue = "DELETE";
                                        break;
                                    case FileStatus.Conflicted:
                                        strValue = "CONFLICT";
                                        break;
                                }
                                listFiles.Add(item.FilePath, strValue);
                            }
                        }
                    }
                    return new Tuple<bool, bool, Dictionary<string, string>>(linitialized, status.IsDirty, listFiles);
                }
            }
            catch (Exception)
            {
                linitialized = false;
                return new Tuple<bool, bool, Dictionary<string, string>>(linitialized, lretval, listFiles);
            }
        }

        public List<string> GetBranches()
        {
            List<string> liBranches = new List<string>();
            var branches = Repository.ListRemoteReferences(_repoSource)
                         .Where(elem => elem.IsLocalBranch)
                         .Select(elem => elem.CanonicalName
                                             .Replace("refs/heads/", ""));

            if(branches.Count() > 0)
            {
                liBranches = branches.ToList();
            }

            return liBranches;
        }

        public bool CreateNewBranch(string strBranchName)
        {
            try
            {
                using (var repo = new Repository(_localFolder.FullName))
                {
                    string remoteName = string.Empty;
                    string[] splitUrl = _repoSource.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitUrl.Count() > 0)
                    {
                        remoteName = splitUrl[splitUrl.Length - 1].Contains(".git") ? splitUrl[splitUrl.Length - 1].Replace(".git", string.Empty) : splitUrl[splitUrl.Length - 1];
                    }

                    var newBranch = repo.CreateBranch(strBranchName);
                    var remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                    if (remote == null)
                    {
                        repo.Network.Remotes.Add(remoteName, _repoSource);
                        remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                    }

                    repo.Branches.Update(newBranch,
                            b => b.Remote = remote.Name,
                            b => b.UpstreamBranch = newBranch.CanonicalName);

                    var options = new PushOptions
                    {
                        CredentialsProvider = (url, usernameFromUrl, types) => _credentials
                    };
                    repo.Network.Push(newBranch, options);
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public bool IsValidRepoURL(string strGitURL)
        //{
        //    return Repository.IsValid(strGitURL);
        //}

        public string GetAssociateBranch(string LocalFolder)
        {
            InitRepository(LocalFolder);
            string currentBranchName = string.Empty;
            using (var repo = new Repository(LocalFolder))
            {
                currentBranchName = repo.Head.FriendlyName;
            }
            return currentBranchName;
        }

        public List<string> GetLocalBranches(string LocalFolder)
        {
            List<string> listLocalBranches = new List<string>();
            using (var repo = new Repository(LocalFolder))
            {
                var currentBranchName = repo.Branches;
                foreach(var item in currentBranchName)
                {
                    listLocalBranches.Add(item.FriendlyName);
                }
            }
            return listLocalBranches;
        }
        public bool CreateNewLocalBranch(string strBranchName)
        {
            try
            {
                using (var repo = new Repository(_localFolder.FullName))
                {
                    string remoteName = string.Empty;
                    string[] splitUrl = _repoSource.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitUrl.Count() > 0)
                    {
                        remoteName = splitUrl[splitUrl.Length - 1].Contains(".git") ? splitUrl[splitUrl.Length - 1].Replace(".git", string.Empty) : splitUrl[splitUrl.Length - 1];
                    }

                    var newBranch = repo.CreateBranch(strBranchName);
                    var remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                    if (remote == null)
                    {
                        repo.Network.Remotes.Add(remoteName, _repoSource);
                        remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                    }

                    repo.Branches.Update(newBranch,
                            b => b.Remote = remote.Name,
                            b => b.UpstreamBranch = newBranch.CanonicalName);

                    LibGit2Sharp.Commands.Checkout(repo, strBranchName);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SwitchBranch(string strBranchName)
        {
            try
            {
                using (var repo = new Repository(_localFolder.FullName))
                {
                    LibGit2Sharp.Commands.Checkout(repo, strBranchName);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
