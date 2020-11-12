-- Using the address book services database
use addressBook_services;
-- Adding the column to address book database table with null entries
ALTER TABLE addressBookDatabase
ADD dateOfEntry date;
-- Inserting the entries in the newly added dateOfEntry colum with update query
update addressBookDatabase set dateOfEntry = '2018-05-12' where firstName = 'Raj';
update addressBookDatabase set dateOfEntry = '2019-01-12' where firstName = 'Divya';
update addressBookDatabase set dateOfEntry = '2020-12-01' where firstName = 'Vaivaswat';
update addressBookDatabase set dateOfEntry = '2019-03-12' where firstName = 'Shivam';
---Getting the Data inserted between a date in the query
select * from addressBookDatabase where dateOfEntry between '2018-01-01' and CAST(GETDATE() AS Date );