using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.LLM;
using Industrial_AI_Ops.Core.Ports.RAG;
using Industrial_AI_Ops.Core.Ports.Repository;
using Microsoft.Extensions.Logging;

namespace Industrial_AI_Ops.Infrastructure.AI;

/// <summary>
/// Simple in-memory document retriever for demonstration
/// In production, replace with vector database (Pinecone, Weaviate, etc.)
/// </summary>
public class InMemoryDocumentRetriever : IDocumentRetriever
{
    private readonly ILogger<InMemoryDocumentRetriever> _logger;
    private readonly IKnowledgeRepository _repo;

    public InMemoryDocumentRetriever(ILogger<InMemoryDocumentRetriever> logger, IKnowledgeRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<List<RetrievedChunk>> RetrieveAsync(string query, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Retrieving documents for query: {Query}", query);

            var queryLower = query.ToLower();
            var relevantDocs = (await _repo.GetAllDocuments())
                .Where(doc => doc.Content.ToLower().Contains(queryLower) ||
                             doc.Keywords.Any(k => queryLower.Contains(k.ToLower())))
                .OrderByDescending(doc => CalculateRelevanceScore(doc, queryLower))
                .Take(5)
                .Select(doc => new RetrievedChunk
                {
                    Content = doc.Content,
                    Source = doc.Source
                })
                .ToList();

            _logger.LogInformation("Retrieved {Count} relevant documents", relevantDocs.Count);

            return relevantDocs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving documents for query: {Query}", query);
            return new List<RetrievedChunk>();
        }
    }

    private double CalculateRelevanceScore(Document doc, string query)
    {
        var score = 0.0;
        var queryWords = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var word in queryWords)
        {
            if (doc.Content.ToLower().Contains(word))
                score += 1.0;

            if (doc.Keywords.Any(k => k.ToLower().Contains(word)))
                score += 2.0;
        }

        return score;
    }
}