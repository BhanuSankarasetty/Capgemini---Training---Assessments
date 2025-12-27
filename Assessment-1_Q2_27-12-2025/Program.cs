using System;

namespace Question2
{
    public class PatientBill
    {
        // Properties
        #region Properties
        public required string BillId { get; set; }
        public required string PatientName { get; set; }
        public bool HasInsurance { get; set; }
        public decimal ConsultationFee { get; set; }
        public decimal LabCharges { get; set; }
        public decimal MedicineCharges { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalPayable { get; set; }

        // Static storage
        public static PatientBill LastBill { get; set; }
        public static bool HasLastBill { get; set; } = false;
        #endregion

        public PatientBill()
        {
            // Default Constructor
        }

        // Create / Register Bill
        public static void CreateBill()
        {
            Console.Write("Enter Bill Id: ");
            string billId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(billId))
            {
                Console.WriteLine("Bill Id cannot be empty.");
                return;
            }

            Console.Write("Enter Patient Name: ");
            string patientName = Console.ReadLine();

            Console.Write("Is the patient insured? (Y/N): ");
            bool hasInsurance = Console.ReadLine().Trim().ToUpper() == "Y";

            Console.Write("Enter Consultation Fee: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal cf) || cf <= 0)
            {
                Console.WriteLine("Consultation Fee must be greater than 0.");
                return;
            }

            Console.Write("Enter Lab Charges: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal lc) || lc < 0)
            {
                Console.WriteLine("Lab Charges must be >= 0.");
                return;
            }

            Console.Write("Enter Medicine Charges: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal mc) || mc < 0)
            {
                Console.WriteLine("Medicine Charges must be >= 0.");
                return;
            }

            decimal gross = cf + lc + mc;
            decimal discount = hasInsurance ? gross * 0.10m : 0;
            decimal finalPayable = gross - discount;

            PatientBill bill = new PatientBill
            {
                BillId = billId,
                PatientName = patientName,
                HasInsurance = hasInsurance,
                ConsultationFee = cf,
                LabCharges = lc,
                MedicineCharges = mc,
                GrossAmount = gross,
                DiscountAmount = discount,
                FinalPayable = finalPayable
            };

            LastBill = bill;
            HasLastBill = true;

            Console.WriteLine("\nBill created successfully.");
            Console.WriteLine($"Gross Amount: {bill.GrossAmount:F2}");
            Console.WriteLine($"Discount Amount: {bill.DiscountAmount:F2}");
            Console.WriteLine($"Final Payable: {bill.FinalPayable:F2}");
            Console.WriteLine("------------------------------------------------------------");
        }

        // View Bill
        public static void ViewLastBill()
        {
            if (!HasLastBill)
            {
                Console.WriteLine("No bill available. Please create a new bill first.");
                return;
            }

            var b = LastBill;
            Console.WriteLine("\n----------- Last Bill -----------");
            Console.WriteLine($"BillId: {b.BillId}");
            Console.WriteLine($"Patient: {b.PatientName}");
            Console.WriteLine($"Insured: {(b.HasInsurance ? "Yes" : "No")}");
            Console.WriteLine($"Consultation Fee: {b.ConsultationFee:F2}");
            Console.WriteLine($"Lab Charges: {b.LabCharges:F2}");
            Console.WriteLine($"Medicine Charges: {b.MedicineCharges:F2}");
            Console.WriteLine($"Gross Amount: {b.GrossAmount:F2}");
            Console.WriteLine($"Discount Amount: {b.DiscountAmount:F2}");
            Console.WriteLine($"Final Payable: {b.FinalPayable:F2}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("------------------------------------------------------------");
        }

        // Clear Bill
        public static void ClearBill()
        {
            LastBill = null;
            HasLastBill = false;
            Console.WriteLine("Last bill cleared.");
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n================== MediSure Clinic Billing ==================");
                Console.WriteLine("1. Create New Bill (Enter Patient Details)");
                Console.WriteLine("2. View Last Bill");
                Console.WriteLine("3. Clear Last Bill");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        PatientBill.CreateBill();
                        break;
                    case "2":
                        PatientBill.ViewLastBill();
                        break;
                    case "3":
                        PatientBill.ClearBill();
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
