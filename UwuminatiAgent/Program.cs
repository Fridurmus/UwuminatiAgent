using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Newtonsoft.Json;

namespace UwuminatiAgent
{
    class Program
    {
        // Declare a few useful static fields.
        static DiscordClient discord;
        static CommandsNextModule commands;
        
        
        // Start the Bot.
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        // The Bot's main execution loop is an Async Task with infinite Delay, ensuring it won't time out under normal circumstances.
        static async Task MainAsync (string[] args)
        {

            // We need a secure way to hold token data
            var json = "";

            // Unpack the token data into our struct.
            using (var configFile = File.OpenRead("config.json"))
            using (var configReader = new StreamReader(configFile, new UTF8Encoding(false)))
                json = await configReader.ReadToEndAsync();
            var jsonConfig = JsonConvert.DeserializeObject<ConfigJson>(json);

            // Init a variable to hold our config struct, so it's easy to change later.
            var config = new DiscordConfiguration
            {
                Token = jsonConfig.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            // Initialize the variable holding our Discord Client.
            discord = new DiscordClient(config);

            // Basic ping/pong functionality, essentially a debug function early on.
            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync("pong!");
            };

            // Set up the string prefix for commands. In this case, because it's convention, we're using "!".
            // the space is important for presentability reasons.
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "!",
                EnableDms = true,
                EnableMentionPrefix = true
            });

            // Register our commands from Commands.cs.
            commands.RegisterCommands<Commands>();
            
            // Connect to the service.
            await discord.ConnectAsync();
            
            // Make sure the await has infinite delay, so that the bot won't naturally time out!
            await Task.Delay(-1);
        }

        // I lifted this concept wholesale from a bot example, but it's real, real good.
        // Using a public struct, we can hold JSON data without worry.
        public struct ConfigJson
        {
            [JsonProperty("token")]
            public string Token { get; private set; }
        }
    }
}
