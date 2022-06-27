using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Business.Entities
{
    public class Tweet
    {
        public System.Guid ID { get; set; }
        //public System.Guid UserID { get; set; }
        public string UserName{ get; set; }
        public string Message { get; set; }
        public System.DateTime CreatedAt { get; set; }

        public virtual ICollection<LikeTweet> LikeTweet { get; set; }
        public virtual ICollection<Tag> Tag { get; set; }
        //public virtual ICollection<Tag> Tag1 { get; set; } 
        public virtual AppUser User { get; set; }
    }
}
