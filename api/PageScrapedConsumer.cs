using MassTransit;

namespace api;

public class PageScrapedConsumer : IConsumer<PageScraped>
{
    public async Task Consume(ConsumeContext<PageScraped> context)
    {
        var message = context.Message;
        Console.WriteLine($"Page scraped: {message.Url}");
        
        await Task.CompletedTask;
    }
}