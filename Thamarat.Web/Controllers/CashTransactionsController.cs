using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;


namespace Thamarat.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CashTransactionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CashTransactionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _unitOfWork.CashTransactions.FindAsync(t => true, includeProperties: "CashAccount");

            var grouped = transactions
                .GroupBy(t => t.CashAccount?.Name)
                .Select(g => new
                {
                    CashAccount = g.Key,
                    In = g.Where(t => t.Type == "In").Sum(t => t.Amount),
                    Out = g.Where(t => t.Type == "Out").Sum(t => t.Amount),
                    Net = g.Where(t => t.Type == "In").Sum(t => t.Amount) - g.Where(t => t.Type == "Out").Sum(t => t.Amount)
                });

            ViewBag.Summary = grouped;

            return View(transactions.OrderByDescending(t => t.Date));
        }
    }
}
