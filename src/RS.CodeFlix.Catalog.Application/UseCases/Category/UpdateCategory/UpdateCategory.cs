using MediatR;
using RS.CodeFlix.Catalog.Application.Interfaces;
using RS.CodeFlix.Catalog.Application.UseCases.Category.Common;
using RS.CodeFlix.Catalog.Domain.Repository;
using RS.CodeFlix.Catalog.Flunt.Notifications;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategory : Notifiable, IUpdateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModelOutput> Handle(UpdateCategoryInput request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Get(request.Id, cancellationToken);

            if (category == null)
            {
                AddNotification("Category", $"Category {request.Id} not found");
                return null!;
            }

            category.Update(request.Name, request.Description);
            if (category.Invalid)
            {
                AddNotifications(category.Notifications);
                return null!;
            }
            if (request.IsActive != null && request.IsActive != category.IsActive) 
            {
                if ((bool)request.IsActive!) category.Activate();
                else category.Deactivate();
            }
            if(category.Invalid)
            {
                AddNotifications(category.Notifications);
                return null!;
            }
            await _categoryRepository.Update(category, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return CategoryModelOutput.FromCategory(category);
        }
    }
}
