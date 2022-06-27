using Microblogger.Business;
using Microblogger.Business.Entities;
using Microblogger.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblogger.Controllers
{
    public class TweetController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JWTConfig _jWTConfig;
        private readonly AppDBContext _db;
        public TweetController(ILogger<UserController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IOptions<JWTConfig> jWTConfig, AppDBContext db)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _jWTConfig = jWTConfig.Value;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
