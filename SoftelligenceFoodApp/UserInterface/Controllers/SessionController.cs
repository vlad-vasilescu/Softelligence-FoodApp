﻿using BusinessLogic;
using Logic.Implementations;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;
using BusinessLogic.BusinessExceptions;
using System;
using System.Linq;

namespace UserInterface.Controllers
{
    public class SessionController : Controller
    {
        private readonly AdminService adminService;
        public SessionController(AdminService adminService)
        {
            this.adminService = adminService;

        }

        public IActionResult Index()
        {
            var sessionsList = adminService.GetAllSessions();
            return View(sessionsList);
        }

        public IActionResult History()
        {
            ViewBag.ViewName = "Session";
            var sessionList = adminService.GetAllSessions();
            return View(sessionList);

        }

        public IActionResult NewSession()
        {
            SessionVM session = new SessionVM();

            try
            {
                session.Session = adminService.GetActiveSession();
                session.HasActiveSession = true;
                session.Stores = session.Session.Stores
                                                .ToList();
            }
            catch (SessionNotFoundException)
            {
                session.HasActiveSession = false;
                session.Stores = adminService.GetAllStores()
                                            .ToList();
            }
            return View(session);

        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            Session session = adminService.GetSessionById(id.Value);
            return View(session);
        }

        [HttpPost]
        public IActionResult Create([FromForm]SessionVM newSession)
        {
            if (ModelState.IsValid)
            {
                Session sessionToCreate = new Session();
                sessionToCreate.StartTime = DateTime.Now;

                for (int i = 0; i < newSession.Stores.Count(); i++)
                {
                    if (newSession.SelectedStores[i])
                    {
                        var currentStore = adminService.GetStoreById(newSession.Stores[i].Id);
                        sessionToCreate.AddStore(currentStore);
                    }

                }
                adminService.StartSession(sessionToCreate);
            }
            return RedirectToAction("NewSession");
        }

        [HttpGet]
        public IActionResult CloseRestaurant(int? id)
        {
            Store store = adminService.GetStoreById(id.Value);
            return View(store);
        }

        [HttpPost]
        public IActionResult CloseRestaurant([FromForm]Store storeToRemoveFromSession)
        {
            if (ModelState.IsValid)
            {
                storeToRemoveFromSession = adminService.GetStoreById(storeToRemoveFromSession.Id);
                storeToRemoveFromSession.IsActive = false;
                adminService.UpdateStore(storeToRemoveFromSession);
            }
            return RedirectToAction("NewSession");
        }

        [HttpGet]
        public IActionResult FinalizeSession(int? id)
        {
            Session session = adminService.GetSessionById(id.Value);

            return View(session);
        }

        [HttpPost]
        public IActionResult FinalizeSession([FromForm]SessionVM sessionToFinalize)
        {
            if (ModelState.IsValid)
            {
                sessionToFinalize.HasActiveSession = false;
                
            }
            return RedirectToAction("Index");
        }
    }
}