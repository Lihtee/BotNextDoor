using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace BotNextDoor
{
    class Program
    {
        private const string Base64Token = "T0RreU1EYzVNakUxTnpRMU16QTJOamcxLllWSHJndy45V080cTh3TFZCYmpXeU5YZGl1SDR3dnNsbzA=";

        private DiscordSocketClient _client;

        private CommandService _commands;

        private CommandHandler _commandHandler;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _commands = new CommandService(new CommandServiceConfig());

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            var token = Encoding.Default.GetString(Convert.FromBase64String(Base64Token));

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            _commandHandler = new CommandHandler(_client, _commands);
            await _commandHandler.InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}