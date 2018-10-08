using DataSynchronizationLib.DataSetPopTableAdapters;
using NFine.Code;
using NFine.Data;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Repository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.APPManage
{
    public class user_leaveRepository : RepositoryBase<user_leaveEntity>, Iuser_leaveRepository
    {
        public salesActualChangeRes leave(user_leaveEntity entity)
        {
            
            
            this.Insert(entity);
            ImarketSalesRepository salesRep = new marketSalesRepository();
            String ShopCode = "";
            String SalesNo = "";
            //ShopCode = ;
            SalesNo = salesRep.GetUserInfo(entity.userid).SalesNo;
            var res = from c in dbcontext.Set<marketSalesEntity>()
                      where c.sales_No.Equals(SalesNo) && c.Active == 1
                      select c;
    
            if (res.Count() >= 1)
            {
           
            try
                {
                
                    
                
                
                    ShopCode = res.First().sales_ShopNo;
              
                DateTime nowTime = System.DateTime.Now;
                    string CREATE_DATE = nowTime.ToString("yyyyMMddHHmmss");
                    string lvType = "LV01";
                    ///1	产假
                    ///2	病假
                    ///3	事假
                    ///4	婚假
                    ///5	丧假
                    ///6	护理假
                    ///7	年休假
                    ///8	休息
                    if (entity.leave_type.Equals("1"))
                    {
                        lvType = "LV03";
                    }
                    else if (entity.leave_type.Equals("2"))
                    {
                        lvType = "LV04";
                    }
                    else if (entity.leave_type.Equals("3"))
                    {
                        lvType = "LV02";
                    }
                    else if (entity.leave_type.Equals("4"))
                    {
                        lvType = "LV05";
                    }
                    else if (entity.leave_type.Equals("5"))
                    {
                        lvType = "LV06";
                    }
                    else if (entity.leave_type.Equals("6"))
                    {
                        lvType = "LV07";
                    }
                    else if (entity.leave_type.Equals("7"))
                    {
                        lvType = "LV01";
                    }
                    else if (entity.leave_type.Equals("8"))
                    {
                        lvType = "LV08";
                    }

                    DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_LV_DATA_UPLOAD1TableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_LV_DATA_UPLOAD1TableAdapter();
                    ad.InsertQuery(entity.id, 1, SalesNo, CREATE_DATE, CREATE_DATE, "PSIadmin_APP@" + CREATE_DATE + "@APP", "PSIadmin_APP@" + CREATE_DATE + "@APP", 1, "FLNET",
                        ShopCode, SalesNo, lvType, entity.StartDateTime.Value, entity.StartDateTime.Value.ToString("hhmm"), entity.EndDateTime.Value, entity.EndDateTime.Value.ToString("hhmm"),
                        entity.desc, entity.id, 1, "PSIadmin_APP", nowTime, nowTime, 0);

                    QueriesTableAdapter tad = new QueriesTableAdapter();
                    String outMessage = "";

                    tad.SP_SALES_BATCH_LV_UPLOAD_A("FLNET", entity.id, out outMessage);
                    if (!outMessage.Equals("OK"))
                    {
                        throw new Exception(outMessage);
                    }
                    entity.is_Sync = 1;
                    this.Update(entity);

                    

                    return new salesActualChangeRes() { errorCode = "0001", isOk = true, errorMessage = "请假成功" };
                }
                catch (Exception ex){
                    this.Delete(entity);
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
                }
            }
            else
            {
                this.Delete(entity);
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "请假失败，没有对应门店，无法使用该功能" };
            }
            
        }
    }
}
