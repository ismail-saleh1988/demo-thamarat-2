using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Thamarat.Web.Models;

namespace Thamarat.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var revenues = await _unitOfWork.Revenues.GetAllAsync();
            var expenses = await _unitOfWork.Expenses.GetAllAsync();
            var workers = await _unitOfWork.Workers.GetAllAsync();

            var totalRevenue = revenues.Sum(r => r.Amount);
            var totalExpense = expenses.Sum(e => e.Amount);
            var profit = Math.Max(totalRevenue - totalExpense, 0);

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalExpense = totalExpense;
            ViewBag.TotalWorkers = workers.Count();
            ViewBag.Profit = profit;

            return View();
        }
    }

}
