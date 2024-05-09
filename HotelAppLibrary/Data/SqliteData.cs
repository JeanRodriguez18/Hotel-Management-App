﻿using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SqliteData : IDatabaseData
    {
        private readonly ISqliteDataAccess _db;
        private readonly string connectionStringName = "SqliteDb";
        public SqliteData(ISqliteDataAccess db)
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
            string sql = @"select rt.Id, rt.Title, rt.Description, rt.Price
	                    from Rooms r
	                    inner join RoomTypes rt on rt.id = r.RoomTypeId
	                    where r.Id not in (
	                    select b.RoomId
	                    from Bookings b 
	                    where (@startDate < b.StartDate and @endDate > b.EndDate)
		                    or (b.StartDate <= @endDate and @endDate < b.EndDate)
		                    or (b.StartDate <= @startDate and @startDate < b.EndDate)
	                    )
	                    group by rt.Id, rt.Title, rt.Description, rt.Price";

            var output = _db.LoadData<RoomTypeModel, dynamic>(sql, new { startDate, endDate }, connectionStringName).ToList();


            output.ForEach(x => x.Price = x.Price / 100);

            return output;
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
