using MediatR;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using RS.CodeFlix.Catalog.Flunt.Validations;

namespace RS.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategoryInput : Notifiable, IRequest
    {
        public DeleteCategoryInput(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
               .Requires()
               .IsNotEmpty(Id, "Id", "O Id é obrigatório")
           );
        }
    }
}
