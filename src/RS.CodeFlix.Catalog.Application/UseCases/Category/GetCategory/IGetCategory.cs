using MediatR;
using RS.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.GetCategory
{
    public interface IGetCategory: IRequestHandler<GetCategoryInput, CategoryModelOutput> 
    { }
}
