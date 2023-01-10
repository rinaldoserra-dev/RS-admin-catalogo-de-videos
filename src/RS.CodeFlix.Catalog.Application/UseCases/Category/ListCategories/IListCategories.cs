using MediatR;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.ListCategories
{
    public interface IListCategories 
        : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
    {
    }
}
