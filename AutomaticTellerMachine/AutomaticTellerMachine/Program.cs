using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    class Bank : Form
    {
        private static int noOfAccounts = 3;
        private bankAccount[] accounts;
        private Thread atm1_t;
        private Thread atm2_t;

        public Bank()
        {
            accounts = new bankAccount[noOfAccounts];
            this.Visible = false;
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            accounts[0] = new bankAccount("Steve", 111111, 1111, 300);
            accounts[1] = new bankAccount("Peter", 222222, 2222, 750);
            accounts[2] = new bankAccount("David", 333333, 3333, 3000);

            ATM.myBank = this;




            Thread ATM1 = new Thread(new ThreadStart(startatm1));
            Thread ATM2 = new Thread(new ThreadStart(startatm2));

            ATM1.Start();
            ATM2.Start();

            //   new Thread(() =>
            //   {
            //       Thread.Sleep(6000);
            //       createATM1s();
            //  }) .Start();

            //  new Thread(() =>
            //   {
            //        Thread.Sleep(6000);
            //        createATM2s();
            //    }).Start();

            //      ThreadStart atm1 = new ThreadStart(createATM1s);
            //       atm1_t = new Thread(atm1);
            //      ThreadStart atm2 = new ThreadStart(createATM2s);
            //       atm2_t = new Thread(atm2);

            //  ATM ATM1 = new ATM(true, true);
            //   ATM ATM2 = new ATM(false, true);
        }
        private void startatm1()
        {
            Thread.Sleep(3000);
            var frm = new ATM(true, true);
            frm.Visible = false;
            frm.ShowDialog();
        }
        private void startatm2()
        {
            Thread.Sleep(2000);
            var frm = new ATM(false, true);
            frm.Visible = false;
            frm.ShowDialog();
        }
        /*       public void createATM1s()
               {

                   ATM ATM1 = new ATM(true, true);

               }
               public void createATM2s()
               {

                   ATM ATM2 = new ATM(false, true);
        */


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
    class ATM : Form
    {
        //Reference to bank
        public static Bank myBank;
        static int fontSize = 18;
        static int noOfSideButtons = 6;
        public static int sideButtonWidth;
        public static int sideButtonHeight;
        //To see if these machines are meant to work or not
        private bool working;
        //The things on the form
        private ATMButton[] buttons;
        ATMScreen myATMScreen;
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
            private string ID;
            private Button myButton;
            private ATM myATM;
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


            //Sets the (new) id of the button
            public void setID(string tempID)
            {
                ID = tempID;
                myButton.Text = "";
            }
        }



        class ATMScreen
        {
            TextBox backing;
            TextBox main;
            Label input;
            Label error;
            Label[] sides;
            public ATMScreen(int startX, int startY, int endX, int endY, Form myATM)
            {
                endX = endX + startX;
                endY = endY + startY;
                int width = endX - startX;
                int length = endY - startY;
                int buffer = width / 40;
                //Setting up the backing
                backing = new TextBox();
                backing.Multiline = true;
                backing.ReadOnly = true;
                backing.SetBounds(startX, startY, width, length);
                backing.SendToBack();
                //Setting up the side buttons
                sides = new Label[noOfSideButtons];
                int sidesWidth = width/5;
                int sideHeight = length / 4;
                for(int i = 0; i<noOfSideButtons/2; i++)
                {
                    sides[i*2] = new Label();
                    sides[i*2].SetBounds(startX+1, startY + (length / 3)*i + (length/15) , sidesWidth, sideHeight);
                    myATM.Controls.Add(sides[i * 2]);

                    sides[(i * 2) + 1] = new Label();
                    sides[(i * 2) + 1].SetBounds(endX - sidesWidth-1, startY + (length / 3) * i + (length / 15), sidesWidth, sideHeight);
                    sides[(i * 2) + 1].TextAlign = System.Drawing.ContentAlignment.TopRight;
                    myATM.Controls.Add(sides[(i * 2)+1]);
                }
                //Setting up the main text box
                main = new TextBox();
                main.Multiline = true;
                main.ReadOnly = true;
                main.SetBounds(startX + sidesWidth + buffer, startY + buffer, width - 2*(sidesWidth + buffer), length / 2 - 4 * buffer);
                main.TextAlign = HorizontalAlignment.Left;
                //Setting up the error message box
                error = new Label();
                error.SetBounds(startX + sidesWidth + buffer, length / 2 + 2*buffer, width - 2 * (sidesWidth + buffer), length / 4);
                error.TextAlign = System.Drawing.ContentAlignment.TopLeft;
                
                //Setting up the input
                input = new Label();
                input.SetBounds(startX + sidesWidth + buffer, length / 4 * 3 + buffer, width - 2 * (sidesWidth + buffer), length/4);
                input.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                myATM.Controls.Add(input);
                myATM.Controls.Add(error);
                myATM.Controls.Add(main);
                myATM.Controls.Add(backing);
            }
            public void blankOut()
            {
                main.Text = "";
                input.Text = "";
                error.Text = "";
                for (int i = 0; i < noOfSideButtons; i++)
                {
                    sides[i].Text = "";
                }
            }

            public void setMain(string thisInput)
            {
                main.Text = thisInput;
            }
            public void setInput(string thisInput)
            {
                input.Text = thisInput;
            }
            public void setError(string thisInput)
            {
                error.Text = thisInput;
            }
            public void setSide(string thisInput, int number)
            {
                sides[number].Text = thisInput;
            }
            public string getInput()
            {
                return(input.Text);
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
            //Sets the size of the forms
            this.Bounds = Screen.PrimaryScreen.Bounds;
            if (first)
            {
                this.SetBounds(0, 0, this.Width / 2, this.Height - 40);
            }
            else
            {
                this.SetBounds(this.Width / 2, 0, this.Width / 2, this.Height - 40);
            }
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Show();
            //Setting up the buttons
            buttons = new ATMButton[noOfButtons+ noOfSideButtons];
            int buttonID = 0;
            int genericBuffer = 50;
            int buttonBuffer = 10;
            int buttonX = (this.Width - 2 * genericBuffer - Convert.ToInt32(Math.Sqrt(noOfButtons)*buttonBuffer)) / Convert.ToInt32(Math.Sqrt(noOfButtons));
            int buttonY = (this.Height/2 - 2*genericBuffer - Convert.ToInt32(Math.Sqrt(noOfButtons) * buttonBuffer)) / Convert.ToInt32(Math.Sqrt(noOfButtons));
            for (int i = 0; i< Math.Sqrt(noOfButtons); i++)
            {
                for (int counter = 0; counter < Math.Sqrt(noOfButtons); counter++)
                {
                    buttons[i] = new ATMButton(Convert.ToString(buttonID), this);
                    buttons[i].getButton().SetBounds(genericBuffer + i*(buttonBuffer+buttonX), (this.Height/2 + genericBuffer) + counter * (buttonBuffer + buttonY), buttonX, buttonY);
                    this.Controls.Add(buttons[i].getButton());
                    buttonID++;
                }
            }
            buttonID = 0;
            sideButtonWidth = this.Width / 10;
            sideButtonHeight = (this.Height / 2 - 2 * (genericBuffer + buttonBuffer)) / 3; 
            for (int i = 0; i < noOfSideButtons/2; i++)
            {
                buttons[noOfButtons+(i * 2)] = new ATMButton("Side"+ Convert.ToString(noOfButtons+(i*2)), this);
                buttons[noOfButtons + (i * 2)].getButton().SetBounds(genericBuffer, genericBuffer + i * (3*buttonBuffer + sideButtonHeight), sideButtonWidth, sideButtonHeight);
                buttons[noOfButtons + (i * 2)].setID("Side" + Convert.ToString(i * 2));
                this.Controls.Add(buttons[noOfButtons + (i * 2)].getButton());
                buttons[noOfButtons + (i * 2) + 1] = new ATMButton("Side" + Convert.ToString(noOfButtons + (i * 2) +1), this);
                buttons[noOfButtons + (i * 2) + 1].getButton().SetBounds(this.Width - genericBuffer - sideButtonWidth, genericBuffer + i * (3*buttonBuffer + sideButtonHeight), sideButtonWidth, sideButtonHeight);
                buttons[noOfButtons + (i * 2)+1].setID("Side" + Convert.ToString((i * 2)+1));
                this.Controls.Add(buttons[noOfButtons + (i * 2) + 1].getButton());
            }
            //Setting up the screen
            myATMScreen = new ATMScreen(genericBuffer+sideButtonWidth+buttonBuffer, genericBuffer, this.Width - ((genericBuffer + sideButtonWidth+buttonBuffer) * 2), (this.Height / 2) - genericBuffer, this);
        }
        
        
        //Sets the static variables
        public void setBank(Bank tempBank)
        {
            myBank = tempBank;
            myFont = new System.Drawing.Font("Times New Roman", fontSize);
        }

        //Changes the phase
        public void changePhase(string tempPhase)
        {
            phase = tempPhase;
            inputted = "";
            myATMScreen.blankOut();
            switch (phase)
            {
                case "AccountNum":
                    myATMScreen.setMain("Please enter your account number:");
                    break;
                case "Pin":
                    myATMScreen.setMain("Please enter your pin:");
                    break;
                case "Options":
                    myATMScreen.setMain("What would you like to do?");
                    myATMScreen.setSide("Check Balance", 0);
                    myATMScreen.setSide("Withdraw Cash", 1);
                    myATMScreen.setSide("Exit", 2);
                    break;
                case "Balance":
                    myATMScreen.setMain("Your current balance is £" + account.getBalance());
                    myATMScreen.setError("Please press Cancel to return to the menu");
                    break;
                case "Withdraw":
                    myATMScreen.setMain("How much would you like to withdraw?");
                    myATMScreen.setSide("£10", 0);
                    myATMScreen.setSide("£20", 1);
                    myATMScreen.setSide("£50", 2);
                    myATMScreen.setSide("£100", 3);
                    myATMScreen.setSide("Other", 5);
                    myATMScreen.setError("You can also press cancel to return to the menu");
                    break;
                case "readNumber":
                    myATMScreen.setMain("Enter how much you would like to withdraw.");
                    myATMScreen.setError("You can also press cancel to return to the menu");
                    myATMScreen.setInput(Convert.ToString(balanceInput));
                    break;
            }
        }

        bool complete = false;
        string inUse = "";
        public void withdraw(string buttonID)
        {
            //set the transaction to be in use and incomplete
            inUse = Thread.CurrentThread.Name;
            complete = false;

            switch (buttonID)
            {
                case "Side0":
                    //display withdrawing (amount)
                    myATMScreen.setMain("Withdrawing £10...");
                    Application.DoEvents();
                    Thread.Sleep(5000);
                    //wait for 5 seconds

                    if (working)
                        //if race condition IS NOT enabled
                    {
                        if (inUse != Thread.CurrentThread.Name)
                            //if the account isnt in use by another thread 
                        {

                            Thread.Sleep(5000);
                            withdraw("Side0");
                            //wait for 5 seconds and then run the method again knowing the amount wanted withdrawn
                        }
                    } 
                    
                    if(account.getBalance() >= 10)
                        //if there is (amount) or more in the account then
                    {
                        account.decrementBalance(10);
                        //withdraw amount
                    }
                    else
                    {
                        //if there isnt (amount) in the account

                        myATMScreen.setMain("Insufficient Funds");
                        //tell the user there isnt enough in the account
                        Application.DoEvents();
                        Thread.Sleep(3000);
                        changePhase("AccountNum");
                        //reset the ATM
                    }
                    
                    //set inUse to blank and the transaction as complete.
                    inUse = "";
                    complete = true;
                    break;
                case "Side1":
                    //withdraw 20
                    myATMScreen.setMain("Withdrawing £20...");
                    Application.DoEvents();
                    Thread.Sleep(5000);

                    if (working)
                    {
                        if (inUse != Thread.CurrentThread.Name)
                        {
                            Thread.Sleep(5000);
                            withdraw("Side1");
                        }
                    }

                    if (account.getBalance() >= 20)
                    {
                        account.decrementBalance(20);
                    }
                    else
                    {
                        myATMScreen.setMain("Insufficient Funds");
                        Application.DoEvents();
                        Thread.Sleep(3000);
                        changePhase("AccountNum");
                    }

                    inUse = "";
                    complete = true;
                    break;
                case "Side2":
                    //withdraw 50
                    myATMScreen.setMain("Withdrawing £50...");
                    Application.DoEvents();
                    Thread.Sleep(5000);

                    if (working)
                    {
                        if (inUse != Thread.CurrentThread.Name)
                        {
                            Thread.Sleep(5000);
                            withdraw("Side2");
                        }
                    }

                    if (account.getBalance() >= 50)
                    {
                        account.decrementBalance(50);
                    }
                    else
                    {
                        myATMScreen.setMain("Insufficient Funds");
                        Application.DoEvents();
                        Thread.Sleep(3000);
                        changePhase("AccountNum");
                    }

                    inUse = "";
                    complete = true;
                    break;
                case "Side3":
                    //withdraw 100
                    myATMScreen.setMain("Withdrawing £100...");
                    Application.DoEvents();
                    Thread.Sleep(5000);

                    if (working)
                    {
                        if (inUse != Thread.CurrentThread.Name)
                        {
                            Thread.Sleep(5000);
                            withdraw("Side3");
                        }

                    }

                    if (account.getBalance() >= 100)
                    {
                        account.decrementBalance(100);
                    }
                    else
                    {
                        myATMScreen.setMain("Insufficient Funds");
                        Application.DoEvents();
                        Thread.Sleep(3000);
                        changePhase("AccountNum");
                    }

                    inUse = "";
                    complete = true;
                    break;
                case "Side4":
                    //blank
                    changePhase("withdraw");
                    break;
                case "Side5":
                    changePhase("readNumber");
                    break;
            }
            
            if(complete == true)
            {
                myATMScreen.setMain("Please take your money");
                myATMScreen.setError("Your new balance is £" + account.getBalance());
                Application.DoEvents();
                Thread.Sleep(5000);

                changePhase("AccountNum");
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
                    options(buttonID);
                    break;
                case "Balance":
                    if(buttonID == "Cancel")
                    {
                        changePhase("Options");
                    }
                    
                    break;
                case "Withdraw":
                    if (buttonID == "Cancel")
                    {
                        changePhase("Options");
                    }
                    else
                    {
                        withdraw(buttonID);
                    }
                    
                    break;
                case "readNumber":
                    readNumber(buttonID);
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
                    myATMScreen.setError("Please only input a 6 digit code");
                    return;
                }
                //check account
                int tempInt;
                tempInt = myBank.findAccount(Convert.ToInt32(inputted));
                //If account doesn't exist then shout at them
                if (tempInt == -1)
                {
                    changePhase("AccountNum");
                    myATMScreen.setError("Please enter a valid account number");
                    return;
                }
                account = myBank.getAccount(tempInt);
                changePhase("Pin");
            }
            else if (buttonID == "Clear")
            {
                changePhase("AccountNum");
            }
            else if (buttonID != "Cancel" && buttonID.Length == 1)
            {
                myATMScreen.setInput(myATMScreen.getInput() + buttonID);
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
                if(inputted.Length != 4)
                {
                    changePhase("Pin");
                    myATMScreen.setError("Pins consist of 4 characters");
                }
                //If length isn't 6 then shout at them
                else if (account.getPin() == Convert.ToInt32(inputted))
                {
                    changePhase("Options");
                }
                else if(numberOfAttempts == 3)
                {
                    changePhase("AccountNum");
                    numberOfAttempts = 0;
                }
                else
                {
                    changePhase("Pin");
                    numberOfAttempts++;
                    myATMScreen.setError("You inputted the incorrect PIN. You have " + Convert.ToString(3-numberOfAttempts) + " left");
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
            else if(buttonID.Length == 1) {
                myATMScreen.setInput(myATMScreen.getInput() + "*");
                inputted += buttonID;
            }
        }
        public void options(string buttonID)
        {
            if (buttonID == "Cancel")
            {
                changePhase("AccountNum");
            }
            else if(buttonID == "Side0")
            {
                changePhase("Balance");
            }
            else if (buttonID == "Side1")
            {
                changePhase("Withdraw");
            }
            else if (buttonID == "Side2")
            {
                changePhase("AccountNum");
            }
        } 


        String balanceInput = "";
        public void readNumber(string buttonID)
        {
            
            //If they press enter 
            if (buttonID == "Enter")
            {
                //if they dont enter anything
                if (balanceInput.Length == 0)
                {
                    changePhase("readNumber");
                    myATMScreen.setError("Enter a valid number to withdraw");
                }
                else if (buttonID == "Clear")
                {
                    balanceInput = "";
                    myATMScreen.setInput("");
                }
                else {
                    //withdraw (amount)
                    myATMScreen.setMain("Withdrawing £" + balanceInput + "...");
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    if (working)
                    {
                        if (inUse != Thread.CurrentThread.Name)
                        {
                            Thread.Sleep(5000);
                            readNumber("Enter");
                        }   
                    }

                    if (account.getBalance() >= Convert.ToInt32(balanceInput))
                    {
                        account.decrementBalance(Convert.ToInt32(balanceInput));
                    }
                    else
                    {
                        myATMScreen.setMain("Insufficient Funds");
                        Application.DoEvents();
                        Thread.Sleep(3000);
                        changePhase("AccountNum");
                    }

                    inUse = "";
                    complete = true;
                    myATMScreen.setInput("Your new balance is £" + account.getBalance());
                    Application.DoEvents();
                    Thread.Sleep(5000);
                }

                changePhase("AccountNum");
                    
            }
            else if (buttonID == "Clear")
            {
                balanceInput = "";
                myATMScreen.setInput("");

            }
            else if (buttonID == "Cancel")
            {
                changePhase("AccountNum");
            }
            else if (buttonID.Length == 1)
            {
                //
                balanceInput += buttonID;
                myATMScreen.setInput(balanceInput);
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

            new Bank();
        }
    }
}
