using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Repository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.APPManage
{
    public class marketMessageListApp
    {
        ImarketMessageListRepository repository = new marketMessageListRepository();

        IsysUserPushMessageRepository repository2 = new sysUserPushMessageRepository();
        IsysUserPushDeviceRepository deviceRep = new sysUserPushDeviceRepository();
        public salesActualChangeRes regDevice(string userid, string deviceType, string pushId)
        {
            try
            {
                deviceRep.Delete(p => p.pushToken.Equals(pushId));
                deviceRep.Insert(new sysUserPushDeviceEntity() { id = System.Guid.NewGuid().ToString(), deviceType = deviceType, pushToken = pushId, userId = userid });
                return new salesActualChangeRes() { errorCode = "0001", isOk = true, errorMessage = "注册成功！" };

            }
            catch (Exception ex)
            {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
            }

        }
        public List<marketMessageListEntity> getMessageList()
        {
            return repository.IQueryable().OrderByDescending(p => p.CREATE_DATE).ToList(); ;
        }
        public salesActualChangeRes DeleteMessage(string id)
        {
            try
            {
                var ent = repository2.FindEntity(id);
            ent.isDeleted = 1;
            repository2.Update(ent);
                return new salesActualChangeRes() { errorCode = "0001", isOk = true, errorMessage = "删除成功！" };
            }
            catch (Exception ex)
            {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
    }
}
        public List<sysUserPushMessageEntity> getMessageList(String userId)
        {
            return repository2.FindList("select  * from sys_user_pushmessage where (isDeleted is null or isDeleted=0) and userId='"+ userId + "' order by CREATE_DATE desc  limit 100").ToList();
            //return repository2.IQueryable().Where(p=>p.userId.Equals(userId)).OrderByDescending(p => p.CREATE_DATE && p.isDeleted != 1).Take(100).ToList(); ;
        }
    }
}
