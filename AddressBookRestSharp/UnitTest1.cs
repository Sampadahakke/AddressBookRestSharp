using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace AddressBookRestSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient(" http://localhost:4000");
        }

        private IRestResponse getContacts()
        {
            RestRequest request = new RestRequest("/Contacts", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }

        //Creating method to get contacts
        [TestMethod]
        public void onCallingGETApi_ReturnContactList()
        {
            IRestResponse response = getContacts();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Contacts> dataResponse = JsonConvert.DeserializeObject<List<Contacts>>(response.Content);
            Assert.AreEqual(15, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.Id +"FirstName: " + item.FirstName + "LastName: " + item.LastName + "Address: " + item.Address + "City: " + item.City + "State: " + item.State + "Zip: " + item.Zip + "PhoneNumber: " + item.PhoneNumber);
            }
        }

        //Creating method to add contacts
        [TestMethod]
        public void givenContact_OnPost_ShouldReturnAddedContact()
        {
            RestRequest request = new RestRequest("/Contacts", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("FirstName", "Dhanashree");
            jObjectbody.Add("LastName", "Hakke");
            jObjectbody.Add("Address", "near DC road");
           jObjectbody.Add("City", "Satara");
            jObjectbody.Add("State", "MH");
            jObjectbody.Add("Zip",416305);
            jObjectbody.Add("PhoneNumber", 9765721521);
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Contacts dataResponse = JsonConvert.DeserializeObject<Contacts>(response.Content);
            Assert.AreEqual("Dhanashree", dataResponse.FirstName);
            Assert.AreEqual("Hakke", dataResponse.LastName);
            Assert.AreEqual("near DC road", dataResponse.Address);
            Assert.AreEqual("MH", dataResponse.State);
            Assert.AreEqual(416305, dataResponse.Zip);
            Assert.AreEqual("Satara", dataResponse.City);
            Assert.AreEqual(9765721521, dataResponse.PhoneNumber);
        }

        //Creating method to update contacts
        [TestMethod]
        public void givenContact_OnPUT_ShouldReturnUpdatedContact()
        {
            RestRequest request = new RestRequest("/Contacts/3", Method.PUT);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("FirstName", "Shraddha");
            jObjectbody.Add("LastName", "Khot");
            jObjectbody.Add("Address", "A.C Colony");
            jObjectbody.Add("City", "Kolhapur");
            jObjectbody.Add("State", "MH");
            jObjectbody.Add("Zip", 416307);
            jObjectbody.Add("PhoneNumber", 8928707381);
            request.AddOrUpdateParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Contacts dataResponse = JsonConvert.DeserializeObject<Contacts>(response.Content);
            Assert.AreEqual("Shraddha", dataResponse.FirstName);
            Assert.AreEqual("Khot", dataResponse.LastName);
           
        }

        //Creating method to delete contact
        [TestMethod]
        public void givenContact_OnDELETE_ShouldReturnContact()
        {
            RestRequest request = new RestRequest("/Contacts/6", Method.DELETE);
            JObject jObjectbody = new JObject();
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.NotFound);
            Contacts dataResponse = JsonConvert.DeserializeObject<Contacts>(response.Content);
            Assert.AreEqual(null, dataResponse.FirstName);
            Assert.AreEqual(null, dataResponse.LastName);

        }
    }
}