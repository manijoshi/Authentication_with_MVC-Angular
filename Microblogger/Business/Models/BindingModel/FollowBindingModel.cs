using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Models.BindingModel
{
    public class FollowBindingModel
    {
        public System.Guid UserID { get; set; }
        public System.Guid UserToFollowID { get; set; }
        public string Email { get; set; }
    }
}
