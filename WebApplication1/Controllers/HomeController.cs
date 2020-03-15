using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ParserCore;
using WebApplication1.ParserCore.Rozetka;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        GoodsContext db = new GoodsContext();

        public ActionResult Index()
        {
            return View();
        }

        ParserWorker<List<SmartphoneParams>> parser;

        [HttpPost]
        public async Task<ActionResult> GetData(string linkString)
        {
            ViewBag.SyncType = "Asynchronous";

            parser = new ParserWorker<List<SmartphoneParams>>(new RozetkaParser());
            parser.OnNewData += Parser_OnNewDataAsync;
            parser.Settings = new RozetkaParserSettings(linkString);
            await Task.Run(() => parser.Start());

            return View("Index");
        }

        private void Parser_OnNewDataAsync(object arg1, List<SmartphoneParams> arg2)
        {
            List<Smartphone> sm = new List<Smartphone>();

            foreach (var item in arg2)
            {
                sm.Add(new Smartphone()
                {
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image
                });
            }

            db.Smartphones.AddRange(sm);
            db.SaveChanges();
            // db.Dispose();
        }

        [Authorize]
        [HttpGet]
        public ActionResult ViewDB()
        {
            if (User.IsInRole("Admin"))
            {
                return View("ViewDataToAdmin", db.Smartphones);
            }
            else
                return View("ViewDataToUser", db.Smartphones);
        }

        [HttpGet]
        public ActionResult EditDB(int id)
        {
            Smartphone sm = db.Smartphones.Find(id);
            if (sm != null)
            {
                return View("EditSmartphoneData", sm);
            }

            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult EditDB(Smartphone sm, System.Web.HttpPostedFileBase uploadImage)
        {
            if (uploadImage == null)
            {
                sm.Image = db.Smartphones.AsNoTracking().FirstOrDefault(x => x.Id == sm.Id).Image;

                db.Entry(sm).State = EntityState.Modified;
                db.SaveChanges();

                return View("ViewDataToAdmin", db.Smartphones);
            }

            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
    
                using (var binaryReader = new System.IO.BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                sm.Image = imageData;

                db.Entry(sm).State = EntityState.Modified;
                db.SaveChanges();

                return View("ViewDataToAdmin", db.Smartphones);
            }

            return View("EditSmartphoneData", sm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("CreateNewObject");
        }
        [HttpPost]
        public ActionResult Create(Smartphone sm, System.Web.HttpPostedFileBase uploadImage)
        {
            if (uploadImage == null)
            {
                sm.Image = null;

                db.Entry(sm).State = EntityState.Added;
                db.SaveChanges();

                return View("ViewDataToAdmin", db.Smartphones);
            }

            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new System.IO.BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                sm.Image = imageData;

                db.Entry(sm).State = EntityState.Added;
                db.SaveChanges();

                return View("ViewDataToAdmin", db.Smartphones);
            }

            return View("EditSmartphoneData", sm);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Smartphone sm = db.Smartphones.Find(id);
            if (sm == null)
            {
                return HttpNotFound();
            }

            return View("DeleteObject", sm);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) 
        {
            Smartphone sm = db.Smartphones.Find(id);
            if (sm == null)
            {
                return HttpNotFound();
            }
            db.Smartphones.Remove(sm);
            db.SaveChanges();

            return View("ViewDataToAdmin", db.Smartphones);
        }
    }
}