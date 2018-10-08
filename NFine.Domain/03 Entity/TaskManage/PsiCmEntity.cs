using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class PsiCmEntity : IEntity<PsiCmEntity>
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? day { get; set; }

        public DateTime? createdTime { get; set; }
        public DateTime? modifyTime { get; set; }
        /// <summary>
        /// 月目标总数
        /// </summary>
        public int? cmQty { get; set; }
       
        /// <summary>
        /// 状态:0:无 1:审核通过 2:审核拒绝
        /// </summary>
        public int? status { get; set; }
        public string Prodect { get; set; }
        /// <summary>
        /// 目前使用城市
        /// </summary>
        public string orgId { get; set; }
        /// <summary>
        /// 父级组织ID
        /// </summary>
        public string PorgId { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string BrandCode { get; set; }

    }
}
