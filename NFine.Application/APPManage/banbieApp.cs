
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.APPManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.APPManage
{
    public class banbieApp
    {
        private IbanbieRepository service = new banbieRepository();

        public List<BanbieEntity> getBanbie(string shopCode)
        {
            return service.IQueryable().Where(p => p.shopCode.Equals(shopCode)).ToList();
        }
        public void saveEnts(string shopCode, int A_StartH, int A_StartM, int A_EndH, int A_EndM, int B_StartH, int B_StartM, int B_EndH, int B_EndM
            , int C_StartH, int C_StartM, int C_EndH, int C_EndM, int D_StartH, int D_StartM, int D_EndH, int D_EndM)
        {
            var ents = service.IQueryable().Where(p => p.shopCode.Equals(shopCode) && p.type.Equals("A")).ToList();
            if (ents.Count() > 0)
            {
                var ent = ents.First();
                ent.startH = A_StartH.ToString();
                ent.startM = A_StartM.ToString();
                ent.endH = A_EndH.ToString();
                ent.endM = A_EndM.ToString();
                service.Update(ent);
            }
            else
            {
                var ent = new BanbieEntity() {
                    id = System.Guid.NewGuid().ToString(),
                    type = "A",
                    shopCode = shopCode,
                    startH = A_StartH.ToString(),
                    startM = A_StartM.ToString(),
                    endH = A_EndH.ToString(),
                    endM = A_EndM.ToString()
                
                };
                service.Insert(ent);

            }
            ents = service.IQueryable().Where(p => p.shopCode.Equals(shopCode) && p.type.Equals("B")).ToList();
            if (ents.Count() > 0)
            {
                var ent = ents.First();
                ent.startH = B_StartH.ToString();
                ent.startM = B_StartM.ToString();
                ent.endH = B_EndH.ToString();
                ent.endM = B_EndM.ToString();
                service.Update(ent);
            }
            else
            {
                var ent = new BanbieEntity()
                {
                    id = System.Guid.NewGuid().ToString(),
                    type = "B",
                    shopCode = shopCode,
                    startH = B_StartH.ToString(),
                    startM = B_StartM.ToString(),
                    endH = B_EndH.ToString(),
                    endM = B_EndM.ToString()

                };
                service.Insert(ent);

            }
            ents = service.IQueryable().Where(p => p.shopCode.Equals(shopCode) && p.type.Equals("C")).ToList();
            if (ents.Count() > 0)
            {
                var ent = ents.First();
                ent.startH = C_StartH.ToString();
                ent.startM = C_StartM.ToString();
                ent.endH = C_EndH.ToString();
                ent.endM = C_EndM.ToString();
                service.Update(ent);
            }
            else
            {
                var ent = new BanbieEntity()
                {
                    id = System.Guid.NewGuid().ToString(),
                    type = "C",
                    shopCode = shopCode,
                    startH = C_StartH.ToString(),
                    startM = C_StartM.ToString(),
                    endH = C_EndH.ToString(),
                    endM = C_EndM.ToString()

                };
                service.Insert(ent);

            }
            ents = service.IQueryable().Where(p => p.shopCode.Equals(shopCode) && p.type.Equals("D")).ToList();
            if (ents.Count() > 0)
            {
                var ent = ents.First();
                ent.startH = D_StartH.ToString();
                ent.startM = D_StartM.ToString();
                ent.endH = D_EndH.ToString();
                ent.endM = D_EndM.ToString();
                service.Update(ent);
            }
            else
            {
                var ent = new BanbieEntity()
                {
                    id = System.Guid.NewGuid().ToString(),
                    type = "D",
                    shopCode = shopCode,
                    startH = D_StartH.ToString(),
                    startM = D_StartM.ToString(),
                    endH = D_EndH.ToString(),
                    endM = D_EndM.ToString()

                };
                service.Insert(ent);

            }
        }
    }
}
