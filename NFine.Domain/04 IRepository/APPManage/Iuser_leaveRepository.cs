using NFine.Data;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._04_IRepository.APPManage
{
    public interface Iuser_leaveRepository : IRepositoryBase<user_leaveEntity>
    {
        salesActualChangeRes leave(user_leaveEntity entity);
    }
}
