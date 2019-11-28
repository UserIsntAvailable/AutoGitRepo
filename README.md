# AutoGitRepo

This is a tool made for automate the process of create a github repository.

## Installation
You need to compile the code with your token on Program.cs provided by github for use the GithubAPI.

Once compiled, you can put the .exe anywhere and after you can set it in your PC as a Global program or use it like a normal .exe if you will do that you can skip the next section.

### Steps for set .exe as a environment variable
 -1. Open start menu,<br />
 -2. Search **Edit the system environment variables** then click<br />
 -3. Click Environment variables button<br />
 -4. There you see two boxes, in System Variables box find path variable<br />
 -5. Click Edit<br />
 -6. a window pops up, Click New<br />
 -7. Type the **Directory** path of the .exe (Directory means exclude the file name from path)<br />
 -8. Click Ok on all open windows and **close all the command prompt**<br />

## Usage
If you set it as a enverionment variable you need to call the program with _arg_ on the folder that you want start your new repo or if you want utilise the .exe you can put it in the folder for create the repo

### Arguments

These are the arguments that you can use in AutoGitRepo:

[-n | --name]         (required)   Set the name of the new repository
[-d | --description]  (optional)   Set the description of the new repository
[-p | --is-private]   (optional)   If this argument is present your new repository will be private
[-g | --gitignore]    (optional)   Name from the [gitignore template](https://github.com/github/gitignore)(without the)

If you want add more arguments you only need to change the Parameters class with those you need and assign it the args[pos] or contrary delete arguments that you don't need (see here: https://developer.github.com/v3/repos/)


