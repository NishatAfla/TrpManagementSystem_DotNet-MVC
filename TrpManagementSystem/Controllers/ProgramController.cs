using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrpManagementSystem.DTO;
using TrpManagementSystem.EF;

namespace TrpManagementSystem.Controllers
{
    public class ProgramController : Controller
    {
        DotnetDBEntities db = new DotnetDBEntities();

        public static Program Convert(ProgramDTO d)
        {
            return new Program
            {
                ProgramId = d.ProgramId,
                ChannelId = d.ChannelId,
                ProgramName = d.ProgramName,
                TRPScore = d.TRPScore,
               
                AirTime = TimeSpan.Parse(d.AirTime)
            };


        }
        public static ProgramDTO Convert(Program d)
        {
            return new ProgramDTO
            {
                ProgramId = d.ProgramId,
                ChannelId = d.ChannelId,
                ProgramName = d.ProgramName,
                TRPScore = d.TRPScore,

                AirTime = d.AirTime.ToString(@"hh\:mm\:ss")
            };
        }
        public static List<ProgramDTO> Convert(List<Program> data)
        {
            var list = new List<ProgramDTO>();
            foreach (var d in data)
            {
                list.Add(Convert(d));
            }
            return list;
        }

        public ActionResult ProgramList()
        {
            var data = db.Programs.ToList();
            return View(Convert(data));
        }
        [HttpGet]

        public ActionResult CreateProgram()
        {
            var channels = db.ChannelTbls.Select(c => new SelectListItem
            {
                Value = c.ChannelId.ToString(),
                Text = c.ChannelName
            }).ToList();

            ViewBag.Channels = channels;
            return View(new ProgramDTO());
           // return View(new Program());
        }
        [HttpPost]
        public ActionResult CreateProgram(ProgramDTO d)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Programs.Add(Convert(d));
                    db.SaveChanges();
                    return RedirectToAction("ProgramList");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving data: " + ex.Message);
                }
            }

            // Repopulate ViewBag for dropdown on post-back
            ViewBag.Channels = db.ChannelTbls.Select(c => new SelectListItem
            {
                Value = c.ChannelId.ToString(),
                Text = c.ChannelName
            }).ToList();

            
            return View(d);


        }

          [HttpGet]
          public ActionResult Edit(int id)
          {

              var exobj = db.Programs.Find(id);

              return View(Convert(exobj));
          }
          [HttpPost]
          public ActionResult Edit(ProgramDTO d)
          {
            if (ModelState.IsValid)
            {
                var exobj = db.Programs.Find(d.ProgramId);
                if (exobj != null)
                {
                   
                        // Map properties manually to handle AirTime conversion
                        exobj.ProgramName = d.ProgramName;
                        exobj.TRPScore = d.TRPScore;

                        // Convert AirTime from string to TimeSpan
                        if (TimeSpan.TryParse(d.AirTime, out TimeSpan parsedAirTime))
                        {
                            exobj.AirTime = parsedAirTime;
                        }
                        else
                        {
                            ModelState.AddModelError("AirTime", "Invalid time format. Please use a valid TimeSpan format (e.g., 'hh:mm:ss').");
                            return View(d);
                        }

                        

                        db.SaveChanges();
                        return RedirectToAction("ProgramList");
                   
                }
                else
                {
                    ModelState.AddModelError("", "Program not found.");
                }
            }

            return View(d);

          }
        
          [HttpGet]
          public ActionResult DeleteProgram(int id)
          {
              var exobj = db.Programs.Find(id);
              return View(Convert(exobj));
          }
          [HttpPost]
          public ActionResult DeleteProgram(int Id, string dcsn)
          {
              if (dcsn.Equals("Yes"))
              {
                  var exobj = db.Programs.Find(Id);
                  db.Programs.Remove(exobj);
                  db.SaveChanges();

              }
              return RedirectToAction("ProgramList");
          }
    }
}