using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Business.Models.DTO
{
    public class FollowDTO
    {
        public Guid UserID { get; set; }
        public Guid UserToFollowID { get; set; }
    }
}
