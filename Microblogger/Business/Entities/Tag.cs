using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Business.Entities
{
    public class Tag
    {
        public System.Guid ID { get; set; }
        public System.Guid TweetID { get; set; }
        public string TagName { get; set; }
        public Nullable<int> SearchCount { get; set; }

        public virtual Tweet Tweet { get; set; }
        //public virtual Tweet Tweet1 { get; set; }
    }
}
