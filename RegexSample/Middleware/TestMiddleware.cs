using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Middleware;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Bot.Samples.Middleware
{
    public class TestMiddleware : IMiddleware, IContextCreated, ISendActivity, IReceiveActivity
    {
        public Task ContextCreated(IBotContext context, MiddlewareSet.NextDelegate next)
        {
            context.Reply("test");

            return Task.CompletedTask;
        }

        public Task ReceiveActivity(IBotContext context, MiddlewareSet.NextDelegate next)
        {
            context.Reply("test");

            return Task.CompletedTask;
        }

        public Task SendActivity(IBotContext context, IList<IActivity> activities, MiddlewareSet.NextDelegate next)
        {
            context.Reply("test");

            return Task.CompletedTask;
        }
    }
}
