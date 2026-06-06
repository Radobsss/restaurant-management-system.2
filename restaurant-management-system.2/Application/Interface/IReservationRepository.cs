using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Application.Interface
{
    public interface IReservationRepository
    {
        List<Reservation> GetAll();

        Reservation? GetById(int id);

        List<Reservation> GetByTableId(int tableId);

        void Add(Reservation reservation);

        void Update(Reservation reservation);
    }
}