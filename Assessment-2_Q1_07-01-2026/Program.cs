using System;
using System.Collections.Generic;
namespace  DigitalPettyCashLedger
{
    interface IReportable
    {
        void GetSummary();
    }
    abstract class Transaction
    {
        
        #region Properties 
        public int Id {get; set; }
        public DateTime Date {get; set; }
        public int Amount {get; set; }
        public string Description {get; set; }
        #endregion

            
    }
    class ExpenseTransaction : Transaction, IReportable
    {
        #region Properties 
        public string Category;
        #endregion

        public void GetSummary()
        {
            Console.WriteLine($"This is a unique transaction for Expense of Category {Category}");
        }
    }
    class IncomeTransaction  : Transaction, IReportable
    {
        #region Properties 
        public string Source;
        #endregion

        public void GetSummary()
        {
            Console.WriteLine($"This is a unique transaction for Income of Source {Source}");
        }
    }

    class Ledger<T> where T : Transaction
    {
        private List<T> Transactions;
        public Ledger()
        {
            this.Transactions = new List<T>();
        }
        public void AddEntry(T entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            Transactions.Add(entry);
        }

        public List<T> GetTransactionsByDate(DateTime date)
        {   
            List<T> TransactionsOnSpecificDate = new List<T>();
            foreach (T transaction in Transactions)
            {
                if (transaction.Date.Date == date.Date)
                {
                    TransactionsOnSpecificDate.Add(transaction);
                }   
            }
            return TransactionsOnSpecificDate;
        }
        public int CalculateTotal()
        {
            int totalAmount = 0;
            foreach (T transaction in Transactions)
            {
                totalAmount += transaction.Amount;
            }
            return totalAmount;
        }

        public int CalculateAmountAfterBalancing()
        {
            int totalAmount = 0;
            foreach (T transaction in Transactions)
            {   
                if(transaction is IncomeTransaction)
                totalAmount += transaction.Amount;
                else 
                totalAmount -= transaction.Amount;
            }
            return totalAmount;
        }
        public void getAllTransactions()
        {
            foreach (T transaction in Transactions)
            {
                Console.WriteLine(
                    $"ID: {transaction.Id}, " +
                    $"Date: {transaction.Date}, " +
                    $"Amount: {transaction.Amount}, " +
                    $"Description: {transaction.Description}"
                );

                if (transaction is IReportable reportable)
                {
                    reportable.GetSummary();
                }
            }
        }

    }
    

    class Program
    {
        static void Main(string[] args)
        {
            Ledger<Transaction> ledger = new Ledger<Transaction>();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n----------------------------------------------------------------------");
                Console.WriteLine("----------------------- DigitalPettyCashLedger -----------------------");
                Console.WriteLine("1. Add Expense");
                Console.WriteLine("2. Add Income");
                Console.WriteLine("3. View All Transactions");
                Console.WriteLine("4. View Transactions By Date");
                Console.WriteLine("5. View Total Amount");
                Console.WriteLine("6. View Balanced Amount");
                Console.WriteLine("0. Exit");
                Console.WriteLine("----------------------------------------------------------------------");
                Console.Write("Enter your choice: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter Expense Amount: ");
                        int expAmount = int.Parse(Console.ReadLine());

                        Console.Write("Enter Category: ");
                        string category = Console.ReadLine();

                        Console.Write("Enter Description: ");
                        string expDesc = Console.ReadLine();

                        ledger.AddEntry(new ExpenseTransaction
                        {
                            Id = Guid.NewGuid().GetHashCode(),
                            Amount = expAmount,
                            Category = category,
                            Description = expDesc,
                            Date = DateTime.Now
                        });

                        Console.WriteLine("✅ Expense added successfully.");
                        break;

                    case 2:
                        Console.Write("Enter Income Amount: ");
                        int incAmount = int.Parse(Console.ReadLine());

                        Console.Write("Enter Source: ");
                        string source = Console.ReadLine();

                        Console.Write("Enter Description: ");
                        string incDesc = Console.ReadLine();

                        ledger.AddEntry(new IncomeTransaction
                        {
                            Id = Guid.NewGuid().GetHashCode(),
                            Amount = incAmount,
                            Source = source,
                            Description = incDesc,
                            Date = DateTime.Now
                        });

                        Console.WriteLine("✅ Income added successfully.");
                        break;

                    case 3:
                        Console.WriteLine("\n----------------------- All Transactions ---------------------------");
                        ledger.getAllTransactions();
                        break;

                    case 4:
                        Console.Write("Enter date (yyyy-mm-dd): ");
                        DateTime date = DateTime.Parse(Console.ReadLine());

                        Console.WriteLine("\n------------------- Transactions On Date --------------------");
                        var list = ledger.GetTransactionsByDate(date);
                        foreach (var t in list)
                        {
                            Console.WriteLine($"ID: {t.Id}, Amount: {t.Amount}, Desc: {t.Description}");
                        }
                        break;

                    case 5:
                        Console.WriteLine("\n----------------------- Total Amount ----------------------------");
                        Console.WriteLine("Total Amount (Raw Sum): " + ledger.CalculateTotal());
                        break;

                    case 6:
                        Console.WriteLine("\n----------------------- Balanced Amount ----------------------------");
                        Console.WriteLine("Net Balance: " + ledger.CalculateAmountAfterBalancing());
                        break;

                    case 0:
                        running = false;
                        Console.WriteLine("Exiting Ledger. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

    }
}