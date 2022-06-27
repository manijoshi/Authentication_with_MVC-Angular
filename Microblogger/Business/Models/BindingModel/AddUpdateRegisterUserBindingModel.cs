using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.BindingModel
{
    public class AddUpdateRegisterUserBindingModel
    {
        //added id
        //public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
    }
}
