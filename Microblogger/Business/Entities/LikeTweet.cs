using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Business.Entities
{
    public class LikeTweet
    {
        public System.Guid ID { get; set; }
        public System.Guid TweetID { get; set; }
        public string UserName { get; set; }
        
        public virtual Tweet Tweet { get; set; }
        public virtual AppUser User { get; set; }
    }
}
