using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Business.Models.DTO
{
    public class UserDTO
    {
        public UserDTO(string firstName,string lastName,string email,string userName,string location,string phoneNumber, string id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
            Location = location;
            PhoneNumber = phoneNumber;
            ID = id;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string ID { get; set; }
    }
}
