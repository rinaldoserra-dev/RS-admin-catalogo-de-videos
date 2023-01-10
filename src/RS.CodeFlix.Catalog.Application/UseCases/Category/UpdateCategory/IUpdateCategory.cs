using MediatR;
using RS.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory
{
    public interface IUpdateCategory 
        : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
    {
    }
}
