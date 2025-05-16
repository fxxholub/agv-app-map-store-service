namespace AgvAppMapStoreService.Core;

public class Map
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SvgContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}