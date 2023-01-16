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

        public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            var toSkip = (input.Page - 1) * input.PerPage;
            var query = _categories.AsNoTracking();
            query = AddOrderToQuery(query, input.OrderBy, input.Order);
            
            if (!String.IsNullOrWhiteSpace(input.Search))
                query = query.Where(x => x.Name.Contains(input.Search));

            var total = await query.CountAsync();
            var items = await query
                .Skip(toSkip)
                .Take(input.PerPage)
                .ToListAsync();

            return new SearchOutput<Category>(input.Page, input.PerPage, total, items);
        }

        private IQueryable<Category> AddOrderToQuery(
            IQueryable<Category> query,
            string orderProperty,
            SearchOrder order
        )
        {
            return (orderProperty.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderBy(x => x.Name)
            };
        }
    }
}
