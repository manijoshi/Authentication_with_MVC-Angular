using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Business.Entities
{
    public class Follow
    {
        public System.Guid ID { get; set; }
        public System.Guid Follower_UserID { get; set; }
        public System.Guid Following_UserID { get; set; }//followed_userId
        
        public virtual AppUser User { get; set; }
        //public virtual User User1 { get; set; }
    }
}
