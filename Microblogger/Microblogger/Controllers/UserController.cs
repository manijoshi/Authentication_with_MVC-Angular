using Microblogger.Business;
using Microblogger.Business.Entities;
using Microblogger.Business.Enums;
using Microblogger.Business.Models;
using Microblogger.Business.Models.DTO;
using Microblogger.Models;
using Microblogger.Models.BindingModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.BindingModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JWTConfig _jWTConfig;
        private readonly AppDBContext _db;
        
        public UserController(ILogger<UserController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IOptions<JWTConfig> jWTConfig, AppDBContext db)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _jWTConfig = jWTConfig.Value;
            _db=db;
        }
        [HttpPost("RegisterUser")]
        public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            try
            {
                var user = new AppUser()
                {
                    //added id

                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Location = model.Location,
                    Image = model.Image

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK,"User has been Registered!",null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error,"", result.Errors.Select(x => x.Description).ToArray()));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpGet("GetAllUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<object> GetAllUser()
        {
            try
            {
                var users = _userManager.Users.Select(x => new UserDTO(x.FirstName, x.LastName, x.Email,x.UserName, x.PhoneNumber, x.Location, x.Id));
                return await Task.FromResult(users);
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("GetUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public AppUser GetUser(string ID)
        {
            try
            {
                return _userManager.Users.Where(X => X.Id == ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AppUser GetUserByEmail(string email)
        {
            try
            {
                return _userManager.Users.Where(X => X.Email == email).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromBody] loginBindingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.FindByEmailAsync(model.Email);
                        var user = new UserDTO(appUser.FirstName, appUser.LastName, appUser.Email, appUser.UserName, appUser.Location, appUser.PhoneNumber, appUser.Id);
                        user.Token = GenerateToken(appUser);
                        return await Task.FromResult(user);
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", null));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid Email or Password", null));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        private string GenerateToken(AppUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jWTConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),
                Audience = _jWTConfig.Audience,
                Issuer = _jWTConfig.Issuer
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        [HttpPost("FollowUser")]
        public async Task<object> Follow(FollowBindingModel followBindingModel)
        {
            //FollowDTO followDTO = new FollowDTO();
            
            try
            {
                //var users = _userManager.Users.Select(x => new UserDTO(x.Id, x.LastName, x.Email, x.UserName, x.PhoneNumber, x.Location));
                Follow follow = new Follow();
                follow.ID = System.Guid.NewGuid();
                follow.Follower_UserID = followBindingModel.UserID;
                follow.Following_UserID = followBindingModel.UserToFollowID;
               
                _db.Follow.Add(follow);

                var _users = (GetAllUser().Result as IQueryable<UserDTO>).ToList();
                AppUser user = GetUser(followBindingModel.UserID.ToString());
                AppUser FOLLOWINGuser =  GetUser(followBindingModel.UserToFollowID.ToString());

                if(user.Follow == null)
                {
                    user.Follow = new List<Follow>();
                }
                user.Follow.Add(follow);
                //FOLLOWINGuser.SecurityStamp

                follow.User = user;

                _db.SaveChanges();
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "You are now following user with Id : "+ follow.User.FirstName, null));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpPost("UnfollowUser")]
        public async Task<object> UnFollow(FollowBindingModel followBindingModel)
        {
            try
            {
                Follow unfollow = _db.Follow.Where(x=>x.Following_UserID==followBindingModel.UserToFollowID && followBindingModel.UserID==x.Follower_UserID).Single();
                _db.Follow.Remove(unfollow);
                _db.SaveChanges();
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "You've succesfully unfollowed ", null));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpGet("GetAllFollowing")]
        public async Task<object> GetAllFollowing(FollowBindingModel followBindingModel)
        {
            try
            {
                IList<AppUser> followingList = new List<AppUser>();
                //AppUser followers;
                AppUser user;
                //list of users i follow
                IEnumerable<Follow> followedusers = _db.Follow.Where(ds => ds.Follower_UserID.ToString() == followBindingModel.UserID.ToString());

                var i = 0;
                foreach (var item in followedusers)
                {
                    user = _userManager.Users.Where(dr => dr.Id == item.Following_UserID.ToString()).FirstOrDefault();
                    
                    followingList.Add(user);

                }
                return followingList;
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpGet("GetAllFollowers")]
        public async Task<object> GetAllFollowers(FollowBindingModel followBindingModel)
        {
            try
            {
                IList<AppUser> followersList = new List<AppUser>();
                AppUser user;
                string userId = GetUserByEmail(followBindingModel.Email).Id.ToString();

                //list of users following me
                IEnumerable<Follow> followerusers = _db.Follow.Where(ds => ds.Following_UserID.ToString() == userId.ToString());
                
                foreach (var item in followerusers)
                {
                    user = _userManager.Users.Where(dr => dr.Id == item.Follower_UserID.ToString()).FirstOrDefault();

                    followersList.Add(user);

                }
                return followersList;
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }
}
