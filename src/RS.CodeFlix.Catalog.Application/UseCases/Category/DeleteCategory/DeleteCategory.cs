using MediatR;
using RS.CodeFlix.Catalog.Application.Interfaces;
using RS.CodeFlix.Catalog.Domain.Repository;
using RS.CodeFlix.Catalog.Flunt.Notifications;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategory : Notifiable, IDeleteCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Invalid)
            {
                AddNotifications(request);
                return Unit.Value;
            }

            var category = await _categoryRepository.Get(request.Id, cancellationToken);

            if (category == null)
            {
                AddNotification("Category", $"Category {request.Id} not found");
                return Unit.Value;
            }

            await _categoryRepository.Delete(category, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return Unit.Value;
        }
    }
}
