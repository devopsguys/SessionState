using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SessionStatePerformanceTestMVC.Controllers
{
    public class SessionController : Controller
    {
        //
        // GET: /Session/

        public ActionResult Simple(int id = 1)
        {
            Common.SessionStateManager sessionStateMgr = new Common.SimpleSessionStateManager();
            sessionStateMgr.Start();
            sessionStateMgr.AddDataToSession(id);
            sessionStateMgr.LoadDataFromSession();
            sessionStateMgr.RemoveDataFromSession();
            sessionStateMgr.Finish();
            return View();
        }


        public ActionResult Complex(int id = 1)
        {
            Common.SessionStateManager sessionStateMgr = new Common.ComplexSessionStateManager();
            sessionStateMgr.Start();
            sessionStateMgr.AddDataToSession(id);
            sessionStateMgr.LoadDataFromSession();
            sessionStateMgr.RemoveDataFromSession();
            sessionStateMgr.Finish();
            return View();
        }

    }
}
