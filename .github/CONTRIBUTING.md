# Contributing to the PRPG Discord bot

First off, thanks for taking the time to contribute!

The following is a set of guidelines and explanations for contributing to the PRPG Discord Bot.
These are mostly guidelines, not rules. Use your best judgement, and feel free to propose changes to this document in a pull request.


### Table of Contents
[What should I know before I get started?](#what-should-i-know-before-i-get-started)

[How Can I Contribute?](#how-can-i-contribute?)
* [Reporting Bugs](#reporting-bugs)
* [Suggesting Enhancements](#suggesting-enhancements)
* [Your First Code Contribution](#your-first-code-contribution)

[Styleguides](#styleguides)


## What should I know before I get started?
Before starting to work on this project, you need to make sure to categorize under one of the following types of contributors:
* [PRPG Developer](#prpg-developer) - Has a profound understanding of [C#](https://docs.microsoft.com/en-us/dotnet/csharp/getting-started/) or [Discord.NET](https://github.com/RogueException/Discord.Net) and actively is able to program parts of the bot without much guidance.
* [PRPG Contributor](#prpg-contributor) - You can navigate the code and fix small errors, like typos. Or are able to create small commands with the power of copy paste.
* [PRPG Tester](#prpg-tester) - You are here to help us test, not to program or even look at the code.

### PRPG Developer
Hello, and welcome yet again. The PRPG Developers are a rare breed to come by, so I'm really grateful you've decided to help us out with the project!

Before you start, make sure you have a good understanding of the C# language. I won't test you on this, no worries. But I might ask you why you're using one method over another I'd have used. 
It's also useful to have some knowledge about [Discord.NET](https://github.com/RogueException/Discord.Net). It's what powers the bot and allows us to do all this magic.

As a PRPG Developer you can work standalone, but we'd like it a lot if you'd join our testing server on discord. This way we can discuss any changes or additions you make to the project, when necessary. Feel free to write me up a DM if you'd like to join our Developer Chat. 

### PRPG Contributor
Welcome on in! And thanks again for taking your time out to read.

Our contributors take away a lot of stress on our Developers. Not only do you guys help us with ideas, you guys also make sure we don't break anything and leave the bedbugs to eat everything.

If you haven't yet, please use the command: `?rank PRPG Contributors` in any of our discord chats. This will add you to the contributor role. 
_Please note that you may be subject of removal from this chat if you misuse it._

### PRPG Tester
Hey there, and welcome! 

To test, all you have to do is join our discord server, and start talking in our testing channel.
If you want to help test any ongoing problems or report one, please head over to our [issues page](https://github.com/dylanpiera/PRPGDiscordBot/issues).


## How Can I Contribute?

### Reporting Bugs

#### Before Submitting A Bug Report
* Make sure your bug isn't a known [issue](https://github.com/dylanpiera/PRPGDiscordBot/issues).
* Make sure it's a bug big enough worth reporting. (F.E. each and every typo doesn't need their own issue.)
 

#### How Do I Sumbit A (Good) Bug Report?
Bugs are tracked as [Github Issues](https://github.com/dylanpiera/PRPGDiscordBot/issues).
Explain the problem and include additional details to help maintainers reproduce the problem:
* **Use a clear and descriptive title** for the issue to identify the problem.
* **Describe the exact steps which reproduce the problem** in as many details as possible. **don't just say what you did, but explain how you did it**.
* **Provide specific examples to demonstrate the steps**. You can (and probably should) include screenshots or animated GIFs which show you following the described steps and clearly demonstrate the problem. You can use [this tool](https://www.cockos.com/licecap/) to record GIFs on macOS and Windows, and [this tool](https://github.com/colinkeenan/silentcast) or [this tool](https://github.com/GNOME/byzanz) on Linux.
* **Describe the behavior you observed after following the steps** and point out what exactly is the problem with that behavior.
* **Explain which behavior you expected to see instead and why.**
* **Make sure to use our [ISSUE_TEMPLATE](https://github.com/dylanpiera/PRPGDiscordBot/blob/master/.github/ISSUE_TEMPLATE.md)** The template is automatically inserted if you open a new issue, so fill it in while you're at it! :)
 

### Suggesting Enhancements

#### Before Submitting An Enhancement Suggestion
* Make sure your enhancement isn't already suggested by someone else in an  [issue](https://github.com/dylanpiera/PRPGDiscordBot/issues). (if it is, please comment on it instead.)
* Make sure it's big enough to be worth reporting. (F.E. each and every typo doesn't need their own issue.)
 
#### How Do I Submit A (Good) Enhancement Suggestion?
Enhancement suggestions are tracked as [Github Issues](https://github.com/dylanpiera/PRPGDiscordBot/issues).
* **Use a clear and descriptive title** for the issue to identify the suggestion.
* **Provide a step-by-step description of the suggested enhancement** in as many details as possible.
* **Describe the current behavior** and **Explain which behavior you expected to see instead** and why.
* **Explain why this enhancement would be useful**.
* **Make sure to use our [ISSUE_TEMPLATE](https://github.com/dylanpiera/PRPGDiscordBot/blob/master/.github/ISSUE_TEMPLATE.md)** The template is automatically inserted if you open a new issue, so fill it in while you're at it! :)

### Your First Code Contribution
Unsure where to begin contributing? You can start by looking through these `beginner` and `help-wanted` issues:

* [Beginner issues][https://github.com/dylanpiera/PRPGDiscordBot/labels/Beginner] - issues which should only require a few lines of code, and a test or two.
* [Help wanted issues][https://github.com/dylanpiera/PRPGDiscordBot/labels/help-wanted] - issues which should be a bit more involved than `beginner` issues.
 
#### Testing your own changes
To test your own changes you either have to create a test bot and test it in your own server, or wait for it to be added to the public test bot and test it on there. **We recommend using your own bot so you can log entries. But we do understand if this ain't a possibility for you.**

#### Setting up the bot
To run the bot, you can use whatever IDE you are most familiar with, or the command line if that's your shtick. I personally recommend Visual Studio 2017. But be aware you will need .NET Core 1.1.2+ to be able to run the Discord .NET api. 
_if you have any problems installing Visual Studio, please google them. You may also ask around in the contributor or developer chat._

To get the bot down on your PC, you can use any git client you would like to use. If you've never used git before I recommend checking out the [Github Desktop Client](https://desktop.github.com/). Or google some tutorials on how to use it.
After this fork the repository and download it to your local machine.

After you've loaded everything into your IDE, and try to build it, you'll probably receive an error. `Sneaky.BotToken` or something along those lines, will be missing.
Since a Bot's token is basically a Bot's password, we store it in here. Same goes for the database credentials. We'll quickly go over on how to create your Sneaky.cs

#### Creating Sneaky.cs
After having created the file, you can copy and paste this codeblock:
```csharp
static public class Sneaky
{
    private static string token = "YOUR TOKEN HERE";
    private static string databaseUrl = "YOUR DATABASE CREDENTIALS. OR EMPTY", user = "<<<", password = "<<<";

    public static string Token { get => token; }
    public static string DatabaseUrl { get => databaseUrl;}
    public static string User { get => user; }
    public static string Password { get => password; }
}
```
After this you can fill in your credentials. If you are unaware what your Bot Token is or haven't created a bot account yet, please follow [this](https://github.com/reactiflux/discord-irc/wiki/Creating-a-discord-bot-&-getting-a-token) tutorial. 
If you do not have Database credentials, you will need to request these with Dylan. These credentials aren't given out freely, and only to long-time PRPG Developers. _Please request your changes to be tested on the public test bot if you need to use the database._

#### Pull Requests
* Fill in [the required template](PULL_REQUEST_TEMPLATE.md)
* Do not include issue numbers in the PR title
* Include screenshots and animated GIFs in your pull request whenever possible.
* Document new code based on the [Documentation Styleguide](#documentation-styleguide)

## Styleguides

### Git Commit Messages

* Use the present tense ("Add feature" not "Added feature")
* Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
* Reference issues and pull requests liberally after the first line
* Consider starting the commit message with an applicable emoji:
    * :art: `:art:` when improving the format/structure of the code
    * :racehorse: `:racehorse:` when improving performance
    * :non-potable_water: `:non-potable_water:` when plugging memory leaks
    * :memo: `:memo:` when writing docs
    * :bug: `:bug:` when fixing a bug
    * :fire: `:fire:` when removing code or files
    * :lock: `:lock:` when dealing with security

### C# & Comment Guidelines.
[To be added]
