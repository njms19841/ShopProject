using DataSynchronizationLib.DataSetPopTableAdapters;
using DataSynchronizationLib.SCMTableAdapters;
using donet.io.rong;
using donet.io.rong.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NFine.Application.APPManage;
using NFine.Application.SystemSecurity;
using NFine.Application.TaskManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Repository.APPManage;
using NFine.Repository.TaskManage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMengPush.Net.Core;

namespace DataSynchronizationLib
{
    public class userModel
    {
      
        public string EMPLOYEE_CODE { get; set; }

      
        public string EMPLOYEE_NAME { get; set; }

      
        public string PhoneNumber { get; set; }

        
        public string Email { get; set; }


        
        public string Password { get; set; }

       
        public string ConfirmPassword { get; set; }
        public int Active { get; set; }


    }
    public class userModel2
    {
        public string PhoneNumber { get; set; }
        
        public string OldPassword { get; set; }

        
        public string NewPassword { get; set; }

        
        public string ConfirmPassword { get; set; }



    }

    
    public class Synchronization
    {
       
       private ImarketMachineModelRepository service = new marketMachineModelRepository();
        private ImarketTvsizeinfoRepository service2 = new marketTvsizeinfoRepository();
        private ImarketChannelMachineRepository channelMachineService = new marketChannelMachineRepository();
        private ImarketSalesShopRepository salesShopService = new marketSalesShopRepository();
        private ImarketSalesRepository salesService = new marketSalesRepository();
        private ImarketSalesActualRepository salesRepository = new marketSalesActualRepository();

        private ImarketMessageListRepository messageRepository = new marketMessageListRepository();
        private ImarketBrandRepository brandRepository = new marketBrandRepository();
        private ImarketSalesShopV2Repository salesV2Repository = new marketSalesShopV2Repository();
        //public void SynSalesAct()
        //{
        //    DataSetPopTableAdapters.TF_BUFACTORYREFINFOTableAdapter factAd = new DataSetPopTableAdapters.TF_BUFACTORYREFINFOTableAdapter();
        //    DataSetPopTableAdapters.JS5_S12_SALES_TRANS_UPLOADTableAdapter ad = new DataSetPopTableAdapters.JS5_S12_SALES_TRANS_UPLOADTableAdapter();

        //    List<salesActualAllModel> ents = salesRepository.GetSalesActual();

        //    foreach (salesActualAllModel ent in ents)
        //    {
        //        try
        //        {
        //            //String id = System.Guid.NewGuid().ToString();
        //            System.Console.WriteLine("Strart");
        //            DataSetPop.TF_BUFACTORYREFINFODataTable subtable = factAd.GetDataBy(ent.T_BUID);
        //            string fid = "";
        //            string fname = "";
        //            foreach (DataSetPop.TF_BUFACTORYREFINFORow row in subtable)
        //            {
        //                fid = row.T_FACTORYID;
        //                fname = row.T_FACTORYID;
        //            }
        //            //System.Console.WriteLine(fname);

        //            if (ent.A_TYPE.Equals("1"))
        //            {
        //                System.Console.WriteLine("1");
        //                ad.InsertQuery(ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, ent.ACTIVE_FLAG, ent.COMPANY_CODE, System.DateTime.ParseExact(ent.SALES_DATE, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture), ent.T_BUID, ent.T_BUNAME, fid, fname
        //                    , ent.MACHINE_MODEL_NO, ent.SALES_QTY, (decimal)ent.SALES_PRICE, (decimal)ent.SALES_AMOUNT, ent.T_TYPEID, ent.T_TYPENAME, ent.T_TVSIZEID, ent.TVSIZE, ent.CH_SALES_CODE,
        //                    ent.CH_SALES_NAME, ent.CH_SHOP, ent.CH_SHOP_CODE, ent.CH_QTY, (decimal)ent.CH_PRICE, (decimal)ent.CH_AMOUNT, ent.SHOP_NAME, ent.CH_REPORT_NAME, ent.CH_REPORT_PHONE_NO,
        //                    ent.CH_MEMO, ent.UP_BY, ent.UP_DATETIME, ent.id, 1, "Normal", "正常", ent.UP_FINISH_FLAG, ent.MACHINE_MODEL_NO

        //                    );
        //            }
        //            else if (ent.A_TYPE.Equals("2"))
        //            {
        //                System.Console.WriteLine("2");
        //                ad.InsertQuery(ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, ent.ACTIVE_FLAG, ent.COMPANY_CODE, System.DateTime.ParseExact(ent.SALES_DATE, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture), ent.T_BUID, ent.T_BUNAME, fid, fname
        //                   , ent.MACHINE_MODEL_NO, ent.SALES_QTY, (decimal)ent.SALES_PRICE, (decimal)ent.SALES_AMOUNT, ent.T_TYPEID, ent.T_TYPENAME, ent.T_TVSIZEID, ent.TVSIZE, ent.CH_SALES_CODE,
        //                   ent.CH_SALES_NAME, ent.CH_SHOP, ent.CH_SHOP_CODE, ent.CH_QTY, (decimal)ent.CH_PRICE, (decimal)ent.CH_AMOUNT, ent.SHOP_NAME, ent.CH_REPORT_NAME, ent.CH_REPORT_PHONE_NO,
        //                   ent.CH_MEMO, ent.UP_BY, ent.UP_DATETIME, ent.id, 1, "Change", "調整", ent.UP_FINISH_FLAG,ent.MACHINE_MODEL_NO

        //                   );

        //            }
        //            else if (ent.A_TYPE.Equals("3"))
        //            {
        //                System.Console.WriteLine("3");
        //                ad.InsertQuery(ent.CREATOR_ID, ent.CREATE_DATE, ent.CREATE_DATE, ent.ACTIVE_FLAG, ent.COMPANY_CODE, System.DateTime.ParseExact(ent.SALES_DATE, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture), ent.T_BUID, ent.T_BUNAME, fid, fname
        //                   , ent.MACHINE_MODEL_NO, ent.SALES_QTY, (decimal)ent.SALES_PRICE, (decimal)ent.SALES_AMOUNT, ent.T_TYPEID, ent.T_TYPENAME, ent.T_TVSIZEID, ent.TVSIZE, ent.CH_SALES_CODE,
        //                   ent.CH_SALES_NAME, ent.CH_SHOP, ent.CH_SHOP_CODE, ent.CH_QTY, (decimal)ent.CH_PRICE, (decimal)ent.CH_AMOUNT, ent.SHOP_NAME, ent.CH_REPORT_NAME, ent.CH_REPORT_PHONE_NO,
        //                   ent.CH_MEMO, ent.UP_BY, ent.UP_DATETIME, ent.id, 1, "Return", "退貨", ent.UP_FINISH_FLAG,ent.MACHINE_MODEL_NO

        //                   );

        //            }

