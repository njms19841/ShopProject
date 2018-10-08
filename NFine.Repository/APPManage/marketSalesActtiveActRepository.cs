using NFine.Data;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Domain._02_ViewModel;
using DataSynchronizationLib;
using System.Configuration;
using DataSynchronizationLib.DataSetPopTableAdapters;

namespace NFine.Repository.APPManage
{

    public class marketSalesActtiveActRepository : RepositoryBase<marketSalesActiveActEntity>, ImarketSalesActtiveActRepository
    {
        private ImarketSalesActtiveActSubRepository repSub = new marketSalesActtiveActSubRepository();
        public salesActualChangeRes deleteAct(string id)
        {
            var ent = this.FindEntity(id);
            ent.is_Delete = 1;
            ent.Modify_Time = System.DateTime.Now;
            
            try
            {
                this.Update(ent);
                SyncToPSI(ent, repSub.IQueryable().Where(p=>p.m_id.Equals(ent.id)).ToList());
            }
            catch (Exception ex)
            {
                ent.is_Delete = 0;
                this.Update(ent);
                return new salesActualChangeRes() { isOk = false, errorCode="0001", errorMessage=ex.Message };
            }
            return new salesActualChangeRes() { isOk = true };
        }

        public salesActualChangeRes deleteActSub(string id)
        {
            try { 
            var subent = repSub.FindEntity(id);
            subent.isDelete = 1;
            repSub.Update(subent);
            var ent = this.FindEntity(subent.m_id);


            try
            {
                SyncToPSI(ent, repSub.IQueryable().Where(p => p.m_id.Equals(ent.id)).ToList());
            }
            catch (Exception ex)
            {
                subent.isDelete = 0;
                repSub.Update(subent);
                return new salesActualChangeRes() { isOk = false, errorCode = "0001", errorMessage = ex.StackTrace};
            }
            return new salesActualChangeRes() { isOk = true };
        }
            catch (Exception ex)
            {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message};
    }
}

