using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class userKaoqinEntity : IEntity<userKaoqinEntity>
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string kaoqin_Type { get; set; }
        public DateTime? checkTime { get; set; }
        public double? LONGITUDE { get; set; }
        public double? LATITUDE { get; set; }
        public double? DISTINCE { get; set; }
        public int? is_Sync { get; set; }
        public string file_id { get; set; }
        public string adder { get; set; }
        


    }
}
