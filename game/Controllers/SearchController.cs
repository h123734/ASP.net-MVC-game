using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using game.Models;
using System.IO;
using PagedList.Mvc;
using PagedList;

namespace game.Controllers
{
    public class SearchController : Controller
    {
        h123734Entities dbs = new h123734Entities();

        // GET: Search
        private int pageSize = 5;
        public ActionResult Index(int page=1)
        {
            int currentPage = page < 1 ? 1 : page;
            var model = dbs.pPrice.OrderBy(x => x.gTitle);
            //var model = (from s in dbs.pPrice select s);
            var result=model.ToPagedList(currentPage, pageSize);
            if (Session["Member"] == null)
            {return View("Index1", "_Layout",result ); }
                else
            return View("Index", "_Layout_user", result);
        }

        

        public ActionResult Detail(int? id)
        {  if (id == null)
            { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }


            var result = dbs.pPrice.FirstOrDefault(x => x.gId == id);
               if(result==null)
                {  return HttpNotFound();
            }
            return View(result);
        }

        
            
        public ActionResult SearchByTitle(string gtitle,int page=1)
        {
            int currentPage = page < 1 ? 1 : page;
            var price = (from m in dbs.pPrice select m);
            ViewBag.gtitle = gtitle;
            if (!String.IsNullOrEmpty(gtitle))
            {
                price = price.Where(s => s.gTitle.Contains(gtitle));
                
                } var result = price.OrderBy(x => x.gTitle).ToPagedList(currentPage,5);
            if (Session["Member"] == null) { return View("Index1", result); }
            else
            return View("Index",result);                  
        }
        //public ActionResult SearchByTitle1(string title)
        //{
        //    var price = from m in dbs.pPrice select m;
        //    if (!String.IsNullOrEmpty(title))
        //    {
        //        price = price.Where(s => s.gTitle.Contains(title));
        //    }
        //    return View("Index1", price);
        //}
    }
}