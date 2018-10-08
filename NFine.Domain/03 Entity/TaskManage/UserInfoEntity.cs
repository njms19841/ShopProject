using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class UserInfoEntity : IEntity<UserInfoEntity>
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        public string sales_No { get; set; }
        public string sales_Name { get; set; }

        public string MANAGE_CENTER { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string SHOP_NAME { get; set; }


    }
}
