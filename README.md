# AutoGitRepo
This is a tool made for automate the process of create a github repository.

## Setup
As a recommendation of [Ben Awad](https://www.youtube.com/user/99baddawg), it is a good idea to set your api tokens as an environment variable to facilitate its use.

If you want to set it as an environment variable, I use _githubApiToken_ as varible name, of course, you could simply compile the code using your own api token.

## Usage
You can set this program too as an environment variable to use it everywhere.

### Arguments

These are the arguments available in AutoGitRepo:

[ --name | -n ]           (required)   Name of the new repository<br />
[ --description | -d ]    (optional)   The description of the new repository<br />
[ --gitignore | -g ]      (optional)   Name from the [gitignore template](https://github.com/github/gitignore)<br />
[ --is-private | -p ]     (optional)   If present your new repository will be private<br />
[ --on-current-dir | -o ] (optional)   If present the repo files will be pasted on the current dir<br />
