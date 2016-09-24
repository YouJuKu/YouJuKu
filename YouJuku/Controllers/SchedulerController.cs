using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
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

        // GET: Scheduler
        public ActionResult Index()
        {
            var scheduler = new DHXScheduler(this)
            {
                Skin = DHXScheduler.Skins.Flat,
                LoadData = true,
                EnableDataprocessor = true,
                InitialDate = new DateTime(2013, 5, 5)
            };
            scheduler.Extensions.Add(SchedulerExtensions.Extension.ActiveLinks);
            scheduler.Extensions.Add(SchedulerExtensions.Extension.Collision);
            scheduler.EnableDynamicLoading(SchedulerDataLoader.DynamicalLoadingMode.Month);

            scheduler.BeforeInit.Add("schedulerAdmin.init()");

            scheduler.Lightbox.Add(new LightboxText("text", "Description"));
            var context = new ApplicationDbContext();
            var users = context.Users.Select(u => new { key = u.Id, label = u.Email }).ToList();

            var select = new LightboxSelect("UserId", "User");
            select.ServerList = "users";
            select.AddOptions(users);

            scheduler.Lightbox.Add(select);
            scheduler.Lightbox.Add(new LightboxTime("time", "Time period"));

            return View(scheduler);
        }

        public ContentResult Data(DateTime from, DateTime to)
        {
            var appUserId = User.Identity.GetUserId();
 
            // load current user's events as appointments
            var apps = _context.SchedulerEvents
                .Where(e => e.UserId == appUserId /*&& e.StartDate < to && e.EndDate > from*/).ToList();
 
            // load others as blocked intervals
            var blocked = _context.SchedulerEvents
                .Where(e => e.UserId != appUserId /*&& e.StartDate < to && e.EndDate > from*/)
                .Select(e => new { e.StartDate, e.EndDate}).ToList();

            var response = new SchedulerAjaxData(apps);
            response.ServerList.Add("blocked_time", blocked);
 
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
                action.TargetId = changedEvent.ID;
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }
 
            return (new AjaxSaveResponse(action));
        }
    }
}