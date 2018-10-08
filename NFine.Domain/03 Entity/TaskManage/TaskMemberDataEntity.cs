using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class TaskMemberDataEntity : IEntity<TaskMemberDataEntity>
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 批号ID
        /// </summary>
        public string beachId { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public string memberId { get; set; }
        /// <summary>
        /// 门店代号
        /// </summary>
        public string shopCode { get; set; }
        /// <summary>
        /// 分类会员ID
        /// </summary>
        public string mfMemberId { get; set; }

        /// <summary>
        /// 进店时间
        /// </summary>
        public DateTime? InTime { get; set; }
        /// <summary>
        /// 会员类型代号
        /// </summary>
        public string MemberTypeCode { get; set; }
        /// <summary>
        /// 会员类型名称
        /// </summary>
        public string MemberTypeName { get; set; }
        /// <summary>
        /// 图像路径
        /// </summary>
        public string picUrl { get; set; }
        /// <summary>
        /// 是否已接待
        /// </summary>
        public int IsRead { get; set; }
        /// <summary>
        /// 接待用户ID
        /// </summary>
        public string readUserId { get; set; }


    }
}
