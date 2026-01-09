namespace Industrial_AI_Ops.Core.Models;

public class Document
{
    public int Id { get; set; }
    public string Source { get; set; }
    public string Content { get; set; }
    public string[] Keywords { get; set; }
    public string Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}