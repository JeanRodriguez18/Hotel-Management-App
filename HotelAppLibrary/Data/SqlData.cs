﻿using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
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


    }
}
