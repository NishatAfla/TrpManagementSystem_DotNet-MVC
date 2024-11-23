using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrpManagementSystem.DTO;
using TrpManagementSystem.EF;

namespace TrpManagementSystem.Controllers
{
    public class TRPController : Controller
    {
        
        
            // GET: Department
            DotnetDBEntities db = new DotnetDBEntities();

            public static ChannelTbl Convert(ChannelDTO d)
            {
                return new ChannelTbl
                {
                    ChannelId = d.ChannelId,
                    ChannelName = d.ChannelName,
                    EstablishedYear = d.EstablishedYear,
                    Country = d.Country
                };
            }
            public static ChannelDTO Convert(ChannelTbl d)
            {
                return new ChannelDTO
                {
                    ChannelId = d.ChannelId,
                    ChannelName = d.ChannelName,
                    EstablishedYear = d.EstablishedYear,
                    Country = d.Country
                };
            }
            public static List<ChannelDTO> Convert(List<ChannelTbl> data)
            {
                var list = new List<ChannelDTO>();
                foreach (var d in data)
                {
                    list.Add(Convert(d));
                }
                return list;
            }
          
            public ActionResult ChannelList()
            {
                var data = db.ChannelTbls.ToList();
                return View(Convert(data));
            }
           [HttpGet]
            public ActionResult CreateChannel()
            {
                return View(new ChannelTbl());
            }
            [HttpPost]
            public ActionResult CreateChannel(ChannelDTO d)
            {
                //
                if (ModelState.IsValid)
                {

                    db.ChannelTbls.Add(Convert(d));
                    db.SaveChanges();
                    return RedirectToAction("ChannelList");
                }
                return View(d);

            }
          
            [HttpGet]
            public ActionResult Edit(int id)
            {
             
                var exobj = db.ChannelTbls.Find(id);
           
                return View(Convert(exobj));
            }
        [HttpPost]
        public ActionResult Edit(ChannelDTO d)
        {
            if (ModelState.IsValid)
            {
                var exobj = db.ChannelTbls.Find(d.ChannelId);
                if (exobj != null)
                {
                    db.Entry(exobj).CurrentValues.SetValues(d);
                    db.SaveChanges();
                    return RedirectToAction("ChannelList");
                }
                else
                {
                    ModelState.AddModelError("", "Channel not found.");
                }
            }
            return View(d);
        
        }
           [HttpGet]
            public ActionResult Delete(int id)
            {
                var exobj = db.ChannelTbls.Find(id);
                return View(Convert(exobj));
            }
         /*   [HttpPost]
            public ActionResult Delete(int Id, string dcsn)
            {
                if (dcsn.Equals("Yes"))
                {
                    var exobj = db.ChannelTbls.Find(Id);
                    db.ChannelTbls.Remove(exobj);
                    db.SaveChanges();

                }
                return RedirectToAction("ChannelList");
            }*/
       

        [HttpPost]
        public ActionResult Delete(int Id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var exobj = db.ChannelTbls.Find(Id);
                if (exobj != null)
                {
                    // Check for related programs
                    if (db.Programs.Any(p => p.ChannelId == Id))
                    {
                        ModelState.AddModelError("", "Cannot delete channel because it has associated programs.");
                        return View(Convert(exobj)); // Re-show the view with an error message
                    }

                    db.ChannelTbls.Remove(exobj);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        // Log exception (optional)
                        ModelState.AddModelError("", "An error occurred while trying to delete the channel.");
                        return View(Convert(exobj));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Channel not found.");
                }
            }
            return RedirectToAction("ChannelList");
        }



    }
}