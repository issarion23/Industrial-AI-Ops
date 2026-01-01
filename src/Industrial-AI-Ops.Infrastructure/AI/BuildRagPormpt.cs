using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.RAG;

namespace Industrial_AI_Ops.Infrastructure.AI;

public static class PromptBuilder
{
    public static string BuildRagPrompt(string question, List<RetrievedChunk> context)
    {
        var docs = string.Join("\n\n---\n\n",
            context.Select(c => $"SOURCE: {c.Source}\n{c.Content}"));

        return
            $"""
             Ты — корпоративный ассистент. Отвечай только на основе предоставленных документов.
             Если ответа НЕТ в документах — скажи: "Нет данных для точного ответа."
             Не выдумывай. Не фантазируй. Обязательно опирайся на факты.
             Если возможно — укажи источники в формате: [SourceName].

             Документы:
             {docs}

             Вопрос:
             {question}
             """;
    }
}
