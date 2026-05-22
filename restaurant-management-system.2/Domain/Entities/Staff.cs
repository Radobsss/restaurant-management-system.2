using restaurant_management_system._2.Domain.Enums;

namespace restaurant_management_system._2.Domain.Entities
{
    public class Staff
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public StaffRole Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}