using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketreportInfoEntity : IEntity<marketreportInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string type
        {
            get; set;
        }
        public string subType
        {
            get; set;
        }
        public string desc
        {
            get; set;
        }
        public string fileId
        {
            get; set;
        }
        public DateTime? F_CreatorTime
        {
            get;set;
        }

        public string F_CreatorUserId
        {
            get; set;
        }

        public bool? F_DeleteMark
        {
            get; set;
        }

        public DateTime? F_DeleteTime
        {
            get; set;
        }

        public string F_DeleteUserId
        {
            get; set;
        }

        public string F_Id
        {
            get; set;
        }

        public DateTime? F_LastModifyTime
        {
            get; set;
        }

        public string F_LastModifyUserId
        {
            get; set;
        }
    }
}
