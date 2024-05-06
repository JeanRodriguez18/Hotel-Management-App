CREATE PROCEDURE [dbo].[spBookings_CheckIn]
	@bookingId int
AS
begin
	set nocount on;

	Update dbo.Bookings set CheckedIn = 1
	where Id = @bookingId;

end