namespace restaurant_management_system._2.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<MenuItem> MenuItems { get; set; } = new();
    }
}