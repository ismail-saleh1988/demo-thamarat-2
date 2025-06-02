using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;

namespace Thamarat.Web.Controllers
{
    public class WorkerAdvancesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkerAdvancesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var advances = await _unitOfWork.WorkerAdvances
                .FindAsync(a => true, includeProperties: "Worker,CashAccount");

            ViewBag.TotalAdvance = advances.Sum(a => a.Amount); // ✅ لحساب الإجمالي
            return View(advances.OrderByDescending(a => a.Date));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Workers = new SelectList(await _unitOfWork.Workers.GetAllAsync(), "Id", "Name");
            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkerAdvance advance)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.WorkerAdvances.AddAsync(advance);

                var worker = await _unitOfWork.Workers.GetByIdAsync(advance.WorkerId);

                var transaction = new CashTransaction
                {
                    Date = advance.Date,
                    Amount = advance.Amount,
                    Type = "Out",
                    Description = $"سلفة للعامل {worker.Name}",
                    CashAccountId = advance.CashAccountId
                };
                await _unitOfWork.CashTransactions.AddAsync(transaction);

                var expense = new Expense
                {
                    Description = $"سلفة للعامل {worker.Name}",
                    Amount = advance.Amount,
                    Date = advance.Date,
                    CashAccountId = advance.CashAccountId
                };
                await _unitOfWork.Expenses.AddAsync(expense);

                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Workers = new SelectList(await _unitOfWork.Workers.GetAllAsync(), "Id", "Name");
            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            return View(advance);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var advance = await _unitOfWork.WorkerAdvances.GetByIdAsync(id);
            if (advance == null) return NotFound();

            ViewBag.Workers = new SelectList(await _unitOfWork.Workers.GetAllAsync(), "Id", "Name", advance.WorkerId);
            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name", advance.CashAccountId);
            return View(advance);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, WorkerAdvance advance)
        {
            if (id != advance.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.WorkerAdvances.Update(advance);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Workers = new SelectList(await _unitOfWork.Workers.GetAllAsync(), "Id", "Name", advance.WorkerId);
            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name", advance.CashAccountId);
            return View(advance);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var advance = await _unitOfWork.WorkerAdvances.GetByIdAsync(id);
            if (advance == null) return NotFound();

            _unitOfWork.WorkerAdvances.Remove(advance);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
