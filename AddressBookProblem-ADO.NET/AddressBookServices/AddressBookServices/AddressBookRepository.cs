// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddressBookRepository.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Praveen Kumar Upadhyay"/>
// --------------------------------------------------------------------------------------------------------------------
namespace AddressBookServices
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.Transactions;
    /// <summary>
    /// Class to execute the ado.net query implementation on the underlying sql database
    /// Using the Data.SqlClient package to establish connections
    /// Using Sql Connection as records are limited in number
    /// </summary>
    public class AddressBookRepository
    {
        // <summary>
        /// For ensuring the established connection using the Sql Connection specifying the property
        /// </summary>
        public static SqlConnection connectionToServer { get; set; }
        /// <summary>
        /// Method to check whether the connection is properly initialised or not
        /// </summary>
        public void EnsureDataBaseConnection()
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            using (connectionToServer)
            {
                Console.WriteLine("The Connection is created");
            }
            connectionToServer.Close();
        }
        /// <summary>
        /// UC1-- Getting all the stored records in the address book service table by fetching all the records
        /// </summary>
        public void GetAllRecords()
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            /// Creating the address book model class object
            AddressBookModel bookModel = new AddressBookModel();
            try
            {
                using (connectionToServer)
                {
                    /// Query to get all the data from the table
                    string query = @"select * from addressBookDatabase";
                    /// Impementing the command on the connection fetched database table

                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Opening the connection to start mapping
                    connectionToServer.Open();
                    /// executing the sql data reader to fetch the records
                    SqlDataReader reader = command.ExecuteReader();
                    /// executing for not null
                    if (reader.HasRows)
                    {
                        /// Moving to the next record from the table
                        /// Mapping the data to the employee model class object
                        while (reader.Read())
                        {
                            bookModel.firstName = reader.GetString(0);
                            bookModel.secondName = reader.GetString(1);
                            bookModel.address = reader.GetString(2);
                            bookModel.city = reader.GetString(3);
                            bookModel.state = reader.GetString(4);
                            bookModel.zip = reader.GetInt64(5);
                            bookModel.phoneNumber = reader.GetInt64(6);
                            bookModel.emailId = reader.GetString(7);
                            bookModel.contactType = reader.GetString(8);
                            bookModel.addressBookName = reader.GetString(9);
                            bookModel.DateOfEntry = reader.GetDateTime(10);
                            Console.WriteLine($"First Name:{bookModel.firstName}\nSecond Name:{bookModel.secondName}\n" +
                                $"Address:{bookModel.address}, {bookModel.city}, {bookModel.state} PinCode: {bookModel.zip}\n" +
                                $"Phone Number: {bookModel.phoneNumber}\nContact Type: {bookModel.contactType}\nAddress Book Name : {bookModel.addressBookName}" +
                                $"Date Of Entry in the Address Book: { bookModel.DateOfEntry}");
                            Console.WriteLine("\n\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }
                    reader.Close();
                }
            }
            /// Catching the null record exception
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            /// Alway ensuring the closing of the connection
            finally
            {
                connectionToServer.Close();
            }
        }
        /// <summary>
        /// UC2 -- Method to insert contact to the table using a stored procedure
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddDataToTable(AddressBookModel model)
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            try
            {
                /// Using the connection established
                using (connectionToServer)
                {
                    /// Implementing the stored procedure
                    SqlCommand command = new SqlCommand("SpAddcontactRecords", connectionToServer);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@fname", model.firstName);
                    command.Parameters.AddWithValue("@sname", model.secondName);
                    command.Parameters.AddWithValue("@address", model.address);
                    command.Parameters.AddWithValue("@city", model.city);
                    command.Parameters.AddWithValue("@state", model.state);
                    command.Parameters.AddWithValue("@zip", model.zip);
                    command.Parameters.AddWithValue("@phoneNo", model.phoneNumber);
                    command.Parameters.AddWithValue("@email", model.emailId);
                    command.Parameters.AddWithValue("@type", model.contactType);
                    command.Parameters.AddWithValue("@bookName", model.addressBookName);
                    /// Opening the connection
                    connectionToServer.Open();
                    var result = command.ExecuteNonQuery();
                    connectionToServer.Close();
                    /// Return the result of the transaction i.e. the dml operation to update data
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            /// Catching any type of exception generated during the run time
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connectionToServer.Close();
            }
        }
        /// <summary>
        /// UC3 -- Method to update the contact type or address book name of a contact when name passed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newData"></param>
        /// <param name="choice"></param>
        /// <returns></returns>
        public bool EditContactUsingName(string name, string newData, int choice)
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            try
            {
                /// Using the connection established
                using (connectionToServer)
                {
                    string query = "";
                    /// Opening the connection
                    connectionToServer.Open();
                    /// Update query  for the table and binding with the parameter passed
                    if (choice == 1)
                    {
                        query = @"update dbo.addressBookDatabase set contactType = @parameter1
                    where firstName = @parameter2";
                    }
                    else if (choice == 2)
                    {
                        query = @"update dbo.addressBookDatabase set addressBookName= @parameter1
                   where firstName = @parameter2";
                    }
                    /// Impementing the command on the connection fetched database table
                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Binding the parameter to the formal parameters
                    command.Parameters.AddWithValue("@parameter1", newData);
                    command.Parameters.AddWithValue("@parameter2", name);
                    /// Storing the result of the executed query
                    var result = command.ExecuteNonQuery();
                    connectionToServer.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            /// Catching any type of exception generated during the run time
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connectionToServer.Close();
            }
        }
        /// <summary>
        /// UC4 -- Method to update the contact type or address book name of a contact when name passed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newData"></param>
        /// <param name="choice"></param>
        /// <returns></returns>
        public bool DeleteContactUsingName(string firstName)
        {
            string name;
            if (firstName == null)
            {
                Console.WriteLine("Enter the first name whose contact you want to delete :");
                name = Console.ReadLine();
            }
            else
            {
                name = firstName;
            }
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            try
            {
                /// Using the connection established
                using (connectionToServer)
                {

                    /// Opening the connection
                    connectionToServer.Open();
                    /// Update query  for the table and binding with the parameter passed
                    string query = "delete from dbo.addressBookDatabase where firstName = @parameter1";
                    /// Impementing the command on the connection fetched database table
                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Binding the parameter to the formal parameters
                    command.Parameters.AddWithValue("@parameter1", name);
                    /// Storing the result of the executed query
                    var result = command.ExecuteNonQuery();
                    connectionToServer.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            /// Catching any type of exception generated during the run time
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connectionToServer.Close();
            }
        }
        /// <summary>
        /// UC5 -- Function to get the detail of the records belonging to a city or state
        /// </summary>
        /// <param name="gender"></param>
        public void GetTheDetailOfRecordForCityOrState(string data, int choice, int property)
        {
            AddressBookModel bookModel = new AddressBookModel();
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            string query = "";
            try
            {
                /// Using the connection established
                using (connectionToServer)
                {
                    if (choice == 1)
                    {
                        if (property == 1)
                        {
                            /// Query to get the data from the table
                            query = @"select * from dbo.addressBookDatabase
                                   where city=@parameter";
                        }
                        else
                        {
                            query = @"SELECT t.contactId,t.firstName, t.secondName, t.phoneNumber, t.emailId, 
                                     b.addressBookName, d.address, d.city, d.state, d.zip, c.contactType
                                     FROM addressBook_Table t join address_Book b ON t.contactId = b.contactId
                                     join address_Detail d ON t.contactId = d.contactId 
                                     join contact_type c on t.contactId = c.contactId
                                     where d.city = @parameter";
                        }
                    }
                    else if (choice == 2)
                    {
                        if (property == 1)
                        {
                            // Query to get the data from the table
                            query = @"select * from dbo.addressBookDatabase
                                   where state=@parameter";
                        }
                        else
                        {
                            query = @"SELECT t.contactId,t.firstName, t.secondName, t.phoneNumber, t.emailId, 
                                     b.addressBookName, d.address, d.city, d.state, d.zip, c.contactType
                                     FROM addressBook_Table t join address_Book b ON t.contactId = b.contactId
                                     join address_Detail d ON t.contactId = d.contactId 
                                     join contact_type c on t.contactId = c.contactId
                                     where d.state = @parameter";
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong Choice....");
                    }
                    /// Impementing the command on the connection fetched database table
                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Binding the parameter to the formal parameters
                    command.Parameters.AddWithValue("@parameter", data);
                    /// Opening the connection to start mapping
                    connectionToServer.Open();
                    /// executing the sql data reader to fetch the records
                    SqlDataReader reader = command.ExecuteReader();
                    /// executing for not null
                    if (reader.HasRows)
                    {
                        /// Moving to the next record from the table
                        /// Mapping the data to the retrieved data from executing the query on the table
                        while (reader.Read())
                        {
                            if (property == 1)
                            {
                                bookModel.firstName = reader.GetString(0);
                                bookModel.secondName = reader.GetString(1);
                                bookModel.address = reader.GetString(2);
                                bookModel.city = reader.GetString(3);
                                bookModel.state = reader.GetString(4);
                                bookModel.zip = reader.GetInt64(5);
                                bookModel.phoneNumber = reader.GetInt64(6);
                                bookModel.emailId = reader.GetString(7);
                                bookModel.contactType = reader.GetString(8);
                                bookModel.addressBookName = reader.GetString(9);
                                Console.WriteLine($"First Name:{bookModel.firstName}\nSecond Name:{bookModel.secondName}\n" +
                                    $"Address:{bookModel.address}, {bookModel.city}, {bookModel.state} PinCode: {bookModel.zip}\n" +
                                    $" Phone Number: {bookModel.phoneNumber}\n Contact Type: {bookModel.contactType}\n Address Book Name : {bookModel.addressBookName}");
                                Console.WriteLine("\n\n");
                            }
                            else
                            {
                                bookModel.contactID = reader.GetInt32(0);
                                bookModel.firstName = reader.GetString(1);
                                bookModel.secondName = reader.GetString(2);
                                bookModel.phoneNumber = reader.GetInt64(3);
                                bookModel.emailId = reader.GetString(4);
                                bookModel.addressBookName = reader.GetString(5);
                                bookModel.address = reader.GetString(6);
                                bookModel.city = reader.GetString(7);
                                bookModel.state = reader.GetString(8);
                                bookModel.ZIP = reader.GetInt32(9);
                                bookModel.contactType = reader.GetString(10);

                                Console.WriteLine($"First Name:{bookModel.firstName}\nSecond Name:{bookModel.secondName}\n" +
                                    $"Address:{bookModel.address}, {bookModel.city}, {bookModel.state} PinCode: {bookModel.ZIP}\n" +
                                    $"Phone Number: {bookModel.phoneNumber}\nContact Type: {bookModel.contactType}\nAddress Book Name : {bookModel.addressBookName}");
                                Console.WriteLine("\n\n");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data found");
                    }
                    reader.Close();
                }
            }
            /// Catching any type of exception generated during the run time
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connectionToServer.Close();
            }
        }
        /// <summary>
        /// UC6 -- Function to Get the count of the records stored inside the address book database 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="choice"></param>
        public void GetCountOfCityOrState(string data, int choice, int property)
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            string query = "";
            try
            {
                /// Using the connection established
                using (connectionToServer)
                {
                    if (choice == 1)
                    {
                        if (property == 1)
                        {
                            /// Query to get the data from the table
                            query = @"select Count(firstName) from dbo.addressBookDatabase
                                   where city=@parameter group by city";
                        }
                        else
                        {
                            query = @"SELECT Count(t.firstName)
                                     FROM addressBook_Table t join address_Book b ON t.contactId = b.contactId
                                     join address_Detail d ON t.contactId = d.contactId 
                                     join contact_type c on t.contactId = c.contactId
                                     where d.city = @parameter group by d.city";
                        }
                    }
                    else if (choice == 2)
                    {
                        if (property == 1)
                        {
                            // Query to get the data from the table
                            query = @"select Count(firstName) from dbo.addressBookDatabase
                                   where state=@parameter group by state";
                        }
                        else
                        {
                            query = @"SELECT Count(t.firstName)
                                     FROM addressBook_Table t join address_Book b ON t.contactId = b.contactId
                                     join address_Detail d ON t.contactId = d.contactId 
                                     join contact_type c on t.contactId = c.contactId
                                     where d.state = @parameter group by d.state";
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong Choice....");
                    }
                    /// Impementing the command on the connection fetched database table
                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Binding the parameter to the formal parameters
                    command.Parameters.AddWithValue("@parameter", data);
                    /// Opening the connection to start mapping
                    connectionToServer.Open();
                    /// executing the sql data reader to fetch the records
                    SqlDataReader reader = command.ExecuteReader();
                    /// executing for not null
                    if (reader.HasRows)
                    {
                        /// Moving to the next record from the table
                        /// Mapping the data to the retrieved data from executing the query on the table
                        while (reader.Read())
                        {
                            int count = reader.GetInt32(0);
                            Console.WriteLine($"Number of Contacts Stored in {data} = {count}");
                            Console.WriteLine("\n\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data found");
                    }
                    reader.Close();
                }
            }
            /// Catching any type of exception generated during the run time
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connectionToServer.Close();
            }
        }
        /// <summary>
        /// UC7 -- Function to get the sorted data by alphabetically for the passed city
        /// </summary>
        /// <param name="cityName"></param>
        public void SortDetailsAlphabeticallyByCity(string cityName, int property)
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            /// Assigning the query variable to initialise the query string
            string query = "";
            /// Creating the address book model class object
            AddressBookModel bookModel = new AddressBookModel();
            try
            {
                using (connectionToServer)
                {
                    if (property == 1)
                    {
                        /// Query to get all the data from the table
                        query = @"select * from addressBookDatabase where city = @parameter order by firstName";
                    }
                    else
                    {
                        query = @"SELECT t.contactId,t.firstName, t.secondName, t.phoneNumber, t.emailId, 
                                     b.addressBookName, d.address, d.city, d.state, d.zip, c.contactType
                                     FROM addressBook_Table t join address_Book b ON t.contactId = b.contactId
                                     join address_Detail d ON t.contactId = d.contactId 
                                     join contact_type c on t.contactId = c.contactId
                                     where d.city = @parameter";
                    }
                    /// Impementing the command on the connection fetched database table
                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Binding the parameter to the formal parameters
                    command.Parameters.AddWithValue("@parameter", cityName);
                    /// Opening the connection to start mapping
                    connectionToServer.Open();
                    /// executing the sql data reader to fetch the records
                    SqlDataReader reader = command.ExecuteReader();
                    /// executing for not null
                    if (reader.HasRows)
                    {
                        /// Moving to the next record from the table
                        /// Mapping the data to the employee model class object
                        while (reader.Read())
                        {
                            if (property == 1)
                            {
                                bookModel.firstName = reader.GetString(0);
                                bookModel.secondName = reader.GetString(1);
                                bookModel.address = reader.GetString(2);
                                bookModel.city = reader.GetString(3);
                                bookModel.state = reader.GetString(4);
                                bookModel.zip = reader.GetInt64(5);
                                bookModel.phoneNumber = reader.GetInt64(6);
                                bookModel.emailId = reader.GetString(7);
                                bookModel.contactType = reader.GetString(8);
                                bookModel.addressBookName = reader.GetString(9);
                                Console.WriteLine($"First Name:{bookModel.firstName}\nSecond Name:{bookModel.secondName}\n" +
                                    $"Address:{bookModel.address}, {bookModel.city}, {bookModel.state} PinCode: {bookModel.zip}\n" +
                                    $" Phone Number: {bookModel.phoneNumber}\n Contact Type: {bookModel.contactType}\n Address Book Name : {bookModel.addressBookName}");
                                Console.WriteLine("\n\n");
                            }
                            else
                            {
                                bookModel.contactID = reader.GetInt32(0);
                                bookModel.firstName = reader.GetString(1);
                                bookModel.secondName = reader.GetString(2);
                                bookModel.phoneNumber = reader.GetInt64(3);
                                bookModel.emailId = reader.GetString(4);
                                bookModel.addressBookName = reader.GetString(5);
                                bookModel.address = reader.GetString(6);
                                bookModel.city = reader.GetString(7);
                                bookModel.state = reader.GetString(8);
                                bookModel.ZIP = reader.GetInt32(9);
                                bookModel.contactType = reader.GetString(10);

                                Console.WriteLine($"First Name:{bookModel.firstName}\nSecond Name:{bookModel.secondName}\n" +
                                    $"Address:{bookModel.address}, {bookModel.city}, {bookModel.state} PinCode: {bookModel.ZIP}\n" +
                                    $"Phone Number: {bookModel.phoneNumber}\nContact Type: {bookModel.contactType}\nAddress Book Name : {bookModel.addressBookName}");
                                Console.WriteLine("\n\n");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }
                    reader.Close();
                }
            }
            /// Catching the null record exception
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            /// Alway ensuring the closing of the connection
            finally
            {
                connectionToServer.Close();
            }
        }
        /// <summary>
        /// UC8 -- Function to get the count of the records stored in contact type using the group by clause
        /// </summary>
        public void GetCountOfContactType(int property)
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            string query = "";
            try
            {
                /// Using the connection established
                using (connectionToServer)
                {
                    /// Query to get the data from the table
                    if (property == 1)
                    {
                        query = @"select contactType, Count(firstName) from dbo.addressBookDatabase
                                    group by contactType";
                    }
                    else
                    {
                        query = @"SELECT c.contactType, Count(t.firstName)
                                     FROM addressBook_Table t join address_Book b ON t.contactId = b.contactId
                                     join address_Detail d ON t.contactId = d.contactId 
                                     join contact_type c on t.contactId = c.contactId
                                     group by c.contactType";
                    }
                    /// Impementing the command on the connection fetched database table
                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Opening the connection to start mapping
                    connectionToServer.Open();
                    /// executing the sql data reader to fetch the records
                    SqlDataReader reader = command.ExecuteReader();
                    /// executing for not null
                    if (reader.HasRows)
                    {
                        /// Moving to the next record from the table
                        /// Mapping the data to the retrieved data from executing the query on the table
                        while (reader.Read())
                        {
                            string contactTypeInAddressBook = reader.GetString(0);
                            int count = reader.GetInt32(1);
                            Console.WriteLine($"Number of Contacts Stored as {contactTypeInAddressBook} = {count}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data found");
                    }
                    reader.Close();
                }
            }
            /// Catching any type of exception generated during the run time
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connectionToServer.Close();
            }
        }
        /// <summary>
        /// UC9 -- Function to get the entire data from the table after implementing the concept of ER-Diagram
        /// Using the concept of join on the table
        /// </summary>
        public void GetAllDataFromTableUsingJoin()
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            /// Creating the address book model class object
            AddressBookModel bookModel = new AddressBookModel();
            try
            {
                using (connectionToServer)
                {
                    /// Query to get all the data from the table
                    string query = @"SELECT t.contactId,t.firstName, t.secondName, t.phoneNumber, t.emailId, 
                                     b.addressBookName, d.address, d.city, d.state, d.zip, c.contactType
                                     FROM addressBook_Table t join address_Book b ON t.contactId = b.contactId
                                     join address_Detail d ON t.contactId = d.contactId 
                                     join contact_type c on t.contactId = c.contactId;";
                    /// Impementing the command on the connection fetched database table

                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Opening the connection to start mapping
                    connectionToServer.Open();
                    /// executing the sql data reader to fetch the records
                    SqlDataReader reader = command.ExecuteReader();
                    /// executing for not null
                    if (reader.HasRows)
                    {
                        /// Moving to the next record from the table
                        /// Mapping the data to the employee model class object
                        while (reader.Read())
                        {
                            bookModel.contactID = reader.GetInt32(0);
                            bookModel.firstName = reader.GetString(1);
                            bookModel.secondName = reader.GetString(2);
                            bookModel.phoneNumber = reader.GetInt64(3);
                            bookModel.emailId = reader.GetString(4);
                            bookModel.addressBookName = reader.GetString(5);
                            bookModel.address = reader.GetString(6);
                            bookModel.city = reader.GetString(7);
                            bookModel.state = reader.GetString(8);
                            bookModel.ZIP = reader.GetInt32(9);
                            bookModel.contactType = reader.GetString(10);

                            Console.WriteLine($"First Name:{bookModel.firstName}\nSecond Name:{bookModel.secondName}\n" +
                                $"Address:{bookModel.address}, {bookModel.city}, {bookModel.state} PinCode: {bookModel.ZIP}\n" +
                                $"Phone Number: {bookModel.phoneNumber}\nContact Type: {bookModel.contactType}\nAddress Book Name : {bookModel.addressBookName}");
                            Console.WriteLine("\n\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }
                    reader.Close();
                }
            }
            /// Catching the null record exception
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            /// Alway ensuring the closing of the connection
            finally
            {
                connectionToServer.Close();
            }
        }
        /// <summary>
        /// UC10 -- Function to ensure that the other use cases are working fine after the join operation
        /// </summary>
        public void EnsuringOtherUseCasesForJoinedTable()
        {
            Console.WriteLine("Enter the choice for the use cases you want to ensure ===>");
            Console.WriteLine("1.Get data by City.");
            Console.WriteLine("2.Get data by State.");
            Console.WriteLine("3.Size Of address book by City.");
            Console.WriteLine("4.Size Of address book by State.");
            Console.WriteLine("5.Sort alphabetically by name for a City.");
            Console.WriteLine("6.Get Number Of Contact By Type.");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter the city you want to fetch data for...");
                    string city = Console.ReadLine();
                    GetTheDetailOfRecordForCityOrState(city, 1, 2);
                    break;
                case 2:
                    Console.WriteLine("Enter the state you want to fetch data for...");
                    string state = Console.ReadLine();
                    GetTheDetailOfRecordForCityOrState(state, 2, 2);
                    break;
                case 3:
                    Console.WriteLine("Enter the city you want to fetch size of address book for...");
                    string cityForCount = Console.ReadLine();
                    GetCountOfCityOrState(cityForCount, 2, 2);
                    break;
                case 4:
                    Console.WriteLine("Enter the city you want to fetch size of address book for...");
                    string stateForCount = Console.ReadLine();
                    GetCountOfCityOrState(stateForCount, 2, 2);
                    break;
                case 5:
                    Console.WriteLine("Enter the city you want to fetch record alphabetically of address book for...");
                    string cityForSort = Console.ReadLine();
                    SortDetailsAlphabeticallyByCity(cityForSort, 2);
                    break;
                case 6:
                    GetCountOfContactType(2);
                    break;
            }
        }
        /// <summary>
        /// UC17 -- Check for the updated data and return an integral value after the update
        /// </summary>
        /// <param name="name"></param>
        /// <param name="updatedData"></param>
        /// <param name="choice"></param>
        /// <returns> -1 : If the data is inconsistent , 0 - If the updated data do not matches
        /// 1- If the updated data is find </returns>
        public int GetTheUpdatedData(string name, string updatedData, int choice)
        {
            string query;
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            try
            {
                /// Using the connection established
                using (connectionToServer)
                {
                    /// Opening the connection
                    connectionToServer.Open();
                    /// Update query  for the table and binding with the parameter passed
                    if (choice == 1)
                    {
                        query = "select contactType from dbo.addressBookDatabase where firstName = @parameter1";
                    }
                    else
                    {
                        query = "select addressBookName from dbo.addressBookDatabase where firstName = @parameter1";
                    }
                    /// Impementing the command on the connection fetched database table
                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Binding the parameter to the formal parameters
                    command.Parameters.AddWithValue("@parameter1", name);
                    /// executing the sql data reader to fetch the records
                    SqlDataReader reader = command.ExecuteReader();
                    /// executing for not null
                    if (reader.HasRows)
                    {
                        /// Moving to the next record from the table
                        /// Mapping the data to the retrieved data from executing the query on the table
                        while (reader.Read())
                        {
                            string dataRetrieved = reader.GetString(0);
                            connectionToServer.Close();
                            /// Returning the match with updated data for the modified contact
                            return (dataRetrieved.Equals(updatedData) ? 1 : 0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data found");
                        return -1;
                    }
                }
            }
            /// Catching any type of exception generated during the run time
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connectionToServer.Close();
            }
            return -1;
        }
        /// <summary>
        /// UC18 -- Get the detail of the record in the address book entered within a time frame
        /// </summary>
        /// <param name="date"></param>
        public void RetrieveAllTheContactAddedInBetweenADate(DateTime date)
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            /// Creating the address book model class object
            AddressBookModel bookModel = new AddressBookModel();
            try
            {
                using (connectionToServer)
                {
                    /// Query to get all the data from the table
                    string query = @"select * from addressBookDatabase where dateOfEntry between @parameter and CAST(GETDATE() AS Date );";
                    /// Impementing the command on the connection fetched database table
                    SqlCommand command = new SqlCommand(query, connectionToServer);
                    /// Binding the parameter to the formal parameters
                    command.Parameters.AddWithValue("@parameter", date);
                    /// Opening the connection to start mapping
                    connectionToServer.Open();
                    /// executing the sql data reader to fetch the records
                    SqlDataReader reader = command.ExecuteReader();
                    /// executing for not null
                    if (reader.HasRows)
                    {
                        /// Moving to the next record from the table
                        /// Mapping the data to the employee model class object
                        while (reader.Read())
                        {
                            bookModel.firstName = reader.GetString(0);
                            bookModel.secondName = reader.GetString(1);
                            bookModel.address = reader.GetString(2);
                            bookModel.city = reader.GetString(3);
                            bookModel.state = reader.GetString(4);
                            bookModel.zip = reader.GetInt64(5);
                            bookModel.phoneNumber = reader.GetInt64(6);
                            bookModel.emailId = reader.GetString(7);
                            bookModel.contactType = reader.GetString(8);
                            bookModel.addressBookName = reader.GetString(9);
                            bookModel.DateOfEntry = reader.GetDateTime(10);
                            Console.WriteLine($"First Name:{bookModel.firstName}\nSecond Name:{bookModel.secondName}\n" +
                                $"Address:{bookModel.address}, {bookModel.city}, {bookModel.state} PinCode: {bookModel.zip}\n" +
                                $"Phone Number: {bookModel.phoneNumber}\nContact Type: {bookModel.contactType}\nAddress Book Name : {bookModel.addressBookName}\n" +
                                $"Date Of Entry in the Address Book: {bookModel.DateOfEntry}");
                            Console.WriteLine("\n\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }
                    reader.Close();
                }
            }
            /// Catching the null record exception
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            /// Alway ensuring the closing of the connection
            finally
            {
                connectionToServer.Close();
            }
        }
    }
}

