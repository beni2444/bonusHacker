namespace api;

public record PageScraped
{
    public string Url { get; init; } = "";
    public string? Name { get; init; }
}