using RS.CodeFlix.Catalog.Domain.SeedWork;
using RS.CodeFlix.Catalog.Flunt.Validations;

namespace RS.CodeFlix.Catalog.Domain.Entity
{
    public class Category : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Category(string name, string description, bool isActive = true) : base()
        {
            Name = name;
            Description = description;
            IsActive = isActive;
            CreatedAt = DateTime.Now;
            Validate();
        }

        public void Activate()
        {
            IsActive = true;
            Validate();
        }

        public void Deactivate()
        {
            IsActive = false;
            Validate();
        }

        public void Update(string name, string? description = null)
        {
            Name = name;
            Description = description ?? Description;
            Validate();
        }

        private void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNullOrWhiteSpace(Name, "Name_IsNullOrWhiteSpace", $"{nameof(Name)} should not be empty or null")
                .HasMinLen(Name, 3, "Name_HasMinLen", $"{nameof(Name)} should be at leats 3 characters long")
                .HasMaxLen(Name, 255, "Name_HasMaxLen", $"{nameof(Name)} should be less or equal 255 characters long")
                .IsNotNull(Description, "Description_IsNotNull", $"{nameof(Description) } should not be null")
                .HasMaxLen(Description, 10_000, "Description_HasMaxLen", $"{nameof(Description)} should be less or equal 10.000 characters long")
                );
        }
    }
}
