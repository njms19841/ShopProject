using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class aspnetusersEntity : IEntity<aspnetusersEntity>
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EMPLOYEE_CODE { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string EMPLOYEE_NAME { get; set; }
        /// <summary>
        /// 头像文件ID
        /// </summary>
        public string ico_imageId { get; set; }

        public string LOGIN_PASSWORD { get; set; }
        public DateTime? Last_sync_Time { get; set; }
        public int? active { get; set; }

    }
}
