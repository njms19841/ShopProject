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
using DataSynchronizationStanbyLib.DataSetStanbyTableAdapters;
using NFine.Domain.IRepository.SystemSecurity;
using NFine.Repository.SystemSecurity;

namespace NFine.Repository.APPManage
{
    
    public class marketSalesActualRepository : RepositoryBase<marketSalesActualEntity>, ImarketSalesActualRepository
    {
        public salesActualChangeRes SuppUp(String id, String salesNo, String filesId)
        {
            try
            {



                V_SALES_TRANS_APP_QUERYTableAdapter transAd = new V_SALES_TRANS_APP_QUERYTableAdapter();
                var ent = transAd.GetDataByID(id);


                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Monday)//
                {
                    if (!ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.ToString("yyyyMMdd"))
                     && !ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))
                     && !ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.AddDays(-2).ToString("yyyyMMdd"))
                     && !ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.AddDays(-3).ToString("yyyyMMdd"))
                     && !ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.AddDays(-4).ToString("yyyyMMdd"))
                     && !ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.AddDays(-5).ToString("yyyyMMdd"))
                     && !ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.AddDays(-6).ToString("yyyyMMdd"))
                     && !ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.AddDays(-7).ToString("yyyyMMdd"))
                     )
                    {
                        return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "补录发票失败，只能补录今天和上周的发票" };
                    }
                }
                else
                {
                    if (!ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.ToString("yyyyMMdd"))
                     && !ent.First().SALES_DATE.ToString("yyyyMMdd").Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))
                    
                         )
                    {
                        return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "补录发票失败，只能补录今天和昨天的发票" };
                    }

                }
                //    else
                //    {
                //        if (!ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))

                //            )
                //        {
                //            return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正今天和昨天的销售数据" };
                //        }
                //    }
               


                //var ent = this.IQueryable().Where(p=>p.i);
                //ent.file_id = filesId;
                //ent.Modify_Time = System.DateTime.Now;
                //this.Update(ent);
                if (!ent.First().EMPLOYEE_ID.Equals(salesNo))
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法补录其他人提交的资料！" };
                }
                JS5_S12_SALES_ATT_SUPP_UPTableAdapter ad = new JS5_S12_SALES_ATT_SUPP_UPTableAdapter();
                String newId = System.Guid.NewGuid().ToString();
                DateTime now = System.DateTime.Now;
                ad.InsertQuery(newId, 1, salesNo, now.ToString("yyyyMMddHHmmss"), now.ToString("yyyyMMddHHmmss"), "PSIadmin_APP@" + now.ToString("yyyyMMddHHmmss") + "@APP", "PSIadmin_APP@" + now.ToString("yyyyMMddHHmmss") + "@APP"
                   , 1, "FLNET", id, "SalesSupp", "无", newId, 1, salesNo, now, now);
                DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
                QueriesTableAdapter tad = new QueriesTableAdapter();
                if (filesId != null && !filesId.Equals(""))
                {
                    string[] fileids = filesId.Split(",".ToCharArray());
                    string physical_path = System.DateTime.Now.ToString("yyyyMM");

                    foreach (string fileid in fileids)
                    {
                        //写入发票信息
                        fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", salesNo, System.DateTime.Now, salesNo, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "P14019", newId, "PDG1342", "001", "發票照片", newId,
                            "Normal", "正常", 1, "PSIadmin_APP", now, now);
                    }

                }
                String outMessage = "";
                tad.SP_SALES_ATT_SUPP_UP("FLNET", newId, out outMessage);
                if (!outMessage.Equals("OK"))
                {
                    throw new Exception(outMessage);
                }
                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "上报成功" };
            }
            catch (Exception ex)
            {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
            }


        }
        public List<salesActualAllModel> GetSalesActualByUserId(string UserId)
        {
            aspnetusersEntity ent = dbcontext.Set<aspnetusersEntity>().AsQueryable().Where(p => p.Id.Equals(UserId)).First();


            //var t = from  p in dbcontext.Set<marketSalesEntity>()
                //    join c in dbcontext.Set<aspnetusersEntity>() on new { PhoneNumber = p.sales_No } equals new { PhoneNumber = c.EMPLOYEE_CODE }
                 //   where c.Id.Equals(UserId) && c.active==1
                  //  select p
                    ;
            string sn = ent.EMPLOYEE_CODE;



            DateTime date = System.DateTime.Now;
            string CREATE_DATE = date.ToString("yyyyMMdd");
            string CREATE_DATE2 = date.AddDays(-1).ToString("yyyyMMdd");

            var res = from x in dbcontext.Set<marketSalesActualEntity>()
                      join p in dbcontext.Set<marketSalesShopEntity>() on new { ShopCode = x.SHOP_CODE } equals new { ShopCode = p.SHOP_CODE }
                      where x.sales_No.Equals(sn) && (x.Actual_Day.Equals(CREATE_DATE)|| x.Actual_Day.Equals(CREATE_DATE2))
                      select new salesActualAllModel()
                      {
                          id = x.id,
                          CREATOR_ID = x.sales_No,
                          CREATE_DATE = CREATE_DATE,
                          CREATE_INFO = "PSIadmin_APP@" + CREATE_DATE + "@APP",
                          ACTIVE_FLAG = 1,
                          COMPANY_CODE = "FLNET",
                          SALES_DATE = x.Actual_Day,
                          T_BUID = p.T_BUID,
                          T_BUNAME = p.T_BUNAME,
                          MACHINE_MODEL_NO = x.MACHINE_MODEL_NO,
                          SALES_QTY = x.Actual_Qty.Value,
                          SALES_PRICE = x.Actual_Price.Value,
                          SALES_AMOUNT = x.Actual_Qty.Value * x.Actual_Price.Value,
                          T_TYPEID = p.T_TYPEID,
                          T_TYPENAME = p.T_TYPENAME,
                         
                          CH_SALES_CODE = p.DEALERE_CODE,
                          CH_SALES_NAME = p.DEALERE_NAME,
                          CH_SHOP = p.SHOP_NAME,
                          CH_SHOP_CODE = p.SHOP_CODE,
                          CH_QTY = x.Actual_Qty.Value,
                          CH_PRICE = x.Actual_Price.Value,
                          CH_AMOUNT = x.Actual_Qty.Value * x.Actual_Price.Value,
                          SHOP_NAME = p.SHOP_NAME,
                          CH_REPORT_ID = x.sales_No,
                         
                          CH_MEMO = "",
                          UP_FINISH_FLAG = 1,
                          UP_BY = "PSIadmin_APP",
                          UP_DATETIME = date,
                          A_TYPE = x.Actual_Type
                      };

            return res.ToList();
        }
        public List<salesActualAllModel> GetSalesActual()
        {
            DateTime date = System.DateTime.Now;
            string CREATE_DATE = date.ToString("yyyyMMddHHmmss");

            var res = from x in dbcontext.Set<marketSalesActualEntity>()
                      join p in dbcontext.Set<marketSalesShopEntity>() on new { ShopCode = x.SHOP_CODE } equals new { ShopCode = p.SHOP_CODE }
                      join j in dbcontext.Set<marketTvsizeinfoEntity>() on new { Tvsizeid = x.T_TVSIZEID } equals new { Tvsizeid = j.T_TVSIZEID }
                      join t in dbcontext.Set<marketSalesEntity>() on new { sales_No = x.sales_No, ShopCode = x.SHOP_CODE } equals new { sales_No = t.sales_No, ShopCode = t.sales_ShopNo }
                      where x.isSync == 0
                      select new salesActualAllModel()
                      {
                          id=x.id,
                          CREATOR_ID = x.sales_No,
                          CREATE_DATE = CREATE_DATE,
                          CREATE_INFO = "PSIadmin_APP@" + CREATE_DATE + "@APP",
                          ACTIVE_FLAG = 1,
                          COMPANY_CODE = "FLNET",
                          SALES_DATE = x.Actual_Day,
                          T_BUID = p.T_BUID,
                          T_BUNAME = p.T_BUNAME,
                          MACHINE_MODEL_NO = x.MACHINE_MODEL_NO,
                          SALES_QTY = x.Actual_Qty.Value,
                          SALES_PRICE = x.Actual_Price.Value,
                          SALES_AMOUNT = x.Actual_Qty.Value * x.Actual_Price.Value,
                          T_TYPEID = p.T_TYPEID,
                          T_TYPENAME = p.T_TYPENAME,
                          T_TVSIZEID = j.T_TVSIZEID,
                          TVSIZE = j.TVSIZE.Value,
                          CH_SALES_CODE = p.DEALERE_CODE,
                          CH_SALES_NAME = p.DEALERE_NAME,
                          CH_SHOP = p.SHOP_NAME,
                          CH_SHOP_CODE = p.SHOP_CODE,
                          CH_QTY = x.Actual_Qty.Value,
                          CH_PRICE = x.Actual_Price.Value,
                          CH_AMOUNT = x.Actual_Qty.Value * x.Actual_Price.Value,
                          SHOP_NAME = p.SHOP_NAME,
                          CH_REPORT_ID = x.sales_No,
                          CH_REPORT_NAME = t.sales_Name,
                          CH_REPORT_PHONE_NO = t.sales_PhoneNumber,
                          CH_MEMO = "",
                          UP_FINISH_FLAG = 1,
                          UP_BY = "PSIadmin_APP",
                          UP_DATETIME = date, A_TYPE=x.Actual_Type
                      };
                   
            return res.ToList() ;



        }
        public void SyncToPSI(String id,String UP_TYPE_CODE,String UP_TYPE_NAME)
        {
            salesActualAllModel ent = GetSalesActualByid(id);
            TF_BUFACTORYREFINFOTableAdapter factAd = new DataSynchronizationLib.DataSetPopTableAdapters.TF_BUFACTORYREFINFOTableAdapter();
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_TRANS_UPLOADTableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_TRANS_UPLOADTableAdapter();
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
            DataSetPop.TF_BUFACTORYREFINFODataTable subtable = factAd.GetDataBy(ent.T_BUID);
            string fid = "";
            string fname = "";
            foreach (DataSetPop.TF_BUFACTORYREFINFORow row in subtable)
            {
                fid = row.T_FACTORYID;
                fname= row.T_FACTORYID;
            }
            //string newid = System.Guid.NewGuid().ToString();
            ad.InsertQuery(ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, ent.ACTIVE_FLAG, ent.COMPANY_CODE, System.DateTime.ParseExact(ent.SALES_DATE, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),  ent.T_BUID, ent.T_BUNAME, fid, fname
                , ent.MACHINE_MODEL_NO, ent.SALES_QTY, (decimal)ent.SALES_PRICE, (decimal)ent.SALES_AMOUNT, ent.T_TYPEID, ent.T_TYPENAME, ent.T_TVSIZEID, (decimal)ent.TVSIZE, ent.CH_SALES_CODE,
                ent.CH_SALES_NAME, ent.CH_SHOP, ent.CH_SHOP_CODE, ent.CH_QTY, (decimal)ent.CH_PRICE, (decimal)ent.CH_AMOUNT, ent.SHOP_NAME, ent.CH_REPORT_NAME, ent.CH_REPORT_PHONE_NO,
                ent.CH_MEMO, ent.UP_BY, ent.UP_DATETIME, id, 1, UP_TYPE_CODE, UP_TYPE_NAME,ent.UP_FINISH_FLAG,ent.MACHINE_MODEL_NO,ent.GOODS_TYPE_CODE,ent.GOODS_TYPE_NAME,"",ent.CONSUMER_NAME,ent.CONSUMER_PHONE_NO,ent.ACT_NO,ent.ACT_NAME
                ,ent.BOARD_FLAG,null,null, ent.SAMPLE_SN_NO);
            if (ent.file_id!=null && !ent.file_id.Equals(""))
            {
               string[] fileids= ent.file_id.Split(",".ToCharArray());
                string physical_path = System.DateTime.Now.ToString("yyyyMM");

                foreach (string fileid in fileids)
                {
                    //写入发票信息
                    fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.CREATOR_ID, System.DateTime.Now, ent.CREATOR_ID, System.DateTime.Now, 1, "jpg", 0, "", id + ".jpg", physical_path, "", "APP", id, "PDG1342", "001", "发票照片", id,
                        UP_TYPE_CODE, UP_TYPE_NAME, 1, ent.UP_BY, ent.UP_DATETIME, ent.UP_DATETIME);
                }

            }
            QueriesTableAdapter tad = new QueriesTableAdapter();
            String outMessage = "";
           
            tad.SP_SALES_BATCH_TRANS_UPLOAD_A(ent.COMPANY_CODE, id, out outMessage);
            if (!outMessage.Equals("OK"))
            {
                throw new Exception(outMessage);
            }
        }
        public void SyncToPSI2(String id, String UP_TYPE_CODE, String UP_TYPE_NAME,DataSynchronizationStanbyLib.DataSetStanby.V_SALES_TRANS_APP_QUERYRow oldData)
        {
            salesActualAllModel ent = GetSalesActualByid(id);
            TF_BUFACTORYREFINFOTableAdapter factAd = new DataSynchronizationLib.DataSetPopTableAdapters.TF_BUFACTORYREFINFOTableAdapter();
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_TRANS_UPLOADTableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_TRANS_UPLOADTableAdapter();
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
            JS5_S12_SALES_TRANS_CONSUMER_UTableAdapter cusAd = new JS5_S12_SALES_TRANS_CONSUMER_UTableAdapter();
            //DataSetPop.TF_BUFACTORYREFINFODataTable subtable = factAd.GetDataBy(ent.T_BUID);
            string fid = "OT";
            string fname = "其他";
            /*foreach (DataSetPop.TF_BUFACTORYREFINFORow row in subtable)
            {
                fid = row.T_FACTORYID;
                fname = row.T_FACTORYID;
            }*/
            //string newid = System.Guid.NewGuid().ToString();
            if (ent.Actual_Type.Equals("1")|| ent.Actual_Type.Equals("3"))
            {
                if (ent.Actual_Type.Equals("3"))
                {
                    ent.SALES_DATE = System.DateTime.Now.ToString("yyyyMMdd");
                }
                else {
                    ent.CH_NO = null;
                }
                ad.InsertQuery(ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, ent.ACTIVE_FLAG, ent.COMPANY_CODE, System.DateTime.ParseExact(ent.SALES_DATE, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture), "NA", "NA", fid, fname
                    , ent.MACHINE_MODEL_NO, ent.SALES_QTY, (decimal)ent.SALES_PRICE, (decimal)ent.SALES_AMOUNT, ent.T_TYPEID, ent.T_TYPENAME, ent.T_TVSIZEID, (decimal)ent.TVSIZE, ent.CH_SALES_CODE,
                    ent.CH_SALES_NAME, ent.CH_SHOP, ent.CH_SHOP_CODE, ent.CH_QTY, (decimal)ent.CH_PRICE, (decimal)ent.CH_AMOUNT, ent.SHOP_NAME, ent.CH_REPORT_NAME, ent.CH_REPORT_PHONE_NO,
                    ent.CH_MEMO, ent.UP_BY, ent.UP_DATETIME, id, 1, UP_TYPE_CODE, UP_TYPE_NAME, ent.UP_FINISH_FLAG, ent.MACHINE_MODEL_NO, ent.GOODS_TYPE_CODE, ent.GOODS_TYPE_NAME, "", ent.CONSUMER_NAME, ent.CONSUMER_PHONE_NO, ent.ACT_NO, ent.ACT_NAME
                    , ent.BOARD_FLAG, ent.CH_NO, 1,ent.SAMPLE_SN_NO);

                cusAd.InsertQuery(Guid.NewGuid().ToString(), 1, ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, "PSIadmin_APP@" + ent.CREATE_DATE + "@APP", "PSIadmin_APP@" + ent.CREATE_DATE + "@APP", 1, "FLNET"
                    , id, ent.CONSUMER_NAME, "", 0, ent.CONSUMER_AGE, ent.CONSUMER_XINGBIE, ent.CONSUMER_ARERID, ent.CONSUMER_ADD, "", "", ent.CONSUMER_PHONE_NO, "", DateTime.Parse("1900-01-01"), "", "", "无", id, 1,
                     ent.UP_BY, ent.UP_DATETIME, ent.UP_DATETIME
                    );
                if (ent.file_id != null && !ent.file_id.Equals(""))
                {
                    string[] fileids = ent.file_id.Split(",".ToCharArray());
                    string physical_path = System.DateTime.Now.ToString("yyyyMM");

                    foreach (string fileid in fileids)
                    {
                        //写入发票信息
                        fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.CREATOR_ID, System.DateTime.Now, ent.CREATOR_ID, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "APP", id, "PDG1342", "001", "发票照片", id,
                            UP_TYPE_CODE, UP_TYPE_NAME, 1, ent.UP_BY, ent.UP_DATETIME, ent.UP_DATETIME);
                    }

                }
                QueriesTableAdapter tad = new QueriesTableAdapter();
                String outMessage = "";

                tad.SP_SALES_BATCH_TRANS_UPLOAD_A(ent.COMPANY_CODE, id, out outMessage);
                if (!outMessage.Equals("OK"))
                {
                    throw new Exception(outMessage);
                }
            }
            else
            {
                string newid = Guid.NewGuid().ToString();
                ad.InsertQuery(ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, 1, ent.COMPANY_CODE, oldData.SALES_DATE, "NA", "NA", fid, fname
                   , oldData.MACHINE_MODEL_NO, -oldData.SALES_QTY, oldData.SALES_PRICE, -oldData.SALES_AMOUNT, oldData.T_TYPEID, oldData.T_TYPENAME, oldData.T_TVSIZEID, oldData.TVSIZE, ent.CH_SALES_CODE,
                   ent.CH_SALES_NAME, ent.CH_SHOP, ent.CH_SHOP_CODE, oldData.SALES_QTY, oldData.SALES_PRICE, oldData.SALES_AMOUNT, oldData.SHOP_NAME, ent.CH_REPORT_NAME, ent.CH_REPORT_PHONE_NO,
                   ent.CH_MEMO, ent.UP_BY, ent.UP_DATETIME, newid, 1, UP_TYPE_CODE, UP_TYPE_NAME, ent.UP_FINISH_FLAG, oldData.MACHINE_MODEL_NO, oldData.GOODS_TYPE_CODE, oldData.GOODS_TYPE_NAME, "", oldData.CONSUMER_NAME, oldData.CONSUMER_PHONE_NO, oldData.ACT_NO, oldData.ACT_NAME
                   , oldData.BOARD_FLAG,ent.CH_NO , 0, ent.SAMPLE_SN_NO);
                cusAd.InsertQuery(Guid.NewGuid().ToString(), 1, ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, "PSIadmin_APP@" + ent.CREATE_DATE + "@APP", "PSIadmin_APP@" + ent.CREATE_DATE + "@APP", 1, "FLNET"
                    , newid, ent.CONSUMER_NAME, "", 0, ent.CONSUMER_AGE, ent.CONSUMER_XINGBIE, ent.CONSUMER_ARERID, ent.CONSUMER_ADD, "", "", ent.CONSUMER_PHONE_NO, "", DateTime.Parse("1900-01-01"), "", "", "无", newid, 1,
                     ent.UP_BY, ent.UP_DATETIME, ent.UP_DATETIME
                    );
                QueriesTableAdapter tad = new QueriesTableAdapter();
                String outMessage = "";

                tad.SP_SALES_BATCH_TRANS_UPLOAD_A(ent.COMPANY_CODE, newid, out outMessage);
                if (!outMessage.Equals("OK"))
                {
                    throw new Exception(outMessage);
                }

                ad.InsertQuery(ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, ent.ACTIVE_FLAG, ent.COMPANY_CODE, System.DateTime.ParseExact(ent.SALES_DATE, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture), "NA", "NA", fid, fname
                  , ent.MACHINE_MODEL_NO, ent.SALES_QTY, (decimal)ent.SALES_PRICE, (decimal)ent.SALES_AMOUNT, ent.T_TYPEID, ent.T_TYPENAME, ent.T_TVSIZEID, (decimal)ent.TVSIZE, ent.CH_SALES_CODE,
                  ent.CH_SALES_NAME, ent.CH_SHOP, ent.CH_SHOP_CODE, ent.CH_QTY, (decimal)ent.CH_PRICE, (decimal)ent.CH_AMOUNT, ent.SHOP_NAME, ent.CH_REPORT_NAME, ent.CH_REPORT_PHONE_NO,
                  ent.CH_MEMO, ent.UP_BY, ent.UP_DATETIME, id, 1, UP_TYPE_CODE, UP_TYPE_NAME, ent.UP_FINISH_FLAG, ent.MACHINE_MODEL_NO, ent.GOODS_TYPE_CODE, ent.GOODS_TYPE_NAME, "", ent.CONSUMER_NAME, ent.CONSUMER_PHONE_NO, ent.ACT_NO, ent.ACT_NAME
                  , ent.BOARD_FLAG, ent.CH_NO, 1, ent.SAMPLE_SN_NO);
                cusAd.InsertQuery(Guid.NewGuid().ToString(), 1, ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, "PSIadmin_APP@" + ent.CREATE_DATE + "@APP", "PSIadmin_APP@" + ent.CREATE_DATE + "@APP", 1, "FLNET"
                    , id, ent.CONSUMER_NAME, "", 0, ent.CONSUMER_AGE, ent.CONSUMER_XINGBIE, ent.CONSUMER_ARERID, ent.CONSUMER_ADD, "", "", ent.CONSUMER_PHONE_NO, "", DateTime.Parse("1900-01-01"), "", "", "无", id, 1,
                     ent.UP_BY, ent.UP_DATETIME, ent.UP_DATETIME
                    );
                if (ent.file_id != null && !ent.file_id.Equals(""))
                {
                    string[] fileids = ent.file_id.Split(",".ToCharArray());
                    string physical_path = System.DateTime.Now.ToString("yyyyMM");

                    foreach (string fileid in fileids)
                    {
                        //写入发票信息
                        fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.CREATOR_ID, System.DateTime.Now, ent.CREATOR_ID, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "APP", id, "PDG1342", "001", "发票照片", id,
                            UP_TYPE_CODE, UP_TYPE_NAME, 1, ent.UP_BY, ent.UP_DATETIME, ent.UP_DATETIME);
                    }

                }
                
                 outMessage = "";

                tad.SP_SALES_BATCH_TRANS_UPLOAD_A(ent.COMPANY_CODE, id, out outMessage);
                if (!outMessage.Equals("OK"))
                {
                    throw new Exception(outMessage);
                }
            }
            
        }
        public salesActualAllModel GetSalesActualByid(String id)
        {
            DateTime date = System.DateTime.Now;
            string CREATE_DATE = date.ToString("yyyyMMddHHmmss");

            var res = from x in dbcontext.Set<marketSalesActualEntity>()
                      join p in dbcontext.Set<marketSalesShopEntity>() on new { ShopCode = x.SHOP_CODE } equals new { ShopCode = p.SHOP_CODE }
                      join j in dbcontext.Set<marketTvsizeinfoEntity>() on new { Tvsizeid = x.T_TVSIZEID } equals new { Tvsizeid = j.T_TVSIZEID }
                      join t in dbcontext.Set<marketSalesEntity>() on new { sales_No = x.sales_No, ShopCode = x.SHOP_CODE } equals new { sales_No = t.sales_No, ShopCode = t.sales_ShopNo }
                      where x.isSync ==0 && x.id.Equals(id)
                      select new salesActualAllModel()
                      {
                          CREATOR_ID = x.sales_No,
                          CREATE_DATE = CREATE_DATE,
                          CREATE_INFO = "PSIadmin_APP@" + CREATE_DATE + "@APP",
                          ACTIVE_FLAG = 1,
                          COMPANY_CODE = "FLNET",
                          SALES_DATE =x.Actual_Day,
                          T_BUID = p.T_BUID,
                          T_BUNAME = p.T_BUNAME,
                          MACHINE_MODEL_NO = x.MACHINE_MODEL_NO,
                          SALES_QTY = x.Actual_Qty.Value,
                          SALES_PRICE = x.Actual_Price.Value,
                          SALES_AMOUNT = x.Actual_Qty.Value * x.Actual_Price.Value,
                          T_TYPEID = p.T_TYPEID,
                          T_TYPENAME = p.T_TYPENAME,
                          T_TVSIZEID = j.T_TVSIZEID,
                          TVSIZE = j.TVSIZE.Value,
                          CH_SALES_CODE = p.DEALERE_CODE,
                          CH_SALES_NAME = p.DEALERE_NAME,
                          CH_SHOP = p.SHOP_NAME,
                          CH_SHOP_CODE = p.SHOP_CODE,
                          CH_QTY = x.Actual_Qty.Value,
                          CH_PRICE = x.Actual_Price.Value,
                          CH_AMOUNT = x.Actual_Qty.Value * x.Actual_Price.Value,
                          SHOP_NAME = p.SHOP_NAME,
                          CH_REPORT_ID = x.sales_No,
                          CH_REPORT_NAME = t.sales_Name,
                          CH_REPORT_PHONE_NO = t.sales_PhoneNumber,
                          CH_MEMO = "",
                          UP_FINISH_FLAG = 1,
                          UP_BY = "PSIadmin_APP",
                          UP_DATETIME = date,
                          GOODS_TYPE_CODE=x.GOODS_TYPE_CODE,
                          GOODS_TYPE_NAME=x.GOODS_TYPE_NAME,
                          CONSUMER_NAME=x.CONSUMER_NAME,
                          CONSUMER_PHONE_NO=x.CONSUMER_PHONE_NO,
                           file_id=x.file_id,
                            ACT_NO=x.ACT_NO,
                            ACT_NAME=x.ACT_NAME, BOARD_FLAG=(x.BOARD_FLAG.HasValue? x.BOARD_FLAG.Value:0),
                             Actual_Type=x.Actual_Type, CH_NO=x.CH_NO, CONSUMER_XINGBIE=x.CONSUMER_XINGBIE,
                              CONSUMER_ADD=x.CONSUMER_ADD, CONSUMER_AGE=x.CONSUMER_AGE, CONSUMER_ARERID=x.CONSUMER_ARERID, SAMPLE_SN_NO=x.SAMPLE_SN_NO
                              
                      };
            if (res.Count() > 0)
            {
                return res.ToList()[0];
            }
            return null;



        }

        public salesActualChangeRes SalesActual(marketSalesActualEntity ent)
        {
            if(ent.Actual_Price <=0)
             {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,销售单价必须大于0" };
            }
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_INVENTORY_INFOTableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_INVENTORY_INFOTableAdapter();

            string CheckInv = ConfigurationManager.AppSettings["CheckInv"];

            string Actual_Day = System.DateTime.Now.ToString("yyyyMMdd");
            if (ent.Actual_Type.Equals("1")) //表示新增销售实际
            {
                

                    if (ent.Actual_Qty <= 0)
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "销量错误,必须是大于0的数字" };
                    }

                    //检查今天是否已上报
                    //if (this.IQueryable().Where(p => p.Actual_Day.Equals(Actual_Day) && p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO) && p.sales_No.Equals(ent.sales_No) && p.SHOP_CODE.Equals(ent.SHOP_CODE)).Count() > 0)
                    //{
                    //    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型今天已经上报过销量，如需要修改请使用销售更正" };

                    //}
                if (CheckInv.Equals("true"))
                {
                    //检查库存是否够
                    try
                    {
                        var res = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;


                        DataSetPop.JS5_S12_INVENTORY_INFODataTable tagel = ad.GetDataBy(res.First().CUSTOMER_CODE, ent.MACHINE_MODEL_NO);
                        if (tagel.Rows.Count > 0)
                        {
                            DataSetPop.JS5_S12_INVENTORY_INFORow row = tagel[0];
                            decimal totalQty = row.INVENTORY_QTY - row.ALLOCATED_QTY - row.WAIT_SHIP_QTY;
                            if (totalQty < ent.Actual_Qty)
                            {
                                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                            }
                        }
                        else
                        {
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                        }
                    }
                    catch
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                    }
                }
                    ent.id = System.Guid.NewGuid().ToString();
                    ent.Actual_Day = Actual_Day;
                    ent.Created_Time = System.DateTime.Now;
                    ent.Modify_Time = System.DateTime.Now;
                   
                    this.Insert(ent);






                try
                {
                    SyncToPSI(ent.id, "Normal", "正常");
                    ent.isSync = 1;
                    this.Update(ent);
                }
                catch (Exception ex) {
                    this.Delete(ent);
                     return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                    //return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
                }

                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "上报成功" };
               


            }
            else if (ent.Actual_Type.Equals("2"))//表示销售实际修正
            {
                if (!ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))
                     && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-2).ToString("yyyyMMdd"))
                    )
                {
                    return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正今天和昨天前天的销售数据" };
                }
                //if (ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && this.IQueryable().Where(p => p.Actual_Day.Equals(ent.Actual_Day) && p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO) && p.sales_No.Equals(ent.sales_No) && p.SHOP_CODE.Equals(ent.SHOP_CODE)).Count() <= 0)
                //{
                //    return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "还未做销量上报,无法修正.如是要提报销量请使用销售上报" };

                //}

                if (ent.Actual_Qty > 0 && CheckInv.Equals("true"))
                    {
                        //检查库存是否够
                        try
                        {
                            var res = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;


                            DataSetPop.JS5_S12_INVENTORY_INFODataTable tagel = ad.GetDataBy(res.First().CUSTOMER_CODE, ent.MACHINE_MODEL_NO);
                            if (tagel.Rows.Count > 0)
                            {
                                DataSetPop.JS5_S12_INVENTORY_INFORow row = tagel[0];
                                decimal totalQty = row.INVENTORY_QTY - row.ALLOCATED_QTY - row.WAIT_SHIP_QTY;
                                if (totalQty < ent.Actual_Qty)
                                {
                                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                                }
                            }
                            else
                            {
                                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                            }
                        }
                        catch
                        {
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                        }
                    }

                    ent.id = System.Guid.NewGuid().ToString();
                    ent.Actual_Day = ent.Actual_Day;
                    ent.Created_Time = System.DateTime.Now;
                    ent.Modify_Time = System.DateTime.Now;
                    this.Insert(ent);
                try
                {
                    SyncToPSI(ent.id, "Change", "調整");
                    ent.isSync = 1;
                    this.Update(ent);
                }
                catch (Exception e){
                    this.Delete(ent);
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                }

                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "更正成功" };
               
            }
            else if (ent.Actual_Type.Equals("3"))//表示销退
            {
                
                    if (ent.Actual_Qty >0 )
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "销退数量错误,必须是小于0的数字" };
                    }
                    ent.id = System.Guid.NewGuid().ToString();
                    ent.Actual_Day = Actual_Day;
                    ent.Created_Time = System.DateTime.Now;
                    ent.Modify_Time = System.DateTime.Now;
                    this.Insert(ent);
                    //增加库存
                    try
                    {
                    SyncToPSI(ent.id, "Return", "退貨");
                    ent.isSync = 1;
                        this.Update(ent);
                    }
                    catch {
                    this.Delete(ent);
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                    }
                    return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "提报成功" };
               
            }
            else
            {
                return new salesActualChangeRes() { errorCode = "0000", isOk = false, errorMessage = "未知的提交类型" };
            }
        }
        private ImarketSalesRepository salesRep = new marketSalesRepository();
        public salesActualChangeRes SalesActualV2(marketSalesActualEntity ent, bool isAndroid)
        {
            if (ent.Actual_Price <= 0)
            {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,销售单价必须大于0" };
            }
            if (ent.Actual_Price > 99999)
            {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,销售单价错误" };
            }
            //检查销售单价是否合理
            JS5_S12_SALES_PRODUCT_PRICETableAdapter priceAd = new JS5_S12_SALES_PRODUCT_PRICETableAdapter();

            var res2 = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;

           
/*
            var priceTable = priceAd.GetDataBy(res2.First().CUSTOMER_CODE, ent.MACHINE_MODEL_NO);

            if (priceTable != null && priceTable.Count > 0)
            {
                DataSetPop.JS5_S12_SALES_PRODUCT_PRICERow row = priceTable[0];
                if(ent.Actual_Price <= row.LIMITED_MIN_TIMES* (double)row.PRICE || ent.Actual_Price>=row.LIMITED_MAX_TIMES * (double)row.PRICE)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,销售单价错误" };
                }
                
            }
            */
            if (ent.GOODS_TYPE_CODE.Equals("NormalGoods"))
            {
                ent.GOODS_TYPE_CODE = "S01";
            }
            if (ent.GOODS_TYPE_CODE.Equals("GroupGoods"))
            {
                ent.GOODS_TYPE_CODE = "S02";
            }
            if (ent.GOODS_TYPE_CODE.Equals("PrototypeGoods"))
            {
                ent.GOODS_TYPE_CODE = "S04";
            }
            //var res2 = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;

            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_INVENTORY_INFOTableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_INVENTORY_INFOTableAdapter();




            string CheckInv = ConfigurationManager.AppSettings["CheckInv"];

            string Actual_Day = System.DateTime.Now.ToString("yyyyMMdd");
            if (ent.Actual_Type.Equals("1")) //表示新增销售实际
            {
                //if (isAndroid)
                //{
                    if (ent.file_id == null || ent.file_id.Equals(""))
                    {
                        string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                        if (salesType.Equals("GuideMan"))
                        {
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,必须上传发票！" };
                        }
                    }
                if (ent.CONSUMER_NAME == null || ent.CONSUMER_PHONE_NO == null || ent.CONSUMER_NAME.Equals("") || ent.CONSUMER_PHONE_NO.Equals(""))
                {
                    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                    if (salesType.Equals("GuideMan"))
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,必须填写顾客姓名和电话号码！" };
                    }
                }
                //}

                //if (ent.GOODS_TYPE_CODE.Equals("S01") && ent.Actual_Qty > 1)
                //{
                //    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                //    if (salesType.Equals("GuideMan"))
                //    {
                //        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,正常销售类型上报数量不能大于1" };
                //    }

                //}
                //if (ent.GOODS_TYPE_CODE.Equals("S04") && ent.Actual_Qty > 1)
                //{
                //    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                //    if (salesType.Equals("GuideMan"))
                //    {
                //        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,正常销售类型上报数量不能大于1" };
                //    }

                //}
                if (ent.Actual_Qty <= 0)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "销量错误,必须是大于0的数字" };
                }

                //检查今天是否已上报
                //if (this.IQueryable().Where(p => p.Actual_Day.Equals(Actual_Day) && p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO) && p.sales_No.Equals(ent.sales_No) && p.SHOP_CODE.Equals(ent.SHOP_CODE)).Count() > 0)
                //{
                //    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型今天已经上报过销量，如需要修改请使用销售更正" };

                //}

                if (CheckInv.Equals("true"))
                {
                    //检查库存是否够
                    try
                    {
                        var res = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;


                        DataSetPop.JS5_S12_INVENTORY_INFODataTable tagel = ad.GetDataBy(res.First().CUSTOMER_CODE, ent.MACHINE_MODEL_NO);
                        if (tagel.Rows.Count > 0)
                        {
                            DataSetPop.JS5_S12_INVENTORY_INFORow row = tagel[0];
                            decimal totalQty = row.INVENTORY_QTY - row.ALLOCATED_QTY - row.WAIT_SHIP_QTY;
                            if (totalQty < ent.Actual_Qty)
                            {
                                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                            }
                        }
                        else
                        {
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                        }
                    }
                    catch
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                    }
                }
                ent.id = System.Guid.NewGuid().ToString();
                ent.Actual_Day = Actual_Day;
                ent.Created_Time = System.DateTime.Now;
                ent.Modify_Time = System.DateTime.Now;

                //try
                //{
                    this.Insert(ent);
                //}
                //catch (Exception e)
                //{
                //    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = e.InnerException.ToString() };
                //}






                try
                {
                    SyncToPSI(ent.id, "Normal", "正常");
                    ent.isSync = 1;
                    this.Update(ent);
                }
                catch (Exception ex)
                {
                    this.Delete(ent);
                     //return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
                }

                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "上报成功" };



            }
            else if (ent.Actual_Type.Equals("2"))//表示销售实际修正
            {
                //if (isAndroid)
                //{
                    if (ent.file_id == null || ent.file_id.Equals(""))
                    {
                        string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                        if (salesType.Equals("GuideMan"))
                        {
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,必须上传发票！" };
                        }
                    }
                if (ent.CONSUMER_NAME == null || ent.CONSUMER_PHONE_NO == null || ent.CONSUMER_NAME.Equals("") || ent.CONSUMER_PHONE_NO.Equals(""))
                {
                    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                    if (salesType.Equals("GuideMan"))
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,必须填写顾客姓名和电话号码！" };
                    }
                }
                //}
                if (System.DateTime.Now.Day <= 3)
                {
                    if (!ent.Actual_Day.StartsWith(System.DateTime.Now.ToString("yyyyMM")) && !ent.Actual_Day.StartsWith(System.DateTime.Now.AddMonths(-1).ToString("yyyyMM"))

                            )
                    {
                        return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正本月和上月的数据" };
                    }
                }
                else
                {
                    if (!ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))

                        )
                    {
                        return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正今天和昨天的销售数据" };
                    }
                }
            
            //else
            //{
            //    if (System.DateTime.Now.DayOfWeek == DayOfWeek.Monday)//
            //    {
            //        if (!ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))
            //             && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-2).ToString("yyyyMMdd"))
            //            )
            //        {
            //            return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正今天和昨天前天的销售数据" };
            //        }
            //    }
            //    else
            //    {
            //        if (!ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))

            //            )
            //        {
            //            return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正今天和昨天的销售数据" };
            //        }
            //    }
            //}

            //if (ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && this.IQueryable().Where(p => p.Actual_Day.Equals(ent.Actual_Day) && p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO) && p.sales_No.Equals(ent.sales_No) && p.SHOP_CODE.Equals(ent.SHOP_CODE)).Count() <= 0)
            //{
            //    return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "还未做销量上报,无法修正.如是要提报销量请使用销售上报" };

            //}

            if (ent.Actual_Qty > 0 && CheckInv.Equals("true"))
                {
                    //检查库存是否够
                    try
                    {
                        var res = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;


                        DataSetPop.JS5_S12_INVENTORY_INFODataTable tagel = ad.GetDataBy(res.First().CUSTOMER_CODE, ent.MACHINE_MODEL_NO);
                        if (tagel.Rows.Count > 0)
                        {
                            DataSetPop.JS5_S12_INVENTORY_INFORow row = tagel[0];
                            decimal totalQty = row.INVENTORY_QTY - row.ALLOCATED_QTY - row.WAIT_SHIP_QTY;
                            if (totalQty < ent.Actual_Qty)
                            {
                                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                            }
                        }
                        else
                        {
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                        }
                    }
                    catch
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                    }
                }

                ent.id = System.Guid.NewGuid().ToString();
                ent.Actual_Day = ent.Actual_Day;
                ent.Created_Time = System.DateTime.Now;
                ent.Modify_Time = System.DateTime.Now;
                
                    this.Insert(ent);
               
                try
                {
                    SyncToPSI(ent.id, "Change", "調整");
                    ent.isSync = 1;
                    this.Update(ent);
                }
                catch (Exception ex)
                {
                    this.Delete(ent);
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
                    // return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                }

                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "更正成功" };

            }
            else if (ent.Actual_Type.Equals("3"))//表示销退
            {

                if (ent.Actual_Qty > 0)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "销退数量错误,必须是小于0的数字" };
                }
                ent.id = System.Guid.NewGuid().ToString();
                ent.Actual_Day = Actual_Day;
                ent.Created_Time = System.DateTime.Now;
                ent.Modify_Time = System.DateTime.Now;
                this.Insert(ent);
                //增加库存
                try
                {
                    SyncToPSI(ent.id, "Return", "退貨");
                    ent.isSync = 1;
                    this.Update(ent);
                }
                catch (Exception ex)
                {
                    this.Delete(ent);
                    // return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
                }
                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "提报成功" };

            }
            else
            {
                return new salesActualChangeRes() { errorCode = "0000", isOk = false, errorMessage = "未知的提交类型" };
            }
        }
        public salesActualChangeRes SalesActualV3(marketSalesActualEntity ent, bool isAndroid)
        {
            
            ImarketSampleActRepository sampRep = new marketSampleActRepository();
            if (ent.Actual_Price <= 0)
            {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,销售单价必须大于0" };
            }
            if (ent.Actual_Price > 99999)
            {
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,销售单价错误" };
            }
            
            //判断SN是否为销样状态
            if (ent.SAMPLE_SN_NO != null && !ent.SAMPLE_SN_NO.Equals(""))
            {
                /**
                var salesAd = new V_SALES_TRANS_APP_QUERYTableAdapter();
                var snCount = salesAd.SNCountQuery(ent.SAMPLE_SN_NO);
                if (snCount > 0)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,该样机已经销售" };
                }
                **/
                //产品状态（001：上样 ，002：維修，003銷售下架，004 外借，005 歸還，006 销样）
                var snEnt = sampRep.IQueryable().First(p => p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO) && p.SN_NO.Equals(ent.SAMPLE_SN_NO) && p.SHOP_CODE.Equals(ent.SHOP_CODE)&& p.isDeleted!=1);
                if (snEnt != null)
                {
                    if (snEnt.PRODUCT_STATUS_CODE.Equals("006"))
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,该样机已销样" };
                    }
                    if (!snEnt.PRODUCT_STATUS_CODE.Equals("001") && !snEnt.PRODUCT_STATUS_CODE.Equals("003") && !snEnt.PRODUCT_STATUS_CODE.Equals("005"))
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,维修/外借状态样机无法进行销售" };
                    }
                    if (ent.Actual_Qty > 1)
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,样机销售数量不能大于1" };
                    }
                }
                else
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,样机序号未找到！" };
                }

            }
            //return new salesActualChangeRes() { errorCode = "111", isOk = true };
            //检查销售单价是否合理
            JS5_S12_SALES_PRODUCT_PRICETableAdapter priceAd = new JS5_S12_SALES_PRODUCT_PRICETableAdapter();

            var res2 = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;

            //判断建议售价

            var priceTable = priceAd.GetDataByPrd(ent.MACHINE_MODEL_NO);
            if (priceTable != null && priceTable.Count > 0)
            {
                DataSetPop.JS5_S12_SALES_PRODUCT_PRICERow row = priceTable[0];

                if (ent.Actual_Price <= row.LIMITED_MIN_TIMES * (double)row.PRICE || ent.Actual_Price >= row.LIMITED_MAX_TIMES * (double)row.PRICE)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,销售单价错误" };
                }
            }
            else
            {
                priceTable = priceAd.GetDataBy(res2.First().CUSTOMER_CODE, ent.MACHINE_MODEL_NO);

                if (priceTable != null && priceTable.Count > 0)
                {
                    DataSetPop.JS5_S12_SALES_PRODUCT_PRICERow row = priceTable[0];

                    if (ent.Actual_Price <= row.LIMITED_MIN_TIMES * (double)row.PRICE || ent.Actual_Price >= row.LIMITED_MAX_TIMES * (double)row.PRICE)
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,销售单价错误" };
                    }

                    /*
                    if (ent.Actual_Price < 0.1 * (double)row.PRICE || ent.Actual_Price > 1.5 * (double)row.PRICE)
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,销售单价错误" };
                    }*/

                }
            }
            
        
            
            if (ent.GOODS_TYPE_CODE.Equals("NormalGoods"))
            {
                ent.GOODS_TYPE_CODE = "S01";
                if (ent.Actual_Qty > 20)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,数量不能超过20台，请分多次上报" };
                }
            }
            if (ent.GOODS_TYPE_CODE.Equals("GroupGoods"))
            {
                ent.GOODS_TYPE_CODE = "S02";
                if (ent.Actual_Qty > 500)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,数量不能超过500台,请分多次上报" };
                }
            }
            if (ent.GOODS_TYPE_CODE.Equals("PrototypeGoods"))
            {
                ent.GOODS_TYPE_CODE = "S04";
            }
            //var res2 = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;
            
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_INVENTORY_INFOTableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_INVENTORY_INFOTableAdapter();




            string CheckInv = ConfigurationManager.AppSettings["CheckInv"];

            string Actual_Day = ent.Actual_Day;
            if (ent.Actual_Type.Equals("1")) //表示新增销售实际
            {
                if (System.DateTime.Now.Day <= 3)
                {
                    if (!ent.Actual_Day.StartsWith(System.DateTime.Now.ToString("yyyyMM")) && !ent.Actual_Day.StartsWith(System.DateTime.Now.AddMonths(-1).ToString("yyyyMM"))

                            )
                    {
                        return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法上报指定日期的数据，只能上报本月和上月的数据" };
                    }
                }
                else
                {
                    if (!ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))

                        )
                    {
                        return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法上报指定日期的数据，只能上报今天和昨天的销售数据" };
                    }
                }
                
                //if (isAndroid)
                //{
                if (ent.file_id == null || ent.file_id.Equals(""))
                {
                    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                    if (salesType.Equals("GuideMan"))
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,必须上传发票！" };
                    }
                }
                if (ent.CONSUMER_NAME == null || ent.CONSUMER_PHONE_NO == null || ent.CONSUMER_NAME.Equals("") || ent.CONSUMER_PHONE_NO.Equals(""))
                {
                    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                    if (salesType.Equals("GuideMan"))
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,必须填写顾客姓名和电话号码！" };
                    }
                }
                
                if (res2.First().HAVEPEOPLE_FLAG == 1)
                {
                    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                    if (!salesType.Equals("GuideMan"))
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,业务员不能上报有促门店销量！" };
                    }
                }
               
                //}

                //if (ent.GOODS_TYPE_CODE.Equals("S01") && ent.Actual_Qty > 1)
                //{
                //    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                //    if (salesType.Equals("GuideMan"))
                //    {
                //        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,正常销售类型上报数量不能大于1" };
                //    }

                //}
                //if (ent.GOODS_TYPE_CODE.Equals("S04") && ent.Actual_Qty > 1)
                //{
                //    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                //    if (salesType.Equals("GuideMan"))
                //    {
                //        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,正常销售类型上报数量不能大于1" };
                //    }

                //}
                if (ent.Actual_Qty <= 0)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "销量错误,必须是大于0的数字" };
                }

                //检查今天是否已上报
                //if (this.IQueryable().Where(p => p.Actual_Day.Equals(Actual_Day) && p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO) && p.sales_No.Equals(ent.sales_No) && p.SHOP_CODE.Equals(ent.SHOP_CODE)).Count() > 0)
                //{
                //    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型今天已经上报过销量，如需要修改请使用销售更正" };

                //}
                
                if (CheckInv.Equals("true"))
                {
                    //检查库存是否够

                    try
                    {
                        var res = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) && x.ACTIVE_FLAG==1 select x;


                        DataSetPop.JS5_S12_INVENTORY_INFODataTable tagel = ad.GetDataBy(res.First().CUSTOMER_CODE, ent.MACHINE_MODEL_NO);
                        if (tagel.Rows.Count > 0)
                        {
                            DataSetPop.JS5_S12_INVENTORY_INFORow row = tagel[0];
                            decimal totalQty = row.INVENTORY_QTY - row.ALLOCATED_QTY - row.WAIT_SHIP_QTY;
                            if (totalQty < ent.Actual_Qty)
                            {
                                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                            }
                        }
                        else
                        {
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                        }
                    }
                    catch
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                    }
                }
                
                ent.id = System.Guid.NewGuid().ToString();
                ent.Actual_Day = Actual_Day;
                ent.Created_Time = System.DateTime.Now;
                ent.Modify_Time = System.DateTime.Now;
                
                //try
                //{
                this.Insert(ent);
                //}
                //catch (Exception e)
                //{
                //    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = e.InnerException.ToString() };
                //}






                try
                {
                    
                    SyncToPSI2(ent.id, "Normal", "正常",null);
                    
                    ent.isSync = 1;
                    this.Update(ent);
                    if (ent.SAMPLE_SN_NO != null && !ent.SAMPLE_SN_NO.Equals(""))
                    {
                        var snEnt = sampRep.IQueryable().First(p => p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO) && p.SN_NO.Equals(ent.SAMPLE_SN_NO) && p.SHOP_CODE.Equals(ent.SHOP_CODE));
                        if (snEnt != null)
                        {
                            snEnt.PRODUCT_STATUS_CODE = "006";
                            sampRep.Update(snEnt);
                        }
                    }
                    }
                catch (Exception ex)
                {
                    this.Delete(ent);
                    //return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
                }

                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "上报成功" };



            }
            else if (ent.Actual_Type.Equals("2"))//表示销售实际修正
            {
               
                V_SALES_TRANS_APP_QUERYTableAdapter transAd = new V_SALES_TRANS_APP_QUERYTableAdapter();
                var testent = transAd.GetDataByID(ent.CH_NO);
                var returnList = transAd.GetReturnDataByChNo(testent.First().TRANS_NO.ToString());
                if (returnList.Count > 0)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "上报失败,销售已经退货，请继续使用退货冲销！" };
                }
                //var ent = this.IQueryable().Where(p=>p.i);
                //ent.file_id = filesId;
                //ent.Modify_Time = System.DateTime.Now;
                //this.Update(ent);
                if (!testent.First().EMPLOYEE_ID.Equals(ent.sales_No))
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法修改其他人提交的资料！" };
                }
                //if (isAndroid)
                //{
                if (ent.file_id == null || ent.file_id.Equals(""))
                {
                    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                    if (salesType.Equals("GuideMan"))
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,必须上传发票！" };
                    }
                }
                if (ent.CONSUMER_NAME == null || ent.CONSUMER_PHONE_NO == null || ent.CONSUMER_NAME.Equals("") || ent.CONSUMER_PHONE_NO.Equals(""))
                {
                    string salesType = salesRep.IQueryable().First(p => p.sales_No.Equals(ent.sales_No) && p.Active == 1).POP_TYPE_CODE;
                    if (salesType.Equals("GuideMan"))
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法完成上报,必须填写顾客姓名和电话号码！" };
                    }
                }
                //}
                if (System.DateTime.Now.Day <= 3)
                {
                    if (!ent.Actual_Day.StartsWith(System.DateTime.Now.ToString("yyyyMM")) && !ent.Actual_Day.StartsWith(System.DateTime.Now.AddMonths(-1).ToString("yyyyMM"))

                            )
                    {
                        return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正本月和上月的数据" };
                    }
                }
                else
                {
                    if (!ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))

                        )
                    {
                        return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正今天和昨天的销售数据" };
                    }
                }
                if (ent.Actual_Qty < 0)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "销量错误,必须是大于等于0的数字" };
                }
                //else
                //{
                //    if (System.DateTime.Now.DayOfWeek == DayOfWeek.Monday)//
                //    {
                //        if (!ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))
                //             && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-2).ToString("yyyyMMdd"))
                //            )
                //        {
                //            return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正今天和昨天前天的销售数据" };
                //        }
                //    }
                //    else
                //    {
                //        if (!ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && !ent.Actual_Day.Equals(System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd"))

                //            )
                //        {
                //            return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "无法修正指定日期的数据，只能修正今天和昨天的销售数据" };
                //        }
                //    }
                //}

                //if (ent.Actual_Day.Equals(System.DateTime.Now.ToString("yyyyMMdd")) && this.IQueryable().Where(p => p.Actual_Day.Equals(ent.Actual_Day) && p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO) && p.sales_No.Equals(ent.sales_No) && p.SHOP_CODE.Equals(ent.SHOP_CODE)).Count() <= 0)
                //{
                //    return new salesActualChangeRes() { errorCode = "0002", isOk = false, errorMessage = "还未做销量上报,无法修正.如是要提报销量请使用销售上报" };

                //}

                if (ent.Actual_Qty > 0 && CheckInv.Equals("true"))
                {
                    //检查库存是否够
                    try
                    {
                        var res = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;


                        DataSetPop.JS5_S12_INVENTORY_INFODataTable tagel = ad.GetDataBy(res.First().CUSTOMER_CODE, ent.MACHINE_MODEL_NO);
                        if (tagel.Rows.Count > 0)
                        {
                            DataSetPop.JS5_S12_INVENTORY_INFORow row = tagel[0];
                            decimal totalQty = row.INVENTORY_QTY - row.ALLOCATED_QTY - row.WAIT_SHIP_QTY;
                            if (totalQty < ent.Actual_Qty)
                            {
                                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                            }
                        }
                        else
                        {
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                        }
                    }
                    catch
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "该机型库存不足，无法完成上报" };
                    }
                }

                ent.id = System.Guid.NewGuid().ToString();
                ent.Actual_Day = ent.Actual_Day;
                ent.Created_Time = System.DateTime.Now;
                ent.Modify_Time = System.DateTime.Now;

                this.Insert(ent);

                try
                {
                    SyncToPSI2(ent.id, "Change", "調整", testent.First());
                    ent.isSync = 1;
                    this.Update(ent);
                    if (ent.SAMPLE_SN_NO != null && !ent.SAMPLE_SN_NO.Equals(""))
                    {
                        var snEnt = sampRep.IQueryable().First(p => p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO) && p.SN_NO.Equals(ent.SAMPLE_SN_NO) && p.SHOP_CODE.Equals(ent.SHOP_CODE));
                        if (snEnt != null)
                        {
                            snEnt.PRODUCT_STATUS_CODE = "006";
                            sampRep.Update(snEnt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Delete(ent);
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
                    // return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                }

                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "更正成功" };

            }
            else if (ent.Actual_Type.Equals("3"))//表示销退
            {
                V_SALES_TRANS_APP_QUERYTableAdapter transAd = new V_SALES_TRANS_APP_QUERYTableAdapter();
                var testent = transAd.GetDataByID(ent.CH_NO);

                var returnList = transAd.GetReturnDataByChNo(testent.First().TRANS_NO.ToString());

                //var ent = this.IQueryable().Where(p=>p.i);
                //ent.file_id = filesId;
                //ent.Modify_Time = System.DateTime.Now;
                //this.Update(ent);
                if (!testent.First().EMPLOYEE_ID.Equals(ent.sales_No))
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "无法修改其他人提交的资料！" };
                }
                if (ent.Actual_Qty >= 0)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "销退数量错误,必须是小于0的数字" };
                }
                if (returnList.Count > 0)
                {
                    int totalRetunQty = (int)returnList.Sum(p => p.SALES_QTY) + (ent.Actual_Qty.Value );
                    if (totalRetunQty * -1 > testent.First().SALES_QTY)
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "销退数量错误,总量不能大于销售数量" };
                    }
                }
                else
                {
                    if (ent.Actual_Qty * -1 > testent.First().SALES_QTY)
                    {
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "销退数量错误,不能大于销售数量" };
                    }
                }
                ent.id = System.Guid.NewGuid().ToString();
                ent.Actual_Day = Actual_Day;
                ent.Created_Time = System.DateTime.Now;
                ent.Modify_Time = System.DateTime.Now;
                this.Insert(ent);
                //增加库存
                try
                {
                    SyncToPSI2(ent.id, "Return", "退貨", testent.First());
                    ent.isSync = 1;
                    this.Update(ent);
                }
                catch (Exception ex)
                {
                    this.Delete(ent);
                    // return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "账号异常，无法完成上报，请联络管理员" };
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
                }
                return new salesActualChangeRes() { errorCode = "1000", isOk = true, errorMessage = "提报成功" };

            }
            else
            {
                return new salesActualChangeRes() { errorCode = "0000", isOk = false, errorMessage = "未知的提交类型" };
            }
        }
    }
}
