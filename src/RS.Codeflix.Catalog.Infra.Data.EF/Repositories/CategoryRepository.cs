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

        public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
        {
            //return await _categories.FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
            return await _categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id ==  id, cancellationToken);
        }
        public Task Update(Category aggregate, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Entry(aggregate).State = EntityState.Modified);
            //return Task.FromResult(_categories.Update(aggregate)); 
        }

        public Task Delete(Category aggregate, CancellationToken cancellationToken)
        {
            return Task.FromResult(_categories.Remove(aggregate));
        }

        public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
