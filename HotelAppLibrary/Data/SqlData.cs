using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SqlData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDB";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public List<RoomTypeModel> GetAvailablesRoomTypes(DateTime startDate, DateTime endDate)
        {
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableTypes", new {startDate, endDate}, connectionStringName, true);
        }


        public void BookGuests(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {

            GuestModel guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuests_Insert", new { firstName, lastName }, connectionStringName, true).First();


            RoomTypeModel roomtype = _db.LoadData<RoomTypeModel, dynamic>("select * from dbo.roomTypes where Id = @Id", new { Id = roomTypeId }, connectionStringName, false).First();

            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("dbo.spRoom_GetAvailableRooms", new {startDate, endDate, roomTypeId }, connectionStringName, true);

            _db.SaveData("dbo.spBookings_Insert", new { roomId = availableRooms.First().Id, guestId = guest.Id, startDate = startDate, endDate = endDate, totalPrice = timeStaying.Days * roomtype.Price }, connectionStringName, true);

        }

        public List<FullBookingModel> SearchBookings(string lastName)
        {
            return _db.LoadData<FullBookingModel, dynamic>("dbo.spBookings_Search", new { lastName, todaysDate = DateTime.Now.Date }, connectionStringName, true);
        }

    }
}
