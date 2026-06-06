using restaurant_management_system._2.Domain.Enums;

namespace restaurant_management_system._2.Domain.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public MenuItemType Type { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}