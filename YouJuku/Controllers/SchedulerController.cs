using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using Microsoft.AspNet.Identity;
using YouJuku.Models;

namespace YouJuku.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private const int FirstHour = 14;
        private const int LastHour = 23;
        private const int HourSize = 132;

        // GET: Scheduler
        public ActionResult Index()
        {
            var scheduler = new DHXScheduler(this)
            {
                LoadData = true,
                EnableDataprocessor = true
            };

            scheduler.Extensions.Add(SchedulerExtensions.Extension.ActiveLinks);
            scheduler.Extensions.Add(SchedulerExtensions.Extension.Multisection);

            //get grid settings
            scheduler.Config.first_hour = FirstHour;
            scheduler.Config.last_hour = LastHour;
            scheduler.Config.hour_size_px = HourSize;
            scheduler.Config.week_date = "%l";
            scheduler.Config.day_date = "%D";
            scheduler.Config.hour_date = "%h:%i %A";

            //set duration of time period on light box
            scheduler.Config.event_duration = 60; 
            scheduler.Config.auto_end_date = true;

            var users = _context.Users;
            var options = new object[users.Count()];
            var timeline = new TimelineView("timeline", "user_id")
            {
                RenderMode = TimelineView.RenderModes.Bar,
                X_Start = 14,
                X_Size = 10,
                FirstHour = 2,
                LastHour = 11,
                X_Date = "%h:%i %A"
            };
            
            var i = 0;
            foreach (var user in users)
            {
                options[i] = new {key = user.Id, label = user.Email};
                i++;
            }

            timeline.AddOptions(options);
            scheduler.Views.Add(timeline);
        
            scheduler.BeforeInit.Add("youjukuScheduler.init()");
            AddLightBox(scheduler);

            return View(scheduler);
        }

        public ContentResult Data(int? timeshift, Int64? uid/*DateTime from, DateTime to*/)
        {
            var appUserId = User.Identity.GetUserId();
 
            // load current user's events as appointments
            var events = _context.SchedulerEvents.ToList();
 
            // load others as blocked intervals
            var blocked = _context.SchedulerEvents
                .Where(e => e.UserId != appUserId)
                .Select(e => new { e.UserId, e.StartDate, e.EndDate}).ToList();

            var response = new SchedulerAjaxData(events);
 
            return response;
        }

        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            try
            {
                var changedEvent = DHXEventsHelper.Bind<SchedulerEvent>(actionValues);
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        _context.SchedulerEvents.Add(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        _context.Entry(changedEvent).State = EntityState.Deleted;
                        break;
                    case DataActionTypes.Update:
                        _context.Entry(changedEvent).State = EntityState.Modified;
                        break;
                    case DataActionTypes.Error:
                        break;
                }
                _context.SaveChanges();
                action.TargetId = changedEvent.Id;
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }
 
            return (new AjaxSaveResponse(action));
        }

        private static void AddLightBox(DHXScheduler scheduler)
        {
            scheduler.Lightbox.Add(new LightboxText("text", "Class Notes"));
            var context = new ApplicationDbContext();
            var users = context.Users.Select(u => new { key = u.Id, label = u.Email, color = u.Color }).ToList();

            var select = new LightboxSelect("UserId", "Teacher");
            select.ServerList = "users";
            select.AddOptions(users);

            scheduler.Lightbox.Add(select);
            scheduler.Lightbox.Add(new LightboxTime("time", "Time"));
        }
    }
}