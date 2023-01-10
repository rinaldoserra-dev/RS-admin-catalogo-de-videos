using RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Domain.SeedWork;
using RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace RS.CodeFlix.Catalog.Domain.Repository
{
    public interface ICategoryRepository 
        : IGenericRepository<Category>,
        ISearchableRepository<Category>
    {
        
    }
}
