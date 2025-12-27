using System;

namespace Question1
{
    class SaleTransaction
    {
        #region Properties
        public string InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal SellingAmount { get; set; }
        public string ProfitOrLossStatus { get; set; }
        public decimal ProfitOrLossAmount { get; set; }
        public decimal ProfitMarginPercent { get; set; }
        // Static storage (ONLY ONE transaction)
        public static SaleTransaction LastTransaction { get; set; }
        public static bool HasLastTransaction { get; set; } = false;
        #endregion

        public SaleTransaction()
        {
            // Default Constructor
        }
        
        // Register / Create Transaction
        public static void CreateTransaction()
        {
            SaleTransaction t = new SaleTransaction();

            Console.Write("Enter Invoice No: ");
            t.InvoiceNo = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(t.InvoiceNo))
            {
                Console.WriteLine("Invoice No cannot be empty.");
                return;
            }

            Console.Write("Enter Customer Name: ");
            t.CustomerName = Console.ReadLine();

            Console.Write("Enter Item Name: ");
            t.ItemName = Console.ReadLine();

            Console.Write("Enter Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                Console.WriteLine("Quantity must be greater than 0.");
                return;
            }
            t.Quantity = qty;

            Console.Write("Enter Purchase Amount (total): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal pa) || pa <= 0)
            {
                Console.WriteLine("Purchase Amount must be greater than 0.");
                return;
            }
            t.PurchaseAmount = pa;

            Console.Write("Enter Selling Amount (total): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal sa) || sa < 0)
            {
                Console.WriteLine("Selling Amount must be >= 0.");
                return;
            }
            t.SellingAmount = sa;

            ComputeProfitLoss(t);

            LastTransaction = t;
            HasLastTransaction = true;

            Console.WriteLine("\nTransaction saved successfully.");
            PrintCalculation(t);
        }

        // View Transaction
        public static void ViewLastTransaction()
        {
            if (!HasLastTransaction)
            {
                Console.WriteLine("No transaction available. Please create a new transaction first.");
                return;
            }

            var t = LastTransaction;
            Console.WriteLine("\n-------------- Last Transaction --------------");
            Console.WriteLine($"InvoiceNo: {t.InvoiceNo}");
            Console.WriteLine($"Customer: {t.CustomerName}");
            Console.WriteLine($"Item: {t.ItemName}");
            Console.WriteLine($"Quantity: {t.Quantity}");
            Console.WriteLine($"Purchase Amount: {t.PurchaseAmount:F2}");
            Console.WriteLine($"Selling Amount: {t.SellingAmount:F2}");
            Console.WriteLine($"Status: {t.ProfitOrLossStatus}");
            Console.WriteLine($"Profit/Loss Amount: {t.ProfitOrLossAmount:F2}");
            Console.WriteLine($"Profit Margin (%): {t.ProfitMarginPercent:F2}");
            Console.WriteLine("--------------------------------------------");
        }

        // Recalculate
        public static void Recalculate()
        {
            if (!HasLastTransaction)
            {
                Console.WriteLine("No transaction available. Please create a new transaction first.");
                return;
            }

            ComputeProfitLoss(LastTransaction);
            PrintCalculation(LastTransaction);
        }

        // Core Calculation Logic
        private static void ComputeProfitLoss(SaleTransaction t)
        {
            if (t.SellingAmount > t.PurchaseAmount)
            {
                t.ProfitOrLossStatus = "PROFIT";
                t.ProfitOrLossAmount = t.SellingAmount - t.PurchaseAmount;
            }
            else if (t.SellingAmount < t.PurchaseAmount)
            {
                t.ProfitOrLossStatus = "LOSS";
                t.ProfitOrLossAmount = t.PurchaseAmount - t.SellingAmount;
            }
            else
            {
                t.ProfitOrLossStatus = "BREAK-EVEN";
                t.ProfitOrLossAmount = 0;
            }

            t.ProfitMarginPercent =
                Math.Round((t.ProfitOrLossAmount / t.PurchaseAmount) * 100, 2);
        }

        private static void PrintCalculation(SaleTransaction t)
        {
            Console.WriteLine($"Status: {t.ProfitOrLossStatus}");
            Console.WriteLine($"Profit/Loss Amount: {t.ProfitOrLossAmount:F2}");
            Console.WriteLine($"Profit Margin (%): {t.ProfitMarginPercent:F2}");
            Console.WriteLine("------------------------------------------------------");
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n================== QuickMart Traders ==================");
                Console.WriteLine("1. Create New Transaction (Enter Purchase & Selling Details)");
                Console.WriteLine("2. View Last Transaction");
                Console.WriteLine("3. Calculate Profit/Loss (Recompute & Print)");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        SaleTransaction.CreateTransaction();
                        break;
                    case "2":
                        SaleTransaction.ViewLastTransaction();
                        break;
                    case "3":
                        SaleTransaction.Recalculate();
                        break;
                    case "4":
                        Console.WriteLine("Thank you. Application closed normally.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
