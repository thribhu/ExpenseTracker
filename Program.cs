using System;
using System.Reflection;
using System.Threading;
namespace ExpenseTracker
{
    public  class Option
    {
        public String Name { get; set; }
        public Action OnSelect { get; set; }
        
        public Option(String name, Action onSelect)
        {
            this.Name = name;
            this.OnSelect = onSelect;
        }

    }
    public class Expense
    {
        public Double Amount { get; set; }
        public string Type { get; set; }
        public bool IsPaid { get; set; }

        public Expense(double amount, string type, bool isPaid)
        {
            Amount = amount;
            Type = type;
            IsPaid = isPaid;
        }
    }
    public class Expenses
    {

        // instance member
        public List<Expense> _Expenses = new List<Expense>();
        public List<Option> _Options = new List<Option>();

        public Expense SelectedExpense { get; set; }
        public Option SelectedOption { get; set; }
        
        public Expenses()
        {
            if(_Expenses.Count > 0)
            {
                this.SelectedExpense = _Expenses[0];
            }
            if(_Options.Count > 0)
            {
                this.SelectedOption = _Options[0];
            }
        }
        
        public void AddExpense()
        {
            Console.Clear();
            Console.WriteLine("\t###INFO: An Expense should have type, amount in CAD. Its marked unpaid as default.###");
            Console.Write("\t Type:  ");
            var ExpenseType = Console.ReadLine();
            Console.Write("\t Amount: $");
            var _Amount = Convert.ToDouble(Console.ReadLine());
            const bool isPaid = false;

            if(ExpenseType != null)
            {
            Expense ExpenseToAdd = new Expense(_Amount, ExpenseType, isPaid);
            Console.WriteLine($"\t {ExpenseType} Expense added. Validating and saving to Expenses List");
            this._Expenses.Add(ExpenseToAdd);
            }
        }

        public void UpdateExpense(Expense expense)
        {
            Console.Clear();
            bool setPaid = !expense.IsPaid;
            Console.WriteLine($"\t###Note: Setting ${expense.Type} expense of CAD ${expense.Amount} to {setPaid.ToString()}.###") ;
            if(this._Expenses.Contains(expense))
            {
                var ExpenseIndex = this._Expenses.IndexOf(expense);
                this._Expenses[ExpenseIndex].IsPaid = setPaid;
                Console.WriteLine("Expense Updated");
                Console.Clear();
            }
        }

        public void RemoveExpense(Expense expense)
        {
            Console.Clear();
            if (this._Expenses.Contains(expense))
            {
                var expenseIndex = this._Expenses.IndexOf(expense);
                this._Expenses.RemoveAt(expenseIndex);
                //this._Expenses.Remove(this._Expenses.Single(e => e.Amount == expense.Amount && e.Type == expense.Type));
                Console.WriteLine($"\t###INFO: Deleting {expense.Type} expense of CAD ${expense.Amount}.###");
            }
        }

        public void AddOptionsToList(Option option)
        {
               if(option == null)
            {
                Console.WriteLine("Error: Invalid value provided as argument. Expecting an Option as argument");
                return;
            }
               this._Options.Add(option);
        }

    }

    public class Menu
    {
        ConsoleKeyInfo KeyInfo;
        public int ActiveOptionIndex;
        public int ActiveExpenseIndex;

        public Menu(int activeOptionIndex=0, int activeExpenseIndex=0) {
            this.ActiveOptionIndex = activeOptionIndex;
            this.ActiveExpenseIndex = activeExpenseIndex;
        }

        public void DisplayOptionMenu(List<Option> options, int selectedOptionIndex)
        {
            int index = selectedOptionIndex;
            WriteOptionsMenu(options, options[index]);
            do
            {
                KeyInfo = Console.ReadKey();
                if(KeyInfo.Key == ConsoleKey.DownArrow)
                {
                   if(index + 1 < options.Count)
                    {
                        ++index;
                        WriteOptionsMenu(options, options[index]);
                    }
                }
                if (KeyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteOptionsMenu(options, options[index]);
                    }
                }
                if (KeyInfo.Key == ConsoleKey.Enter)
                {
                    options[index].OnSelect.Invoke();
                    index = 0;
                }
            }     
            while (KeyInfo.Key != ConsoleKey.Escape);
            Console.ReadKey();
        }
        static void WriteOptionsMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();

