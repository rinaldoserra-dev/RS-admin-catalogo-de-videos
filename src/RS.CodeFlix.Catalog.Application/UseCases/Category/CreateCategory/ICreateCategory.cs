using MediatR;
using RS.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory :IRequestHandler<CreateCategoryInput, CategoryModelOutput>
    { }
}
