using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class ProductController1 : Controller
    {
        private readonly ApplicationDbContext db;
        private ApplicationDbContext _db;

        public ProductController1(ApplicationDbContext db)
        {
            _db = db;

        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product;
            foreach (var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategorId);
            };

            return View(objList);
        }
        //GİDİŞATA GÖRE ALT TARAF DEĞİŞİKLİK GÖSTEREBİLİR...

        //GET UPSERT
        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            if (id==null)
            {
                return View(product);
            }
            else
            {
                product = _db.Product.Find(id);
                if (product==null)
                {
                    return NotFound();
                }
                return View(product);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }


        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id);
            if (obj==null)
            {
                return NotFound();
            }
            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
