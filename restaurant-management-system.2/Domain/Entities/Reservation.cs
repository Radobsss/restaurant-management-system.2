using restaurant_management_system._2.Domain.Enums;

namespace restaurant_management_system._2.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public int TableId { get; set; }

        public Table? Table { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int GuestCount { get; set; }

        public ReservationStatus Status { get; set; }
    }
}