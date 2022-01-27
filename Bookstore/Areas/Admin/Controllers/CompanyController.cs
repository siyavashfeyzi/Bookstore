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
    public class CompanyController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var company = new Company();

            if (id == null)
            {
                //this is for create 
                return View(company);
            }

            // this is for edit
            company = _unitOfWork.Company.Get(id.GetValueOrDefault());

            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);

                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Company.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Company.Get(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "خطا در حذف شرکت";
                return Json(new { success = false, message = "خطای حذف" });
            }

            _unitOfWork.Company.Remove(objFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "شرکت با موفقیت حذف شد";
            return Json(new { success = true, message = "حذف با موفقیت انجام شد" });

        }

        /*
           [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Company.GetAllAsync();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.Company.GetAsync(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Error deleting Company";
                return Json(new { success = false, message = "Error while deleting" });
            }

            await _unitOfWork.Company.RemoveAsync(objFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Company successfully deleted";
            return Json(new { success = true, message = "Delete Successful" });

        }
        */
        #endregion

    }
}
