using api.Data;
using api.Data.Config;
using MassTransit;

namespace api;

public class PageScrapedConsumer : IConsumer<PageScraped>
{
    private BonusHackerDbContext _bonusHackerDbContext;

    public PageScrapedConsumer(BonusHackerDbContext bonusHackerDbContext)
    {
        _bonusHackerDbContext = bonusHackerDbContext;
    }

    public async Task Consume(ConsumeContext<PageScraped> context)
    {
        _bonusHackerDbContext.ScrapedProducts.Add(new ScrapedProduct
        {
            Id = Guid.NewGuid(),
            Url = context.Message.Url,
            Name = context.Message.Name,

        });
        await _bonusHackerDbContext.SaveChangesAsync();
        var message = context.Message;
        Console.WriteLine($"Page scraped: {message.Url}");

    }
}