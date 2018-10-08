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
using DataSynchronizationLib.DataSetPopTableAdapters;

namespace NFine.Repository.APPManage
{
    public class marketSalesOtherRepository : RepositoryBase<marketSalesOtherEntity>, ImarketSalesOtherRepository
    {
        private ImarketBrandRepository brandRepostiory = new marketBrandRepository();
        private ImarketTvsizeinfoRepository tvsizeRep = new marketTvsizeinfoRepository();
        private ImarketSalesShopRepository shopRep = new marketSalesShopRepository();
        public void SyncToPSI(marketSalesOtherEntity ent)
        {
            DataSynchronizationLib.DataSetPopTableAdapters.V_COMPETITOR_MACHINETableAdapter bandAd = new DataSynchronizationLib.DataSetPopTableAdapters.V_COMPETITOR_MACHINETableAdapter();
            string brandName = "";// ent.BRAND_CODE;//brandRepostiory.IQueryable().First(p => p.BRAND_CODE.Equals(ent.BRAND_CODE)).BRAND_NAME;
            var list = bandAd.GetDataById(ent.BRAND_CODE);
            if (list != null && list.Count > 0)
            {
                brandName= list.First().BRAND;
            }
            else
            {
                brandName= "";
            }
            string shopName = shopRep.IQueryable().First(p => p.SHOP_CODE.Equals(ent.shopCode) && p.ACTIVE_FLAG == 1).SHOP_NAME;
            int tvsize = int.Parse(ent.T_TVIZEID);//tvsizeRep.IQueryable().First(p => p.T_TVSIZEID.Equals(ent.T_TVIZEID)).TVSIZE.Value;
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_COMPETITOR_UPTableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_COMPETITOR_UPTableAdapter();
            ad.InsertQuery(ent.id, 1, ent.userid, ent.Created_Time.Value.ToString("yyyyMMddHHmmss"), ent.Modify_Time.Value.ToString("yyyyMMddHHmmss"), "PSIadmin_APP@" + ent.Created_Time.Value.ToString("yyyyMMddHHmmss") + "@APP", "PSIadmin_APP@" + ent.Modify_Time.Value.ToString("yyyyMMddHHmmss") + "@APP"
               ,1, "FLNET",ent.userid,ent.BRAND_CODE, brandName,ent.KEY_MODEL_CODE, ent.KEY_MODEL_CODE, (decimal)ent.SALES_PRICE.Value,ent.T_TVIZEID, tvsize,ent.IS_CURVED_FLAG.Value, ent.IS_OLED_FLAG.Value
               , ent.IS_QUANTUM_DOT_FLAG.Value, "",ent.id,1, "PSIadmin_APP",ent.Created_Time.Value,ent.Modify_Time,ent.shopCode, shopName,ent.IS_SMART_TV_FLAG,ent.ACT_NO,ent.ACT_NAME);
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
            QueriesTableAdapter tad = new QueriesTableAdapter();
            String outMessage = "";
            if (ent.prdFileIds != null && !ent.prdFileIds.Equals(""))
            {
                string[] fileids = ent.prdFileIds.Split(",".ToCharArray());
                string physical_path = System.DateTime.Now.ToString("yyyyMM");

                foreach (string fileid in fileids)
                {
                    //写入发票信息
                    fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.userid, System.DateTime.Now, ent.userid, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "P13055",ent.id, "PDG1351", "001", "竞品照片", ent.id,
                        "Normal", "正常", 1, "PSIadmin_APP", ent.Created_Time.Value, ent.Created_Time);
                }

            }
            tad.SP_SALES_COMPETITOR_UPLOAD("FLNET", ent.id, out outMessage);
            if (!outMessage.Equals("OK"))
            {
                throw new Exception(outMessage);
            }
        }
        public salesActualChangeRes insertSales(marketSalesOtherEntity ent)
        {
            ent.id = System.Guid.NewGuid().ToString();
           
            ent.Created_Time = System.DateTime.Now;
            ent.Modify_Time = System.DateTime.Now;

            this.Insert(ent);
            try
            {
                SyncToPSI(ent);
                ent.isSync = 1;
                this.Update(ent);
            }
            catch(Exception ex)
            {
                this.Delete(ent);
                //return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message};
            }


            return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "提报成功" };
            //return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
        }
    }
}
