using Microsoft.EntityFrameworkCore;
using RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Domain.Repository;
using RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace RS.Codeflix.Catalog.Infra.Data.EF.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CodeflixCatalogDbContext _context;
        private DbSet<Category> _categories => _context.Set<Category>();

        public CategoryRepository(CodeflixCatalogDbContext context)
        {
            _context = context;
        }

        public async Task Insert(Category aggregate, CancellationToken cancellationToken)
        {
            //await _context.Categories.AddAsync(aggregate, cancellationToken);
            await _categories.AddAsync(aggregate, cancellationToken);
        }
        public Task Delete(Category aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Category> Get(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Update(Category aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
