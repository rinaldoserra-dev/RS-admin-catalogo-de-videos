using MediatR;
using RS.CodeFlix.Catalog.Application.Common;
using RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.ListCategories
{
    public class ListCategoriesInput : PaginatedListInput, IRequest<ListCategoriesOutput>
    {
        public ListCategoriesInput(
            int page = 1, 
            int perPage = 15, 
            string search = "", 
            string sort = "", 
            SearchOrder dir = SearchOrder.Asc) 
            : base(page, perPage, search, sort, dir)
        {
        }
    }
}
