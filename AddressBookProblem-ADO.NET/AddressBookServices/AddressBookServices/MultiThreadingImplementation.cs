// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiThreadingImplementation.cs" company="Bridgelabz">
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
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;

    public class MultiThreadingImplementation
    {
        // <summary>
        /// For ensuring the established connection using the Sql Connection specifying the property
        /// </summary>
        public static SqlConnection connectionToServer { get; set; }
        /// <summary>
        /// Declaring a list of address book model object type
        /// </summary>
        public static List<AddressBookModel> bookModels = new List<AddressBookModel>();
        /// <summary>
        /// Creating an instance of the address book repository
        /// </summary>
        public static AddressBookRepository bookRepository = new AddressBookRepository();
        /// <summary>
        /// Default constructor to intialise the list with data instances
        /// </summary>
        public MultiThreadingImplementation()
        {
            bookModels.Add(new AddressBookModel
            {
                firstName = "Shambhu",
                secondName = "Mehta",
                address = "Sec-5",
                city = "Jabalpur",
                state = "MP",
                zip = 482005,
                phoneNumber = 99984565,
                emailId = "shambhu@gmail.com",
                contactType = "Profession",
                addressBookName = "PraveenRecord",
                DateOfEntry = Convert.ToDateTime("2018-04-09")
            });
            bookModels.Add(new AddressBookModel
            {
                firstName = "Shivani",
                secondName = "Mehta",
                address = "Sec-5",
                city = "Jabalpur",
                state = "MP",
                zip = 482005,
                phoneNumber = 88984565,
                emailId = "shivani@gmail.com",
                contactType = "Friends",
                addressBookName = "PraveenRecord",
                DateOfEntry = Convert.ToDateTime("2018-05-09")
            });
            bookModels.Add(new AddressBookModel
            {
                firstName = "Saloni",
                secondName = "Mehta",
                address = "Sec-5",
                city = "Jabalpur",
                state = "MP",
                zip = 482005,
                phoneNumber = 72064565,
                emailId = "saloni@gmail.com",
                contactType = "Family",
                addressBookName = "PraveenRecord",
                DateOfEntry = Convert.ToDateTime("2018-10-09")
            });
        }
        /// <summary>
        /// Method to enter the data entry with the date entry as well to the address book database table
        /// Using the stored procedure named dbo.SpAddcontactRecordsWithDateOfEntry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddDataToAddressBookDatabaseTable(AddressBookModel model)
        {
            // Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            /// Calling the Get connection method to establish the connection to the Sql Server
            connectionToServer = dbc.GetConnection();
            try
            {
                /// Using the connection established
                using (connectionToServer)
                {
                    /// Implementing the stored procedure
                    SqlCommand command = new SqlCommand("SpAddcontactRecordsWithDateOfEntry", connectionToServer);
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
                    command.Parameters.AddWithValue("@entryDate", model.DateOfEntry);
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
        /// UC21 -- Method to add the data records via multithreads
        /// Multithread basically used to ensure maximum utilizaion of cpu resources namely
        /// Thread is an element responsible to execute a piece of code
        /// </summary>
        public void AddingMultipleContactDetailsToAddressBookThreading()
        {
            /// Iterating over bookModels list to add the data records to the databse using the instance of AddressBookModel
            bookModels.ForEach(contactRecord =>
            {
                /// Underneath utilising the ThreadStart delegate to add record to the database
                /// Each iteration is utilising a single thread
                Thread thread = new Thread(() =>
                {
                    Console.WriteLine("Record being added" + contactRecord.firstName);
                    /// Calling the adder method to insert the record to the address book table
                    AddDataToAddressBookDatabaseTable(contactRecord);
                    /// Printing the current thread ID
                    Console.WriteLine("Current Thread Id is: " + Thread.CurrentThread.ManagedThreadId);
                    /// Printing the added record ending message
                    Console.WriteLine("Contact added:" + contactRecord.firstName);
                });
                /// Start Method is used to contact the OS regarding the begining of execution of th current thread
                thread.Start();
                /// Join Method is used to ensure that the main thread does not exits before the child threads ends
                thread.Join();
            });
            ///
            bookRepository.GetAllRecords();
        }
    }
}
