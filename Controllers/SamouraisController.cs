using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using Samourais.Data;
using Samourais.Models;

namespace Samourais.Controllers
{
    public class SamouraisController : Controller
    {
        private SamouraisContext db = new SamouraisContext();

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            SamouraiVM samouraiVM = new SamouraiVM()
            {
                Samourai = new Samourai()
            };
            samouraiVM = InitListSamouraiVM(samouraiVM);
            return View(samouraiVM);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                UpdateSamourai(samouraiVM);
                db.Samourais.Add(samouraiVM.Samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            samouraiVM = InitListSamouraiVM(samouraiVM);
            return View(samouraiVM);
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            SamouraiVM samouraiVM = new SamouraiVM()
            {
                Samourai = samourai
            };
            samouraiVM = InitListSamouraiVM(samouraiVM);
            if(samourai.Arme != null)
            {
                samouraiVM.IdArme = samourai.Arme.Id;
                samouraiVM.ListeArmes.Add(samourai.Arme);
            }
            
            return View(samouraiVM);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                Samourai sam = db.Samourais.Find(samouraiVM.Samourai.Id);
                Arme arme = sam.Arme;
                List<ArtMartial> lam = sam.ArtMartials;
                samouraiVM.Samourai = sam;
                samouraiVM = UpdateSamourai(samouraiVM);
                db.Entry(sam).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            samouraiVM = InitListSamouraiVM(samouraiVM);
            return View(samouraiVM);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<Arme> GetArmesDisponibles()
        {
            return db.Armes.Where(y => !db.Samourais.Select(x => x.Arme.Id).ToList().Contains(y.Id)).ToList();
        }

        private List<ArtMartial> GetArtMartials()
        {
            return db.ArtMartials.ToList();
        }

        private SamouraiVM UpdateSamourai(SamouraiVM samouraiVM)
        {
            samouraiVM.Samourai.Arme = db.Armes
                .FirstOrDefault(x => x.Id == samouraiVM.IdArme);
            samouraiVM.Samourai.ArtMartials = db.ArtMartials
                .Where(x => samouraiVM.idArtMartials
                .Contains(x.Id))
                .ToList();
            return samouraiVM;
        }

        private SamouraiVM InitListSamouraiVM(SamouraiVM samouraiVM)
        {
            samouraiVM.ListeArmes = GetArmesDisponibles();
            samouraiVM.ListeArtMartials = GetArtMartials();
            samouraiVM.idArtMartials = samouraiVM.Samourai.ArtMartials
                .Select(x => x.Id)
                .ToList();
            return samouraiVM;
        }
    }
}
