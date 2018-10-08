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

namespace NFine.Repository.APPManage
{
    public class marketBrandRepository : RepositoryBase<marketBrandEntity>, ImarketBrandRepository
    {
        
    }
}
