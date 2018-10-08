using DataSynchronizationLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSynchronization
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{

               Synchronization syn = new Synchronization();

            //syn.SynUser();
            //syn.SynShopInfo();
            //syn.SynSalesManagerOrg();
            //syn.synSap();
            //syn.SynSalesManagerOrg();

            //syn.SynTaskMessage();

            //syn.synSamle();
            
            syn.deleteHis();
            syn.SynUser();
            syn.SynShop2();
            syn.SynBrand();
            syn.SynMachineModel();
            syn.SynShopInfo();
            syn.SynSalesManagerOrg();
            syn.SynTasks();
            

            //}
            //catch (Exception ex)
            //{
            //    System.Console.Write(ex.Message);
            //}
            //System.Console.ReadKey();

        }
    }
}
