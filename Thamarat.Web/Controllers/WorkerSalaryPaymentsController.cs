using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;

namespace Thamarat.Web.Controllers
{
    public class WorkerSalaryPaymentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkerSalaryPaymentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var salaries = await _unitOfWork.WorkerSalaryPayments.FindAsync(p => true, includeProperties: "Worker,CashAccount");
            return View(salaries);
        }
        [HttpGet]
        public async Task<IActionResult> CalculateSalary(int workerId, DateTime toDate)
        {
            var worker = await _unitOfWork.Workers.GetByIdAsync(workerId);
            if (worker == null)
                return NotFound();

            var cycle = (await _unitOfWork.WorkerCycles.FindAsync(c => c.WorkerId == workerId)).FirstOrDefault();
            if (cycle == null)
                return BadRequest("لا توجد دورة عمل حالية.");

            var days = (toDate - cycle.StartDate).Days + 1;
            var totalSalary = days * worker.DailyWage;

            var advances = await _unitOfWork.WorkerAdvances.FindAsync(
                a => a.WorkerId == workerId && a.Date >= cycle.StartDate && a.Date <= toDate);
            var totalAdvance = advances.Sum(a => a.Amount);

            var netPay = totalSalary - totalAdvance;
            if (netPay < 0) netPay = 0;

            return Json(new { netPay, days, totalSalary, totalAdvance });
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Workers = new SelectList(await _unitOfWork.Workers.GetAllAsync(), "Id", "Name");
            ViewBag.CashAccounts = new SelectList(await _unitOfWork.CashAccounts.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkerSalaryPayment payment)
        {
            var worker = await _unitOfWork.Workers.GetByIdAsync(payment.WorkerId);
            if (worker == null) return NotFound();

            var cycle = (await _unitOfWork.WorkerCycles.FindAsync(c => c.WorkerId == worker.Id)).FirstOrDefault();
            if (cycle == null)
            {
                ModelState.AddModelError("", "لم يتم بدء دورة عمل لهذا العامل.");
                return View(payment);
            }

            var days = (payment.ToDate - cycle.StartDate).Days + 1;
            var totalSalary = days * worker.DailyWage;

            var advances = await _unitOfWork.WorkerAdvances.FindAsync(
                a => a.WorkerId == worker.Id && a.Date >= cycle.StartDate && a.Date <= payment.ToDate);
            var totalAdvance = advances.Sum(a => a.Amount);

            payment.FromDate = cycle.StartDate;
            payment.DaysWorked = days;
            payment.DailyWage = worker.DailyWage;
            payment.TotalSalary = totalSalary;
            payment.TotalAdvance = totalAdvance;
            payment.NetPay = totalSalary - totalAdvance;
            payment.PaymentDate = DateTime.Now;

            await _unitOfWork.WorkerSalaryPayments.AddAsync(payment);
            // خصم من الحساب
            var transaction = new CashTransaction
            {
                Date = DateTime.Now,
                Amount = payment.PaidAmount,
                Type = "Out",
                Description = $"دفع راتب لـ {worker.Name} من {payment.FromDate:yyyy-MM-dd} إلى {payment.ToDate:yyyy-MM-dd}",
                CashAccountId = payment.CashAccountId
            };
            await _unitOfWork.CashTransactions.AddAsync(transaction);

            // تسجيـل كمصروف
            var expense = new Expense
            {
                Description = $"راتب العامل {worker.Name} عن الفترة {payment.FromDate:yyyy-MM-dd} - {payment.ToDate:yyyy-MM-dd}",
                Amount = payment.PaidAmount,
                Date = DateTime.Now,
                CashAccountId = payment.CashAccountId
            };
            await _unitOfWork.Expenses.AddAsync(expense);


            // Remove old cycle (reset)
            var oldCycle = await _unitOfWork.WorkerCycles.GetByIdAsync(cycle.Id);
            if (oldCycle != null) _unitOfWork.WorkerCycles.Remove(oldCycle);

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
