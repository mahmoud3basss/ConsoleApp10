using System.Security.Principal;

namespace ConsoleApp10
{
    public class Account
    {
        public string Name { get; set; }
        public double Balance { get; set; }

        public Account(string Name = "Unnamed Account", double Balance = 0.0)
        {
            this.Name = Name;
            this.Balance = Balance;
        }

        public virtual bool Deposit(double amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                return true;
            }
            return false;
        }

        public virtual bool Withdraw(double amount)
        {
            if (Balance - amount >= 0)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Name}: ${Balance:F2}";
        }

        // Overload + operator to sum two account balances
        public static Account operator +(Account acc1, Account acc2)
        {
            return new Account("Combined Account", acc1.Balance + acc2.Balance);
        }
    }

    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; }

        public SavingsAccount(string Name = "Unnamed Savings Account", double Balance = 0.0, double InterestRate = 0.0)
            : base(Name, Balance)
        {
            this.InterestRate = InterestRate;
        }

        public override bool Deposit(double amount)
        {
            if (base.Deposit(amount))
            {
                // Apply interest on the deposit
                Balance += Balance * InterestRate / 100;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Name} (Savings): ${Balance:F2} at {InterestRate}% interest";
        }
    }

    public class CheckingAccount : Account
    {
        private const double FeePerWithdrawal = 1.50;

        public CheckingAccount(string Name = "Unnamed Checking Account", double Balance = 0.0)
            : base(Name, Balance)
        {
        }

        public override bool Withdraw(double amount)
        {
            double totalAmount = amount + FeePerWithdrawal;
            if (Balance - totalAmount >= 0)
            {
                Balance -= totalAmount;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Name} (Checking): ${Balance:F2}";
        }
    }

    public class TrustAccount : SavingsAccount
    {
        private const int MaxWithdrawalsPerYear = 3;
        private int withdrawalsThisYear = 0;

        public TrustAccount(string Name = "Unnamed Trust Account", double Balance = 0.0, double InterestRate = 0.0)
            : base(Name, Balance, InterestRate)
        {
        }

        public override bool Deposit(double amount)
        {
            if (base.Deposit(amount))
            {
                if (amount >= 5000)
                {
                    Balance += 50; // Bonus for deposits of $5000 or more
                }
                return true;
            }
            return false;
        }

        public override bool Withdraw(double amount)
        {
            if (withdrawalsThisYear >= MaxWithdrawalsPerYear)
            {
                Console.WriteLine($"Cannot withdraw: Max withdrawals reached for the year.");
                return false;
            }

            if (amount <= Balance * 0.20)
            {
                withdrawalsThisYear++;
                return base.Withdraw(amount);
            }
            else
            {
                Console.WriteLine($"Cannot withdraw: Withdrawal amount exceeds 20% of the balance.");
                return false;
            }
        }

        public override string ToString()
        {
            return $"{Name} (Trust): ${Balance:F2} with {InterestRate}% interest";
        }
    }

    public static class AccountUtil
    {
        // Utility helper functions for Account class
        public static void Display(List<Account> accounts)
        {
            Console.WriteLine("\n=== Accounts ==========================================");
            foreach (var acc in accounts)
            {
                Console.WriteLine(acc);
            }
        }

        public static void Deposit(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Depositing to Accounts =================================");
            foreach (var acc in accounts)
            {
                if (acc.Deposit(amount))
                    Console.WriteLine($"Deposited {amount} to {acc}");
                else
                    Console.WriteLine($"Failed Deposit of {amount} to {acc}");
            }
        }

        public static void Withdraw(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Withdrawing from Accounts ==============================");
            foreach (var acc in accounts)
            {
                if (acc.Withdraw(amount))
                    Console.WriteLine($"Withdrew {amount} from {acc}");
                else
                    Console.WriteLine($"Failed Withdrawal of {amount} from {acc}");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Accounts
            var accounts = new List<Account>();
            accounts.Add(new Account());
            accounts.Add(new Account("Larry"));
            accounts.Add(new Account("Moe", 2000));
            accounts.Add(new Account("Curly", 5000));

            AccountUtil.Display(accounts);
            AccountUtil.Deposit(accounts, 1000);
            AccountUtil.Withdraw(accounts, 2000);

            // Savings
            var savAccounts = new List<SavingsAccount>();
            savAccounts.Add(new SavingsAccount());
            savAccounts.Add(new SavingsAccount("Superman"));
            savAccounts.Add(new SavingsAccount("Batman", 2000));
            savAccounts.Add(new SavingsAccount("Wonderwoman", 5000, 5.0));

            AccountUtil.Display(accounts);
            AccountUtil.Deposit(accounts, 1000);
            AccountUtil.Withdraw(accounts , 2000);

            // Checking
            var checAccounts = new List<CheckingAccount>();
            checAccounts.Add(new CheckingAccount());
            checAccounts.Add(new CheckingAccount("Larry2"));
            checAccounts.Add(new CheckingAccount("Moe2", 2000));
            checAccounts.Add(new CheckingAccount("Curly2", 5000));

            AccountUtil.Display(accounts);
            AccountUtil.Deposit(accounts, 1000);
            AccountUtil.Withdraw(accounts, 2000);
            AccountUtil.Withdraw(accounts, 2000);

            // Trust
            var trustAccounts = new List<TrustAccount>();
            trustAccounts.Add(new TrustAccount());
            trustAccounts.Add(new TrustAccount("Superman2"));
            trustAccounts.Add(new TrustAccount("Batman2", 2000));
            trustAccounts.Add(new TrustAccount("Wonderwoman2", 5000, 5.0));

            AccountUtil.Display(accounts);
            AccountUtil.Deposit(accounts, 1000);
            AccountUtil.Deposit(accounts, 6000);
            AccountUtil.Withdraw(accounts, 2000);
            AccountUtil.Withdraw(accounts, 3000);
            AccountUtil.Withdraw(accounts, 500);

            // Combining two accounts using overloaded + operator
            Account combinedAccount = accounts[0] + accounts[1];
            Console.WriteLine($"Combined Account: {combinedAccount}");


        }
    }
}
