using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoGitRepo {
    class Program {

        #region Private Properties
        private static int LineCount { get; set; }
        private static StringBuilder Output { get; set; } = new StringBuilder();

        private static string RepositoryName { get; set; }
        private static string RepositoryDescription { get; set; } = "";
        private static string GitIgnore { get; set; } = "";
        private static bool IsPrivate { get; set; } = false;
        #endregion

        static void Main(string[] args) {

            // If 0 arguments are given print description of the app and help argument 
            if (args.Length == 0) {
                Console.WriteLine("This is a tool made for automate the process of create a github repository.\n\nA help argument will be added soon :)");
                return;
            }

            // Else run the command checker
            else {
                // Loop each argument in args
                for (int i = 0; i < args.Length; i += 2) {
                    switch (args[0 + i]) {

                        case "--name":
                        case "-n":
                            if (args.Contains("--name") & args.Contains("-n")) {
                                throw new RepetedArgumentsException("--name and -n are repeted");
                            }
                            RepositoryName = args[1 + i];
                            break;

                        case "--description":
                        case "-d":
                            if (args.Contains("--description") & args.Contains("-d")) {
                                throw new RepetedArgumentsException("--description and -d are repeted");
                            }
                            RepositoryDescription = args[1 + i];
                            break;

                        case "--is_private":
                        case "-p":
                            if (args.Contains("--is_private") & args.Contains("-p")) {
                                throw new RepetedArgumentsException("--is_private and -p are repeted");
                            }
                            IsPrivate = true;
                            // This argument dosen't need a parameter.
                            i -= 1;
                            break;

                        case "--gitignore":
                        case "-g":
                            if (args.Contains("--gitignore") & args.Contains("-g")) {
                                throw new RepetedArgumentsException("--gitignore and -g are repeted");
                            }
                            GitIgnore = args[1 + i];
                            break;

                        case "--help":
                        case "-h":
                            if (args.Contains("--help") & args.Contains("-h")) {
                                throw new RepetedArgumentsException("--help and -h are repeted");
                            }
                            Console.WriteLine("This will added soon");
                            return;

                        default:
                            throw new ArgumentNotImplementedException($"The argument {args[0 + i]} is not implement yet");
                    }
                }

                // Check if the Repository Name it wasn't initialized
                if(RepositoryName == null) {
                    throw new ArgumentNameNotUsedException("--name(-n) is a obligatory argument");
                }
            }

            // Initializer of json
            List<JsonAttributes> jsonData = new List<JsonAttributes> {
                new JsonAttributes() {
                    name = RepositoryName,
                    description = RepositoryDescription,
                    @private = IsPrivate,
                    auto_init = true,
                    gitignore_template = GitIgnore,
                    license_template = "mit"
                }
            };

            // Your github key
            const string myToken = "yourTokenKey";

            // Commands using curl with GithubAPI
            string batCommand = $@"
read -r -d '' PAYLOAD <<EOP
{JsonConvert.SerializeObject(jsonData.ToArray()).Replace("[", "").Replace("]", "")}
EOP

JSON=`curl -H ""Authorization: token {myToken}"" -d ""$PAYLOAD"" https://api.github.com/user/repos `

JSON=${{JSON//\""}}
JSON=${{JSON//\{{}}
JSON=${{JSON//\}}}}
JSON=${{JSON//\: /\=}}

page_url=`echo $JSON | grep -oP 'full_name=\K\w+/\w+'`

echo $page_url";

            // Create the run.bat 
            using (var file = File.Create("run.bat")) {
                file.Close();
            };

            // Write the commands that needs the .bat
            File.WriteAllText("run.bat", batCommand);

            // Initializing mainProcess( run.bat )
            using (Process mainProcess = new Process()) {

                // Process Info
                mainProcess.StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = $"/C bash run.bat",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                // Event that catch the output from run.bat
                mainProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) => {


                    // Append line numbers to each line of the output.
                    if (!string.IsNullOrEmpty(e.Data)) {
                        LineCount++;
                        Output.Append("\n[" + LineCount + "]: " + e.Data);
                    }
                });

                // Starting process
                mainProcess.Start();

                // Getter of the page of the repo
                mainProcess.BeginOutputReadLine();
                mainProcess.WaitForExit();

                // Stating gitProcess ( git clone of the projet created )
                using (Process gitProcess = new Process()) {

                    // The link for the repo that was created
                    string page_url = Output.ToString().Replace("\n", "").Replace("[1]: ", "");

                    // Process Info
                    gitProcess.StartInfo = new ProcessStartInfo {
                        FileName = "cmd.exe",
                        Arguments = $"/C git clone https://github.com/{page_url}",
                        UseShellExecute = false
                    };

                    // Starting process
                    gitProcess.Start();

                    /* If you are not connected with your github account
                    the program will wait until you put your credentials */
                    gitProcess.WaitForExit();

                    // Close gitProcess
                    gitProcess.Close();
                }

                // Wait until curl will finish
                mainProcess.WaitForExit();

                // Close mainProcess
                mainProcess.Close();

                // Deleting run.bat ( For only have the .exe compiled )
                File.Delete("run.bat");
            }
        }
    }
    class JsonAttributes {
        public string name { get; set; }
        public string description { get; set; }
        public bool @private { get; set; }
        public bool auto_init { get; set; }
        public string gitignore_template { get; set; }
        public string license_template { get; set; }
    }
}
