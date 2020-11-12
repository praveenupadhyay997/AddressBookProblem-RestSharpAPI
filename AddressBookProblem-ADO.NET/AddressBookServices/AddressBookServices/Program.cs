// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Praveen Kumar Upadhyay"/>
// --------------------------------------------------------------------------------------------------------------------
namespace AddressBookServices
{
    using System;
    using System.ComponentModel;
    class Program
    {
        /// <summary>
        /// Creating  the instance of the address book repository
        /// </summary>
        public static AddressBookRepository repository = new AddressBookRepository();
        /// <summary>
        /// Creating the address book model instance
        /// </summary>
        public static AddressBookModel bookModel = new AddressBookModel();
        /// <summary>
        /// Method to take the input for the new records
        /// </summary>
        public static void TakeInputOfRecords()
        {
            Console.WriteLine("Enter the First Name :");
            bookModel.firstName = Console.ReadLine();
            Console.WriteLine("Enter the Second Name :");
            bookModel.secondName = Console.ReadLine();
            Console.WriteLine("Enter the Address :");
            bookModel.address = Console.ReadLine();
            Console.WriteLine("Enter the City :");
            bookModel.city = Console.ReadLine();
            Console.WriteLine("Enter the State :");
            bookModel.state = Console.ReadLine();
            Console.WriteLine("Enter the Zip :");
            bookModel.zip = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Enter the Phone Number :");
            bookModel.phoneNumber = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Enter the email-id :");
            bookModel.emailId = Console.ReadLine();
            Console.WriteLine("Enter the contact type :");
            bookModel.contactType = Console.ReadLine();
            Console.WriteLine("Enter the address book name :");
            bookModel.addressBookName = Console.ReadLine();
        }
        /// <summary>
        /// Method to take input to update the record inside the address book with help of name passed
        /// </summary>
        public static void UpdateCall()
        {
            Console.WriteLine("Enter the name of the record to edit.");
            string recordName = Console.ReadLine();
            /// Getting the data which you want to update
            Console.WriteLine("Enter the choice you want to update ===>");
            Console.WriteLine("1.Contact Type.");
            Console.WriteLine("2.Address Book Name.");
            int choice = Convert.ToInt32(Console.ReadLine());
            bool result = false;
            switch(choice)
            {
                case 1:
                    Console.WriteLine("Enter the new contact type (Friends,Family and Profession) -");
                    string type = Console.ReadLine();
                    /// Getting the return for the result of the update query
                    result = repository.EditContactUsingName(recordName, type, choice);
                        break;
                case 2:
                    Console.WriteLine("Enter the address book name -");
                    string addressBookName = Console.ReadLine();
                    /// Getting the return for the result of the update query
                    result = repository.EditContactUsingName(recordName, addressBookName, choice);
                    break;

                default:
                    Console.WriteLine("Entered choice is wrong.....");
                    break;
            }
            /// Testing for the success of the update to the table
            Console.WriteLine((result)? "Updated Successfully": "Update failed");
        }
        /// <summary>
        /// Method driver for the city or state details by particular city or state
        /// </summary>
        public static void GetByCityOrState()
        {
            Console.WriteLine("Enter the choice you want to retrieve data ===>");
            Console.WriteLine("1.City.");
            Console.WriteLine("2.State.");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the name of City or State by which you want the data -");
            string cityOrState = Console.ReadLine();
            repository.GetTheDetailOfRecordForCityOrState(cityOrState, choice, 1);
        }
        /// <summary>
        /// Method driver for the city or state count by particular city or state
        /// </summary>
        public static void GetCountByParticularCityOrState()
        {
            Console.WriteLine("Enter the choice you want to retrieve data ===>");
            Console.WriteLine("1.City.");
            Console.WriteLine("2.State.");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the name of City or State by which you want the data -");
            string cityOrState = Console.ReadLine();
            repository.GetCountOfCityOrState(cityOrState, choice, 1);
        }
        /// <summary>
        /// Method driver for sorting the data records by the name passed
        /// </summary>
        public static void SortByName()
        {
            Console.WriteLine("Enter the name of City by which you want to sort the data alphabetically by name -");
            string city = Console.ReadLine();
            repository.SortDetailsAlphabeticallyByCity(city, 1);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("Welcome to the Address Book Data Retrieval Program");
            Console.WriteLine("==================================================");
            AddressBookRepository repository = new AddressBookRepository();
            repository.EnsureDataBaseConnection();
            Console.ReadKey();
            /// UC1 -- Getting all the records from the address book table
            repository.GetAllRecords();
            /// UC2 -- Insert a record to the address book
            TakeInputOfRecords();
            /// Testing for the success of the insertion to the table
            Console.WriteLine(repository.AddDataToTable(bookModel) ? "Inserted Successfully" : "Insert failed");
            /// UC3 -- Update a record to the address book
            UpdateCall();
            /// UC4 -- Delete a record from the table
            Console.WriteLine(repository.DeleteContactUsingName(null) ? "Deleted Successfully" : "Delete failed");
            /// UC5 -- Get the details of the record of the contacts in the address book of a city or state
            GetByCityOrState();
            /// UC6 -- Get the count of the record of the contacts in the address book of a city or state
            GetCountByParticularCityOrState();
            /// UC7 -- Sort the data by first name for the given city
            SortByName();
            /// UC8 -- Get the count of the contacts stored in a particular contact type
            repository.GetCountOfContactType(1);
            /// UC9 -- Getall the details from the ER - Diagram
            repository.GetAllDataFromTableUsingJoin();
            /// UC10-- Ensuring the other use cases working fine for retrieval
            repository.EnsuringOtherUseCasesForJoinedTable();
            /// UC18 -- Retrieving the data from the address book database entered within a time frame
            repository.RetrieveAllTheContactAddedInBetweenADate(Convert.ToDateTime("2018-01-01"));
            /// UC21 -- Implementing Multithreading concept to the address book problem
            /// Adding multiple data to the address book database using the multiple threads
            MultiThreadingImplementation threadingImplementation = new MultiThreadingImplementation();
            threadingImplementation.AddingMultipleContactDetailsToAddressBookThreading();
        }
    }
}
