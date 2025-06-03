using AutoMapper;
using server.Dto;
using server.Entities;
using server.Helper;
using server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Interface.Repository;
using server.Service;
using server.Dto.Auth;

namespace server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtHelper helper;
        private readonly IConfiguration _config;
        private readonly IMapper mapper;
        private readonly IEmailService _emailService;
        private const string FrontendUrl = "http://localhost:4200";

        public AuthController(IUserRepository userRepository, IJwtHelper helper,IMapper mapper,IEmailService config)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.helper = helper;
            _emailService = config;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ResponseDto>> Login([FromBody] LoginUserReqDto req)
        {
            User? user =await this.userRepository.GetUserByEmail(req.Email);
            ResponseDto res = new ResponseDto();

            if (user == null)
            {
                res.IsSuccessed = false;
                res.Message = "Invalid Credentials!";
                return BadRequest(res);
            }

            if (!BCrypt.Net.BCrypt.Verify(req.Password,user.Password))
            {
                res.IsSuccessed = false;
                res.Message = "Invalid Credentials!";
                return BadRequest(res);
            }
            var RefreshToken = helper.GenerateRefreshToken();

            user.RefreshToken = RefreshToken;
            user.RefreshTokenExpire=DateTime.Now.AddDays(2);

            await userRepository.UpdateUser(user);

            LoginUserResDto userDetail = new LoginUserResDto()
            {
                AccessToken = this.helper.GenerateJwtToken(user),
                RefreshToken=RefreshToken,
                userData=this.mapper.Map<UserDto>(user)
            };

            res.Data= userDetail;

            return Ok(res);
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ResponseDto>> Register([FromBody] RegisterUserReqDto req)
        {
            User? user = await this.userRepository.GetUserByEmail(req.Email);
            ResponseDto res = new ResponseDto();
            if (user != null)
            {
                res.IsSuccessed = false;
                res.Message = $"User with email {req.Email} already exsist";
                return BadRequest(res);
            }

            User newUser = new User()
            {
                UserName = req.UserName,
                Email = req.Email,
                Address = req.Address,
                Role=UserRoles.USER.ToString(),
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                RefreshToken="",
                RefreshTokenExpire=DateTime.Now.AddDays(2),
                CreatedDate=req.CreatedBy
               
            };
            bool result = await this.userRepository.AddUser(newUser);

            if (!result)
            {
                res.IsSuccessed = false;
                res.Message = $"Internal Server error";
                return BadRequest(res);
            }
            res.Message = "User registered successfully";
            return Ok(res);
        }

        
    
        [HttpPost]
        [Route("refresh")]
         public async Task<ActionResult<ResponseDto>> RefreshToken([FromBody] RefreshTokenDto req)
         {
             ResponseDto res = new ResponseDto();
             if(string.IsNullOrEmpty(req.RefreshToken) || string.IsNullOrEmpty(req.AccessToken)){
                res.IsSuccessed=false;
                res.Message = "Invalid Request";
                return BadRequest(res);
             }

             var refreshToken=req.RefreshToken;
             var accessToken=req.AccessToken;

             var principal = helper.GetPrincipalFromExpiredToken(accessToken);
             var email = principal.Identity.Name;

             User? user = await this.userRepository.GetUserByEmail(email);

             if(user==null || user.RefreshToken!=refreshToken || user.RefreshTokenExpire<=DateTime.Now){
                res.IsSuccessed=false;
                res.Message = "Invalid Request";
                return BadRequest(res);
             }
            
            refreshToken=helper.GenerateRefreshToken();
            accessToken=helper.GenerateJwtToken(user);

            user.RefreshToken = refreshToken;

            await userRepository.UpdateUser(user);
            
            //RefreshTokenDto data = new RefreshTokenDto(){
            //    AccessToken=accessToken,
            //    RefreshToken=refreshToken
            //};

            LoginUserResDto data = new LoginUserResDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                userData = this.mapper.Map<UserDto>(user)
            };

            res.Data=data;

            return Ok(res);
         }
   
        [HttpGet,Authorize]
        [Route("revoke")]
         public async Task<ActionResult<ResponseDto>> Revoke()
         {
            ResponseDto res = new ResponseDto();
            var email = User.Identity.Name;
             User? user = await this.userRepository.GetUserByEmail(email);
             if(user==null){
                res.IsSuccessed=false;
                res.Message = "Invalid Request";
                return BadRequest(res);
             }
             user.RefreshToken = "";
             user.RefreshTokenExpire=DateTime.Now;

             await this.userRepository.UpdateUser(user);
             
             res.Message = "User Successfully logout";
             return Ok(res);
         }
        //[HttpPost]
        //[Route("forgot-password")]
        //public async Task<ActionResult<ResponseDto>> ForgotPassword([FromBody] ForgotPasswordDto req)
        //{
        //    ResponseDto res = new ResponseDto();

        //    if (string.IsNullOrEmpty(req.Email))
        //    {
        //        res.IsSuccessed = false;
        //        res.Message = "Email is required!";
        //        return BadRequest(res);
        //    }

        //    var user = await this.userRepository.GetUserByEmail(req.Email);

        //    if (user == null)
        //    {
        //        res.IsSuccessed = false;
        //        res.Message = "User not found with the given email!";
        //        return BadRequest(res);
        //    }

        //    // Generate Reset Token (you can generate a JWT token or a GUID-based token)
        //    var resetToken = Guid.NewGuid().ToString();
        //    user.ResetPasswordToken = resetToken;
        //    user.ResetPasswordExpire = DateTime.Now.AddHours(1); // valid for 1 hour

        //    await userRepository.UpdateUser(user);

        //    // Create reset link
        //    var resetLink = $"{Request.Scheme}://{Request.Host}/reset-password?token={resetToken}";

        //    // Send Email
        //    var subject = "Password Reset Request";
        //    var body = $"Hi {user.UserName},<br/><br/>Please reset your password by clicking the link below:<br/><br/><a href='{resetLink}'>Reset Password</a><br/><br/>If you did not request a password reset, please ignore this email.";

        //    await _emailService.SendEmailAsync(user.Email, subject, body);

        //    res.Message = "Password reset link has been sent to your email.";
        //    return Ok(res);
        //}
        [HttpPost]
        [Route("forgot-password")]
        public async Task<ActionResult<ResponseDto>> ForgotPassword([FromBody] Dto.Auth.ForgotPasswordDto req)
        {
            ResponseDto res = new ResponseDto();

            if (string.IsNullOrEmpty(req.Email))
            {
                res.IsSuccessed = false;
                res.Message = "Email is required!";
                return BadRequest(res);
            }

            var user = await this.userRepository.GetUserByEmail(req.Email);

            if (user == null)
            {
                res.IsSuccessed = false;
                res.Message = "User not found with the given email!";
                return BadRequest(res);
            }

            // Generate Reset Token (you can generate a JWT token or a GUID-based token)
            var resetToken = Guid.NewGuid().ToString();
            user.ResetPasswordToken = resetToken;
            user.ResetPasswordExpire = DateTime.Now.AddHours(1); // valid for 1 hour

            await userRepository.UpdateUser(user);


            // Create reset link
            var resetLink = $"{FrontendUrl}/reset-password?token={resetToken}";

            // Send Email
            var subject = "Password Reset Request";
            var body = $"Hi {user.UserName},<br/><br/>Please reset your password by clicking the link below:<br/><br/><a href='{resetLink}'>Reset Password</a><br/><br/>If you did not request a password reset, please ignore this email.";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            res.Message = "Password reset link has been sent to your email.";
            return Ok(res);
        }
        [HttpPost]
        [Route("reset-password")]
        public async Task<ActionResult<ResponseDto>> ResetPassword([FromBody] Dto.Auth.ResetPasswordDto req)
        {
            ResponseDto res = new ResponseDto();

            if (string.IsNullOrEmpty(req.Token) || string.IsNullOrEmpty(req.NewPassword))
            {
                res.IsSuccessed = false;
                res.Message = "Invalid Request!";
                return BadRequest(res);
            }

            var user = await this.userRepository.GetUserByResetToken(req.Token);

            if (user == null || user.ResetPasswordExpire < DateTime.Now)
            {
                res.IsSuccessed = false;
                res.Message = "Invalid or expired token!";
                return BadRequest(res);
            }

            // Update password
            user.Password = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordExpire = null;

            await userRepository.UpdateUser(user);

            res.Message = "Password has been reset successfully.";
            return Ok(res);
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<ActionResult<ResponseDto>> RegisterAdmin([FromBody] RegisterAdminDto req)
        {
            User? user = await this.userRepository.GetUserByEmail(req.Email);
            ResponseDto res = new ResponseDto();
            if (user != null)
            {
                res.IsSuccessed = false;
                res.Message = $"User with email {req.Email} already exsist";
                return BadRequest(res);
            }
            // Convert string to Enum

            User newUser = new User()
            {
                UserName = req.UserName,
                Email = req.Email,

                Role = UserRoles.ADMIN.ToString(),
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                RefreshToken = "",
                RefreshTokenExpire = DateTime.Now.AddDays(2),
                CreatedDate = DateTime.Now


            };
            bool result = await this.userRepository.AddUser(newUser);
            if (!result)
            {
                res.IsSuccessed = false;
                res.Message = $"Internal Server error";
                return BadRequest(res);
            }
            res.Message = "admin registered successfully";
            return Ok(res);
        }

        //register-seller
        [HttpPost]
        [Route("register-seller")]
        public async Task<ActionResult<ResponseDto>> RegisterSeller([FromBody] RegisterSellerDto req)
        {
            User? user = await this.userRepository.GetUserByEmail(req.Email);
            ResponseDto res = new ResponseDto();
            if (user != null)
            {
                res.IsSuccessed = false;
                res.Message = $"User with email {req.Email} already exsist";
                return BadRequest(res);
            }
            // Convert string to Enum
            if (!Enum.TryParse(req.BusinessType, out BusinessTypes businessType))
            {
                res.IsSuccessed = false;
                res.Message = "Invalid Business Type";
                return BadRequest(res);
            }

            User newUser = new User()
            {
                UserName = req.UserName,
                Email = req.Email,
                Address = req.Address,
                Role = UserRoles.SELLER.ToString(),
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                RefreshToken = "",
                RefreshTokenExpire = DateTime.Now.AddDays(2),
                BusinessName = req.BusinessName,
                PhoneNumber = req.PhoneNumber,
                BusinessType = businessType, // Assign Business Type
                GSTNumber = req.GSTNumber,
                CreatedDate=DateTime.Now
            };
            bool result = await this.userRepository.AddUser(newUser);

            if (!result)
            {
                res.IsSuccessed = false;
                res.Message = $"Internal Server error";
                return BadRequest(res);
            }
            res.Message = "User registered successfully";
            return Ok(res);
        }
        [HttpGet("users-with-role")]
        public async Task<ActionResult<ResponseDto>> GetUsersWithRoleUser(string role)
        {
            ResponseDto res = new ResponseDto();

            List<User> users = await userRepository.GetUsersByRole(role);

            if (users == null || !users.Any())
            {
                res.IsSuccessed = false;
                res.Message = "No users found with role 'User'";
                return NotFound(res);
            }

            res.IsSuccessed = true;
            res.Message = "Users with role 'User' retrieved successfully";
            res.Data = users.Select(user => new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Address = user.Address,
                Role = user.Role
            }).ToList();



            return Ok(res);
        }     

       
    }
}
