using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevTube.Api;
using DevTube.Business;
using DevTube.Models;

namespace DevTube.Controllers
{
    public class DocumentController : Controller
    {
        public ActionResult Index(string parentId=null)
        {
            var q = DocumentsHelper.GetAllDocuments().AsQueryable();//.Where(z=>z.Size== 3151738);
            

            if (parentId == null)
            {
                ViewBag.Id = null;
                ViewBag.Level = 1;
                q = q.Where(i => i.Level == 1);
            }
            else
            {
                var doc = DocumentsHelper.Get(parentId);
                
                ViewBag.Id = doc.ParentId;
                ViewBag.Level = doc.Level+1;

                q = q.Where(i => i.ParentId == parentId);
            }
            var model=q.ToArray().Select(DocumentViewModel.From);

            return View(model);

        }

        // GET: Document
        public ActionResult NewDocument()
        {
            var model= DocumentViewModel.From(new Document());

            return View("Document",model);

        }
        public ActionResult Document(string id)
        {

            var model = DocumentViewModel.From(DocumentsHelper.Get(id));

            return View("Document", model);

        }
        [HttpPost]
        public ActionResult NewDocument(DocumentViewModel doc)
        {
            
            DocumentsHelper.InsertOrUpdateDocument(doc.ToDocument());

            return RedirectToAction("Index");
        }

        public ActionResult Create(Document idoc)
        {
            return View();
        }

        public ActionResult Delete(string id)
        {
            DocumentsHelper.Delete(id);

            return RedirectToAction("Index");
        }
    }
}