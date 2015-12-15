using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            var model = DocumentCollectionOperations.GetAllDocuments().Select(DocumentViewModel.From);

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

            var model = DocumentViewModel.From(DocumentCollectionOperations.Get(id));

            return View("Document", model);

        }
        [HttpPost]
        public ActionResult NewDocument(DocumentViewModel doc)
        {
            
            DocumentCollectionOperations.InsertOrUpdateDocument(doc.ToDocument());

            return RedirectToAction("Index");
        }

        public ActionResult Create(Document idoc)
        {
            return View();
        }

        public ActionResult Delete(string id)
        {
            DocumentCollectionOperations.Delete(id);

            return RedirectToAction("Index");
        }
    }
}