using System;
using System.Collections.Generic;

namespace Market.APIService.Models
{
    // AccountController 操作返回的模型。
    public class UserInfoModel
    {
        public string id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string SalesNo { get; set; }
        public string POP_TYPE_CODE { get; set; }
        public string IMToken { get; set; }
        public string PICUrl { get; set; }

        public List<UserShopInfoModel> Shops { get; set; }
}
    public class UserShopInfoModel
    {
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public double LONGITUDE { get; set; }
        public double LATITUDE { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }

    }
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
