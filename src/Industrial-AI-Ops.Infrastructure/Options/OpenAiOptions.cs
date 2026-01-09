namespace Industrial_AI_Ops.Infrastructure.Options;

public class OpenAiOptions
{
    public const string SectionName = nameof(OpenAiOptions);

    public string ApiKey { get; set; }

    public string Model { get; set; }
}