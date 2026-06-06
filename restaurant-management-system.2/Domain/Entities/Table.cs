namespace restaurant_management_system._2.Domain.Entities
{
    public class Table
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int Capacity { get; set; }

        public string Location { get; set; } = string.Empty;

        public bool IsOccupied { get; set; }

        public bool IsReserved { get; set; }

        public string? ReservedBy { get; set; }
    }
}