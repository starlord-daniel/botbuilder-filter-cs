using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Middleware;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotBuilder.Filter
{
    public static class FilterMiddleware 
    {
        public static MiddlewareSet Filter(Func<IBotContext, bool> filterPredicate, params IMiddleware[] middlewareList)
        {
            var middlewareSet = new MiddlewareSet();

            foreach (var middleware in middlewareList)
            {
                Func<IBotContext, MiddlewareSet.NextDelegate, Task> contextCreated = (context, next) =>
                {
                    var testPredicate = (filterPredicate(context) && ((IContextCreated)middleware).ContextCreated(context, next) != null);
                    return testPredicate ? ((IContextCreated)middleware).ContextCreated(context, next) : next();
                };

                Func<IBotContext, MiddlewareSet.NextDelegate, Task> receiveActivity = (context, next) =>
                {
                    var testPredicate = (filterPredicate(context) && ((IReceiveActivity)middleware).ReceiveActivity(context, next) != null);
                    return testPredicate ? ((IReceiveActivity)middleware).ReceiveActivity(context, next) : next();
                };

                Func<IBotContext, IList<IActivity>, MiddlewareSet.NextDelegate, Task> sendActivity = (context, activities, next) =>
                {
                    var testPredicate = (filterPredicate(context) && ((ISendActivity)middleware).SendActivity(context, activities, next) != null);
                    return testPredicate ? ((ISendActivity)middleware).SendActivity(context, activities, next) : next();
                };

                middlewareSet.Use(new Middleware {
                    _contextCreated = contextCreated,
                    _receiveActivity = receiveActivity,
                    _sendActivity = sendActivity
                });
            }

            return middlewareSet;
        }
    }

    internal class Middleware : IMiddleware, IContextCreated, ISendActivity, IReceiveActivity
    {
        internal Func<IBotContext, MiddlewareSet.NextDelegate, Task> _contextCreated;
        internal Func<IBotContext, MiddlewareSet.NextDelegate, Task> _receiveActivity;
        internal Func<IBotContext, IList<IActivity>, MiddlewareSet.NextDelegate, Task> _sendActivity;


        public Task ContextCreated(IBotContext context, MiddlewareSet.NextDelegate next)
        {
            return _contextCreated(context, next);
        }

        public Task ReceiveActivity(IBotContext context, MiddlewareSet.NextDelegate next)
        {
            return _receiveActivity(context, next);
        }

        public Task SendActivity(IBotContext context, IList<IActivity> activities, MiddlewareSet.NextDelegate next)
        {
            return _sendActivity(context, activities, next);
        }
    }
}
