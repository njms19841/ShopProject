
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
    public class paibanApp
    {
        private IpaibanRepository service = new paibanRepository();
        public void saveEnt(paibanEntity ent)
        {
            var oldEnts = service.IQueryable().Where(p => p.Day.Equals(ent.Day) && p.userId.Equals(ent.userId)).ToList(); ;
            if (oldEnts.Count() > 0)
            {
                var oldent = oldEnts.First();
                oldent.StartTime = ent.StartTime;
                oldent.EndTime = ent.EndTime;
                oldent.Type = ent.Type;
                service.Update(oldent);
            }
            else
            {
                ent.id = System.Guid.NewGuid().ToString();
                service.Insert(ent);
            }
        }
        public List<paibanEntity> getPaibanEnts(string userId, string startDay, string endDay)
        {
            return service.FindList("select * from user_paiban where userId='"+ userId + "' and day>='"+ startDay + "' and day<='"+ endDay + "' order by  day ").ToList();
        }
        
    }
}
