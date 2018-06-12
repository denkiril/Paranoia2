/*Here is a sample transaction log of the company:
Feb SLR 4 M
Feb ENT 800 K
Mar SLR 4000 K
Mar ENT 800 K
Apr SLR 4010 K
Apr ENT 810 K

There are four columns:
1. Month of the transaction
2. Reason of the expense (SLR for "salary", ENT for "entertainment", OTR for "other")
3. Amount
4. M, K, or B (M for million, K for thousands, B for billion)

In the example above, April expenses show an inconsistency and should be reported.

Another example:
Jul SLR 4 M
Jul ENR 800 K
Jul OTR 1200 K
Aug SLR 4000 K
Aug ENR 800 K
Aug OTR 1190 K
Sep SLR 4000 K
Sep ENR 800 K
Sep OTR 1190 K

Here, July expenses show an inconsistency and should be reported..

As the computer investigator, write a program, which reads the transaction logs, detects inconsistent expenses 
and prints the exact month containing the "unusual" activities.
The number of lines in the transaction log, as well as the reasons, are not fixed and can contain other values.*/

//You can play with numbers in entries[] in Main()

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paranoia
{
    class Program
    {
        public struct TransactionLogEntry
        {
            public string month;
            public string reason;
            public string amount;
            public string factor;

            public TransactionLogEntry(string entry_str)
            {
                string[] values = {"no", "no", "0", "0"};
                string[] words = entry_str.Split(' ');
                for (int i = 0; i < values.Length && i < words.Length; i++)
                    values[i] = words[i];
                month = values[0];
                reason = values[1];
                amount = values[2];
                factor = values[3];
            }
        } 

        public class ExpensesLog
        {
            struct ExpenseEntry
            {
                public string month;
                public string reason;
                public int amount;
            }

            List<ExpenseEntry> expenses = new List<ExpenseEntry>();

            public void AddEntry(TransactionLogEntry tl_entry)
            {
                int factor = 1;
                switch (tl_entry.factor)
                {
                    case "K": factor = 1000; break;
                    case "M": factor = 1000000; break;
                    case "B": factor = 1000000000; break;
                }

                ExpenseEntry entry = new ExpenseEntry
                {
                    month = tl_entry.month,
                    reason = tl_entry.reason,
                    amount = Convert.ToInt32(tl_entry.amount) * factor
                };

                expenses.Add(entry);
            }

            public void PrintExpensesList()
            {
                foreach (var item in expenses)
                    Console.WriteLine("{0} {1} {2}", item.month, item.reason, item.amount);
            }

            public void PrintUnusualMonth()
            {
                bool consistent = true;
                List<string> reasons = new List<string>();
                foreach (var entry in expenses)
                    if (!reasons.Contains(entry.reason))
                        reasons.Add(entry.reason);

                SortedList<string, int> ReasonAmounts = new SortedList<string, int>();
                foreach (var reason in reasons)
                {
                    ReasonAmounts.Clear();
                    foreach (var entry in expenses)
                        if (entry.reason == reason) ReasonAmounts.Add(entry.month, entry.amount);
                    double val_average = ReasonAmounts.Values.Average();
                    foreach (var r_amount in ReasonAmounts)
                        if (r_amount.Value > val_average)
                        {
                            Console.WriteLine("Unusual month is {0} with {1} {2}", r_amount.Key, reason, r_amount.Value);
                            consistent = false;
                        }
                }

                if (consistent) Console.WriteLine("All expenses are consistent.");
            }
        }

        static void Main(string[] args)
        {
            //transaction log
            string[] entries = {
                "Jul SLR 4 M",
                "Jul ENR 800 K",
                "Jul OTR 1200 K",
                "Aug SLR 4000 K",
                "Aug ENR 800 K",
                "Aug OTR 1190 K",
                "Sep SLR 4000 K",
                "Sep ENR 800 K",
                "Sep OTR 1190 K"
                };

            ExpensesLog log1 = new ExpensesLog();
            foreach (var entry_str in entries)
                log1.AddEntry(new TransactionLogEntry(entry_str));

            log1.PrintExpensesList();
            Console.WriteLine();
            log1.PrintUnusualMonth();

            Console.ReadKey();
        }
    }
}