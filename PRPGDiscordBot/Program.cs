using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using PokeAPI;

public class Program
{
    public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    private DiscordSocketClient _client;
    private CommandService commands;
    private IServiceProvider services;

    public async Task MainAsync()
    {
        DataFetcher.ShouldCacheData = true;

        _client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Info, MessageCacheSize = 100 });
        commands = new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false, LogLevel = LogSeverity.Info });

        _client.Log += Log;
        _client.Ready += Ready;

        services = new ServiceCollection().BuildServiceProvider();

        await InstallCommands();

        await _client.LoginAsync(TokenType.Bot, Sneaky.Token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private Task _client_UserUpdated(SocketUser arg1, SocketUser arg2)
    {
        throw new NotImplementedException();
    }

    private async Task Ready()
    {
        await Log(new LogMessage(LogSeverity.Info, "Ready Event", "Bot is ready for use."));
    }

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

    public async Task HandleCommand(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        if (message == null) return;

        int argPos = 0;

        if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

        var context = new CommandContext(_client, message);

        var result = await commands.ExecuteAsync(context, argPos, services);
        if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
    }

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
