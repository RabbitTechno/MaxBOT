using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxBOT
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commads { get; set; }
        static async Task Main(string[] args)
        {

            var jasonReader = new JsonReader(); // var contain our vaild Token & Prefix
            await jasonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jasonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true

            };
            Client = new DiscordClient(discordConfig);
            Client.Ready += Client_Ready; //Client_Ready is a method down there

            await Client.ConnectAsync();
            await Task.Delay(-1); // -1 to still running until we stop it

           


        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            
            return Task.CompletedTask;
        }

    }
}
