﻿using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace AutoGitRepo {

    class Program {

        static readonly string apiToken = Environment.GetEnvironmentVariable("githubApiToken");

        static int Main(string[] args) {

            var rootCommand
                = new RootCommand("A tool made for automate the process of create a github repository.") {

                new Argument<string>(
                    "name",
                    "Name of the new repository"),

                new Option(
                    new string[] { "--description", "-d" },
                    "The description of the new repository"
                    ) { Argument = new Argument<string>() },

                new Option(
                    new string[] { "--gitignore", "-g" },
                    "Name from the gitignore template ( see README )"
                    ) { Argument = new Argument<string>() },

                new Option(
                    new string[] { "--is-private", "-p" },
                    "Sets the project private"
                    ) { Argument = new Argument<bool>() },

                new Option(
                    new string[] { "--on-current-dir", "-o" },
                    "Sets the name of the new repository"
                    ) { Argument = new Argument<bool>() },
            };

            rootCommand.Handler = CommandHandler.Create((string name, string description, string gitignore, bool isPrivate, bool isCurrentDir) => {

                var json = JsonSerializer.Serialize(new {
                    name,
                    description,
                    /* I need to do this because for some reason command line api force
                     * me to name the lambda parameters exactly as the options names */
                    gitignore_template = gitignore,
                    @private = isPrivate,
                    auto_init = true,
                    license_template = "mit"
                });

                using HttpClient httpClient = new HttpClient();

                var header = httpClient.DefaultRequestHeaders;

                header.Add(
                    "User-Agent",
                    "Chrome/42.0.2311.135");

                header.Add(
                    "Authorization",
                    $"token {apiToken}");

                var resp = httpClient.PostAsync(
                    "https://api.github.com/user/repos",
                    new StringContent(json, Encoding.UTF8, "application/json")).Result;

                if (!resp.IsSuccessStatusCode) {

                    Console.WriteLine(resp.Content.ReadAsStringAsync().Result);

                    return;
                }

                Console.WriteLine("Your repository was created succesfully.");
            });

            // If args are passed
            return args.Length != 0
            // Invoke them
            ? rootCommand.Invoke(args)
            // else, call help method
            : rootCommand.Invoke("-h");
        }
    }
}
