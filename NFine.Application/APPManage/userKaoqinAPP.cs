using NFine.Code;
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
    public class userKaoqinAPP
    {
        private IuserKaoqinRepository service = new userKaoqinRepository();
        public void Submit(userKaoqinEntity itemsEntity)
        {
           
            //itemsEntity.id = Common.GuId();
            service.Submit(itemsEntity);
        }
        public salesActualChangeRes Submit2(userKaoqinEntity itemsEntity)
        {
            //获取地址
           return service.Submit2(itemsEntity);
        }
        public List<userKaoqinEntity> GetClock(string userId)
        {
            DateTime day = DateTime.Parse(System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            return  service.IQueryable().Where(p => p.checkTime >day && p.userId.Equals(userId) && p.is_Sync==1).ToList();
        }
        public List<userKaoqinEntity> GetClock(string userId,string startDate,string endDate)
        {
            //DateTime day = DateTime.Parse(System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime starday = DateTime.Parse(startDate + " 00:00:00");
            DateTime endday = DateTime.Parse(endDate + " 23:59:59");
            return service.IQueryable().Where(p => p.checkTime >= starday && p.checkTime<= endday && p.userId.Equals(userId) && p.is_Sync == 1).ToList();
        }
    }
}
