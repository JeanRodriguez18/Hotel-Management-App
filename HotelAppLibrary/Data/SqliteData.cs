using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SqliteData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private readonly string connectionStringName = "SqliteDb";
        public SqliteData(ISqlDataAccess db)
        {
            _db = db;
        }

        public void BookGuests(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
            throw new NotImplementedException();
        }

        public void CheckInGuest(int bookingId)
        {
            throw new NotImplementedException();
        }

        public List<RoomTypeModel> GetAvailablesRoomTypes(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public RoomTypeModel GetRoomTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public List<FullBookingModel> SearchBookings(string lastName)
        {
            throw new NotImplementedException();
        }
    }
}
