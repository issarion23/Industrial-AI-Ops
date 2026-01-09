using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Industrial_AI_Ops.Infrastructure.Repository;

public class KnowledgeRepository : IKnowledgeRepository
{
    private readonly AppDbContext _dbContext;
    
    public KnowledgeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Document>> GetAllDocuments()
    {
        var documents = await _dbContext.Documents.Where(d => d.IsActive).ToListAsync();

        return documents;
    }
}