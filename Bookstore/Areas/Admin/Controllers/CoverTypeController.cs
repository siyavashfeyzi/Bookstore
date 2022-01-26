using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var coverType = new CoverType();

            if (id == null)
            {
                //this is for create 
                return View(coverType);
            }

            // this is for edit
            coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());

            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                if (coverType.Id == 0)
                {
                    _unitOfWork.CoverType.Add(coverType);

                }
                else
                {
                    _unitOfWork.CoverType.Update(coverType);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.CoverType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.CoverType.Get(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "خطا در حذف دسته";
                return Json(new { success = false, message = "خطای حذف" });
            }

            _unitOfWork.CoverType.Remove(objFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "دسته با موفقیت حذف شد";
            return Json(new { success = true, message = "حذف با موفقیت انجام شد" });

        }

        /*
           [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.CoverType.GetAllAsync();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.CoverType.GetAsync(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Error deleting CoverType";
                return Json(new { success = false, message = "Error while deleting" });
            }

            await _unitOfWork.CoverType.RemoveAsync(objFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "CoverType successfully deleted";
            return Json(new { success = true, message = "Delete Successful" });

        }
        */
        #endregion

    }
}
