# botbuilder-filter-cs
A Middleware to filter the context of a conversation based on a given predicate. Works with [Bot Builder v4](https://github.com/Microsoft/botbuilder-dotnet)

Based on [botbuilder-filter for Node.JS](https://github.com/ryanvolum/botbuilder-filter)

## Background

When we .use middleware, every message we receive gets passed through it. We use middleware to gather things like intent or sentiment, or to log telemetry, or to transform messages (e.g. translation), or to intercept a message. However, we don't always want our middleware to run. For instance, sentiment analysis on text is only useful if that text is fairly long. "No" registers as having incredibly low sentiment, though it doesn't necessarily indicate a disgruntled user. In this case, we would only want to call our sentiment analysis service if our incoming message is over a specific length. 

Enter middleware filtering. FilterMiddleware.Filter is just a static function that only runs middleware if a predicate is true.

If we want our bot to pass route messages through a piece of middleware, we .Use use that middleware from the Startup.cs:

```csharp
services.AddSingleton<BotFrameworkAdapter>(_ =>
  {
      return new BotFrameworkAdapter(Configuration)
          .Use(new TestMiddleware());
  });
```

If we only want that middleware to run based on custom logic we pass in our Filter function, which takes a predicate and middleware:

```csharp
services.AddSingleton<BotFrameworkAdapter>(_ =>
  {
      return new BotFrameworkAdapter(Configuration)
          .Use(FilterMiddleware.Filter(lengthPredicate, new TestMiddleware()));
  });
```

A predicate is just a boolean function. In the above case, our lengthPredicate is checks whether the length of the message in context.request.text is over 5 characters:

```sharp
Func<IBotContext, bool> lengthPredicate = (context) => 
{
    return (context.Request.Type == "message" && context.Request.AsMessageActivity().Text != null && context.Request.AsMessageActivity().Text.Length < 5);
};
```

For the sake of demonstration, our TestMiddleware replies to the user if it runs:

```csharp
  public Task SendActivity(IBotContext context, IList<IActivity> activities, MiddlewareSet.NextDelegate next)
  {
      context.Reply("test");
      return Task.CompletedTask;
  }
```

So, the sample will only reply from middleware if the incoming message is less than 5 characters long! Of course, you can define any predicate. 

Note, we can pass an arbitrary number of middlewares into the filter!

Neither of the above pieces of middleware will run if the length predicate is not met.
