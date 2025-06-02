using Microsoft.AspNetCore.Mvc;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;

namespace Thamarat.Web.Controllers
{
    public class WorkersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var workers = await _unitOfWork.Workers.GetAllAsync();
            return View(workers);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Worker worker)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Workers.AddAsync(worker);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(worker);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var worker = await _unitOfWork.Workers.GetByIdAsync(id);
            if (worker == null) return NotFound();
            return View(worker);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Worker worker)
        {
            if (id != worker.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _unitOfWork.Workers.Update(worker);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(worker);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var worker = await _unitOfWork.Workers.GetByIdAsync(id);
            if (worker == null) return NotFound();

            _unitOfWork.Workers.Remove(worker);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
