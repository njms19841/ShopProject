using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Market.APIService.Models;
using Market.APIService.Providers;
using Market.APIService.Results;
using NFine.Application.APPManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using System.Net;
using donet.io.rong;
using Newtonsoft.Json;
using donet.io.rong.models;

namespace Market.APIService.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoModel GetUserInfo()
        {
            try { 
            marketSalesApp app = new marketSalesApp();
            
            UserInfoResultModel t = app.GetUserInfo(User.Identity.GetUserId());

                UserInfoModel model = new UserInfoModel
                {
                    No = t.No,
                    Name = t.Name,
                    PhoneNumber = t.PhoneNumber,
                    SalesNo = t.SalesNo
                     , POP_TYPE_CODE = t.POP_TYPE_CODE, id = User.Identity.GetUserId(),

                    PICUrl = "https://iretailerapp.flnet.com/userPic.jpg"

                /// ShopNo = t.ShopNo
            };
                String appKey = "y745wfm8y1y6v";
                String appSecret = "njmewTIin5p";
                RongCloud rongcloud = RongCloud.getInstance(appKey, appSecret);
                JsonSerializer serializer = new JsonSerializer();

               
                // 获取 Token 方法 
                TokenReslut usergetTokenResult = rongcloud.user .getToken(model.id, model.Name, model.PICUrl);
                if (usergetTokenResult.getCode() == 200)
                {
                    model.IMToken = usergetTokenResult.getToken();
                }

                marketShopApp shopApp = new marketShopApp();
            List<marketSalesShopEntity> shops = shopApp.getShopByUserId(User.Identity.GetUserId());
            model.Shops = new List<UserShopInfoModel>();
            foreach (marketSalesShopEntity shop in shops)
            {
                UserShopInfoModel shopmodel = new UserShopInfoModel() { CustomerCode=shop.CUSTOMER_CODE, CustomerName=shop.CUSTOMER_NAME, ShopCode=shop.SHOP_CODE, ShopName= shop.SHOP_NAME, LATITUDE=shop.LATITUDE.Value, LONGITUDE=shop.LONGITUDE.Value  };
                model.Shops.Add(shopmodel);
            }
            return model;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);

            }
        }
        [Route("UserInfoTest")]
        [HttpGet]
        public UserInfoModel UserInfoTest()
        {

            marketSalesApp app = new marketSalesApp();

            UserInfoResultModel t = app.GetUserInfo();

            UserInfoModel model = new UserInfoModel
            {
                No = t.No,
                /// ShopNo = t.ShopNo
            };
            
            return model;
        }

        // POST api/Account/Logout
        /// <summary>
        /// 登出功能
        /// </summary>
        /// <returns></returns>
        [Route("Logout")]
        
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
        [AllowAnonymous]
        [Route("ChangePasswordByUserName")]
        public async Task<IHttpActionResult> ChangePasswordByUserName(ChangePasswordBindingModel2 model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            ApplicationUser user = UserManager.FindByName(model.PhoneNumber);
            if (user == null)
            {
                return BadRequest("用户不存在");
            }
            IdentityResult result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [AllowAnonymous]
        [Route("CreateUser")]
        public async Task<IHttpActionResult> CreateUser(RegisterBindingModel model)
        {
            try
            {
                if (!this.RequestContext.IsLocal)
                {
                    return BadRequest("API禁止被访问");
                }


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var salesApp = new marketSalesApp();
                

                
                string userName = salesApp.findUserNameByEmpCode(model.EMPLOYEE_CODE);
                if (userName != null)
                {
                    ApplicationUser user = UserManager.FindByName(userName);
                    IdentityResult result1 = await UserManager.ChangePasswordAsync(user.Id, user.LOGIN_PASSWORD,
                    model.Password);
                    if (!result1.Succeeded)
                    {
                       
                        return GetErrorResult(result1);
                    }

                    user.UserName = model.EMPLOYEE_CODE;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.EMPLOYEE_NAME = model.EMPLOYEE_NAME;
                    user.EMPLOYEE_CODE = model.EMPLOYEE_CODE;
                    user.LOGIN_PASSWORD = model.Password;
                    user.Last_sync_Time = System.DateTime.Now;
                    user.active = model.Active;
                    result1 = await UserManager.UpdateAsync(user);

                    if (!result1.Succeeded)
                    {
                        return GetErrorResult(result1);
                    }
                }
                else
                {

                    ApplicationUser user = new ApplicationUser()
                    {
                        UserName = model.EMPLOYEE_CODE,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        EMPLOYEE_NAME = model.EMPLOYEE_NAME,
                        EMPLOYEE_CODE = model.EMPLOYEE_CODE,
                        active=model.Active,
                        LOGIN_PASSWORD = model.Password,
                        Last_sync_Time = System.DateTime.Now
                    };
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            
            if (!this.RequestContext.IsLocal)
            {
                return BadRequest("API禁止被访问");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { PhoneNumber = model.PhoneNumber,Email= model.PhoneNumber+"@foxconn.com",
               LOGIN_PASSWORD=model.Password, Last_sync_Time= System.DateTime.Now
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("UpdateUserInfo")]
        public async Task<IHttpActionResult> UpdateUserInfo(UpdateBindingModel model)
        {
            
            if (!this.RequestContext.IsLocal)
            {
                return BadRequest("API禁止被访问");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var salesApp = new marketSalesApp();
            string userName = salesApp.findUserNameByEmpCode(model.EMPLOYEE_CODE);
           
            if (userName == null)
            {
                return BadRequest("用户不存在");
            }
            ApplicationUser user = UserManager.FindByName(userName);
            IdentityResult result = await UserManager.ChangePasswordAsync(user.Id, user.LOGIN_PASSWORD,
                model.Password);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            user.UserName = model.EMPLOYEE_CODE;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.EMPLOYEE_NAME = model.EMPLOYEE_NAME;
            user.EMPLOYEE_CODE = model.EMPLOYEE_CODE;
            user.LOGIN_PASSWORD = model.Password;
            user.Last_sync_Time = System.DateTime.Now;
            result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        //// POST api/Account/RegisterExternal
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        //[Route("RegisterExternal")]
        //public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var info = await Authentication.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return InternalServerError();
        //    }

        //    var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

        //    IdentityResult result = await UserManager.CreateAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result); 
        //    }
        //    return Ok();
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region 帮助程序

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // 没有可发送的 ModelState 错误，因此仅返回空 BadRequest。
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits 必须能被 8 整除。", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
