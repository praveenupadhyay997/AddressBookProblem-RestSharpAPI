// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DBConnection.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Praveen Kumar Upadhyay"/>
// --------------------------------------------------------------------------------------------------------------------
namespace AddressBookUnitTestProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using AddressBookServices;
    using RestSharp;
    using System.Net;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System;

    [TestClass]
    public class UnitTestClass
    {
        /// <summary>
        /// Instantinating the rest client class which translates a dedicated resp api operation to https request
        /// </summary>
        RestClient restClient;
        /// <summary>
        /// Initialising the base url as the base for the underlying data
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            restClient = new RestClient("http://localhost:3000");
        }
        /// <summary>
        /// Common arrange for the entire MS Test Cases
        /// </summary>
        public static AddressBookRepository bookRepository = new AddressBookRepository();
        /// <summary>
        /// Instance of the address book model so as to create a entity for addition
        /// </summary>
        public static AddressBookModel addressBookModel = new AddressBookModel();
        /// <summary>
        /// Method to get the data in json format requested from the api's data hosting server
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetAddressBookList()
        {
            /// Arrange
            RestRequest request = new RestRequest("/addressBook", Method.GET);
            /// Act
            IRestResponse response = restClient.Execute(request);
            /// Returning the json formatted result block
            return response;
        }
        /// <summary>
        /// TC 1 -- Check For Update of the Record and integrity of update method
        /// </summary>
        [TestMethod]
        public void EditUsingNameAndCheckForUpdate()
        {
            /// Act
            bool expected = true;
            /// Invoking the edit contact using name method so as to update the contact type
            /// Vijay -First name for the data row
            /// Family - New contact type
            /// 1 - update for contact type, 2- update the address book name
            bool actual = bookRepository.EditContactUsingName("Vijay", "Family", 1);
            /// Assert
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// TC 2 -- Check For Delete of the Record and integrity of delete method
        /// </summary>
        [TestMethod]
        public void DeleteUsingNameAndCheckForDelete()
        {
            /// Act
            bool expected = true;
            /// Invoking the delete contact using  first name method so as to delete the contact type
            /// Raju -First name for the data row
            bool actual = bookRepository.DeleteContactUsingName("Raju");
            /// Assert
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// TC 3 -- Check For Addition of the Record and integrity of add to record method
        /// </summary>
        [TestMethod]
        public void AddUsingInstanceOfAddressBookModelAndCheckForAdd()
        {
            /// Act
            bool expected = true;
            /// Initialising the instances with the values of the data attributes
            addressBookModel = new AddressBookModel
            {
                firstName = "Shardul",
                secondName = "Mehta",
                address = "Sec-5",
                city = "Jabalpur",
                state = "MP",
                zip = 482005,
                phoneNumber = 98784565,
                emailId = "shardul@gmail.com",
                contactType = "Profession",
                addressBookName = "PraveenRecord"
            };
            /// Invoking the add contact using the instance of the address bookmodel class so as to add the record
            bool actual = bookRepository.AddDataToTable(addressBookModel);
            /// Assert
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// TC 4 -- Check For Update of the Record and integrity of update method after the update
        /// </summary>
        [TestMethod]
        public void CheckUsingNameAndForUpdateAfterTheUpdate()
        {
            /// Act
            int expected = 1;
            /// Invoking the edit contact using name method so as to update the contact type
            /// Vijay -First name for the data row
            /// Family - New contact type
            /// 1 - update for contact type, 2- update the address book name
            bool actualAfterUpdate = bookRepository.EditContactUsingName("Vijay", "Friends", 1);
            int actual = bookRepository.GetTheUpdatedData("Vijay", "Friends", 1);
            /// Assert
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// TC 5(UC 22) -- On calling the address book rest API return the list of the schema stored inside the database
        /// </summary>
        [TestMethod]
        public void OnCallingTheAddressBookRestAPI_RetrievesAllData()
        {
            /// Act 
            IRestResponse response = GetAddressBookList();
            /// Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            List<AddressBookModel> addressBookDataResponse = JsonConvert.DeserializeObject<List<AddressBookModel>>(response.Content);
            Assert.AreEqual(1, addressBookDataResponse.Count);

            foreach (AddressBookModel bookModel in addressBookDataResponse)
            {
                Console.WriteLine($"First Name:{bookModel.firstName}\nSecond Name:{bookModel.secondName}\n" +
                                $"Address:{bookModel.address}, {bookModel.city}, {bookModel.state} PinCode: {bookModel.zip}\n" +
                                $"Phone Number: {bookModel.phoneNumber}\nContact Type: {bookModel.contactType}\nAddress Book Name : {bookModel.addressBookName}\n" +
                                $"Date Of Entry in the Address Book: { bookModel.DateOfEntry}");
                Console.WriteLine("\n\n");
            }
        }
    }
}
