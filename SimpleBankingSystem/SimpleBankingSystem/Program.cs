using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace SimpleBankingSystem
{
    class Accounts
    {
        /*Method to check the login.txt file to see if username and password are correct*/
        public bool LoginMatch(string username, string password)
        {
            try
            {
                //program will read through each line in login.txt
                string[] allLogin = File.ReadAllLines("login.txt");
                foreach (string details in allLogin)
                    {
                    //Each line in login.txt is formatted as username|password
                    //program will compare the inputs and return true if they both match
                        string[] profile = details.Split("|");
                        if (profile[0].Equals(username) && profile[1].Equals(password))
                            return true;
                    }
                //After looking through each line, if nothing is returned, the program returns false
                return false;
            }
            //If the method encounters an exception, it prints the following line
            catch(Exception e)
            {
                Console.WriteLine("\n\n\n\t\tSomething went wrong with the login function");
                return false;
            }
            
        }

        //The method below is used to send the emails for CreateAccount and Statement
        public void Emailing(string subject, string body, string recepient)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                //Sets up the smtp
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential("mosaabb2002@outlook.com", "doodi2009");

                //Sets up the message
                message.From = new MailAddress("mosaabb2002@outlook.com");
                message.To.Add(new MailAddress(recepient));
                message.Subject = subject;
                message.IsBodyHtml = false;
                message.Body = body;
                Console.WriteLine("\t\tSending Email now.." + message.Body);
                smtp.Send(message);
                Console.WriteLine("\t\tEmail sent successfully!!");

            }
            //If an exception is thrown while sending the email, it will print the message below and wait
            //for the user to input a key
            catch (Exception e)
            {
                Console.WriteLine("\t\t\nSomething went wrong with sending the email");
                Console.ReadKey();
            }
        }

        //The method below creates a new customer account. It also creates a file and sends the details to
        //the user's email
        public void CreateAccount()
        {
            //Clears the screen and sets up initialises the variables for the method
            Console.Clear();
            string accountID = "", firstname, lastname, phone, address, email, body = "";
            //Due to the large amount of changes to the cursor location, two arrays were used to represent the x and y value
            int[] cursorX = new int[5];
            int[] cursorY = new int[5];

            try
            {
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                   CREATE A NEW ACCOUNT                   |");
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     ENTER THE DETAILS                    |");
                Console.WriteLine("\t\t|                                                          |");

                //prints out the area to enter the first name
                Console.Write("\t\t|   first name: ");
                //Saves the cursor position to be moved later
                cursorX[0] = Console.CursorLeft;
                cursorY[0] = Console.CursorTop;
                Console.WriteLine("                                           |");
                //This action is repeated several times for the other four variables

                Console.Write("\t\t|   last name: ");
                cursorX[1] = Console.CursorLeft;
                cursorY[1] = Console.CursorTop;
                Console.WriteLine("                                            |");

                Console.Write("\t\t|   phone: ");
                cursorX[2] = Console.CursorLeft;
                cursorY[2] = Console.CursorTop;
                Console.WriteLine("                                                |");

                Console.Write("\t\t|   address: ");
                cursorX[3] = Console.CursorLeft;
                cursorY[3] = Console.CursorTop;
                Console.WriteLine("                                              |");

                Console.Write("\t\t|   email: ");
                cursorX[4] = Console.CursorLeft;
                cursorY[4] = Console.CursorTop;
                Console.WriteLine("                                                |");

                Console.WriteLine("\t\t|==========================================================|");

                //The cursor is moved to certain areas to match the fields printed earlier
                //This allows the user to fill out the information for the new account
                Console.SetCursorPosition(cursorX[0], cursorY[0]);
                firstname = Console.ReadLine();
                Console.SetCursorPosition(cursorX[1], cursorY[1]);
                lastname = Console.ReadLine();
                Console.SetCursorPosition(cursorX[2], cursorY[2]);
                phone = Console.ReadLine();
                Console.SetCursorPosition(cursorX[3], cursorY[3]);
                address = Console.ReadLine();
                Console.SetCursorPosition(cursorX[4], cursorY[4]);
                email = Console.ReadLine();

                //Checks if the email is valid
                if (!email.Contains("@gmail.com") && !email.Contains("@hotmail.com") && !email.Contains("@uts.edu.au"))
                    throw new Exception("Email is invalid");
                //Checks if the phone number is valid
                if (phone.Length != 10)
                    throw new Exception("Error: Phone number is invalid");

                //Asks the user for an input to indicate if the details are correct
                //If the user enters 'n', the user is prompted to enter the details again
                //Otherwise, the user is allowed to continue
                Console.Write("\n\t\tAre the details correct? (y/n): ");
                string input = Console.ReadLine();
                if (input.Equals("n"))
                    CreateAccount();
            
                //This loop creates the unique account number
                //The loop will continue repeating until a unique account number is found
                bool uniqueID = false;
                while (!uniqueID)
                {
                    //variable to create random numbers, this will be used to create the unique account numbers
                    //This will look for a random six digit number
                    Random random = new Random();
                    int randomID = random.Next(100000, 1000000);

                    //Random number is converted to a string and saved to the account number variable
                    accountID = randomID.ToString();
                    
                    //This loop will read the file that contains all of the current account numbers
                    string[] ids = File.ReadAllLines("accountIDs.txt");
                    foreach(string id in ids)
                    {
                        //If the random number is equal to any of the account numbers already used,
                        //the uniqueID variable will be set to false and the loop will be broken
                        if (id.Equals(accountID))
                        {
                            uniqueID = false;
                            break;
                        }
                        else
                        {
                        //If the random number is not equal to any current account number,
                        //the uniqueID variable is set to true to indicate that the number is unique
                            uniqueID = true;
                        }
                        //The loop continues until the random number is compared against all of the current account numbers
                    }

                    //If the account number is confirmed to be unique, it is added to the file containing all of the active
                    //account numbers
                    if (uniqueID)
                    {
                        File.AppendAllText("accountIDs.txt", "\n"+accountID);
                    }
                }
                
                //file name is created with the account number
                string fileName = accountID.ToString() + ".txt";

                //The file details are formatted and saved to the fileDetails variable. Balance will start off as 0
                //as the account is brand new
                string fileDetails = "AccountNo|" + accountID + "\nBalance|0" + "\nFirst Name|" + firstname
                + "\nLast Name|" + lastname + "\nAddress|" + address + "\nPhone|" + phone + "\nEmail|" + email;
                
                //A new file is created using the new file name and file details
                File.WriteAllText(fileName, fileDetails);

                //user is notified that the account is created successfully and the new account number
                Console.WriteLine("\t\tAccount created successfully!"); 
                Console.WriteLine("\t\tAccount number: " + accountID);

                //The customer is then emailed the details of the new account. The body will look similar to the account file.
                //The title uses the customer's name and the details are emailed to the new account's email
                body = "AccountNo: " + accountID + "\nFirst Name: " + firstname
                + "\nLast Name: " + lastname + "\nAddress: " + address + "\nPhone: " + phone + "\nEmail: " + email;
                Emailing(firstname + "'s New Account Details", body, email);
                
                //The program waits for an input to continue
                Console.Write("\t\tPress any key to continue: ");
                Console.ReadKey();
            }
            //These catch methods are used in many of the methods in this program
            //Notify the user of a file error and ask if they want to try again, if they enter 'y',
            //the method is called again
            catch (IOException e)
            {
                Console.WriteLine("\n\n\n\t\tCould not create file for account");
                Console.Write("\t\tTry again? (y/n): ");
                string input = Console.ReadLine();
                if (input.Equals("y"))
                    CreateAccount();
            }
            //Notify the user of a general error and ask if they want to try again
            //Notify the user of a file error and ask if they want to try again, if they enter 'y',
            //the method is called again
            catch (Exception e)
            {
                Console.WriteLine("\n\n\t\t" + e.Message);
                Console.Write("\t\tTry again? (y/n): ");
                string input = Console.ReadLine();
                if (input.Equals("y"))
                    CreateAccount();
            }
        }

        //Method to find an account and display its details to the user
        public void FindAccount()
        {
            Console.Clear();
            string accountID, input;

            try 
            {
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     SEARCH AN ACCOUNT                    |");
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     ENTER THE DETAILS                    |");
                Console.WriteLine("\t\t|                                                          |");
                Console.Write("\t\t|   Account Number: ");
                int anCursorX = Console.CursorLeft;
                int anCursorY = Console.CursorTop;
                Console.Write("                                       |");
                Console.WriteLine("\n\t\t|==========================================================|");
                Console.SetCursorPosition(anCursorX, anCursorY);

                accountID = Console.ReadLine();
                string fileName = accountID + ".txt";
            
                //Check if the account number is valid by checking if there are more than 10 digits
                if (accountID.Length > 10)
                    throw new Exception("Account number is invalid");

                //Finds the correct file and saves all of its data
                string[] details = File.ReadAllLines(fileName);
                Console.WriteLine("\n\n\t\tAccount Found!");

                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     ACCOUNT DETAILS                      |");
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                                                          |");

                //loops through the lines in the file and prints out the details for the user to see
                for (int i = 0; i < 7; i++) {
                    string[] splitLine = details[i].Split("|");
                    Console.WriteLine(String.Format("\t\t|   {0, -10}: {1,-43}|",splitLine[0], splitLine[1]));
                }

                Console.WriteLine("\t\t|==========================================================|");

                //After printing out the details, the user is asked if they would like to check another account,
                //if they write 'y', the method is called again and the user is asked to enter another account number
                Console.Write("\t\t Check another account? (y/n): ");
                input = Console.ReadLine();
                if (input.ToLower() == "y")
                    FindAccount();
            }
            catch (IOException e)
            {
                Console.WriteLine("\n\n\t\tCouldn't find account");
                Console.Write("\t\tTry again?(y/n): ");
                input = Console.ReadLine();
                if (input == "y")
                    FindAccount();
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\t\tError with finding the account");
                Console.WriteLine("\t\tError: " + e.Message);
                Console.Write("\t\tTry again?(y/n): ");
                input = Console.ReadLine();
                if (input == "y")
                    FindAccount();
            }
        }

        //Method to withdraw an amount from an account's balance
        public void Withdraw()
        {
            Console.Clear();
            string accountID, input, fileName;
            int amount;
            try
            {
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                         WITHDRAW                         |");
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     ENTER THE DETAILS                    |");
                Console.WriteLine("\t\t|                                                          |");

                Console.Write("\t\t|   Account Number: ");
                int anCursorX = Console.CursorLeft;
                int anCursorY = Console.CursorTop;
                Console.Write("                                       |");

                Console.Write("\n\t\t|   Amount: $");
                int amountCursorX = Console.CursorLeft;
                int amountCursorY = Console.CursorTop;
                Console.Write("                                              |");

                Console.WriteLine("\n\t\t|==========================================================|");

                Console.SetCursorPosition(anCursorX, anCursorY);
                accountID = Console.ReadLine();

                fileName = accountID + ".txt";

                if (accountID.Length > 10)
                    throw new Exception("Error: Account number is invalid");

                //Finds the file and reads all of the lines in it 
                string[] details = File.ReadAllLines(fileName);
                Console.WriteLine("\n\n\t\tAccount found! Enter the amount...");

                //The user is now able to enter the amount to withdraw
                //The input is converted from string to int
                Console.SetCursorPosition(amountCursorX, amountCursorY);
                input = Console.ReadLine();
                amount = Convert.ToInt32(input);

                //The balance is found on the second line on every file and so splitLine copies the information in that line
                int balance = 0;
                string[] splitLine = details[1].Split("|");

                //The number for balance is coverted to an int. The program checks if the amount to withdraw is higher than
                //the available balance. If the amount is too high, an exception is thrown. Otherwise, the system deducts
                //the amount from the balance and updates the information in the file.
                balance = Convert.ToInt32(splitLine[1]);
                if (amount > balance)
                    throw new Exception("Balance is too low");
                balance -= amount;
                details[1] = "Balance|" + balance.ToString();

                //The transaction is saved on the file using the date, type of transaction, amount withdrawn and the balance remaining
                string date = DateTime.Today.Day + "." + DateTime.Today.Month + "." + DateTime.Today.Year;
                File.WriteAllLines(fileName, details);
                File.AppendAllText(fileName, date + "|Withdraw|" + amount + "|" + balance);

                Console.WriteLine("\t\tWithdraw Successful!");
            }
            catch (IOException e)
            {
                Console.WriteLine("\n\n\t\tAccount not found");
                Console.Write("\t\tRetry? (y/n)");
                input = Console.ReadLine();
                if (input == "y")
                    Withdraw();
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\t\tIssue completing withdraw");
                Console.WriteLine("\t\tError: " + e.Message);
                Console.Write("\t\tRetry? (y/n)");
                input = Console.ReadLine();
                if (input == "y")
                    Withdraw();
            }
        }

        //A method to deposit an amount into an account's balance. This one is very similar to the withdraw method
        public void Deposit()
        {
            Console.Clear();
            string accountID, input, fileName;
            int amount;

            try 
            {
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                          DEPOSIT                         |");
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     ENTER THE DETAILS                    |");
                Console.WriteLine("\t\t|                                                          |");

                Console.Write("\t\t|   Account Number: ");
                int anCursorX = Console.CursorLeft;
                int anCursorY = Console.CursorTop;
                Console.Write("                                       |");

                Console.Write("\n\t\t|   Amount: ");
                int amountCursorX = Console.CursorLeft;
                int amountCursorY = Console.CursorTop;
                Console.Write("                                               |");

                Console.WriteLine("\n\t\t|==========================================================|");

                Console.SetCursorPosition(anCursorX, anCursorY);
                accountID = Console.ReadLine();
                fileName = accountID + ".txt";

                if (accountID.Length > 10)
                    throw new Exception("Error: Account number is invalid");

                string[] details = File.ReadAllLines(fileName);
                Console.WriteLine("\n\n\t\tAccount found! Enter the amount...");
                Console.SetCursorPosition(amountCursorX, amountCursorY);
                input = Console.ReadLine();
                amount = Convert.ToInt32(input);

                int balance = 0;
                string[] splitLine = details[1].Split("|");

                balance = Convert.ToInt32(splitLine[1]);
                balance += amount;
                splitLine[1] = balance.ToString();
                details[1] = "Balance|" + balance.ToString();

                string date = DateTime.Today.Day + "." + DateTime.Today.Month + "." + DateTime.Today.Year;
                File.WriteAllLines(fileName, details);
                File.AppendAllText(fileName, date + "|Deposit|" + amount + "|" + balance);
                Console.WriteLine("\t\tDeposit Successful!");
            }
            catch (IOException e)
            {
                Console.WriteLine("\n\n\t\tAccount not found");
                Console.WriteLine("\t\tError: " + e.Message);
                Console.Write("\t\tRetry? (y/n)");
                input = Console.ReadLine();
                if (input == "y")
                    Deposit();
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\t\tIssue completing deposit");
                Console.WriteLine("\t\tError: " + e.Message);
                Console.Write("\t\tRetry? (y/n)");
                input = Console.ReadLine();
                if (input == "y")
                    Deposit();
            }
        }

        public void Statement()
        {
            Console.Clear();
            string accountID, input, name = "", email = "";

            try
            {
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                         STATEMENT                        |");
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     ENTER THE DETAILS                    |");
                Console.WriteLine("\t\t|                                                          |");
                Console.Write("\t\t|   Account Number: ");
                int anCursorX = Console.CursorLeft;
                int anCursorY = Console.CursorTop;
                Console.Write("                                       |");
                Console.WriteLine("\n\t\t|==========================================================|");
                Console.SetCursorPosition(anCursorX, anCursorY);

                accountID = Console.ReadLine();
                string fileName = accountID + ".txt";
            
                if (accountID.Length > 10)
                    throw new Exception("Account number is invalid");

                //program finds and reads all of the data in the file
                string[] details = File.ReadAllLines(fileName);
                Console.WriteLine("\n\n\t\tAccount Found! The statement is displayed below...");

                string body = "";

                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                  SIMPLE BANKING SYSTEM                   |");
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|   Account Statement                                      |");
                Console.WriteLine("\t\t|                                                          |");

                //The information in the file is printed for the user to view
                for (int i = 0; i < 7; i++)
                {
                    string[] splitLine = details[i].Split("|");
                    Console.WriteLine(String.Format("\t\t|   {0, -10}: {1,-43}|", splitLine[0], splitLine[1]));
                    body += "\n" + splitLine[0] + ": " + splitLine[1];

                    //The program saves the first name and the email. This will be used later for the email
                    if (splitLine[0].Equals("First Name"))
                        name = splitLine[1];
                    if (splitLine[0].Equals("Email"))
                        email = splitLine[1];
                }

                //This loop will add the information to the body variable, this will be the body of the email
                //This will also ensure that only the last 5 transactions are displayed
                for (int i = details.Length - 1, j = 0 ; i > 6 && j < 5; i--, j++)
                {
                    Console.WriteLine(String.Format("\t\t|   {0,-55}|", details[i]));
                    body += "\n" + details[i];
                }

                Console.WriteLine("\t\t|==========================================================|");

                //After displaying the account's statement, the user is asked if they want to email it or not
                Console.Write("\t\t Email statement? (y/n): ");
                input = Console.ReadLine();
                //If the user wants to email the statement, they must enter 'y'. The program will use the name
                //and email saved earlier as well as the body variable that was created.
                if (input.ToLower() == "y")
                {
                    string subject = name + "'s Bank Statement";
                    //The statement will be sent to the email in the file 
                    Emailing(subject, body, email);
                }     
            }
            catch (IOException e)
            {
                Console.WriteLine("\n\n\t\tCouldn't find account");
                Console.Write("\t\tTry again?(y/n): ");
                input = Console.ReadLine();
                if (input == "y")
                    FindAccount();
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\n\t\tError with finding the account");
                Console.WriteLine("\t\tError: " + e.Message);
                Console.Write("\t\tTry again?(y/n): ");
                input = Console.ReadLine();
                if (input == "y")
                    FindAccount();
            }
        }

        //Method to delete a user
        public void Delete()
        {
            Console.Clear();
            string accountID, input;

            try 
            { 
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     DELETE AN ACCOUNT                    |");
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     ENTER THE DETAILS                    |");
                Console.WriteLine("\t\t|                                                          |");
                Console.Write("\t\t|   Account Number: ");
                int anCursorX = Console.CursorLeft;
                int anCursorY = Console.CursorTop;
                Console.Write("                                       |");
                Console.WriteLine("\n\t\t|==========================================================|");
                Console.SetCursorPosition(anCursorX, anCursorY);

                accountID = Console.ReadLine();
                string fileName = accountID + ".txt";

                if (accountID.Length > 10)
                    throw new Exception("Account number is invalid");

                string[] details = File.ReadAllLines(fileName);
                Console.WriteLine("\n\n\t\tAccount Found! Details are displayed below...");

                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                     ACCOUNT DETAILS                      |");
                Console.WriteLine("\t\t|==========================================================|");
                Console.WriteLine("\t\t|                                                          |");

                //The account's details are displayed for the user
                
                for (int i = 0; i < 7; i++)
                {
                    string[] splitLine = details[i].Split("|");
                    Console.WriteLine(String.Format("\t\t|   {0, -10}: {1,-43}|", splitLine[0], splitLine[1]));
                }

                Console.WriteLine("\t\t|==========================================================|");

                //The user is asked if they would like to delete the account
                Console.Write("\t\t Delete? (y/n): ");
                input = Console.ReadLine();
                //If the user enters 'y', the account's file is deleted
                if (input.ToLower() == "y")
                    File.Delete(fileName);

                //the system will also loop through the active account numbers in accountIDs.txt file
                //It will look for the line with the specific account number and remove it. The file
                // then finally updated
                string[] ids = File.ReadAllLines("accountIDs.txt");
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i].Equals(accountID))
                        ids[i] = "\b";
                }
                File.WriteAllLines("accountIDs.txt", ids);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\t\tError deleting the account");
                Console.WriteLine("\t\tError: " + e.Message);
                Console.Write("\t\tRetry? (y/n)");
                input = Console.ReadLine();
                if (input == "y")
                    Delete();
            }

        }
    }

    class Bank
    {
        //Creates a new variable of type Accounts. The methods will be used through this variable
        Accounts accounts;

        public Bank()
        {
            this.accounts = new Accounts();
        }

        static void Main(string[] args)
        {
            new Bank().use();
        }

        void use()
        {
            //This variable will indicate if the login was successful. The loop will continue until loginSuccess is true
            bool loginSuccess = false;
            while (!loginSuccess)
            {
                //The LoginPage method will return true if successful and false otherwise. This will indicate to the program
                //whether to go through to the main page or not
                loginSuccess = LoginPage();
                if (!loginSuccess)
                {
                    //This message is printed if the login was unsuccessful as indicated by loginSuccess variable
                    Console.WriteLine("\n\n\n\t\t\tWrong detials! Try Again");
                    Console.ReadKey();
                }
                    
            } 
            MainPage();
        }

        bool LoginPage()
        {
            //Using the basic method for now just to make sure everything works
            Console.Clear();
            Console.WriteLine("\t\t|==========================================================|");
            Console.WriteLine("\t\t|             WELCOME TO SIMPLE BANKING SYSTEM             |");
            Console.WriteLine("\t\t|==========================================================|");
            Console.WriteLine("\t\t|                      LOGIN TO START                      |");
            Console.Write("\t\t|   Username: ");
            int loginCursorX = Console.CursorTop;
            int loginCursorY = Console.CursorLeft;
            Console.Write("                                             |");
            Console.Write("\n\t\t|   Password: ");
            int passCursorX = Console.CursorTop;
            int passCursorY = Console.CursorLeft;
            Console.Write("                                             |");
            Console.WriteLine("\n\t\t|==========================================================|");

            Console.SetCursorPosition(loginCursorY, loginCursorX);
            string username = Console.ReadLine();

            Console.SetCursorPosition(passCursorY, passCursorX);
            string password = "";

            //This loop hides the password input and replaces it with *. The input is added to the password input string
            do
            {
                string inputPwd = "*";
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password = password + key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        inputPwd = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            
            //The username and password are put through the LoginMatch method to check whether they are correct or not
            return accounts.LoginMatch(username, password);
        }

        //The next few methods will call on the methods in the Accounts class. These methods are called on through the
        //main page
        void CreateAccount()
        {
            accounts.CreateAccount();
        }

        void FindAccount()
        {
            accounts.FindAccount();
        }

        void Deposit()
        {
            accounts.Deposit();
        }

        void Withdraw()
        {
            accounts.Withdraw();
        }

        void Statement()
        {
            accounts.Statement();
        }

        void Delete()
        {
            accounts.Delete();
        }

        void MainPageInfo()
        {
            Console.Clear();
            Console.WriteLine("\t\t|==========================================================|");
            Console.WriteLine("\t\t|             WELCOME TO SIMPLE BANKING SYSTEM             |");
            Console.WriteLine("\t\t|==========================================================|");
            Console.WriteLine("\t\t|    1. Create a new account                               |");
            Console.WriteLine("\t\t|    2. Search for an account                              |");
            Console.WriteLine("\t\t|    3. Deposit                                            |");
            Console.WriteLine("\t\t|    4. Withdraw                                           |");
            Console.WriteLine("\t\t|    5. A/C statment                                       |");
            Console.WriteLine("\t\t|    6. Delete account                                     |");
            Console.WriteLine("\t\t|    7. Exit                                               |");
            Console.WriteLine("\t\t|==========================================================|");
            Console.Write("\t\t|    Enter your choice (1-7): ");
            int choiceCursorX = Console.CursorLeft;
            int choiceCursosY = Console.CursorTop;
            Console.Write("                             |");
            Console.WriteLine("\n\t\t|==========================================================|");
            Console.SetCursorPosition(choiceCursorX, choiceCursosY);
        }

        //This method will get the choice from the user for the main page and call on the correct method
        void MainPage()
        {
            try
            {
                //MainPageInfo method is called upon to display the ui and options for the user
                MainPageInfo();
                //the user's choice will be entered through userInput and converted to an int
                string userInput = Console.ReadLine();
                int input = Convert.ToInt32(userInput);
                //If the input isn't 7, the loop will continue
                while (input != 7)
                {
                    //switch statement using the input will call on the correct method based on the choices displayed to the user
                    switch (input)
                    {
                        case 1:
                            CreateAccount();
                            break;
                        case 2:
                            FindAccount();
                            break;
                        case 3:
                            Deposit();
                            break;
                        case 4:
                            Withdraw();
                            break;
                        case 5:
                            Statement();
                            break;
                        case 6:
                            Delete();
                            break;
                        //default is called if the choice does not match the options provided
                        default:
                            MainPageInfo();
                            userInput = Console.ReadLine();
                            input = Convert.ToInt32(userInput);
                            break;
                    }
                    MainPageInfo();
                    userInput = Console.ReadLine();
                    input = Convert.ToInt32(userInput);
                }
                //The loop only ends when the user enters 7 as their choice. At which point the debugging stops.
                //The program is set in the settings to close the console once the debugging ends
            }
            //If the user inputs an invalid choice such as a letter, the user is warned and allowed to enter their choice
            //again
            catch (FormatException e)
            {
                Console.WriteLine("\n\t\tYour choice was invalid, press any key to try again");
                Console.ReadKey();
                MainPage();
            } 

        }
    }
}
