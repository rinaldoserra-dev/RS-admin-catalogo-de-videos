using MediatR;
using RS.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory
{
    public class CreateCategoryInput :IRequest<CategoryModelOutput>
    {
        public CreateCategoryInput(
            string name,
            string? description = null,
            bool isActive = true)
        {
            Name = name;
            Description = description ?? "";
            IsActive = isActive;
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
