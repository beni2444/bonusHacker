// Program.cs

using api;
using MassTransit;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var username = context.Configuration.GetValue<string>("RabbitMq:Username");
        var password = context.Configuration.GetValue<string>("RabbitMq:Password");
        
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PageScrapedConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {

                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                
                cfg.ConfigureEndpoints(ctx);
            });
        });
    })
    .Build();

await host.RunAsync();