        //ImarketSalesActtiveActRepository repository = new marketSalesActtiveActRepository();
        public salesActualChangeRes insertEnt(marketSalesActiveActEntity ent, List<marketSalesActiveActSubEntity> subEnt)
        {
            try
            {
                ent.id = System.Guid.NewGuid().ToString();
                ent.Created_Time = System.DateTime.Now;
                ent.Modify_Time = System.DateTime.Now;
                ImarketSalesActtiveActSubRepository sub = new marketSalesActtiveActSubRepository();
                var lastEnt = this.IQueryable().OrderByDescending(p => p.Created_Time).FirstOrDefault();
                ent.ACT_REPORT_NO = "ACA" + ent.Created_Time.Value.ToString("yyyyMM") + "00001";
                if (lastEnt != null && lastEnt.Created_Time.Value.Year.Equals(ent.Created_Time.Value.Year) && lastEnt.Created_Time.Value.Month.Equals(ent.Created_Time.Value.Month))
                {
                    if (lastEnt.ACT_REPORT_NO != null && !lastEnt.ACT_REPORT_NO.Equals(""))
                    {
                        int no = int.Parse(lastEnt.ACT_REPORT_NO.Substring(9));
                        no = no + 1;
                        ent.ACT_REPORT_NO = "ACA" + ent.Created_Time.Value.ToString("yyyyMM") + no.ToString("00000");
                    }
                }

                try
                {
                    this.Insert(ent);
                    int i = 1;
                    //foreach (var tempent in subEnt)
                    //{
                    //    tempent.m_id = ent.id;
                    //    tempent.id = System.Guid.NewGuid().ToString();
                    //    tempent.ACT_REPORT_ITEM_NO = i;
                    //    sub.Insert(tempent);
                    //    i = i + 1;
                    //}
                    SyncToPSI(ent, null);
                    ent.isSync = 1;
                    this.Update(ent);
                }
                catch (Exception e)
                {
                    ent.is_Delete = 1;
                    this.Update(ent);
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = e.Message };

                }
                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "上报成功" };
            }
            catch (Exception ex)
            {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message};
            }

        }

        private void SyncToPSI(marketSalesActiveActEntity ent, List<marketSalesActiveActSubEntity> subEnts)
        {
            
            DateTime date = System.DateTime.Now;
            string CREATE_DATE = date.ToString("yyyyMMddHHmmss");
            JS5_S12_SALES_ACT_DATA_M_UPTableAdapter ad = new JS5_S12_SALES_ACT_DATA_M_UPTableAdapter();
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
            JS5_S12_SALES_ACT_DATA_D_UPTableAdapter adSub = new JS5_S12_SALES_ACT_DATA_D_UPTableAdapter();
            String StatusCode = "002";
            String StatusName = "已確認";
            if (ent.is_Delete == 1)
            {
                 StatusCode = "004";
                 StatusName = "已作废";
            }
            ad.InsertQuery(ent.id, 1, ent.sales_No, CREATE_DATE, CREATE_DATE, "PSIadmin_APP@" + CREATE_DATE + "@APP", "PSIadmin_APP@" + CREATE_DATE + "@APP", 1, "FLNET",
                ent.ACT_NO,ent.ACT_NAME,ent.ACT_REPORT_NO,ent.SHOP_CODE,ent.SHOP_NAME, StatusCode, StatusName, "无","无", ent.id,1, "PSIadmin_APP",date,date);
            
            if (ent.file_id_Type002 != null && !ent.file_id_Type002.Equals(""))
            {
                string[] fileids = ent.file_id_Type002.Split(",".ToCharArray());
                string physical_path = System.DateTime.Now.ToString("yyyyMM");

                foreach (string fileid in fileids)
                {
                    //写入发票信息
                    fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.sales_No, System.DateTime.Now, ent.sales_No, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "P13051", ent.id, "PDG1350", "002", "活動照片", ent.id,
                        "Normal", "正常", 1, "PSIadmin_APP", date, date);
                }

            }
            if (ent.file_id_Type003 != null && !ent.file_id_Type003.Equals(""))
            {
                string[] fileids = ent.file_id_Type003.Split(",".ToCharArray());
                string physical_path = System.DateTime.Now.ToString("yyyyMM");

                foreach (string fileid in fileids)
                {
                    //写入发票信息
                    fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.sales_No, System.DateTime.Now, ent.sales_No, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "P13051", ent.id, "PDG1350", "003", "DM照片", ent.id,
                        "Normal", "正常", 1, "PSIadmin_APP", date, date);
                }

            }
            if (ent.file_id_Type004 != null && !ent.file_id_Type004.Equals(""))
            {
                string[] fileids = ent.file_id_Type004.Split(",".ToCharArray());
                string physical_path = System.DateTime.Now.ToString("yyyyMM");

                foreach (string fileid in fileids)
                {
                    //写入发票信息
                    fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.sales_No, System.DateTime.Now, ent.sales_No, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "P13051", ent.id, "PDG1350", "004", "其他", ent.id,
                        "Normal", "正常", 1, "PSIadmin_APP", date, date);
                }

            }
            //foreach (var subEnt in subEnts)
            //{
            //    String subStatusCode = "002";
            //    String subStatusName = "已確認";
            //    if (subEnt.isDelete == 1)
            //    {
            //        subStatusCode = "004";
            //        subStatusName = "已作废";
            //    }
            //    adSub.InsertQuery(subEnt.id, 1, ent.sales_No, CREATE_DATE, CREATE_DATE, "PSIadmin_APP@" + CREATE_DATE + "@APP", "PSIadmin_APP@" + CREATE_DATE + "@APP", 1, "FLNET"
            //  , ent.ACT_REPORT_NO, subEnt.ACT_REPORT_ITEM_NO.Value, subEnt.BRAND_CODE, subEnt.BRAND_NAME, subEnt.MACHINE_MODEL_NO, (decimal)subEnt.SALES_PRICE.Value, subEnt.T_TVSIZEID, subEnt.TVSIZE.Value, subEnt.IS_NEW_PRD_FLAG.Value
            //  , subStatusCode, subStatusName, "无", "无", subEnt.id, 1, "PSIadmin_APP", date, date);
            //    //写入图片信息
            //    if (subEnt.file_id_Type001 != null && !subEnt.file_id_Type001.Equals(""))
            //    {
            //        string[] fileids = subEnt.file_id_Type001.Split(",".ToCharArray());
            //        string physical_path = System.DateTime.Now.ToString("yyyyMM");

            //        foreach (string fileid in fileids)
            //        {
            //            //写入发票信息
            //            fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.sales_No, System.DateTime.Now, ent.sales_No, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "P13051", ent.id, "PDG1350", "001", "促銷照片", subEnt.id,
            //                "Normal", "正常", 1, "PSIadmin_APP", date, date);
            //        }

            //    }
            //}

            
            QueriesTableAdapter tad = new QueriesTableAdapter();
            String outMessage = "";

            tad.SP_SALES_ACT_DATA_UPLOAD("FLNET", ent.id, out outMessage);
            if (!outMessage.Equals("OK"))
            {
                throw new Exception(outMessage);
            }
            
        }
    }
}
