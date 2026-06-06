using System;
using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Domain.Enums;

namespace restaurant_management_system._2.Service
{
    public class ReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly ITableRepository tableRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            ITableRepository tableRepository)
        {
            this.reservationRepository = reservationRepository;
            this.tableRepository = tableRepository;
        }

        public List<Reservation> GetAllReservations()
        {
            return reservationRepository.GetAll()
                .OrderBy(r => r.StartTime)
                .ToList();
        }

        public List<Reservation> GetReservationsByTable(int tableNumber)
        {
            Table? table = tableRepository.GetByNumber(tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            return reservationRepository.GetByTableId(table.Id)
                .OrderBy(r => r.StartTime)
                .ToList();
        }

        public Reservation CreateReservation(
    int tableNumber,
    string customerName,
    int guestCount,
    DateTime startTime,
    DateTime endTime)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name cannot be empty.");

            if (guestCount <= 0)
                throw new ArgumentException("Guest count must be greater than 0.");

            if (startTime >= endTime)
                throw new ArgumentException("Start time must be before end time.");

            if (startTime < DateTime.Now)
                throw new ArgumentException("Reservation cannot be in the past.");

            Table? table = tableRepository.GetByNumber(tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (table.IsOccupied)
                throw new ArgumentException("Occupied table cannot be reserved.");

            if (table.IsReserved)
                throw new ArgumentException("Table is already reserved.");

            if (guestCount > table.Capacity)
                throw new ArgumentException("Guest count is greater than table capacity.");

            bool hasConflict = reservationRepository
                .GetByTableId(table.Id)
                .Any(r =>
                    r.Status != ReservationStatus.Cancelled &&
                    startTime < r.EndTime &&
                    endTime > r.StartTime);

            if (hasConflict)
                throw new ArgumentException("This table already has a reservation for this time.");

            Reservation reservation = new Reservation
            {
                TableId = table.Id,
                CustomerName = customerName.Trim(),
                GuestCount = guestCount,
                StartTime = startTime,
                EndTime = endTime,
                Status = ReservationStatus.Confirmed
            };

            table.IsReserved = true;
            table.IsOccupied = false;
            table.ReservedBy = customerName.Trim();

            reservationRepository.Add(reservation);
            tableRepository.Update(table);

            return reservation;
        }

        public Reservation CancelReservation(int reservationId)
        {
            Reservation? reservation = reservationRepository.GetById(reservationId);

            if (reservation == null)
                throw new ArgumentException("Reservation not found.");

            if (reservation.Status == ReservationStatus.Cancelled)
                throw new ArgumentException("Reservation is already cancelled.");

            reservation.Status = ReservationStatus.Cancelled;
            reservationRepository.Update(reservation);

            Table? table = tableRepository.GetById(reservation.TableId);

            if (table != null)
            {
                bool hasOtherActiveReservations = reservationRepository
                    .GetByTableId(table.Id)
                    .Any(r =>
                        r.Id != reservation.Id &&
                        r.Status != ReservationStatus.Cancelled);

                if (!hasOtherActiveReservations && !table.IsOccupied)
                {
                    table.IsReserved = false;
                    tableRepository.Update(table);
                }
            }

            return reservation;
        }
    }
}