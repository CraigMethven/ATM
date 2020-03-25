using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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
    class Bank : Form {
        private static int noOfAccounts = 3;
        private bankAccount[] accounts;

        public Bank()
        {

            accounts = new bankAccount[noOfAccounts];
            this.Visible = false;
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            accounts[0] = new bankAccount("Steve", 111111, 1111, 300);
            accounts[1] = new bankAccount("Peter", 222222, 2222, 750);
            accounts[2] = new bankAccount("David", 333333, 3333, 3000);

            

            ATM atm1 = new ATM(true,true);
            ATM atm2 = new ATM(false,true);
            Thread ATM2_T = new Thread(new ThreadStart(atm1.contructForm));
            Thread ATM1_T = new Thread(new ThreadStart(atm2.contructForm));

            ATM1_T.Start();
            ATM2_T.Start();

        }

        public void createATM1s()
        {
            ATM.myBank = this;
            ATM ATM1 = new ATM(true, true);
        }
        public void createATM2s()
        {
            ATM.myBank = this;
            ATM ATM2 = new ATM(false, true);
        }



        //Finds an account and if found returns place in the array else returns -1
        public int findAccount(int inputtingNum)
        {
            for (int i = 0; i < noOfAccounts; i++)
            {
                if (accounts[i].getAccountNum() == inputtingNum)
                {
                    return i;
                }
            }
            return -1;
        }
        //Returns the bank account in the place passed in
        public bankAccount getAccount(int arrayNum)
        {
            return accounts[arrayNum];
        }
    }

    //ATM Class
    class ATM
    {
        //Reference to bank
        public static Bank myBank;
        //To see if these machines are meant to work or not
        private bool working;
        //The form we place things on
        private Form myATM;
        //The things on the form
        private ATMButton[] buttons;
        TextBox ATMScreen;
        //If this is to be the one on the left or right
        private bool first;
        //How many buttons there are
        private static int noOfButtons = 16;
        //Font
        private static System.Drawing.Font myFont;
        //Stores what the ATM is currently meant to be doing
        private string phase;
        //The number the user inputs
        private string inputted;
        //The account this ATM is currently working with
        private bankAccount account;

        //The button class

        class ATMButton
        {
            string ID;
            Button myButton;
            ATM myATM;
            //Constructor for buttons
            public ATMButton(string tempID, ATM tempATM)
            {
                myATM = tempATM;
                ID = tempID;
                translateID();
                myButton = new Button();
                myButton.Font = ATM.myFont;
                myButton.Text = ID;
                myButton.Click += new EventHandler(clicked);
            }
            //Makes the button ID what it's meant to be
            private void translateID()
            {
                switch (ID)
                {
                    case "0":
                        ID = "1";
                        break;
                    case "1":
                        ID = "4";
                        break;
                    case "2":
                        ID = "7";
                        break;
                    case "3":
                        ID = "";
                        break;
                    case "4":
                        ID = "2";
                        break;
                    case "6":
                        ID = "8";
                        break;
                    case "7":
                        ID = "0";
                        break;
                    case "8":
                        ID = "3";
                        break;
                    case "9":
                        ID = "6";
                        break;
                    case "10":
                        ID = "9";
                        break;
                    case "11":
                        ID = "";
                        break;
                    case "12":
                        ID = "Cancel";
                        //myButton.BackColor = Color.red;
                        break;
                    case "13":
                        ID = "Clear";
                        //myButton.BackColor = Color.yellow;
                        break;
                    case "14":
                        ID = "Enter";
                        //myButton.backColor = Color.Green;
                        break;
                    case "15":
                        ID = "";
                        break;
                }
            }
            //Makes the button run a method once clicked
            private void clicked(object sender, System.EventArgs e)
            {
                myATM.buttonClicked(ID);
            }
            //Gets the button variable
            public Button getButton()
            {
                return myButton;
            }
        }
        //Constructor passing in if this ATM is meant to be working && if it is the first one to be displayed
        public ATM(bool myFirst, bool myWorking)
        {
            first = myFirst;
            working = myWorking;
            contructForm();
            changePhase("AccountNum");
        }

        //Put the things on the form
        public void contructForm()
        {
            myATM = new Form();
            //Sets the size of the forms
            myATM.Bounds = Screen.PrimaryScreen.Bounds;
            if (first)
            {
                myATM.SetBounds(0, 0, myATM.Width / 2, myATM.Height - 40);
            }
            else
            {
                myATM.SetBounds(myATM.Width / 2, 0, myATM.Width / 2, myATM.Height - 40);
            }
            myATM.FormBorderStyle = FormBorderStyle.FixedDialog;
            myATM.Show();
            //Setting up the buttons
            buttons = new ATMButton[noOfButtons];
            int buttonID = 0;
            int genericBuffer = 50;
            int buttonBuffer = 10;
            int buttonX = (myATM.Width - 2 * genericBuffer - Convert.ToInt32(Math.Sqrt(noOfButtons) * buttonBuffer)) / Convert.ToInt32(Math.Sqrt(noOfButtons));
            int buttonY = (myATM.Height / 2 - 2 * genericBuffer - Convert.ToInt32(Math.Sqrt(noOfButtons) * buttonBuffer)) / Convert.ToInt32(Math.Sqrt(noOfButtons));
            for (int i = 0; i < Math.Sqrt(noOfButtons); i++)
            {
                for (int counter = 0; counter < Math.Sqrt(noOfButtons); counter++)
                {
                    buttons[i] = new ATMButton(Convert.ToString(buttonID), this);
                    buttons[i].getButton().SetBounds(genericBuffer + i * (buttonBuffer + buttonX), (myATM.Height / 2 + genericBuffer) + counter * (buttonBuffer + buttonY), buttonX, buttonY);
                    myATM.Controls.Add(buttons[i].getButton());
                    buttonID++;
                }
            }
            //Setting up the screen
            ATMScreen = new TextBox();
            ATMScreen.Multiline = true;
            ATMScreen.ReadOnly = true;
            ATMScreen.Font = myFont;
            ATMScreen.SetBounds(genericBuffer, genericBuffer, myATM.Width - (genericBuffer * 2), (myATM.Height / 2) - genericBuffer);
            myATM.Controls.Add(ATMScreen);
        }
        //Sets the static variables
        public void setBank(Bank tempBank)
        {
            myBank = tempBank;
            myFont = new System.Drawing.Font("Times New Roman", 50);
        }

        //Changes the phase
        public void changePhase(string tempPhase)
        {
            phase = tempPhase;
            inputted = "";
            switch (phase)
            {
                case "AccountNum":
                    ATMScreen.Text = "Please enter your account number:\n";
                    break;
                case "Pin":
                    ATMScreen.Text = "Please enter your pin:\n";
                    break;
                case "Options":
                    ATMScreen.Text = "What would you like to do?\n\t1: Check Balance\n\t2: Withdraw Cash\n\t3: Exit";
                    break;
                case "Balance":
                    ATMScreen.Text = "Your current balance is\n";
                    //Run method to get balance for account and add it to ATMScreen
                    ATMScreen.Text += "\nPlease press Cancel to return to the menu";
                    break;
                case "Withdraw":
                    ATMScreen.Text = "How much would you like to withdraw?\n\t1: £10\n\t2: £20\n\t3: $50\n\t4: £100\n\t5: Other\n\nYou can also press cancel to return to the menu";
                    break;
            }
        }
        //Makes something happen once a button has been pressed
        public void buttonClicked(string buttonID)
        {
            switch (phase)
            {
                case "AccountNum":
                    accountNum(buttonID);
                    break;
                case "Pin":
                    pin(buttonID);
                    break;
                case "Options":
                    break;
                case "Balance":
                    if (buttonID == "Cancel")
                    {
                        changePhase("Options");
                    }
                    break;
                case "Withdraw":
                    if (buttonID == "Cancel")
                    {
                        changePhase("Options");
                    }
                    break;
            }
        }

        //Works on the accountNum phase
        public void accountNum(string buttonID)
        {
            //If they press enter 
            if (buttonID == "Enter")
            {
                //If length isn't 6 then shout at them
                if (inputted.Length != 6)
                {
                    changePhase("AccountNum");
                    ATMScreen.Text += "Please only input a 6 digit code\n";
                    return;
                }
                //check account
                int tempInt;
                tempInt = myBank.findAccount(Convert.ToInt32(inputted));
                //If account doesn't exist then shout at them
                if (tempInt == -1)
                {
                    changePhase("AccountNum");
                    ATMScreen.Text += "Please enter a valid account number\n";
                    return;
                }
                account = myBank.getAccount(tempInt);
                changePhase("Pin");
            }
            else if (buttonID == "Clear")
            {
                changePhase("AccountNum");
            }
            else if (buttonID != "Cancel")
            {
                ATMScreen.Text += buttonID;
                inputted += buttonID;
            }
        }

        //For the pin phase
        private int numberOfAttempts = 0;
        public void pin(string buttonID)
        {
            //If they press enter 
            if (buttonID == "Enter")
            {
                //if pin is incorrect length
                if (inputted.Length != 4)
                {
                    changePhase("Pin");
                    ATMScreen.Text += " Pins consist of 4 characters";
                }
                //If length isn't 6 then shout at them
                else if (account.getPin() == Convert.ToInt32(inputted))
                {
                    changePhase("Options");
                }
                else if (numberOfAttempts == 3)
                {
                    changePhase("AccountNum");
                    numberOfAttempts = 0;
                }
                else
                {
                    changePhase("Pin");
                    numberOfAttempts++;
                    ATMScreen.Text += "You inputted the incorrect PIN. You have " + Convert.ToString(3 - numberOfAttempts) + " left";
                }
            }
            else if (buttonID == "Clear")
            {
                changePhase("Pin");
            }
            else if (buttonID == "Cancel")
            {
                changePhase("AccountNum");
            }
            else {
                ATMScreen.Text += buttonID;
                inputted += buttonID;
            }
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

             Application.SetHighDpiMode(HighDpiMode.SystemAware);
             Application.EnableVisualStyles();
             Application.SetCompatibleTextRenderingDefault(false);
             
              Bank myBank = new Bank();
             
             


            // Thread atm2 = new Thread(() => myBank.Show) { IsBackground = false };

            //  atm2.Start();


        }

    }
}
