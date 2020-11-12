create procedure dbo.SpAddcontactRecords
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
	@bookName varchar(50)	
	)
	as begin
	Insert into addressBookDatabase values(@fname,@sname,@address,@city,@state,@zip,@phoneNo,@email,@type,@bookName)
	End