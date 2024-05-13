using HotelAppLibrary.Databases;
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
            string sql = @"select 1 from Guests where FirstName = @firstName and LastName = @lastName";

            int result = _db.LoadData<dynamic, dynamic>(sql, new { firstName, lastName }, connectionStringName).Count();

            if (result == 0)
            {
                sql = @"insert into Guests(FirstName, LastName)
		                values (@firstName, @lastName);";

                _db.SaveData(sql, new { firstName, lastName }, connectionStringName);
            }

            sql = @"select [Id], [FirstName], [LastName] 
	                from Guests
	                where FirstName = @firstName and LastName = @lastName LIMIT 1;";


            GuestModel guest = _db.LoadData<GuestModel, dynamic>(sql, new {firstName, lastName}, connectionStringName).First();

            sql = @"select * from RoomTypes where Id = @roomTypeId";

            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>(sql, new {roomTypeId}, connectionStringName).First();

            TimeSpan timeStaying = endDate.Date.Subtract(startDate);

            sql = @"select r.*
	                from Rooms r
	                inner join RoomTypes rt on rt.id = r.RoomTypeId
	                where r.RoomTypeId = @roomTypeId 
	                and r.Id not in (
	                select b.RoomId
	                from Bookings b 
	                where (@startDate < b.StartDate and @endDate > b.EndDate)
		                or (b.StartDate <= @endDate and @endDate < b.EndDate)
		                or (b.StartDate <= @startDate and @startDate < b.EndDate)
	                );";

            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(sql, new { startDate, endDate, roomTypeId }, connectionStringName);


            sql = @"insert into Bookings(RoomId, GuestId, StartDate, EndDate, TotalPrice)
	                values(@roomId, @guestId, @startDate, @endDate, @totalPrice);";

            _db.SaveData(sql, new { roomId = availableRooms.First().Id, guestId = guest.Id, startDate = startDate, endDate = endDate, totalPrice = roomType.Price * timeStaying.Days }, connectionStringName);
        }

        public void CheckInGuest(int bookingId)
        {

            string sql = @"Update Bookings
                            set CheckedIn = 1
                            where Id = @bookingId;";

            _db.SaveData(sql, new { bookingId }, connectionStringName);
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
            string sql = @"select [Id], [Title], [Description], [Price]
	                        from RoomTypes
	                        where Id = @id;";

            return _db.LoadData<RoomTypeModel, dynamic>(sql, new { id }, connectionStringName).FirstOrDefault();
        }

        public List<FullBookingModel> SearchBookings(string lastName)
        {
            string sql = @"select [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], [b].[EndDate], [b].[CheckedIn], [b].[TotalPrice], 
	                        [g].[FirstName], [g].[LastName], 
	                        [r].[RoomNumber], [r].[RoomTypeId], 
	                        [rt].[Title], [rt].[Description], [rt].[Price] 
	                        from Bookings b
	                        inner join Guests g on b.GuestId = g.Id
	                        inner join Rooms r on b.RoomId = r.Id
	                        inner join RoomTypes rt on r.RoomTypeId = rt.Id 
	                        where g.LastName = @lastName and b.StartDate = @todaysDate";

            DateTime todaysDate = DateTime.Now.Date;
           
            
            var output = _db.LoadData<FullBookingModel, dynamic>(sql, new {lastName, todaysDate}, connectionStringName).ToList();

            output.ForEach(x=>
            {
                x.Price = x.Price / 100;
                x.TotalPrice = x.TotalPrice / 100;
            });

            return output;


        }
    }
}
