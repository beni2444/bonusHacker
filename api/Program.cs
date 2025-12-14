// Program.cs

using api;
using api.Data.Config;
using MassTransit;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var username = context.Configuration.GetValue<string>("RabbitMq:Username");
        var password = context.Configuration.GetValue<string>("RabbitMq:Password");
        var host = context.Configuration.GetValue<string>("RabbitMq:Host");
        
        services.AddDbContext<BonusHackerDbContext>();
        
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PageScrapedConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {

                cfg.Host(host, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                
                cfg.ConfigureEndpoints(ctx);
            });
        });
    })
    .Build();

await host.RunAsync();
