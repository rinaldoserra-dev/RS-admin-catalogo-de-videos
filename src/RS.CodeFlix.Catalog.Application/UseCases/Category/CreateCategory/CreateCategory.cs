using RS.CodeFlix.Catalog.Application.Interfaces;
using DomainEntity = RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Domain.Repository;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using RS.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory
{
    public class CreateCategory : Notifiable, ICreateCategory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategory(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModelOutput> Handle(
            CreateCategoryInput input, 
            CancellationToken cancellationToken)
        {
            var category = new DomainEntity.Category(
                input.Name, 
                input.Description, 
                input.IsActive);

            if (category.Invalid)
            {
                AddNotifications(category);
                return null!;
            }
            await _categoryRepository.Insert(category, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return CategoryModelOutput.FromCategory(category);
        }
    }
}
