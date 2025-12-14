using api.Data;
using api.Data.Config;
using MassTransit;

namespace api;

public class PageScrapedConsumer : IConsumer<PageScraped>
{
    private BonusHackerDbContext _bonusHackerDbContext;
    private ILogger<PageScrapedConsumer> _logger;
    public PageScrapedConsumer(BonusHackerDbContext bonusHackerDbContext, ILogger<PageScrapedConsumer> logger)
    {
        _bonusHackerDbContext = bonusHackerDbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PageScraped> context)
    {
        var product = new ScrapedProduct
        {
            Id = Guid.NewGuid(),
            Url = context.Message.Url,
            Name = context.Message.Name,

        };
        _bonusHackerDbContext.ScrapedProducts.Add(product);
        await _bonusHackerDbContext.SaveChangesAsync();
        _logger.LogInformation("added scraped product to database with name: {0} id: {1} ", product.Name, product.Id);
    }
}