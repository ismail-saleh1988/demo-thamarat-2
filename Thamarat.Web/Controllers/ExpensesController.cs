using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;

namespace Thamarat.Web.Controllers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class ExpensesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpensesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _unitOfWork.Expenses.FindAsync(e => true, includeProperties: "CashAccount");
            ViewBag.TotalExpenses = expenses.Sum(e => e.Amount);
            return View(expenses);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Expenses.AddAsync(expense);
                var affected = await _unitOfWork.SaveAsync();

                if (affected == 0)
                {
                    throw new Exception("لم يتم حفظ أي صف!");
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            return View(expense);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
            if (expense == null) return NotFound();

            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            if (id != expense.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.Expenses.Update(expense);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            return View(expense);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
            if (expense == null) return NotFound();

            _unitOfWork.Expenses.Remove(expense);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
