using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbdulLCTest.Business;
using AbdulLCTest.Domain;
using Microsoft.AspNet.Identity;
using AbdulLCTest.UI.ViewModels;
using AutoMapper;

namespace AbdulLCTest.UI.Controllers
{
   [Authorize]
    public class MessageController : Controller
    {
        private readonly IPostServices _postservice;
        private PostBDO postmsg = new PostBDO();

        public MessageController(IPostServices postservice)
        {
            this._postservice = postservice;
        }

        // GET: Message
        public ActionResult Index()
        {
            var messages = this._postservice.GetAllPosts();
            return View("Index", messages);
        }

        // GET: Message/Details/5
        public ActionResult Details(int id=0)
        {
            if(id==0)
                return RedirectToAction("Index");

            var messagedetail = this._postservice.GetPostById(id);
            return View("Details", messagedetail);
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(PostMessageViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    postmsg.Subject = model.Subject;
                    postmsg.Description = model.Description;
                    postmsg.CreatedBy = User.Identity.GetUserId();
                    postmsg.CreatedDate = DateTime.Now;

                    this._postservice.CreatePost(postmsg);        
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Message/Edit/5
        public ActionResult Edit(int id=0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            var getMessage = this._postservice.GetPostById(id);
            if (getMessage == null)
            {
                return HttpNotFound();
            }
            else
            {
                Mapper.Initialize(cfg => cfg.CreateMap<PostBDO, PostMessageViewModel>());
                var model = Mapper.Map<PostBDO, PostMessageViewModel>(getMessage);
                return View(model);
            }

            //return View();
        }

        // POST: Message/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, PostMessageViewModel model)
        {
            try
            {
                postmsg.Id = id;
                postmsg.Subject = model.Subject;
                postmsg.Description = model.Description;
                postmsg.ModifiedBy = User.Identity.GetUserId();
                postmsg.ModifiedDate = DateTime.Now;

                this._postservice.UpdatePost(postmsg);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Message/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            this._postservice.DeletePost(id);
            return View();
        }

        // POST: Message/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
