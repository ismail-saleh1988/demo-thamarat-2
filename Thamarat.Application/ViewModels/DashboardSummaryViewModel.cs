namespace Thamarat.Web.ViewModels
{
    // Application/ViewModels/DashboardSummaryViewModel.cs
    public class DashboardSummaryViewModel
    {
        public decimal TotalRevenues { get; set; }
        public decimal TotalExpenses { get; set; }
        public int ShareholderCount { get; set; }
        public decimal TotalDistributedProfits { get; set; }
        public decimal TotalCashInAllAccounts { get; set; }
    }

}
