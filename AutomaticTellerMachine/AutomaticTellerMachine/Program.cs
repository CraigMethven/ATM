using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomaticTellerMachine
{
    class bankAccount
    {
        string name;
        int accountNumber;
        int pin;
        int balance;
        //Insert: Name, account number, pin and balance;
        public bankAccount(string tempName, int tempNum, int tempPin, int tempBalance)
        {
            name = tempName;
            accountNumber = tempNum;
            pin = tempPin;
            balance = tempBalance;
        }

        public int getBalance()
        {
            return balance;
        }
        public int getPin()
        {
            return pin;
        }
        public int getAccountNum()
        {
            return accountNumber;
        }

        //Decreased balanced by ammount passed if possible, if not returns false
        public Boolean decrementBalance(int amount)
        {
            if (balance >= amount)
            {
                balance -= amount;
                return true;
            }
            else
            {
                return true;
            }
        }
    }
    /**
     *Control class in charge of the bank accounts 
    */
    class Bank{
        private bankAccount[] accounts;
        public Bank()
        {
            accounts = new bankAccount[3];
            accounts[0] = new bankAccount("Steve", 111111, 1111, 300);
            accounts[1] = new bankAccount("Peter", 222222, 2222, 750);
            accounts[2] = new bankAccount("David", 333333, 3333, 3000);

            ATM.myBank = this;

            Console.WriteLine("i made more things uwu");

            ATM ATM1 = new ATM(true, true);
            ATM ATM2 = new ATM(false, true);
        }
    }

    class ATM
    {
        public static Bank myBank;
        private bool working;
        private Form myATM;
        private Button[] buttons;
        private bool first;
        private static int noOfButtons = 16;

        //Constructor passing in if this ATM is meant to be working && if it is the first one to be displayed
        public ATM(bool myFirst, bool myWorking)
        {
            first = myFirst;
            working = myWorking;
            contructForm();
        }       

        public void contructForm()
        {
            myATM = new Form();
            myATM.WindowState = FormWindowState.Maximized;
            if (first)
            {
                myATM.SetBounds(0, 0, myATM.Width / 2, myATM.Height);
            }
            else
            {
                myATM.SetBounds(myATM.Width / 2, 0, myATM.Width / 2, myATM.Height);
            }

            myATM.Show();
            Console.WriteLine("hello i made things i hope u liked");

            buttons = new Button[noOfButtons];
            for(int i = 0; i< noOfButtons; i++)
            {
                buttons[i] = new Button();
                buttons[i].SetBounds(10 + 65 * i, 310 + 45 * i, 40, 60);
                myATM.Controls.Add(buttons[i]);                
            }

        }
        public void setBank(Bank tempBank)
        {
            myBank = tempBank;
        }
    }
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            Console.WriteLine("pls give money owo");

            Bank myBank = new Bank();
        }
    }
}