            Console.WriteLine("\n________________________________________________________________________________\n");
            Console.WriteLine("Expense Tracker Application");
            Console.WriteLine("\n________________________________________________________________________________\n");
            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write("  ");
                }

                Console.WriteLine(option.Name);
            }
        }
        public void ListAllExpenses(List<ExpenseOption> expenses) {
            int index = 0;
            WriteExpensesAsTable(expenses, expenses[index].Expense);
            do
            {
                KeyInfo = Console.ReadKey();
                if(KeyInfo.Key == ConsoleKey.DownArrow)
                {
                   if(index + 1 < expenses.Count)
                    {
                        ++index;
                        WriteExpensesAsTable(expenses, expenses[index].Expense);
                    }
                }
                if (KeyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteExpensesAsTable(expenses, expenses[index].Expense);
                    }
                }
                if (KeyInfo.Key == ConsoleKey.Enter)
                {
                    expenses[index].OnSelect.Invoke();
                }
            }     
            while (KeyInfo.Key != ConsoleKey.Escape);
            Console.ReadKey();
                
        }
        static void WriteExpensesAsTable(List<ExpenseOption> options, Expense selectedExpense)
        {
            Console.Clear();
                if(options.Count > 0)
                {
                    Console.WriteLine("\n________________________________________________________________________________\n");
                    Console.WriteLine("\t EXPENSES \t");
                    Console.WriteLine("\t###HINT: `O` to mark an expense as paid, `X` to remove an expense");
                    Console.WriteLine("\n________________________________________________________________________________\n");
                    Console.WriteLine("\t\t Action \t\tType \t\tAmount \t\tIsPaid");
                    foreach(ExpenseOption iterator in options)
                    {
                    var expense = iterator.Expense;
                        if(selectedExpense == expense)
                        {
                            Console.Write($"\t\t> {(expense.IsPaid ? "X" : "O")}");
                        }
                        else
                        {
                            Console.Write("\t\t  ");
                        }
                        Console.WriteLine($"\t\t\t{expense.Type} \t\tCAD${expense.Amount} \t{expense.IsPaid.ToString()}");
                    }
                    }
            else {
                Console.WriteLine("###Info: No expenses to show. Add an expense and try again");
            }
            
        }
    }
    public class ExpenseOption
    {
        public Expense Expense { get; set; }
        public Action OnSelect { get; set; }

        public ExpenseOption(Expense expense, Action onSelect)
        {
            Expense = expense;
            OnSelect = onSelect;
        }
    }
    class program
    {

        // Main Method
        public static void Main()
        {
            List<ExpenseOption> Options = new List<ExpenseOption>();
            var expenses = new Expenses();
            var expenseMenu = new Menu();
            var AddExpense = new Option("Add Expense", () => {
                expenses.AddExpense();
                if(expenses._Expenses.Count > 0)
                {
                   foreach(Expense _expense in expenses._Expenses)
                    {
                        List<Expense> _list = new List<Expense>();
                        foreach (ExpenseOption option in Options)
                        {
                            _list.Add(option.Expense);
                        }
                            if(!_list.Contains(_expense)) {
                            {
                            Options.Add(new ExpenseOption(_expense, () => { 
                                  if(_expense.IsPaid)
                                {
                                    expenses.RemoveExpense(_expense);
                                    Options.Remove(Options.Single(Single => Single.Expense == _expense));
                                    Thread.Sleep(1000);
                                    expenseMenu.DisplayOptionMenu(expenses._Options, 0);
                                }
                                  else
                                {
                                    expenses.UpdateExpense(_expense);
                                    Thread.Sleep(1000);
                                    expenseMenu.DisplayOptionMenu(expenses._Options, 0);
                                }
                                }));

                            }

                        }
                    }
                }
                Thread.Sleep(1000);
                expenseMenu.DisplayOptionMenu(expenses._Options, 0);
            });
            var ListExpense = new Option("List Expenses", () => {
                if(Options.Count < 1)
                {
                    Console.WriteLine("###Error: No Expense to show. Add one and try again.");
                    Thread.Sleep(1000);
                    expenseMenu.DisplayOptionMenu(expenses._Options, 0);
                }
                expenseMenu.ListAllExpenses(Options);
            });

            expenses.AddOptionsToList(AddExpense);
            expenses.AddOptionsToList(ListExpense);

            expenseMenu.DisplayOptionMenu(expenses._Options, 0);
        }
    }
}