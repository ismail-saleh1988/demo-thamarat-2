using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;

namespace Thamarat.Web.Controllers
{
    public class WorkerCyclesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkerCyclesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var cycles = await _unitOfWork.WorkerCycles.FindAsync(c => true, includeProperties: "Worker");
            return View(cycles);
        }

        public async Task<IActionResult> Create()
        {
            var workers = await _unitOfWork.Workers.GetAllAsync();
            var currentCycles = await _unitOfWork.WorkerCycles.GetAllAsync();
            var workerIdsWithCycle = currentCycles.Select(c => c.WorkerId).ToList();
            var availableWorkers = workers.Where(w => !workerIdsWithCycle.Contains(w.Id)).ToList();

            ViewBag.Workers = new SelectList(availableWorkers, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkerCycle cycle)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.WorkerCycles.AddAsync(cycle);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Workers = new SelectList(await _unitOfWork.Workers.GetAllAsync(), "Id", "Name");
            return View(cycle);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var cycle = await _unitOfWork.WorkerCycles.GetByIdAsync(id);
            if (cycle == null) return NotFound();

            ViewBag.Workers = new SelectList(await _unitOfWork.Workers.GetAllAsync(), "Id", "Name", cycle.WorkerId);
            return View(cycle);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, WorkerCycle cycle)
        {
            if (id != cycle.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.WorkerCycles.Update(cycle);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Workers = new SelectList(await _unitOfWork.Workers.GetAllAsync(), "Id", "Name", cycle.WorkerId);
            return View(cycle);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cycle = await _unitOfWork.WorkerCycles.GetByIdAsync(id);
            if (cycle == null) return NotFound();

            _unitOfWork.WorkerCycles.Remove(cycle);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
