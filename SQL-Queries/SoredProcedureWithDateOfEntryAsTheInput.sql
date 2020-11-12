create procedure dbo.SpAddcontactRecordsWithDateOfEntry
(	
	@fname	varchar(50),
	@sname	varchar(50),		
	@address varchar(50),		
	@city varchar(50),		
	@state	varchar(50),	
	@zip   bigint,		
	@phoneNo   bigint,
	@email varchar(50),
	@type varchar(50),	
	@bookName varchar(50),
	@entryDate date
)
	as begin
	Insert into addressBookDatabase values(@fname,@sname,@address,@city,@state,@zip,@phoneNo,@email,@type,@bookName, @entryDate)
	End
-- Testing the stored procedure to adding the data to the address book using the inputs
use addressBook_services;
select * from addressBookDatabase;
EXEC SpAddcontactRecordsWithDateOfEntry @fname='Alka', @sname ='Rai', @address ='Sec-1', @city = 'Gorakhpur', @state = 'UP', @zip = 276123, @phoneNo = 98784565,
@email = 'alka@gmail.com', @type = 'Family', @bookName = 'PraveenRecord', @entryDate = '2018-06-09';