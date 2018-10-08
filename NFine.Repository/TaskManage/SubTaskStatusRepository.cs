using NFine.Domain._04_IRepository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Code;
using NFine.Domain._03_Entity.APPManage;
using System.Data.Common;
using System.Linq.Expressions;
using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Domain._03_Entity.TaskManage;

namespace NFine.Repository.TaskManage
{
    public class SubTaskStatusRepository : RepositoryBase<SubTaskStatusEntity>, ISubTaskStatusRepository
    { 
    }
}
