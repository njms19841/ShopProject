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
    public class marketSalesActiveActApp
    {
        private ImarketSalesActtiveActRepository rep = new marketSalesActtiveActRepository();
        private ImarketSalesActtiveActSubRepository repSub = new marketSalesActtiveActSubRepository();

        public salesActualChangeRes insertEnt(marketSalesActiveActEntity ent,List<marketSalesActiveActSubEntity> subEnt)
        {
            return rep.insertEnt(ent, subEnt);
        }
        public List<marketSalesActiveActEntity> getAct(string salesNo,string activeNo)
        {
            return rep.IQueryable().Where(p => p.sales_No.Equals(salesNo) && p.is_Delete != 1 && p.ACT_NO.Equals(activeNo)).ToList();
        }
        public int getActCount(string salesNo, string activeNo,string shopCode)
        {
            DateTime now = System.DateTime.Now;
            DateTime startTime = DateTime.Parse(now.ToString("yyyy-MM-dd ") + " 00:00:00");
            DateTime endTime = DateTime.Parse(now.ToString("yyyy-MM-dd ") + " 23:59:59");
            return rep.IQueryable().Where(p => p.sales_No.Equals(salesNo)&& p.SHOP_CODE.Equals(shopCode) && p.is_Delete != 1 && p.ACT_NO.Equals(activeNo) && p.Created_Time >= startTime && p.Created_Time<= endTime).Count();
        }
        public List<marketSalesActiveActSubEntity> getActSub(string id)
        {
            return repSub.IQueryable().Where(p => p.m_id.Equals(id) && p.isDelete != 1).ToList();
        }
        public salesActualChangeRes deleteAct(string id)
        {

            return rep.deleteAct(id);


        }
        public salesActualChangeRes deleteActSub(string id)
        {
            return rep.deleteActSub(id);

        }

    }
}
