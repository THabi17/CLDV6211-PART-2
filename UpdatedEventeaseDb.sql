use master;
create database EvantEaseDb;
use EvantEaseDb;


create table Venue(
Venue_ID int identity (1,1) primary key not null,
VenueName nvarchar(250) not null,
location nvarchar(250) not null,
capacity int not null CHECK (Capacity > 0),
imageURL varchar(250)
);


create table Event(
Event_ID int identity (1,1) primary key not null,
Venue_ID int null,
EventName nvarchar(250) not null,
EventDate DATETIME DEFAULT GETDATE(),
Description nvarchar(250),
Foreign Key (Venue_ID )  references Venue (Venue_ID) ON DELETE SET NULL
);


create table Booking (
Booking_ID int identity (1,1) primary key not null,
Venue_ID int not null,
Event_ID int not null,
BookingDate DATETIME DEFAULT GETDATE(),
Foreign Key (Venue_ID )  references Venue (Venue_ID) ON DELETE CASCADE,
Foreign Key (Event_ID )  references Event (Event_ID) ON DELETE CASCADE,
CONSTRAINT UQ_Venue_Event UNIQUE (Venue_ID, Event_ID) 
);

CREATE UNIQUE INDEX UQ_Venue_Booking ON Booking (Venue_ID, BookingDate);

Insert into Event(Venue_ID,EventName,Description, EventDate) 
values (1,'One men Show','Show','2025-10-17 13:00:00'),
	   (2,'Painohub','show','2025-04-07 14:00:00');
Select * 
From Event ;


Insert into Venue(VenueName, location,capacity ,imageURL)
Values ('One men Show ','Kibler Park','250','https://eventstoragepart2.blob.core.windows.net/eventcontainer/music%20festival.webp'),
		('Waterstone','Zola','150','https://eventstoragepart2.blob.core.windows.net/eventcontainer/olympics.jpg');

Select *
from Venue


Insert into Booking (Venue_ID,Event_ID,BookingDate)
	Values (1,1,'2025-01-11 13:00:00'),
	       (2,2,'2025-11-11 14:00:00')

		
Select *
From  Booking;







