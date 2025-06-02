using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;

namespace Thamarat.Web.Controllers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class RevenuesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevenuesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var revenues = await _unitOfWork.Revenues.FindAsync(r => true, includeProperties: "CashAccount,Shareholder");
            ViewBag.TotalRevenue = revenues.Sum(r => r.Amount);
            return View(revenues);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            ViewBag.Shareholders = new SelectList(await _unitOfWork.Shareholders.GetAllAsync(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Revenue revenue)
        {
            if (ModelState.IsValid)
            {
                // 1. أضف الإيراد
                await _unitOfWork.Revenues.AddAsync(revenue);

                // 2. أضف حركة نقدية
                var transaction = new CashTransaction
                {
                    Date = revenue.Date,
                    Amount = revenue.Amount,
                    Type = "In",
                    Description = $"إيراد: {revenue.Source}",
                    CashAccountId = revenue.CashAccountId.Value
                };
                await _unitOfWork.CashTransactions.AddAsync(transaction);

                // 3. حفظ الكل
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            ViewBag.Shareholders = new SelectList(await _unitOfWork.Shareholders.GetAllAsync(), "Id", "Name");
            return View(revenue);
        }


        //[HttpPost]
        //public async Task<IActionResult> Create(Revenue revenue)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _unitOfWork.Revenues.AddAsync(revenue);
        //        await _unitOfWork.SaveAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
        //    ViewBag.Shareholders = new SelectList(await _unitOfWork.Shareholders.GetAllAsync(), "Id", "Name");
        //    return View(revenue);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Create(Revenue revenue)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        //        {
        //            Console.WriteLine(error.ErrorMessage);
        //        }

        //        // إعادة تحميل القوائم حتى لا تكون فارغة في حالة فشل الإدخال
        //        ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
        //        ViewBag.Shareholders = new SelectList(await _unitOfWork.Shareholders.GetAllAsync(), "Id", "Name");

        //        return View(revenue);
        //    }

        //    await _unitOfWork.Revenues.AddAsync(revenue);
        //    await _unitOfWork.SaveAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        public async Task<IActionResult> Edit(int id)
        {
            var revenue = await _unitOfWork.Revenues.GetByIdAsync(id);
            if (revenue == null) return NotFound();

            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            ViewBag.Shareholders = new SelectList(await _unitOfWork.Shareholders.GetAllAsync(), "Id", "Name");
            return View(revenue);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Revenue revenue)
        {
            if (id != revenue.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.Revenues.Update(revenue);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            ViewBag.Shareholders = new SelectList(await _unitOfWork.Shareholders.GetAllAsync(), "Id", "Name");
            return View(revenue);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var revenue = await _unitOfWork.Revenues.GetByIdAsync(id);
            if (revenue == null) return NotFound();

            _unitOfWork.Revenues.Remove(revenue);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