        //            System.Console.WriteLine("end");
        //            String outMessage = "";
        //            QueriesTableAdapter tad = new QueriesTableAdapter();
        //            tad.SP_SALES_BATCH_TRANS_UPLOAD_A(ent.COMPANY_CODE, ent.id, out outMessage);
        //            if (!outMessage.Equals("OK"))
        //            {
        //                throw new Exception(outMessage);
        //            }
        //            marketSalesActualEntity en = salesRepository.FindEntity(ent.id);
        //            en.isSync = 1;
        //            salesRepository.Update(en);
        //            System.Console.WriteLine(outMessage);
        //        }
        //        catch (Exception ex2)
        //        {
        //            System.Console.Write(ex2.Message);
        //        }
        //        //salesRepository.FindEntity(ent.id);
        //        //ent
        //    }
        //}
        IPsiSalesEmpOrgRepository orgEmpRep = new PsiSalesEmpOrgRepository();
        IPsiSalesOrgRepository orgRep = new PsiSalesOrgRepository();
        public void deleteHis()
        {
            ITaskMastRepository rep = new TaskMastRepository();
            //DateTime time = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") +" 00:00:00","yyyy-MM-dd HH:mm:ss",CultureInfo.CurrentCulture);
            DateTime time = DateTime.Now.AddMinutes(-10);
            rep.Delete(p => p.taskType.Equals("009") && p.starTime < time);
        }
        public void SynUser()
        {
            var client = new RestClient(ConfigurationManager.AppSettings["ServerUrl"]);
            V_SALES_EMP_ACCOUNTTableAdapter ad = new V_SALES_EMP_ACCOUNTTableAdapter();
            string quyrDate = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss");
            LogApp logApp = new LogApp();
            String bId = System.Guid.NewGuid().ToString();
            var table= ad.GetData(quyrDate);
            //var table = ad.GetDataBy();
            foreach (var item in table)
            {
                if (item.ACTIVE_FLAG == 0)
                {
                    item.LOGIN_PASSWORD = "xxxxxxxa";
                   
                }
                else
                {

                }
                var request = new RestRequest("/api/Account/CreateUser", Method.POST);

                //request.AddBody("grant_type=password&username=XXXXXXXXXX&password=XXXXXXXXXXXX ");
                //request.AddHeader("Content-Type", "application/xml");
                //request.RequestFormat = DataFormat.Xml;
                if (item.MOBILE_PHONE == null)
                {
                    item.MOBILE_PHONE = item.EMPLOYEE_CODE;
                }
                //if (item.EMAIL == null)
                //{
                    item.EMAIL = item.EMPLOYEE_CODE + "@foxconn.com";
                //}
                if (item.LOGIN_PASSWORD == null || item.LOGIN_PASSWORD.Length < 6)
                {
                    item.LOGIN_PASSWORD = "123foxconn$";
                }

                userModel model = new userModel() { EMPLOYEE_CODE=item.EMPLOYEE_CODE, EMPLOYEE_NAME=item.EMPLOYEE_NAME,
                 Email=item.EMAIL, PhoneNumber=item.MOBILE_PHONE, Password=item.LOGIN_PASSWORD, ConfirmPassword=item.LOGIN_PASSWORD,Active=(int)item.ACTIVE_FLAG};
                request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                //request.AddParameter("application/xml", "grant_type=client_credentials&client_id=" + client_id + "&client_secret=" + client_secret, ParameterType.RequestBody);
                //request.AddHeader("Authorization", Token.token_type + " " + Token.access_token);
                //request.XmlSerializer.ContentType = "application/xml";
                IRestResponse response3 = client.Execute(request);
                if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.Console.WriteLine(item.EMPLOYEE_CODE + "==>ok");
                }
                else
                {
                    System.Console.WriteLine(item.EMPLOYEE_CODE + response3.Content);
                    logApp.WriteDbLog(new LogEntity() {
                        F_Account = item.EMPLOYEE_CODE, F_CreatorTime=DateTime.Now, F_Date=DateTime.Now, F_CreatorUserId="Sync", F_Description= response3.Content
                        , F_Id=System.Guid.NewGuid().ToString(), F_ModuleName="Sync_User", F_Result=false, F_ModuleId= "Sync_User", F_IPAddress= bId
                    });
                }


            }
        }
        public void SynSalesManagerOrg()
        {
            
            SCMTableAdapters.JS5_S12_SALES_MANAGE_ORG_INFOTableAdapter ad = new SCMTableAdapters.JS5_S12_SALES_MANAGE_ORG_INFOTableAdapter();
            DateTime startDate1 = DateTime.ParseExact(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00", CultureInfo.CurrentCulture), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
            DateTime EndDate1 = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd 23:59:59", CultureInfo.CurrentCulture), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
            var table = ad.GetDataByDate(startDate1,EndDate1);
            foreach (var row in table)
            {
                var entx =orgRep.FindEntity(row.ID);
                if (entx == null)
                {
                    PsiSalesOrgEntity ent = new PsiSalesOrgEntity()
                    {
                        ACTIVE_FLAG = (int)row.ACTIVE_FLAG,
                        id = row.ID,
                        MANAGE_ORG_CODE = row.MANAGE_ORG_CODE,
                        MANAGE_ORG_NAME = row.MANAGE_ORG_NAME,
                        MANAGE_ORG_TYPE_CODE = row.IsMANAGE_ORG_TYPE_CODENull() ? null : row.MANAGE_ORG_TYPE_CODE,
                        MANAGE_ORG_TYPE_NAME = row.IsMANAGE_ORG_TYPE_NAMENull() ? null : row.MANAGE_ORG_TYPE_NAME,
                        ORG_TREE_CODE = row.IsORG_TREE_CODENull() ? null : row.ORG_TREE_CODE,
                        ORG_TREE_NAME = row.IsORG_TREE_NAMENull() ? null : row.ORG_TREE_NAME,
                        PARENT_ID = row.IsPARENT_IDNull() ? null : row.PARENT_ID,
                        SORTBY = (int)row.SORTBY,
                        TREE_LEVEL = (int)row.TREE_LEVEL
                    };


                    orgRep.Insert(ent);
                }
                else
                {
                    entx.ACTIVE_FLAG = (int)row.ACTIVE_FLAG;
                    entx.MANAGE_ORG_CODE = row.MANAGE_ORG_CODE;
                    entx.MANAGE_ORG_NAME = row.MANAGE_ORG_NAME;
                    entx.MANAGE_ORG_TYPE_CODE = row.IsMANAGE_ORG_TYPE_CODENull() ? null : row.MANAGE_ORG_TYPE_CODE;
                    entx.MANAGE_ORG_TYPE_NAME = row.IsMANAGE_ORG_TYPE_NAMENull() ? null : row.MANAGE_ORG_TYPE_NAME;
                    entx.ORG_TREE_CODE = row.IsORG_TREE_CODENull() ? null : row.ORG_TREE_CODE;
                    entx.ORG_TREE_NAME = row.IsORG_TREE_NAMENull() ? null : row.ORG_TREE_NAME;
                    entx.PARENT_ID = row.IsPARENT_IDNull() ? null : row.PARENT_ID;
                    entx.SORTBY = (int)row.SORTBY;
                    entx.TREE_LEVEL = (int)row.TREE_LEVEL;
                    orgRep.Update(entx);
                }


            }
            
            // return;
            System.Console.WriteLine("=======================================");
            string startDate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd000000");
            string EndDate = DateTime.Now.ToString("yyyyMMdd235959");
            var client = new RestClient(ConfigurationManager.AppSettings["ServerUrl"]);
            V_SALES_EMP_MANAGE_ORG_APPTableAdapter empOrgAd = new V_SALES_EMP_MANAGE_ORG_APPTableAdapter();
           var empTable = empOrgAd.GetData(startDate);
           //var empTable = empOrgAd.GetData();
            foreach (var emprow in empTable)
            {
                
                var ents = orgEmpRep.IQueryable().Where(p => p.EMPLOYEE_ID.Equals(emprow.ID));
                if (ents.Count() <= 0)
                {
                    PsiSalesEmpOrgEntity ent = new PsiSalesEmpOrgEntity()
                    {
                        ACTIVE_FLAG = (int)emprow.ACTIVE_FLAG,
                        COMPANY_CODE = emprow.COMPANY_CODE,
                        EMPLOYEE_CODE = emprow.EMPLOYEE_CODE,
                        EMPLOYEE_NAME = emprow.EMPLOYEE_NAME,
                        id = System.Guid.NewGuid().ToString(),
                        EMPLOYEE_ID = emprow.ID,
                        JOB_CODE = emprow.IsJOB_CODENull()?null:emprow.JOB_CODE,
                        JOB_NAME = emprow.IsJOB_NAMENull()?null:  emprow.JOB_NAME,
                        //LOGIN_PASSWORD = emprow.PWD,
                        ORG_ID = emprow.ORG_ID

                    };
                    orgEmpRep.Insert(ent);
                    /*
                    if (emprow.ACTIVE_FLAG == 1)
                    {
                        var request = new RestRequest("/api/Account/Register", Method.POST);

                        //request.AddBody("grant_type=password&username=XXXXXXXXXX&password=XXXXXXXXXXXX ");
                        //request.AddHeader("Content-Type", "application/xml");
                        //request.RequestFormat = DataFormat.Xml;
                        String Pwd = emprow.PWD;

                        userModel model = new userModel() { PhoneNumber = emprow.EMPLOYEE_CODE, Password = Pwd, ConfirmPassword = Pwd };
                        request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                        //request.AddParameter("application/xml", "grant_type=client_credentials&client_id=" + client_id + "&client_secret=" + client_secret, ParameterType.RequestBody);
                        //request.AddHeader("Authorization", Token.token_type + " " + Token.access_token);
                        //request.XmlSerializer.ContentType = "application/xml";
                        IRestResponse response3 = client.Execute(request);
                        if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                        }
                    }
                    */
                }
                else
                {
                    foreach (var ent in ents)
                    {
                        ent.ACTIVE_FLAG = (int)emprow.ACTIVE_FLAG;
                         ent.COMPANY_CODE = emprow.COMPANY_CODE;
                        ent.EMPLOYEE_CODE = emprow.EMPLOYEE_CODE;
                        ent.EMPLOYEE_NAME = emprow.EMPLOYEE_NAME;
                        ent.EMPLOYEE_ID = emprow.ID;
                        ent.JOB_CODE = emprow.IsJOB_CODENull()?null:emprow.JOB_CODE;
                        ent.JOB_NAME = emprow.IsJOB_NAMENull()?null: emprow.JOB_NAME;
                        
                        ent.ORG_ID = emprow.ORG_ID;
                        System.Console.WriteLine("====");
                        /*
                        if (emprow.ACTIVE_FLAG == 1)
                        {
                            var request = new RestRequest("/api/Account/ChangePasswordByUserName", Method.POST);

                            //request.AddBody("grant_type=password&username=XXXXXXXXXX&password=XXXXXXXXXXXX ");
                            //request.AddHeader("Content-Type", "application/xml");
                            //request.RequestFormat = DataFormat.Xml;
                            String Pwd = ent.LOGIN_PASSWORD;


                            String newPassword = emprow.PWD;

                            userModel2 model = new userModel2() { PhoneNumber = emprow.EMPLOYEE_CODE, OldPassword = Pwd, ConfirmPassword = newPassword, NewPassword = newPassword };
                            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                            //request.AddParameter("application/xml", "grant_type=client_credentials&client_id=" + client_id + "&client_secret=" + client_secret, ParameterType.RequestBody);
                            //request.AddHeader("Authorization", Token.token_type + " " + Token.access_token);
                            //request.XmlSerializer.ContentType = "application/xml";
                            IRestResponse response3 = client.Execute(request);
                            if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                            {

                            }
                        }
                        ent.LOGIN_PASSWORD = emprow.PWD;
                        */
                        orgEmpRep.Update(ent);
                    }
                }
            }

        }
        public void SynShop2()
        {
            
            V_SALES_ACT_EMP_SHOPTableAdapter ad = new V_SALES_ACT_EMP_SHOPTableAdapter();
            var table = ad.GetData();
            foreach (var row in table)
            {
                var ent = salesV2Repository.FindEntity(row.ID);
                if (ent != null)
                {
                    ent.ACTIVE_FLAG = (int)row.ACTIVE_FLAG;
                    ent.ALLOW_FLAG = (int)row.ALLOW_FLAG;
                    ent.CUSTOMER_CODE = row.CUSTOMER_CODE;
                    ent.CUSTOMER_NAME = row.CUSTOMER_NAME;
                    ent.EMPLOYEE_CODE = row.EMPLOYEE_CODE;
                    ent.SHOP_CODE = row.SHOP_CODE;
                    ent.SHOP_ID = row.SHOP_ID;
                    ent.SHOP_NAME = row.SHOP_NAME;
                    ent.T_BUID = row.T_BUID;
                    ent.T_BUNAME = row.T_BUNAME;
                    ent.T_TYPEID = row.T_TYPEID;
                    ent.T_TYPENAME = row.T_TYPENAME;
                    salesV2Repository.Update(ent);

                }
                else
                {
                    ent = new marketSalesShopV2Entity()
                    {
                        ACTIVE_FLAG = (int)row.ACTIVE_FLAG,
                    ALLOW_FLAG = (int)row.ALLOW_FLAG,
                    CUSTOMER_CODE = row.CUSTOMER_CODE,
                     CUSTOMER_NAME = row.CUSTOMER_NAME,
                     EMPLOYEE_CODE = row.EMPLOYEE_CODE,
                     SHOP_CODE = row.SHOP_CODE,
                     SHOP_ID = row.SHOP_ID,
                     SHOP_NAME = row.SHOP_NAME,
                     T_BUID = row.T_BUID,
                     T_BUNAME = row.T_BUNAME,
                     T_TYPEID = row.T_TYPEID,
                     T_TYPENAME = row.T_TYPENAME,
                      id=row.ID
                };
                    salesV2Repository.Insert(ent);
                }
                    
            }
        }
        public void SynMessageList()
        {

            JS5_S12_POST_MESSAGETableAdapter postAd = new JS5_S12_POST_MESSAGETableAdapter();

            string quyrDate = DateTime.Now.AddMinutes(-30).ToString("yyyyMMddHHmmss");
            JS5_S12_POST_INFOTableAdapter ad = new JS5_S12_POST_INFOTableAdapter();
            DataSetPop.JS5_S12_POST_INFODataTable table = ad.GetData();
            string MesssageServerUrl = ConfigurationManager.AppSettings["MesssageServerUrl"];
            string messageFilePatch = ConfigurationManager.AppSettings["messageFilePatch"];
            string html = "<!DOCTYPE html><html><head><meta charset = \"UTF-8\"><title></title></head><body>{0}</body></html> ";
           

            
            foreach (DataSetPop.JS5_S12_POST_INFORow row in table)
            {

                UMengMessagePush<AndroidPostJson> uMAndroidPush = new UMengMessagePush<AndroidPostJson>("59550725677baa17ce0003fe", "grpqx0ayqc1ovn45iqczlrovqrdtvujf");

                marketMessageListEntity ent = messageRepository.FindEntity(row.ID);
                if (ent != null)
                {

                }
                else
                {
                    System.IO.FileStream fs = System.IO.File.Open(messageFilePatch + row.ID + ".html", System.IO.FileMode.OpenOrCreate);
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(string.Format(html, row.POST_PAGE_CONTENT));
                    fs.Write(data, 0, data.Length);
                    //清空缓冲区、关闭流
                    fs.Flush();
                    fs.Close();
                    
                    ent = new marketMessageListEntity()
                    {
                        id = row.ID,
                        CUSTOMER_CODE = row.CUSTOMER_CODE,
                        CUSTOMER_NAME = row.CUSTOMER_NAME
               ,
                        DEALERE_CODE = row.DEALERE_CODE,
                        DEALERE_NAME = row.DEALERE_NAME,
                        MACHINE_MODEL_NO = row.MACHINE_MODEL_NO,
                        Message_Title = row.POST_PAGE_MEMO
               ,
                        POST_PAGE_MEMO = row.POST_PAGE_MEMO,
                        TVSIZE = (int)row.TVSIZE,
                        T_TVSIZEID = row.T_TVSIZEID,
                        ALL_POST_FLAG = (int)row.ALL_POST_FLAG,
                        Message_Url = MesssageServerUrl + row.ID + ".html",
                        CREATE_DATE=System.DateTime.Now
                    };
                    messageRepository.Insert(ent);

                    AndroidPostJson postJson = new AndroidPostJson();
                    postJson.type = CastType.broadcast;
                    var payload = new AndroidPayload();
                    payload.display_type = "notification";
                    payload.body = new ContentBody();
                    payload.body.ticker = "系统公告";
                    payload.body.title = "系统公告";
                    payload.body.icon = "appicon";
                    payload.body.play_lights = "true";
                    payload.body.play_sound = "true";
                    payload.body.play_vibrate = "true";
                    payload.body.text = row.POST_PAGE_MEMO;
                    payload.body.after_open = AfterOpenAction.go_app;
                    //payload.body.custom = "comment-notify";
                    var dic = new Dictionary<string, string>();
                    dic.Add("messageId", row.ID);
                    payload.extra = dic;
                    postJson.payload = payload;
                    postJson.description = "系统公告";

                    ReturnJsonClass resu = uMAndroidPush.SendMessage(postJson);
                }
               
            }
            IsysUserPushMessageRepository PostmessageRepository = new sysUserPushMessageRepository();
            IsysUserPushDeviceRepository deviceRepostitory = new sysUserPushDeviceRepository();
            ImarketSalesRepository SalesRepository = new marketSalesRepository();
            DataSetPop.JS5_S12_POST_MESSAGEDataTable postMessage = postAd.GetData();
            //PostmessageRepository.Delete(p => 1 == 1);

            foreach (DataSetPop.JS5_S12_POST_MESSAGERow row in postMessage)
            {
                System.IO.FileStream fs = System.IO.File.Open(messageFilePatch + row.ID + ".html", System.IO.FileMode.OpenOrCreate);
                byte[] data = System.Text.Encoding.UTF8.GetBytes(string.Format(html, row.POST_PAGE_CONTENT));
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
                var ent = new sysUserPushMessageEntity()
                {
                    MessageType = row.MESSAGE_TYPE_CODE,
                    id = System.Guid.NewGuid().ToString(),
                    CREATE_DATE = DateTime.Now,
                    isRead = 0,
                    isSend = 0,
                    Message_Title = row.POST_PAGE_MEMO,
                    Message_Url = MesssageServerUrl + row.ID + ".html",
                    sourceId = row.ID,
                    userId = row.EMPLOYEE_ID
                };
                PostmessageRepository.Insert(ent);
                //FLW170001
                //取得PhoneNumber
                var salesEnt=SalesRepository.FindEntity(p => p.sales_No.Equals(ent.userId) && p.Active == 1);
                if (salesEnt != null)
                {
                    var deviceList = deviceRepostitory.FindList("SELECT  a.id,a.userId,a.pushToken,a.deviceType FROM sys_user_pushdevice a join aspnetusers b on a.userId = b.Id and b.PhoneNumber='"+ salesEnt .sales_PhoneNumber+ "' ");
                    foreach (var device in deviceList)
                    {
                        if (device.deviceType.Equals("Android") && device.pushToken != null)
                        {
                            UMengMessagePush<AndroidPostJson> uMAndroidPush = new UMengMessagePush<AndroidPostJson>("59550725677baa17ce0003fe", "grpqx0ayqc1ovn45iqczlrovqrdtvujf");

                            AndroidPostJson postJson = new AndroidPostJson();
                            postJson.device_tokens = device.pushToken;
                            postJson.type = CastType.unicast;
                            var payload = new AndroidPayload();
                           
                            payload.display_type = "notification";
                            payload.body = new ContentBody();
                            payload.body.ticker = "系统公告";
                            payload.body.title = "系统公告";
                            payload.body.icon = "appicon";
                            payload.body.play_lights = "true";
                            payload.body.play_sound = "true";
                            payload.body.play_vibrate = "true";
                            payload.body.text = row.POST_PAGE_MEMO;
                            payload.body.after_open = AfterOpenAction.go_app;
                            //payload.body.custom = "comment-notify";
                            var dic = new Dictionary<string, string>();
                            dic.Add("messageId", ent.id);
                            payload.extra = dic;
                            postJson.payload = payload;
                            postJson.description = "系统公告";
                            ReturnJsonClass resu = uMAndroidPush.SendMessage(postJson);
                            System.Console.WriteLine("push to :"+ salesEnt.sales_PhoneNumber +",Error Code:"+ resu.data.error_code);
                        }
                    }
                    
                }


            }


        }
        ITaskMastRepository taskRep = new TaskMastRepository();
        public string getDeviceTokens(string userId,string type)
        {
            IsysUserPushDeviceRepository deviceRep = new sysUserPushDeviceRepository();
           var devices= deviceRep.IQueryable().Where(p => p.userId.Equals(userId) && p.deviceType.Equals(type));
            string strDevices = "";
            foreach (var device in devices)
            {
                if (strDevices.Equals(""))
                {
                    strDevices = device.pushToken;
                }
                else
                {
                    strDevices = strDevices + "," + device.pushToken;
                }
               
            }
            return strDevices;

        }
        public void updateUserName()
        {
            String appKey = "y745wfm8y1y6v";
            String appSecret = "njmewTIin5p";
            RongCloud rongcloud = RongCloud.getInstance(appKey, appSecret);
            JsonSerializer serializer = new JsonSerializer();
            IaspnetusersRepository usersRepository = new aspnetusersRepository();
            marketSalesApp app = new marketSalesApp();
            var users = usersRepository.IQueryable().ToList();
            foreach (var user in users)
            {
                System.Console.WriteLine("=============================");

                UserInfoResultModel t = app.GetUserInfo(user.Id);
                try
                {
                    if (t != null)
                    {
                        TokenReslut usergetTokenResult = rongcloud.user.getToken(user.Id, t.Name == null ? "" : t.Name, "https://iretailerapp.flnet.com/APPQC/userPic.jpg");
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
            // 获取 Token 方法 
            
        }
        public void SynTaskMessage()
        {
            UMengMessagePush<AndroidPostJson> uMAndroidPush = new UMengMessagePush<AndroidPostJson>("59550725677baa17ce0003fe", "grpqx0ayqc1ovn45iqczlrovqrdtvujf");
            var ent = taskRep.FindEntity("2dc1462e-c82c-4420-beab-c47e0040d24d");
            ent.starTime = DateTime.Now;
            ent.endTime = DateTime.Now.AddMinutes(30);
            ent.isRead = 0;
            ent.isReply = 0;
            ent.isDelete = 0;
            taskRep.Update(ent);

            AndroidPostJson postJson = new AndroidPostJson();
            postJson.type = CastType.listcast;
            postJson.device_tokens = getDeviceTokens(ent.createdUserId, "Android");
            var payload = new AndroidPayload();
            payload.display_type = "notification";
            payload.body = new ContentBody();
            payload.body.ticker = ent.taskName;
            payload.body.title = ent.taskName;
            payload.body.icon = "appicon";
            payload.body.play_lights = "true";
            payload.body.play_sound = "true";
            payload.body.play_vibrate = "true";
            payload.body.text = ent.taskName;
            payload.body.after_open = AfterOpenAction.go_app;
            //payload.body.custom = "comment-notify";
            var dic = new Dictionary<string, string>();
            dic.Add("messageId", System.Guid.NewGuid().ToString());
            payload.extra = dic;
            postJson.payload = payload;
            postJson.description = ent.taskName;

            ReturnJsonClass resu = uMAndroidPush.SendMessage(postJson);

            System.Console.WriteLine(resu.ret);

            ent = taskRep.FindEntity("dfa6ea5f-c9dd-4982-b6a4-a9b20d419106");
            ent.starTime = DateTime.Now;
            ent.endTime = DateTime.Now.AddMinutes(30);
            ent.isRead = 0;
            ent.isReply = 0;
            ent.isDelete = 0;
            taskRep.Update(ent);

             postJson = new AndroidPostJson();
            postJson.type = CastType.listcast;
            postJson.device_tokens = getDeviceTokens(ent.createdUserId, "Android");
            payload = new AndroidPayload();
            payload.display_type = "notification";
            payload.body = new ContentBody();
            payload.body.ticker = ent.taskName;
            payload.body.title = ent.taskName;
            payload.body.icon = "appicon";
            payload.body.play_lights = "true";
            payload.body.play_sound = "true";
            payload.body.play_vibrate = "true";
            payload.body.text = ent.taskName;
            payload.body.after_open = AfterOpenAction.go_app;
            //payload.body.custom = "comment-notify";
             dic = new Dictionary<string, string>();
            dic.Add("messageId", System.Guid.NewGuid().ToString());
            payload.extra = dic;
            postJson.payload = payload;
            postJson.description = ent.taskName;
            uMAndroidPush = new UMengMessagePush<AndroidPostJson>("59550725677baa17ce0003fe", "grpqx0ayqc1ovn45iqczlrovqrdtvujf");
            resu = uMAndroidPush.SendMessage(postJson);
            System.Console.WriteLine(resu.ret);
        }
        public void synSap()
        {
            marketSampleActRepository rep = new marketSampleActRepository();
            /*
            var datas= rep.IQueryable().Where(p => 1 == 1).ToList();
            foreach (var ent in datas)
            {
                ent.SAMPLE_UP_NO = "SUA" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                rep.Update(ent);
               
            }
            return;
            */
            var datas = rep.IQueryable().Where(p=>p.SAMPLE_UP_NO.Equals("SUA201805161918108498")).ToList();
            var ad = new JS5_S12_SALES_SAMPLE_INFOTableAdapter();
            foreach (var ent in datas)
            {
                var table = ad.GetData(ent.SAMPLE_UP_NO);
                if (table.Count <= 0)
                {
                    try
                    {
                        var res2 = from x in rep.dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;
                        DateTime date = System.DateTime.Now;
                        string CREATE_DATE = date.ToString("yyyyMMddHHmmss");
                        JS5_S12_SALES_SAMPLE_UPTableAdapter sampleAd = new JS5_S12_SALES_SAMPLE_UPTableAdapter();
                        DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
                        if (ent.isDeleted != null && ent.isDeleted == 1)
                        {
                            sampleAd.InsertQuery(ent.id, 1, ent.sales_No, CREATE_DATE, CREATE_DATE, "PSIadmin_APP@" + CREATE_DATE + "@APP", "PSIadmin_APP@" + CREATE_DATE + "@APP", 0, "FLNET"
                                , ent.SAMPLE_UP_NO, res2.First().CUSTOMER_CODE, res2.First().CUSTOMER_NAME, ent.SHOP_CODE, ent.SHOP_NAME, ent.PRODUCT_TYPE_CODE, ent.PRODUCT_TYPE_NAME, ent.MACHINE_MODEL_NO, ent.SN_NO, ent.SAMPLE_DATE.Value, "无", ent.id, 1, "PSIadmin_APP", date, date, ent.UP_TYPE_CODE, ent.UP_TYPE_NAME,
                                ent.SAMPLE_TYPE_CODE, ent.SAMPLE_TYPE_NAME, ent.SOURCE_SAMPLE_UP_NO, ent.PRODUCT_STATUS_CODE, ent.REMARK
                                );
                        }
                        else
                        {
                            sampleAd.InsertQuery(ent.id, 1, ent.sales_No, CREATE_DATE, CREATE_DATE, "PSIadmin_APP@" + CREATE_DATE + "@APP", "PSIadmin_APP@" + CREATE_DATE + "@APP", 1, "FLNET"
                                , ent.SAMPLE_UP_NO, res2.First().CUSTOMER_CODE, res2.First().CUSTOMER_NAME, ent.SHOP_CODE, ent.SHOP_NAME, ent.PRODUCT_TYPE_CODE, ent.PRODUCT_TYPE_NAME, ent.MACHINE_MODEL_NO, ent.SN_NO, ent.SAMPLE_DATE.Value, "无", ent.id, 1, "PSIadmin_APP", date, date, ent.UP_TYPE_CODE, ent.UP_TYPE_NAME,
                                ent.SAMPLE_TYPE_CODE, ent.SAMPLE_TYPE_NAME, ent.SOURCE_SAMPLE_UP_NO, ent.PRODUCT_STATUS_CODE, ent.REMARK
                                );
                        }
                        if (ent.file_ids != null && !ent.file_ids.Equals(""))
                        {
                            string[] fileids = ent.file_ids.Split(",".ToCharArray());
                            string physical_path = System.DateTime.Now.ToString("yyyyMM");

                            foreach (string fileid in fileids)
                            {
                                //写入发票信息
                                fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.sales_No, System.DateTime.Now, ent.sales_No, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "P13056", ent.id, "PDG1351", "001", "样品照片", ent.id,
                                    "Normal", "正常", 1, "PSIadmin_APP", date, date);
                            }

                        }
                        QueriesTableAdapter tad = new QueriesTableAdapter();
                        String outMessage = "";

                        tad.SP_SALES_SAMPLE_UPLOAD("FLNET", ent.id, out outMessage);
                        System.Console.WriteLine(ent.SAMPLE_UP_NO+":"+outMessage);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ent.SAMPLE_UP_NO + ":====" + ex.Message);
                    }
                }
            }

        }
        public void SynTasks()
        {
            JS5_S12_CRM_MESSAGE_DTableAdapter ad = new JS5_S12_CRM_MESSAGE_DTableAdapter();
            JS5_S12_CRM_MESSAGE_RECEIVERTableAdapter receverAd = new JS5_S12_CRM_MESSAGE_RECEIVERTableAdapter();
            string startDate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd000000");
            string EndDate = DateTime.Now.ToString("yyyyMMdd235959");
            ImarketSalesRepository SalesService = new marketSalesRepository();
            var table = ad.GetDataBy(startDate, EndDate);
            TaskPopApp popApp = new TaskPopApp();
            foreach (var row in table)
            {
                var pops = receverAd.GetData(row.MESSAGE_BILL_NO, row.MESSAGE_SUB_NO);
                foreach (var pop in pops)
                {
                  var task=  taskRep.IQueryable().Where(p => p.MESSAGE_BILL_NO.Equals(row.MESSAGE_BILL_NO) && p.MESSAGE_SUB_NO == (int)row.MESSAGE_SUB_NO && p.RECEIVE_EMPLOYEE_CODE.Equals(pop.RECEIVE_EMPLOYEE_CODE));
                    if (task.Count() <= 0)
                    {
                        var salesInfo = SalesService.GetUserInfoBySalesNo(pop.RECEIVE_EMPLOYEE_CODE);
                        if (salesInfo != null)
                        {
                            string taskId = System.Guid.NewGuid().ToString();
                            TaskMastEntity ent = new TaskMastEntity()
                            {
                                id = taskId,
                                alertType = 1,
                                createdTime = DateTime.ParseExact(row.CREATE_DATE, "yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                                createdUserId=salesInfo.id,
                                 MESSAGE_BILL_NO=pop.MESSAGE_BILL_NO, MESSAGE_SUB_NO=(int)pop.MESSAGE_SUB_NO,  MESSAGE_REPLY_TYPE_CODE=row.MESSAGE_REPLY_TYPE_CODE,
                                  MESSAGE_REPLY_TYPE_NAME=row.MESSAGE_REPLY_TYPE_NAME, RECEIVE_EMPLOYEE_CODE=pop.RECEIVE_EMPLOYEE_CODE, RECEIVE_EMPLOYEE_NAME=pop.RECEIVE_EMPLOYEE_NAME,
                                    desc=row.MESSAGE_CONTENT, freqType=1, taskName=row.MESSAGE_SUBJECT, starTime= DateTime.ParseExact(row.CREATE_DATE, "yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                                     endTime= DateTime.ParseExact(row.CREATE_DATE, "yyyyMMddHHmmss", CultureInfo.CurrentCulture).AddMinutes(5),
                                       importantType=1, isAll=1, isRead=0, isReply=0, RECEIVE_JOB_CODE=pop.RECEIVE_JOB_CODE, RECEIVE_MANAGE_ORG_ID=pop.RECEIVE_MANAGE_ORG_ID,
                                        taskType=row.MESSAGE_TYPE_CODE, taskTypeName=row.MESSAGE_TYPE_NAME,  URGENCY_TYPE_CODE=row.URGENCY_TYPE_CODE, 
                                         URGENCY_TYPE_NAME=row.URGENCY_TYPE_NAME, taskSource=row.SEND_EMPLOYEE_NAME, isDelete=0
                            };
                            taskRep.Insert(ent);

                            

                            popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 2, taskId = taskId, UserId = salesInfo.id, userType = 1 });
                            System.Console.WriteLine("===============================");

                            
                            string AndroIdDevice= getDeviceTokens(salesInfo.id, "Android");
                            if (AndroIdDevice.Length > 0)
                            {
                                AndroidPostJson postJson = new AndroidPostJson();
                                var payload = new AndroidPayload();
                                postJson.type = CastType.listcast;
                                postJson.device_tokens = AndroIdDevice;

                                payload.display_type = "notification";
                                payload.body = new ContentBody();
                                payload.body.ticker = ent.taskName;
                                payload.body.title = ent.taskName;
                                payload.body.icon = "appicon";
                                payload.body.play_lights = "true";
                                payload.body.play_sound = "true";
                                payload.body.play_vibrate = "true";
                                payload.body.text = ent.taskName;
                                payload.body.after_open = AfterOpenAction.go_app;
                                //payload.body.custom = "comment-notify";
                                var dic = new Dictionary<string, string>();
                                dic.Add("messageId", System.Guid.NewGuid().ToString());
                                payload.extra = dic;
                                postJson.payload = payload;
                                postJson.description = ent.taskName;
                                UMengMessagePush<AndroidPostJson> uMAndroidPush = new UMengMessagePush<AndroidPostJson>("59550725677baa17ce0003fe", "grpqx0ayqc1ovn45iqczlrovqrdtvujf");
                                try
                                {
                                    ReturnJsonClass resu = uMAndroidPush.SendMessage(postJson);
                                    System.Console.WriteLine(resu.ret);
                                }
                                catch { }
                            }
                            string IOSDevice = getDeviceTokens(salesInfo.id, "IOS");
                            if (IOSDevice.Length > 0)
                            {

                                IOSPostJson postJson = new IOSPostJson();
                                postJson.type = CastType.unicast;
                                var aps = new Aps()
                                {
                                    alert = "msg",
                                    sound = "default"
                                };
                                var payload = new IOSPayload(aps);
                                JObject jo = JObject.FromObject(payload);
                                var extra = new Dictionary<string, string>();
                                //用户自定义内容，"d","p"为友盟保留字段，key不可以是"d","p"
                                extra.Add("open", "list");
                                extra.ToList().ForEach(x => jo.Add(x.Key, x.Value));

                                postJson.payload = jo;
                                postJson.description = ent.taskName;
                                postJson.device_tokens = IOSDevice;
                                postJson.production_mode = "true";

                                UMengMessagePush<IOSPostJson> uMAndroidPush = new UMengMessagePush<IOSPostJson>("596791cbb27b0a673700001f", "siy2v7u9uzishzimgnslzdukyqkeofhp");
                                try
                                {
                                    ReturnJsonClass resu = uMAndroidPush.SendMessage(postJson);
                                    System.Console.WriteLine(resu.ret);
                                }
                                catch { }
                            }

                        }

                    }
                }
               
            }
        }
        public void SynShopInfo()
        {
            string quyrDate = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss");
            
            DataSetPopTableAdapters .JS5_S12_SALES_SHOPTableAdapter shopAd = new DataSetPopTableAdapters.JS5_S12_SALES_SHOPTableAdapter();
            DataSetPop.JS5_S12_SALES_SHOPDataTable shopTable = shopAd.GetData();
            //OrcalMode.JS5_S12_SALES_SHOPDataTable shopTable = shopAd.GetDataBy(quyrDate);
            foreach (DataSetPop.JS5_S12_SALES_SHOPRow row in shopTable.Rows)
            {

                //try
                //{
                    marketSalesShopEntity ent = salesShopService.FindEntity(row.ID);
                    if (ent != null)
                    {
                        ent.ACTIVE_FLAG = (int)row.ACTIVE_FLAG;
                        ent.AREA = row.AREA;
                        ent.BIG_SHOP = row.BIG_SHOP;
                        ent.CHANNEL = row.CHANNEL;
                        ent.CITY = row.CITY;
                        ent.CITY_LEVEL = row.CITY_LEVEL;
                        ent.COMMUNITY = row.COMMUNITY;
                        ent.CUSTOMER_CODE = row.CUSTOMER_CODE;
                        ent.CUSTOMER_NAME = row.CUSTOMER_NAME;
                        ent.DEALERE_CODE = row.DEALERE_CODE;
                        ent.DEALERE_CODE_SAMPLE = row.DEALERE_CODE_SAMPLE;
                        ent.DEALERE_NAME = row.DEALERE_NAME;
                        ent.DEALERE_TYPE = row.DEALERE_TYPE;
                        ent.FLOOR_AREA = (double)row.FLOOR_AREA;
                        ent.HAVEPEOPLE_FLAG = (int)row.HAVEPEOPLE_FLAG;
                        ent.MANAGE_CENTER = row.MANAGE_CENTER;
                        ent.MEMO = row.MEMO;
                        ent.PIC = row.PIC;
                    ent.Modify_Date = DateTime.Now;//row.LAST_MODIFY_DATE != null ? DateTime.ParseExact(row.LAST_MODIFY_DATE, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>();
                        ent.SHOP_ADDR = row.SHOP_ADDR;
                        ent.SHOP_ATTR = row.SHOP_ATTR;
                        //SHOP_BUILD_DATE=row.SHOP_BUILD_DATE,
                        ent.SHOP_CODE = row.SHOP_CODE;
                        // SHOP_LEAVE_DATE=row.SHOP_LEAVE_DATE,
                        ent.SHOP_LEVEL = row.SHOP_LEVEL;
                        ent.SHOP_NAME = row.SHOP_NAME;
                        ent.SHOP_OLD_NAME = row.SHOP_OLD_NAME;
                        ent.SHOP_PANEL_LEVEL = row.SHOP_PANEL_LEVEL;
                        ent.SHOP_POINT_CUSTOMER = row.SHOP_POINT_CUSTOMER;
                        ent.SHOP_PRODUCTS = row.SHOP_PRODUCTS;
                        ent.SHOP_PRODUCT_TYPE = row.SHOP_PRODUCT_TYPE;
                        ent.SHOP_PROMOTER_CELL = row.SHOP_PROMOTER_CELL;
                        ent.SHOP_PROMOTER_CODE = row.SHOP_PROMOTER_CODE;
                        ent.SHOP_PROMOTER_NAME = row.SHOP_PROMOTER_NAME;
                        //SHOP_REBUILD_DATE=row.SHOP_REBUILD_DATE,
                        ent.SHOP_SALESOFYEAR = row.SHOP_SALESOFYEAR;
                        ent.SHOP_TYPE = row.SHOP_TYPE;
                        ent.STATUS_CODE = row.STATUS_CODE;
                        ent.T_BUID = row.T_BUID;
                        ent.T_BUNAME = row.T_BUNAME;
                        ent.T_TYPEID = row.T_TYPEID;
                        ent.T_TYPENAME = row.T_TYPENAME;
                        ent.LONGITUDE = (double)row.LONGITUDE;
                        ent.LATITUDE = (double)row.LATITUDE;
                        ent.ORG_ID = row.ORG_ID;
                        salesShopService.Update(ent);
                    }
                    else
                    {
                        ent = new marketSalesShopEntity()
                        {
                            ID = row.ID,
                            ACTIVE_FLAG = (int)row.ACTIVE_FLAG,
                            AREA = row.AREA,
                            BIG_SHOP = row.BIG_SHOP,
                            CHANNEL = row.CHANNEL,
                            CITY = row.CITY,
                            CITY_LEVEL = row.CITY_LEVEL,
                            COMMUNITY = row.COMMUNITY,
                            CUSTOMER_CODE = row.CUSTOMER_CODE,
                            CUSTOMER_NAME = row.CUSTOMER_NAME,
                            DEALERE_CODE = row.DEALERE_CODE,
                            DEALERE_CODE_SAMPLE = row.DEALERE_CODE_SAMPLE,
                            DEALERE_NAME = row.DEALERE_NAME,
                            DEALERE_TYPE = row.DEALERE_TYPE,
                            FLOOR_AREA = (double)row.FLOOR_AREA,
                            HAVEPEOPLE_FLAG = (int)row.HAVEPEOPLE_FLAG,
                            MANAGE_CENTER = row.MANAGE_CENTER,
                            MEMO = row.MEMO,
                            PIC = row.PIC,
                            Modify_Date = DateTime.Now,//row.LAST_MODIFY_DATE != null ? DateTime.ParseExact(row.LAST_MODIFY_DATE, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>(),
                    SHOP_ADDR = row.SHOP_ADDR,
                            SHOP_ATTR = row.SHOP_ATTR,
                            //SHOP_BUILD_DATE=row.SHOP_BUILD_DATE,
                            SHOP_CODE = row.SHOP_CODE,
                            // SHOP_LEAVE_DATE=row.SHOP_LEAVE_DATE,
                            SHOP_LEVEL = row.SHOP_LEVEL,
                            SHOP_NAME = row.SHOP_NAME,
                            SHOP_OLD_NAME = row.SHOP_OLD_NAME,
                            SHOP_PANEL_LEVEL = row.SHOP_PANEL_LEVEL,
                            SHOP_POINT_CUSTOMER = row.SHOP_POINT_CUSTOMER,
                            SHOP_PRODUCTS = row.SHOP_PRODUCTS,
                            SHOP_PRODUCT_TYPE = row.SHOP_PRODUCT_TYPE,
                            SHOP_PROMOTER_CELL = row.SHOP_PROMOTER_CELL,
                            SHOP_PROMOTER_CODE = row.SHOP_PROMOTER_CODE,
                            SHOP_PROMOTER_NAME = row.SHOP_PROMOTER_NAME,
                            //SHOP_REBUILD_DATE=row.SHOP_REBUILD_DATE,
                            SHOP_SALESOFYEAR = row.SHOP_SALESOFYEAR,
                            SHOP_TYPE = row.SHOP_TYPE,
                            STATUS_CODE = row.STATUS_CODE,
                            T_BUID = row.T_BUID,
                            T_BUNAME = row.T_BUNAME,
                            T_TYPEID = row.T_TYPEID,
                            T_TYPENAME = row.T_TYPENAME,
                            LONGITUDE = (double)row.LONGITUDE,
                            LATITUDE = (double)row.LATITUDE,
                             ORG_ID=row.ORG_ID
                        };
                        salesShopService.Insert(ent);
                    }
                //}

                //catch (Exception ex)
                //{
                //    System.Console.WriteLine(ex.Message + "  ShopCode:" + row.SHOP_CODE);
                //}
            }
            V_SALES_SHOP_POP_APPTableAdapter ad = new V_SALES_SHOP_POP_APPTableAdapter();

            //OrcalModeTableAdapters.JS5_S12_SALES_SHOP_NEW_POPTableAdapter ad = new OrcalModeTableAdapters.JS5_S12_SALES_SHOP_NEW_POPTableAdapter();
            //DataSetPop.V_SALES_SHOP_POP_INFODataTable popTable = ad.GetDataBy(quyrDate);



            var client = new RestClient(ConfigurationManager.AppSettings["ServerUrl"]);
            //DataSetPop.V_SALES_SHOP_POP_INFODataTable popTable = ad.GetData();
            //DataSetPop.V_SALES_SHOP_POP_INFODataTable popTable = ad.GetDataBy(quyrDate, quyrDate);
            var popTable = ad.GetData(quyrDate);
           
            System.Console.WriteLine("POP Table Data count:"+ popTable.Count);
            foreach (SCM.V_SALES_SHOP_POP_APPRow row in popTable.Rows)
            {
                marketSalesEntity ent = salesService.FindEntity(row.ID);

                
                if (ent != null)
                {

                    DateTime? LastModifyTime = new Nullable<DateTime>();
                    try
                    {
                        LastModifyTime = row.LAST_MODIFY_DATE != null ? DateTime.ParseExact(row.LAST_MODIFY_DATE, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>();
                    }
                    catch
                    {
                        try
                        {
                            LastModifyTime = row.LAST_MODIFY_DATE != null ? DateTime.ParseExact(row.LAST_MODIFY_DATE, "yyyy/M/d HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>();
                        }
                        catch { }
                    }
                   
                    ent.Active = (int)row.ACTIVE_FLAG;
                    ent.Modify_Date = LastModifyTime;
                   // ent.sales_Birthday = row.BIRTH_DAY != null ? DateTime.ParseExact(row.BIRTH_DAY, "yyyy/M/d", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>();
                    //ent.sales_ContractEnd = row.CONTRACT_TO != null ? DateTime.ParseExact(row.CONTRACT_TO, "yyyy/M/d", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>();
                    //ent.sales_ContractStart = row.CONTRACT_FROM != null ? DateTime.ParseExact(row.CONTRACT_FROM, "yyyy/M/d", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>();
                    ent.sales_IdNo = row.ID_NO;
                    //ent.sales_joinDate = row.ON_BOARD_DAY != null ? DateTime.ParseExact(row.ON_BOARD_DAY, "yyyy/M/d", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>();
                    ent.sales_Name = row.EMPLOYEE_NAME;
                    ent.POP_TYPE_CODE = row.POP_TYPE_CODE;
                    ent.sales_PhoneNumber = (row.IsCELL_PHONENull() ? null : row.CELL_PHONE);
                    ent.sales_No = row.EMPLOYEE_CODE;
                    ent.sales_ShopNo = row.SHOP_CODE;
                    ent.sales_six = row.GENDER;
                   // String old_passWord = ent.app_password;
                    //ent.app_password = row.LOGIN_PASSWORD;
                    //ent.POP_TYPE_CODE=row.pop
                    //ent.app_password=row.pa
                    salesService.Update(ent);
                    /*
                    var request = new RestRequest("/api/Account/ChangePasswordByUserName", Method.POST);

                    //request.AddBody("grant_type=password&username=XXXXXXXXXX&password=XXXXXXXXXXXX ");
                    //request.AddHeader("Content-Type", "application/xml");
                    //request.RequestFormat = DataFormat.Xml;
                    String Pwd = (row.IsCELL_PHONENull() ? row.EMPLOYEE_CODE : row.CELL_PHONE);
                    if (row.LOGIN_PASSWORD != null)
                    {
                        Pwd = row.LOGIN_PASSWORD;
                    }
                    String newPassword = ent.app_password;
                    /*
                    if (!ent.sales_PhoneNumber.Equals(row.CELL_PHONE))//如果手机号变更，，设置密码无法登陆
                    {
                        newPassword = "xxx662345";
                    }
                    */
                    /*
                    userModel2 model = new userModel2() { PhoneNumber = (row.IsCELL_PHONENull() ? row.EMPLOYEE_CODE : row.CELL_PHONE), OldPassword = old_passWord, ConfirmPassword = newPassword, NewPassword = newPassword };
                    request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                    //request.AddParameter("application/xml", "grant_type=client_credentials&client_id=" + client_id + "&client_secret=" + client_secret, ParameterType.RequestBody);
                    //request.AddHeader("Authorization", Token.token_type + " " + Token.access_token);
                    //request.XmlSerializer.ContentType = "application/xml";
                    IRestResponse response3 = client.Execute(request);
                    if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                    }
                   */
                }
                else
                {
                    DateTime? LastModifyTime = new Nullable<DateTime>();
                    try
                    {
                        LastModifyTime = row.LAST_MODIFY_DATE != null ? DateTime.ParseExact(row.LAST_MODIFY_DATE, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>();
                    }
                    catch {
                        try
                        {
                            LastModifyTime = row.LAST_MODIFY_DATE != null ? DateTime.ParseExact(row.LAST_MODIFY_DATE, "yyyy/M/d HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>();
                        }
                        catch { }
                    }
                    ent = new marketSalesEntity() {
                     Active=(int)row.ACTIVE_FLAG,
                      Modify_Date= LastModifyTime,
                      // sales_Birthday =row.BIRTH_DAY!=null? DateTime.ParseExact(row.BIRTH_DAY, "yyyy/M/d", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>(),
                       // sales_ContractEnd = row.CONTRACT_TO != null ? DateTime.ParseExact(row.CONTRACT_TO, "yyyy/M/d", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>(),
                         //sales_ContractStart= row.CONTRACT_FROM != null ? DateTime.ParseExact(row.CONTRACT_FROM, "yyyy/M/d", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>(),
                          sales_IdNo=row.ID_NO,
                           //sales_joinDate = row.ON_BOARD_DAY != null ? DateTime.ParseExact(row.ON_BOARD_DAY, "yyyy/M/d", System.Globalization.CultureInfo.CurrentCulture) : new Nullable<DateTime>(),
                            sales_Name=row.EMPLOYEE_NAME,
                             sales_No=row.EMPLOYEE_CODE,
                              sales_PhoneNumber=(row.IsCELL_PHONENull()?null: row.CELL_PHONE) ,
                               sales_ShopNo=row.SHOP_CODE,
                                sales_six=row.GENDER,
                        POP_TYPE_CODE = row.POP_TYPE_CODE,
                    id = row.ID, 

                };
                    salesService.Insert(ent);
                    
                   


                }
                /*
                if (row.ACTIVE_FLAG == 1)
                {

                    var request = new RestRequest("/api/Account/Register", Method.POST);

                    //request.AddBody("grant_type=password&username=XXXXXXXXXX&password=XXXXXXXXXXXX ");
                    //request.AddHeader("Content-Type", "application/xml");
                    //request.RequestFormat = DataFormat.Xml;
                    String Pwd = (row.IsCELL_PHONENull() ? row.EMPLOYEE_CODE : row.CELL_PHONE);
                    if (row.LOGIN_PASSWORD != null)
                    {
                        Pwd = row.LOGIN_PASSWORD;
                    }
                    userModel model = new userModel() { PhoneNumber = (row.IsCELL_PHONENull() ? row.EMPLOYEE_CODE : row.CELL_PHONE), Password = Pwd, ConfirmPassword = Pwd };
                    request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                    //request.AddParameter("application/xml", "grant_type=client_credentials&client_id=" + client_id + "&client_secret=" + client_secret, ParameterType.RequestBody);
                    //request.AddHeader("Authorization", Token.token_type + " " + Token.access_token);
                    //request.XmlSerializer.ContentType = "application/xml";
                    IRestResponse response3 = client.Execute(request);
                    if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                    }
                }
                */
            }

        }
        public void SynBrand()
        {
            DataSetPopTableAdapters.JS5_COMPANY_DOCUMENTTableAdapter BrandAd = new DataSetPopTableAdapters.JS5_COMPANY_DOCUMENTTableAdapter();
            DataSetPop.JS5_COMPANY_DOCUMENTDataTable brandTable = BrandAd.GetDataBy();
            //OrcalMode.JS5_S12_SALES_SHOPDataTable shopTable = shopAd.GetDataBy(quyrDate);
            foreach (DataSetPop.JS5_COMPANY_DOCUMENTRow row in brandTable.Rows)
            {


                marketBrandEntity ent = brandRepository.FindEntity(row.COMPANY_DOCUMENT_ID);
                if (ent != null)
                {
                    ent.BRAND_NAME = row.DOC_NAME;

                    brandRepository.Update(ent);
                }
                else
                {
                    ent = new marketBrandEntity()
                    {
                        id = row.COMPANY_DOCUMENT_ID,
                         BRAND_CODE=row.DOC_CODE,
                          BRAND_NAME=row.DOC_NAME
                       
                    };
                    brandRepository.Insert(ent);
                }
            }
            ImarketProductTypeRepository ptr = new marketProductTypeRepository();
            DataSetPopTableAdapters.JS5_S12_PRODUCT_TYPE_INFOTableAdapter ad = new DataSetPopTableAdapters.JS5_S12_PRODUCT_TYPE_INFOTableAdapter();
            var proTable = ad.GetDataBy();
            foreach (DataSetPop.JS5_S12_PRODUCT_TYPE_INFORow row in proTable.Rows)
            {
                var ent = ptr.FindEntity(row.TREE_NODE_CODE);
                if (ent != null)
                {
                    ent.NAME = row.TREE_NODE_NAME;

                    ptr.Update(ent);
                }
                else
                {
                    ptr.Insert(new marketProductTypeEntity() { CODE = row.TREE_NODE_CODE, NAME = row.TREE_NODE_NAME });
                }
            }

            }
        public void synSamle()
        {
            V_SALES_SAMPLE_APP_QUERYTableAdapter ad = new V_SALES_SAMPLE_APP_QUERYTableAdapter();
            ImarketSampleActRepository rep = new marketSampleActRepository();
            var table = ad.GetData();
            foreach (var row in table)
            {
                var ent = new marketSampleActEntity() { id=row.ID, SHOP_CODE=row.SHOP_CODE, SHOP_NAME=row.SHOP_NAME, MACHINE_MODEL_NO=row.MACHINE_MODEL_NO, PRODUCT_TYPE_CODE=row.PRODUCT_CODE,
                  PRODUCT_TYPE_NAME=row.PRODUCT_NAME};
                
                var list = rep.IQueryable().Where(p => p.id.Equals(row.ID));
                if (list.Count() <= 0)
                {
                  
                    ent.Created_Time = System.DateTime.Now;
                    ent.Modify_Time = System.DateTime.Now;
                    ent.isSync = 0;

                    var lastEnt = rep.IQueryable().OrderByDescending(p => p.Created_Time).FirstOrDefault();
                    ent.SAMPLE_UP_NO = "SUA" + ent.Created_Time.Value.ToString("yyyyMM") + "00001";
                    if (lastEnt != null && lastEnt.Created_Time.Value.Year.Equals(ent.Created_Time.Value.Year) && lastEnt.Created_Time.Value.Month.Equals(ent.Created_Time.Value.Month))
                    {
                        if (lastEnt.SAMPLE_UP_NO != null && !lastEnt.SAMPLE_UP_NO.Equals(""))
                        {
                            int no = int.Parse(lastEnt.SAMPLE_UP_NO.Substring(9));
                            no = no + 1;
                            ent.SAMPLE_UP_NO = "SUA" + ent.Created_Time.Value.ToString("yyyyMM") + no.ToString("00000");
                        }
                    }

                    rep.Insert(ent);
                }
                    
            }
        }
        public void SynMachineModel()
        {
            string quyrDate = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss");
            OrcalModeTableAdapters.JS5_S12_CHANNEL_MACHINETableAdapter channelAd = new OrcalModeTableAdapters.JS5_S12_CHANNEL_MACHINETableAdapter();
            OrcalMode.JS5_S12_CHANNEL_MACHINEDataTable channelTable = channelAd.GetDataByModifyDate(quyrDate);
            DateTime updateTime = System.DateTime.Now;
           // channelMachineService.Delete(p => 1==1);
            foreach (OrcalMode.JS5_S12_CHANNEL_MACHINERow row in channelTable.Rows)
            {
                marketChannelMachineEntity ent = channelMachineService.FindEntity(row.ID);
                if (ent != null)
                {
                    ent.MACHINE_MODEL_NO = row.MACHINE_MODEL_NO;
                    ent.T_TYPEID = row.T_TYPEID;
                    ent.ACTIVE_FLAG = (int)row.ACTIVE_FLAG;
                    ent.ALLOW_FLAG = (int)row.ALLOW_FLAG;
                    ent.Modify_Date = updateTime;
                    channelMachineService.Update(ent);
                }
                else
                {
                    ent = new marketChannelMachineEntity() {
                        ID = row.ID, T_TYPEID=row.T_TYPEID, ACTIVE_FLAG= (int)row.ACTIVE_FLAG,
                        ALLOW_FLAG = (int)row.ALLOW_FLAG, MACHINE_MODEL_NO= row.MACHINE_MODEL_NO,
                         Modify_Date= updateTime

                    };
                    channelMachineService.Insert(ent);
                }

            }
            ///var MachineList = channelMachineService.IQueryable().Where(p => p.Modify_Date < updateTime).ToList();
            //
            //if(channelTable.Rows.Count>0)
            //channelMachineService.Delete(p => p.Modify_Date < updateTime);
            //foreach ()



            DataSetPopTableAdapters.TF_TVSIZEINFOTableAdapter ad1 = new DataSetPopTableAdapters.TF_TVSIZEINFOTableAdapter();

            DataSetPop.TF_TVSIZEINFODataTable data1 = ad1.GetData();
            foreach (DataSetPop.TF_TVSIZEINFORow row in data1.Rows)
            {
                marketTvsizeinfoEntity ent = service2.FindEntity(row.T_TVSIZEID);
                if (ent != null)
                {

                  
                    ent.TVSIZE = row.TVSIZE;
                    ent.T_TVSIZENAME = row.T_TVSIZENAME;
                    service2.Update(ent);

                }
                else
                {
                    ent = new marketTvsizeinfoEntity() {
                        T_TVSIZEID = row.T_TVSIZEID,  TVSIZE=  row.TVSIZE, T_TVSIZENAME=row.T_TVSIZENAME
                    };
                    service2.Insert(ent);
                }

             }

           DataSetPopTableAdapters.JS5_S12_MACHINE_MODELTableAdapter ad = new DataSetPopTableAdapters.JS5_S12_MACHINE_MODELTableAdapter();
            //OrcalMode.JS5_S12_MACHINE_MODELDataTable data = ad.GetDataByLastModifDate(quyrDate);
            DataSetPop.JS5_S12_MACHINE_MODELDataTable data = ad.GetData();

            foreach (DataSetPop.JS5_S12_MACHINE_MODELRow row in data.Rows)
            {
                marketMachineModelEntity ent = service.FindEntity(row.ID);
                if (ent != null)
                {
                    ent.COMPANY_CODE = row.COMPANY_CODE;
                    ent.DESCP = row.DESCP;
                    ent.isActive = (int)row.ACTIVE_FLAG;
                    ent.MACHINE_MODEL_NO = row.MACHINE_MODEL_NO;
                    ent.TVSIZE = row.TVSIZE;
                    ent.SALE_PRICE = row.SALE_PRICE;
                    ent.T_TVSIZEID = row.T_TVSIZEID;
                    ent.BANDCODE = row.CDG1201_CODE;
                    ent.BANDNAME = row.CDG1201_NAME;
                   // ent.Modify_Date = DateTime.ParseExact(row.LAST_MODIFY_DATE, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    service.Update(ent);
                }
                else
                {
                      ent = new marketMachineModelEntity()
                    {
                        id = row.ID, COMPANY_CODE= row.COMPANY_CODE, DESCP=row.DESCP, isActive= (int)row.ACTIVE_FLAG, MACHINE_MODEL_NO= row.MACHINE_MODEL_NO
                        , TVSIZE= row.TVSIZE, SALE_PRICE=row.SALE_PRICE, T_TVSIZEID=row.T_TVSIZEID
                          //Modify_Date =DateTime.ParseExact(row.LAST_MODIFY_DATE, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture)
                       , BANDCODE = row.CDG1201_CODE,
                    BANDNAME = row.CDG1201_NAME
                };
                    service.Insert(ent);
                 }

            }
        }
    }
}
