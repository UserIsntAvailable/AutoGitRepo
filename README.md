# AutoGitRepo

This is a tool made for automate the process of create a github repository.

## Installation
You need to compile the code with your token on Program.cs provided by github for use the GithubAPI.

Once compiled, you can put the .exe anywhere and after you can set it in your PC as a Global program or use it like a normal .exe if you will do that you can skip the next section.

### Steps for set .exe as a environment variable
Open start menu,<br />
Search **Edit the system environment variables** then click<br />
Click Environment variables button<br />
There you see two boxes, in System Variables box find path variable<br />
Click Edit<br />
a window pops up, click New<br />
Type the **Directory** path of the .exe (Directory means exclude the file name from path)<br />
Click Ok on all open windows and **close all the command prompt**<br />

## Usage
If you set it as a enverionment variable you need to call the program with _arg_ on the folder that you want start your new repo or if you want utilise the .exe you can put it in the folder for create the repo

### Arguments

arg [name] [description] [private] [gitignore]

**name**: Name of the new repository<br />
**description**: Description of the repository<br />
**private**: If your repository will be private or not (true or false)<br />
**gitignore**: The gitignore template that you want use (see here: https://github.com/github/gitignore)<br />

If you want add more arguments you only need to change the Parameters class with those you need and assign it the args[pos] or contrary delete arguments that you don't need (see here: https://developer.github.com/v3/repos/)


