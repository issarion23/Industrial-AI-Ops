using Industrial_AI_Ops.Core.Models.LLM;
using OpenAI.Chat;

namespace Industrial_AI_Ops.Infrastructure.AI;

public static class OpenAiRequestBuilder
{
    public static List<ChatMessage> Convert(List<Message> models)
    {
        if (models == null) return new List<ChatMessage>();

        return models
            .Where(m => m != null)
            .Select<Message, ChatMessage>(m => m.Role switch
            {
                ChatRole.Assistant => new AssistantChatMessage(m.Content),
                ChatRole.User => new UserChatMessage(m.Content),
                ChatRole.System => new SystemChatMessage(m.Content),
                _ => throw new ArgumentOutOfRangeException()
            })
            .ToList();
    }
}