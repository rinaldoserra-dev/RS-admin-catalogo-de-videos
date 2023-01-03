using RS.CodeFlix.Catalog.Flunt.Notifications;

namespace RS.CodeFlix.Catalog.Domain.SeedWork
{
    public abstract class Entity: Notifiable
    {
        public Guid Id { get; private set; }
        protected Entity() => Id = Guid.NewGuid();
        
    }
}
