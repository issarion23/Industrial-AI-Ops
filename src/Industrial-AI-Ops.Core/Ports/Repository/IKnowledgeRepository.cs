using Industrial_AI_Ops.Core.Models;

namespace Industrial_AI_Ops.Core.Ports.Repository;

public interface IKnowledgeRepository
{
    Task<List<Document>> GetAllDocuments();
}