/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if not exists (select 1 from dbo.RoomTypes)
BEGIN

    insert into dbo.RoomTypes(Title, Description, Price)
    values ('King Size Bed', 'A room with a king-size bed and a window', 100),
    ('Two Queen Size Beds', 'A room with two queen-size beds and a window', 115),
    ('Executive Suite', 'Two rooms, each with a king-size bed and a window', 205)

END


if not exists (select 1 from dbo.Rooms)
BEGIN
    
    DECLARE @RoomTypedId1 int;
    DECLARE @RoomTypedId2 int;
    DECLARE @RoomTypedId3 int;


    select @RoomTypedId1 = Id from dbo.RoomTypes where Title = 'King Size Bed'
    select @RoomTypedId2 = Id from dbo.RoomTypes where Title = 'Two Queen Size Beds'
    select @RoomTypedId3 = Id from dbo.RoomTypes where Title = 'Executive Suite'


    insert into dbo.Rooms(RoomNumber, RoomTypeId)
    values 
    ('101', @RoomTypedId1),
    ('102', @RoomTypedId1),
    ('103', @RoomTypedId1),
    ('201', @RoomTypedId2),
    ('202', @RoomTypedId2),
    ('301', @RoomTypedId3)



END