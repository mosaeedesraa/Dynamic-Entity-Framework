using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.Web.Configuration;
using TestSite.Models;
using System.Collections;
namespace TestSite.Controllers
{
    public class HomeController : Controller
    {
        public static string ConnectionString = WebConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        Tables<Test> dc = new Tables<Test>(ConnectionString); 
        //
        // GET: /HOme/
        
        public ActionResult Index()
        {
            return View(dc.AllData());
        }

        [HttpPost]
        public ActionResult AddTest(string Name , Int32 Age , string Address)
        {
            Test t = new Test();
            t.Address = Address;
            t.Age = Age;
            t.Name = Name;
            bool StateInsert = dc.Insert(t);
            return Json(StateInsert , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRow(Int32 ID)
        {
            return Json(dc.Find(ID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateTest(Int32 ID , string Name, Int32 Age, string Address)
        {
            Test t = dc.Find(ID);
            t.Address = Address;
            t.Age = Age;
            t.Name = Name;
            bool StateInsert = dc.Update(t);
            return Json(StateInsert, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteTest(Int32 ID)
        {
            return Json(dc.Delete(ID), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CallSP(Int32 ID)
        {
            string[] param = { "@id"};
            ArrayList arr = new ArrayList();
            arr.Add(ID);
            var data = dc.Select("GetRowData", arr, param).AsEnumerable().Select(x => new Test() { 
             ID = x.ID,
             Name = x.Name , Age = x.Age , Address = x.Address
            
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }
    }
}
