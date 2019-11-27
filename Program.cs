using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AutoGitRepo {
    class Program {

        private static int lineCount = 0;
        private static readonly StringBuilder output = new StringBuilder();

        static void Main(string[] args) {

            // Initializer of json
            List<Parameters> jsonData = new List<Parameters> {
                new Parameters() {
                    name = args[0],
                    description = args[1],
                    @private = args[2].Contains("true"),
                    auto_init = true,
                    gitignore_template = args[3],
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
                    // Prepend line numbers to each line of the output.
                    if (!string.IsNullOrEmpty(e.Data)) {
                        lineCount++;
                        output.Append("\n[" + lineCount + "]: " + e.Data);
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
                    string page_url = output.ToString().Replace("\n", "").Replace("[1]: ", "");

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
    // Attributes for the json file
    public class Parameters {
        public string name { get; set; }
        public string description { get; set; }
        public bool @private { get; set; }
        public bool auto_init { get; set; }
        public string gitignore_template { get; set; }
        public string license_template { get; set; }
    }
}
