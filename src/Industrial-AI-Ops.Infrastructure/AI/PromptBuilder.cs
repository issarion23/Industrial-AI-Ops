namespace Industrial_AI_Ops.Infrastructure.AI;

public static class PromptBuilder
{
    public static string BuildRagPrompt()
    {
        return
            """
             Ты — корпоративный ассистент. Отвечай только на основе предоставленных документов.
             Если ответа НЕТ в документах — скажи: "Нет данных для точного ответа."
             Не выдумывай. Не фантазируй. Обязательно опирайся на факты.
             Если возможно — укажи источники в формате: [SourceName].
             """;
    }
}
