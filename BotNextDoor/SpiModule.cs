using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace BotNextDoor
{
    public class SpiModule : ModuleBase<SocketCommandContext>
    {
        // ~say hello world -> hello world
        [Command("spi")]
        [Summary("Send to spi.")]
        public Task SpiAsync()
        {
            var messageRefenence = new MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id);
            return ReplyAsync("Sam spi", false, null, RequestOptions.Default, null, messageRefenence);
        }
    }
}