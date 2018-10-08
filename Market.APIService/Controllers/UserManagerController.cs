using DataSynchronizationLib.SCMTableAdapters;
using DataSynchronizationStanbyLib.KPIDataSetTableAdapters;
using Market.APIService.Models;
using NFine.Application.APPManage;
using NFine.Application.TaskManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.APIService.Controllers
{
   

        public class UserManagerController : Controller
    {
        public ActionResult UserRegeditIndex()
        {
            return View("UserRegeditView");

        }
       

    }
}