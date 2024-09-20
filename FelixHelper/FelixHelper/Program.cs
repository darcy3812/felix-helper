using FelixHelper;
using FelixHelper.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using Telegram.Bot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<BotConfiguration>(context.Configuration.GetSection("BotConfiguration"));
        services.Configure<HostingConfiguration>(context.Configuration.GetSection("HostingConfiguration"));

        services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration botConfiguration = sp.GetService<IOptions<BotConfiguration>>()?.Value;
                    ArgumentNullException.ThrowIfNull(botConfiguration);
                    TelegramBotClientOptions options = new(botConfiguration.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddScoped<FtpService>();
        services.AddScoped<PromotionService>();
        services.AddHostedService<PollingService>();
    })
    .Build();

await host.RunAsync();
