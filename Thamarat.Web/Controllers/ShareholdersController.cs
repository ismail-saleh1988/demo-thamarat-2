using Microsoft.AspNetCore.Mvc;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;

namespace Thamarat.Web.Controllers
{
    public class ShareholdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShareholdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var shareholders = (await _unitOfWork.Shareholders
                .FindAsync(s => true, includeProperties: "Revenues")).ToList();

            // مجموع كل إيرادات المساهمين
            var total = shareholders.Sum(s => s.Revenues?.Sum(r => r.Amount) ?? 0);

            // نحسب النسبة لكل مساهم
            foreach (var s in shareholders)
            {
                var personalTotal = s.Revenues?.Sum(r => r.Amount) ?? 0;
                s.Percentage = total > 0 ? Math.Round((double)(personalTotal / total * 100), 2) : 0;
            }

            return View(shareholders);
        }


        //public async Task<IActionResult> Index()
        //{
        //    var shareholders = await _unitOfWork.Shareholders.GetAllAsync();
        //    return View(shareholders);
        //}

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Shareholder shareholder)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Shareholders.AddAsync(shareholder);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shareholder);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var shareholder = await _unitOfWork.Shareholders.GetByIdAsync(id);
            if (shareholder == null) return NotFound();
            return View(shareholder);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Shareholder shareholder)
        {
            if (id != shareholder.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.Shareholders.Update(shareholder);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shareholder);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var shareholder = await _unitOfWork.Shareholders.GetByIdAsync(id);
            if (shareholder == null) return NotFound();

            _unitOfWork.Shareholders.Remove(shareholder);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
