
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.APPManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.TaskManage
{
    public class PSIApp
    {
        IPsiCmRepository repository = new PsiCmRepository();
        IPsiTagMastRepository tagrepository = new PsiTagMastRepository();
        public int TotalCmQty(string ProdectValue, string MonthValue, string PorgId,string BrandCode)
        {
            
            String startDay = DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd 00:00:00");
            string endDay= DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
           var ents= repository.FindList("SELECT * FROM psi_cm where Prodect='"+ ProdectValue + "' and BrandCode='"+ BrandCode + "' and PorgId='" + PorgId + "'  and day>='"+ startDay + "' and day<='"+ endDay + "'");
            var qty = ents.Sum(p => p.cmQty);

            return qty.HasValue ? qty.Value : 0;
        }
        public List<PsiTagMastEntity> TagList(string ProdectValue, string MonthValue, string PorgId,string BrandCode)
        {
           try
            {
         
                var ents = tagrepository.FindList("SELECT * FROM psi_tagmast where Prodect='" + ProdectValue + "' and BrandCode='"+ BrandCode + "' and PorgId='" + PorgId + "'  and month='"+ MonthValue + "'");


                return ents;
            }
            catch
            {
                return new List<PsiTagMastEntity>();
            }

        }
        public List<PsiTagMastEntity> TagList(string ProdectValue, string MonthValue, string PorgId,string userId,string BrandCode)
        {
            try
            {

            //    var ents = tagrepository.FindList("SELECT * FROM psi_tagmast where Prodect='" + ProdectValue + "' and orgId='" + PorgId + "'  and month='" + MonthValue + "' and UserId='"+ userId + "'");
            var ents = tagrepository.IQueryable().Where(p => p.UserId.Equals(userId) && p.month.Equals(MonthValue) && p.orgId.Equals(PorgId) && p.Prodect.Equals(ProdectValue) && p.BrandCode.Equals(BrandCode)).ToList();

            return ents;
            }
            catch
            {
                return new List<PsiTagMastEntity>();
            }

        }
        public List<PsiCmEntity> CmList(string ProdectValue, string MonthValue, string PorgId,string BrandCode)
        {
           try
            {
                String startDay = DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd 00:00:00");
                string endDay = DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
                var ents = repository.FindList("SELECT * FROM psi_cm where Prodect='" + ProdectValue + "' and BrandCode='"+ BrandCode + "' and PorgId='" + PorgId + "'  and day>='" + startDay + "' and day<='" + endDay + "'");


                return ents;
            }
            catch {
                return new List<PsiCmEntity>();
            }
        }
        public void setStatus(string ProdectValue, string MonthValue, string PorgId, string userId,int status,string BrandCode)
        {
            //try
            //{
            String startDay = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string endDay = DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            var ents = repository.FindList("SELECT * FROM psi_cm where Prodect='" + ProdectValue + "' and BrandCode='"+ BrandCode + "' and orgId='" + PorgId + "'  and day>='" + startDay + "' and day<='" + endDay + "' and userid='" + userId + "'");
            foreach (var ent in ents.ToList())
            {
                ent.status = status;
                ent.modifyTime = DateTime.Now;
                repository.Update(ent);
            }

            
            //}
            //catch {
            //  return new List<PsiCmEntity>();
            //}
        }
        public List<PsiCmEntity> CmList(string ProdectValue, string MonthValue, string PorgId, string userId,string BrandCode)
        {
            //try
            //{
                String startDay = DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd 00:00:00");
                string endDay = DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
                var ents = repository.FindList("SELECT * FROM psi_cm where Prodect='" + ProdectValue + "' and BrandCode='"+ BrandCode + "' and orgId='" + PorgId + "'  and day>='" + startDay + "' and day<='" + endDay + "' and userid='" + userId + "'");


                return ents;
            //}
            //catch {
              //  return new List<PsiCmEntity>();
            //}
        }
        public int TotalCmQty(string ProdectValue, string MonthValue, string PorgId,string userId,string BrandCode)
        {

            String startDay = DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd 00:00:00");
            string endDay = DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            var ents = repository.FindList("SELECT * FROM psi_cm where Prodect='" + ProdectValue + "' and BrandCode='"+ BrandCode + "' and PorgId='" + PorgId + "'  and day>='" + startDay + "' and day<='" + endDay + "' and userid='"+ userId + "'");
            var qty = ents.Sum(p => p.cmQty);

            return qty.HasValue ? qty.Value : 0;
        }
        public void cQty(string ProdectValue, string userId, string createId, string PorgId, string orgid, int Qty,string day,string BrandCode)
        {
            String startDay = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd 00:00:00");
            string endDay = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd 23:59:59");
            var ents = repository.FindList("SELECT * FROM psi_cm where Prodect='" + ProdectValue + "' and BrandCode='"+ BrandCode + "' and PorgId='" + PorgId + "'  and day>='" + startDay + "' and day<='" + endDay + "' and userid='" + userId + "'").ToList();

            if (ents.Count() > 0)
            {
                foreach (var ent in ents)
                {
                    ent.cmQty = Qty;
                    ent.modifyTime = System.DateTime.Now;
                    ent.status = 0;

                    repository.Update(ent);
                }
            }
            else
            {
                repository.Insert(new PsiCmEntity() {
                    id = Guid.NewGuid().ToString(),
                    cmQty = Qty, createdTime=DateTime.Now,modifyTime=DateTime.Now, day = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.CurrentCulture),
                     orgId= orgid, PorgId= PorgId, BrandCode= BrandCode, Prodect =ProdectValue, status=0, UserId=userId
                });
            }
        }
        public void shTag(string ProdectValue, string userId, string createId, string PorgId, string orgid, string MonthValue,int Qty,string BrandCode)
        {
            var ents = tagrepository.IQueryable().Where(p => p.UserId.Equals(userId) &&p.BrandCode.Equals(BrandCode) && p.month.Equals(MonthValue) && p.orgId.Equals(orgid) && p.Prodect.Equals(ProdectValue)).ToList();
            if (ents.Count() > 0)
            {
                foreach (var ent in ents)
                {
                    ent.totalTagQty = Qty;
                    ent.modifyTime = System.DateTime.Now;
                    tagrepository.Update(ent);
                }
            }
            else
            {
                PsiTagMastEntity ent = new PsiTagMastEntity() { id=System.Guid.NewGuid().ToString(),
                    createdTime = System.DateTime.Now,
                    modifyTime= System.DateTime.Now, month= MonthValue, orgId= orgid, BrandCode= BrandCode, PorgId = PorgId, Prodect= ProdectValue, totalTagQty= Qty,
                     UserId= userId

                };
                tagrepository.Insert(ent);
            }
        }

    }
}
