using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Bot.Samples.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : BotController
    {
        public MessagesController(BotFrameworkAdapter adapter) : base(adapter) { }

        protected override Task OnReceiveActivity(IBotContext context)
        {
            if (context.Request.Type == ActivityTypes.Message)
            {
                var sentText = context.Request.AsMessageActivity().Text;
                context.Reply($"You said: {sentText}");
            }

            return Task.CompletedTask;
        }
    }
}

