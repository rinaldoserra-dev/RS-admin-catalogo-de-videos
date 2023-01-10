using RS.CodeFlix.Catalog.Application.UseCases.Category.Common;
using RS.CodeFlix.Catalog.Domain.Repository;
using RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using RS.CodeFlix.Catalog.Flunt.Notifications;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.ListCategories
{
    public class ListCategories : Notifiable, IListCategories
    {
        private readonly ICategoryRepository _categoryRepository;

        public ListCategories(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ListCategoriesOutput> Handle(
            ListCategoriesInput request, 
            CancellationToken cancellationToken)
        {
            var input = new SearchInput(
                    request.Page,
                    request.PerPage,
                    request.Search,
                    request.Sort,
                    request.Dir);

            var searchOutput = await _categoryRepository.Search(
                input,
                cancellationToken);

            var output = new ListCategoriesOutput(
                searchOutput.CurrentPage,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items
                    .Select(x => CategoryModelOutput.FromCategory(x)).ToList()
            );

            //var output = new ListCategoriesOutput(
            //    searchOutput.CurrentPage,
            //    searchOutput.PerPage,
            //    searchOutput.Total,
            //    searchOutput.Items
            //        .Select(CategoryModelOutput.FromCategory).ToList()
            //);

            return output;
        }
    }
}
