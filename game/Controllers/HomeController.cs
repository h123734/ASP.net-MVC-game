using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using game.Models;

namespace game.Controllers
{   
    
    public class HomeController : Controller
    {
        h123734Entities dbo = new h123734Entities();
        
        public ActionResult Index()
        {

            if (Session["Member"] == null)
            { return View("Index", "_Layout"); }                       
                   
           else
            return View("Index","_Layout_user");
        }
        public ActionResult Index1()
        {
            return View("Index1","_Layout_admin");
        }



            public ActionResult About()
        {
            ViewBag.Message = "關於本網站";
            if (Session["Member"] == null)
            { return View("About", "_Layout"); }

            else
                return View("About", "_Layout_user");
        }

       
        public ActionResult List()
        {
            var gps = dbo.pPrice.OrderByDescending(m => m.gDate).ToList();
            ViewBag.Message = "List page.";

            return View("List", "_Layout_admin",gps);
        }
        
        public ActionResult Create()
        {
            return View("Create","_Layout_admin");
        }
        
        [HttpPost]
        public ActionResult Create(pPrice pri)
        {
            if (ModelState.IsValid)
            {
                dbo.pPrice.Add(pri);
                dbo.SaveChanges();
                return RedirectToAction("List");
            }
            return View(pri);
        }
       
        public ActionResult Delete(int id)
        {
            var data = dbo.pPrice.Where(m => m.gId == id).FirstOrDefault();
            dbo.pPrice.Remove(data);
            dbo.SaveChanges();
            return RedirectToAction("List");
        }
        
        public ActionResult Edit(int id)
        {
            var data = dbo.pPrice.Where(m => m.gId == id).FirstOrDefault();
            return View("Edit", "_Layout_admin",data);
        }
        [HttpPost]
        public ActionResult Edit(int gId, string gTitle,string gCompany,DateTime gDate,decimal Price,string Picture,string video,
            decimal size,string type, string player, string language,string purchase )
        
        {
            var data = dbo.pPrice.Where(m => m.gId == gId).FirstOrDefault();
            data.gTitle = gTitle;
            data.gCompany = gCompany;
            data.gDate = gDate;
            data.Price = Price;
            data.Picture = Picture;
            data.video = video;
            data.size = size;
            data.type = type;
            data.player = player;
            data.language = language;
            data.purchase = purchase;

            dbo.SaveChanges();
            return RedirectToAction("List");
        }
       
    ////public ActionResult Upload()
    ////    {
    ////        return View("Upload", "_Layout_admin");
    ////    }



    ////    [HttpPost]
    ////    public ActionResult Upload(HttpPostedFileBase file)
    ////    {
    ////        if (file.ContentLength > 0)
    ////        {
    ////            var fileName = Path.GetFileName(file.FileName);
    ////            var path = Path.Combine(Server.MapPath("~/img"), fileName);
    ////            file.SaveAs(path);
    ////        }
    ////        return RedirectToAction("List");
        //}

        public ActionResult UserDel()
        {
            var dbs = dbo.tMember.OrderByDescending(m => m.fId).ToList();
            ViewBag.Message = "UserList page";

            return View("UserDel", "_Layout_admin",dbs);
        }
        public ActionResult UserDelete(int id)
        {
            var data = dbo.tMember.Where(m => m.fId == id).FirstOrDefault();
            dbo.tMember.Remove(data);
            dbo.SaveChanges();
            return RedirectToAction("UserDel");
        }


        public ActionResult UserEdit()
        {
            int fId = (Session["Member"] as tMember).fId;

            var data = dbo.tMember.Where(m => m.fId == fId).FirstOrDefault();

            return View("UserEdit","_Layout_user", data);
        }


        [HttpPost]
        public ActionResult UserEdit(int fId, string fPwd,string fName,string fEmail)
        {
            var userdata = dbo.tMember.Where(m => m.fId == fId).FirstOrDefault();

            userdata.fPwd = fPwd;
            userdata.fName = fName;
            userdata.fEmail = fEmail;
            dbo.SaveChanges();

            return RedirectToAction("Login","Home");

        }



        public ActionResult Login()
        {
            return View();
        }
        //Post: Home/Login
        [HttpPost]
        public ActionResult Login(string fUserId, string fPwd)
        {   var member = dbo.tMember.Where(m => m.fUserId == fUserId && m.fPwd == fPwd).FirstOrDefault();

            if (member == null)
            {
                ViewBag.Message = "帳密錯誤，登入失敗";
                return View();
            } // 依帳密取得會員並指定給member


            //若member為null，表示會員未註冊


            else if (member.fUserId == "adm" && member.fPwd == "3345678")
            
            {
                return RedirectToAction("Index1");
            }

            else
            ////使用Session變數記錄歡迎詞
            {
                Session["WelCome"] = member.fName+ "歡迎光臨";
                //    //使用Session變數記錄登入的會員物件
                Session["Member"] = member;
                //    //執行Home控制器的Index動作方法
               //UserAccount.login(member.fId.ToString());
               return RedirectToAction("Index");
            }



        }

        //Get:Home/Register
        public ActionResult Register()
        {
            return View();
        }
        //Post:Home/Register
        [HttpPost]
        public ActionResult Register(tMember pMember)
        {
            //若模型沒有通過驗證則顯示目前的View
            if (ModelState.IsValid == false)
            {
                return View();
            }
            // 依帳號取得會員並指定給member
            var member = dbo.tMember
                .Where(m => m.fUserId == pMember.fUserId)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                //將會員記錄新增到tMember資料表
                dbo.tMember.Add(pMember);
                dbo.SaveChanges();
                //執行Home控制器的Login動作方法
                return RedirectToAction("Login");
            }
            ViewBag.Message = "此帳號己有人使用，註冊失敗";
            return View();
        }

        //Get:Index/Logout
        public ActionResult Logout()
        {
            Session.Clear();  //清除Session變數資料
            return RedirectToAction("Index");
        }
        private void GetFiles(string filePath)
        {
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] afi = di.GetFiles("*.*");//*.*可以不要
        }


    }
    
    
}