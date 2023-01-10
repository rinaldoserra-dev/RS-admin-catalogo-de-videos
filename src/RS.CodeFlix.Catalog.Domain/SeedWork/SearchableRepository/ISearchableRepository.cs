namespace RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository
{
    public interface ISearchableRepository<TAggreagate>
        where TAggreagate : AggregateRoot
    {
        Task<SearchOutput<TAggreagate>> Search(
            SearchInput input, 
            CancellationToken cancellationToken);
    }
}
