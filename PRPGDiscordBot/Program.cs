using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using PokeAPI;
using Discord.Addons.Interactive;

public class Program
{
    public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    private DiscordSocketClient _client;
    private CommandService commands;
    private IServiceProvider services;

    /// <summary>
    /// Bot Start Function.
    /// </summary>
    public async Task MainAsync()
    {
        //Makes sure the PokeAPI caches the received data.
        DataFetcher.ShouldCacheData = true;

        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 100
        });
        commands = new CommandService(new CommandServiceConfig
        {
            CaseSensitiveCommands = false,
            LogLevel = LogSeverity.Info
        });

        //adds the Log & Ready method to their respective listeners.
        _client.Log += Log;
        _client.Ready += Ready;

        services = new ServiceCollection().AddSingleton(_client).AddSingleton<InteractiveService>().BuildServiceProvider();

        await InstallCommands();

        //For an explanation on how to set up your own Sneaky file, please check the Contribution Guide, or project Readme.
        await _client.LoginAsync(TokenType.Bot, Sneaky.Token);
        await Log("Booting version 0.2.1","Bootup",LogSeverity.Info);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task Ready()
    {
        await Log(new LogMessage(LogSeverity.Info, "Ready Event", "Bot is ready for use."));
    }

    #region CommandInitialization
    /// <summary>
    /// Initializes all commands. Don't touch unless you know what you're doing.
    /// </summary>
    public async Task InstallCommands()
    {
        _client.MessageReceived += HandleCommand;

        await commands.AddModulesAsync(Assembly.GetEntryAssembly());

        Console.WriteLine("List of Commands on boot-up:");
        foreach (CommandInfo command in commands.Commands)
        {
            Console.WriteLine("!" + command.Name);
        }
        Console.WriteLine("---");
    }

    /// <summary>
    /// The Command Handler. Don't touch unless you know what you're doing.
    /// </summary>
    public async Task HandleCommand(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        if (message == null) return;

        int argPos = 0;

        if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

        var context = new SocketCommandContext(_client, message);

        var result = await commands.ExecuteAsync(context, argPos, services);
        if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
    }
    #endregion

    /// <summary>
    /// This logger is used for all errors the Discord API throws.
    /// You can also use it to log custom errors.
    /// </summary>
    public static async Task Log(LogMessage arg)
    {
        switch (arg.Severity)
        {
            case LogSeverity.Error: Console.ForegroundColor = ConsoleColor.Red; break;
            case LogSeverity.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
            case LogSeverity.Debug: Console.ForegroundColor = ConsoleColor.Green; break;
        }

        await Console.Out.WriteLineAsync($"[{DateTime.Now.ToString("hh:mm:ss")}] [{arg.Severity}] [{arg.Source}] {arg.Message}");
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    /// <summary>
    /// You can use this version of the log method to make custom errors.
    /// </summary>
    public static async Task Log(string msg, string source = "Log Command", LogSeverity logSev = LogSeverity.Debug)
    {
        LogMessage arg = new LogMessage(logSev, source, msg);
        switch (arg.Severity)
        {
            case LogSeverity.Error:
            case LogSeverity.Critical:
                Console.ForegroundColor = ConsoleColor.Red; break;
            case LogSeverity.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
            case LogSeverity.Debug: Console.ForegroundColor = ConsoleColor.Green; break;
        }

        await Console.Out.WriteLineAsync($"[{DateTime.Now.ToString("hh:mm:ss")}] [{arg.Severity}] [{arg.Source}] {arg.Message}");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
