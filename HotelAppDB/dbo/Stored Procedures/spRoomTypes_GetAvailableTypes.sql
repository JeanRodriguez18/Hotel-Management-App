﻿CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableTypes]
	@startDate date,
	@endDate date
AS
BEGIN
	
	set nocount on;

	select rt.Id, rt.Title, rt.Description, rt.Price
	from dbo.Rooms r
	inner join dbo.RoomTypes rt on rt.id = r.RoomTypeId
	where r.Id not in (
	select b.RoomId
	from dbo.Bookings b 
	where (@startDate < b.StartDate and @endDate > b.EndDate)
		or (b.StartDate <= @endDate and @endDate < b.EndDate)
		or (b.StartDate <= @startDate and @startDate < b.EndDate)
	)
	group by rt.Id, rt.Title, rt.Description, rt.Price

END
