using Microblogger.Business.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Business
{
    public class AppDBContext:IdentityDbContext<AppUser,IdentityRole,string>
    {

        public AppDBContext(DbContextOptions options):base(options)
        {
            
        }
        
        public DbSet<Follow> Follow { get; set; }
        //public DbSet<User> User { get; set; }
        public DbSet<LikeTweet> LikeTweet { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Tweet> Tweet { get; set; }
        
    }
}
