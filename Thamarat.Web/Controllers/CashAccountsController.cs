using Microsoft.AspNetCore.Mvc;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;

namespace Thamarat.Web.Controllers
{
    public class CashAccountsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CashAccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _unitOfWork.CashAccounts.GetAllAsync();
            return View(accounts);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CashAccount account)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.CashAccounts.AddAsync(account);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _unitOfWork.CashAccounts.GetByIdAsync(id);
            if (account == null) return NotFound();
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CashAccount account)
        {
            if (id != account.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.CashAccounts.Update(account);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var account = await _unitOfWork.CashAccounts.GetByIdAsync(id);
            if (account == null) return NotFound();

            _unitOfWork.CashAccounts.Remove(account);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
