using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Business.Entities
{
    public class AppUser:IdentityUser
    {
        //public System.Guid ID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Tweet> Tweet { get; set; }
        public virtual ICollection<LikeTweet> LikeTweet { get; set; }
        public virtual ICollection<Follow> Follow { get; set; }
    }
}
