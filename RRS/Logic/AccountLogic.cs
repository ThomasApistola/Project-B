
public static class AccountLogic {
    public static void PrintAccounts(Accounts LoggedInAccount) {
        foreach (Accounts accounts in Database.SelectAccount()) {
            if (accounts.ID != LoggedInAccount.ID) {
                Console.WriteLine($"{accounts.ID} - {accounts.FirstName} {accounts.LastName} - Account level: {accounts.AccountLevel} - {Database.SelectAccountLevel(accounts.AccountLevel).Name}");
            }
        } 
    }

    public static string GetAccountsDisplay(Accounts LoggedInAccount) {
        string returnValue = "";
        foreach (Accounts accounts in Database.SelectAccount()) {
            if (accounts.ID != LoggedInAccount.ID) {
                returnValue += $"ID: {accounts.ID} - {accounts.FirstName} {accounts.LastName} - Account level: {accounts.AccountLevel} - {Database.SelectAccountLevel(accounts.AccountLevel).Name}\n";
            }
        }
        return returnValue;
    }

    public static Accounts GetSelectedAccount(Accounts LoggedInAccount, string input) {
        if (int.TryParse(input, out int output)) {
            foreach (Accounts accounts in Database.SelectAccount()) {
                if (accounts.ID == output && accounts.ID != LoggedInAccount.ID) {
                    return accounts;
                }
            }
        }
        return null;
    }

    public static Accounts GetSelectedAccount(int input) {
        foreach (Accounts accounts in Database.SelectAccount()) {
            if (accounts.ID == input) {
                return accounts;
            }
        }
        return null;
    }

    public static void PrintAccountLevels() {
        foreach (AccountLevel accountlevel in Database.SelectAccountLevel()) {
            Console.WriteLine($"{accountlevel.ID} - {accountlevel.Name}");
        }
    }

    public static void PrintAccounts() {
        foreach (Accounts accounts in Database.SelectAccount()) {
            Console.WriteLine($"{accounts.ID} - {accounts.FirstName} {accounts.LastName}");
        }
    }

    public static AccountLevel GetSelectedAccountlevel(string input) {
        if (int.TryParse(input, out int output)) {
            foreach (AccountLevel accountslevel in Database.SelectAccountLevel()) {
                if (accountslevel.ID == output) {
                    return accountslevel;
                }
            }
        }
        return null;
    }

    public static void ChangeAccountLevel(Accounts selectedAccount, AccountLevel selectedAccountLevel, Accounts LoggedInAccount) {
        if (Database.UpdateAccountLevelForAccount(selectedAccount, selectedAccountLevel, LoggedInAccount)) {
            Console.WriteLine($"Accountlevel for the account of {selectedAccount.FirstName} {selectedAccount.LastName} to level {selectedAccountLevel.ID} - {selectedAccountLevel.Name}, has succeeded");
        } else {
            Console.WriteLine($"There was an error trying to change the accountlevel for the account of {selectedAccount.FirstName} {selectedAccount.LastName}, please try it again later");
        }
    }

    public static bool CanDisplay(string lookupReqeust, Accounts LoggedInAccount) {
        AccountLevel LoggedInAccountsAccountLevel = Database.SelectAccountLevel(LoggedInAccount.AccountLevel);
        switch (lookupReqeust) {
            case "createAdmins":
                return LoggedInAccountsAccountLevel.CanCreateAdmins;
            case "cancelReservations":
                return LoggedInAccountsAccountLevel.CanCancelReservations;
            case "changeReservations":
                return LoggedInAccountsAccountLevel.CanChangeReservation;
            case "changeTimeslots":
                return LoggedInAccountsAccountLevel.CanChangeTimeSlots;
            case "logs":
                return LoggedInAccountsAccountLevel.CanViewLogs;
            default:
                return false;
        }
    }

    public static void SearchForUser_ByID(string userInput) {
        if (int.TryParse(userInput, out int output)) {
            Accounts returnedAccount = Database.SelectAccount(output);
            if (returnedAccount is not null) {
                Console.WriteLine(returnedAccount.FancyToString_WithAccountLevelName());
            } else {
                Console.WriteLine("No results, There were no accounts found with this ID.");
            }
        } else {
            Console.WriteLine("Invalid input, An ID can only consist of numbers. Please try it again with a valid number");
        }
    }

    public static void SearchForUser_ByFirstName(string userInput) {
        List<Accounts> returnedAccounts = Database.SelectAccount(userInput, "firstname", true);
        if (returnedAccounts is not null) {
            foreach (Accounts returnedAccount in returnedAccounts) {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine(returnedAccount.FancyToString_WithAccountLevelName_NoBorder());
                Console.WriteLine("---------------------------------------------------------------");
            }
        } else {
            Console.WriteLine("No results, There were no accounts found with this firstname.");
        }
    }

    public static void SearchForUser_ByLastName(string userInput) {
        List<Accounts> returnedAccounts = Database.SelectAccount(userInput, "lastname", true);
        if (returnedAccounts is not null) {
            foreach (Accounts returnedAccount in returnedAccounts) {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine(returnedAccount.FancyToString_WithAccountLevelName_NoBorder());
                Console.WriteLine("---------------------------------------------------------------");
            }
        } else {
            Console.WriteLine("No results, There were no accounts found with this lastname.");
        }
    }

    public static void SearchForUser_ByEmail(string userInput) {
        Accounts returnedAccount = Database.SelectAccount(userInput, "email");
        if (returnedAccount is not null) {
            Console.WriteLine(returnedAccount.FancyToString_WithAccountLevelName());
        } else {
            Console.WriteLine("No results, There were no accounts found with this e-mail.");
        }
    }
    
    public static bool CreateNewCustomerAccount(string Email, string Password, string FirstName, string LastName, string Gender, int Age, string PhoneNumber, string AccountLanguage, int Accountlevel)
    {
    return Database.Insert(new Accounts(Email, Password, FirstName, LastName, Gender, Age, PhoneNumber, "EN", 3));
    }
    public static void RemoveAccountFromSystem(int AccountLevelID) 
    {
        string ActionResult = Database.DeleteAccount(AccountLevelID);

        if (ActionResult != "invalid input") 
        {
            Console.WriteLine(ActionResult);
        } 
        else 
        {
            Console.WriteLine("There was an error while trying to delete the account, please try it again later");
        }
    }

    public static bool CheckCurrPassword(string input, Accounts LoggedInAccount) => Database.CheckAccountPassword(LoggedInAccount.ID, input);

    public static bool DoesAccountEmailExist(string inputEmail) => Database.DoesEmailAlreadyExist(inputEmail);

    public static int GetInfoSharedCode(Accounts AccountID1, Accounts AccountID2) {
        int resultCheck = Database.SelectSharedInfoCode(AccountID1.ID, AccountID2.ID);
        if (resultCheck == 999999999) {
            Database.InsertInformationShare(AccountID1, AccountID2);
            return Database.SelectSharedInfoCode(AccountID1.ID, AccountID2.ID);
        }
        return resultCheck;
    }

    public static void UpdateShareInfo(int AccountIDOne, int AccountIDTwo, int ShareInfoCode) => Database.UpdateShareInfo(AccountIDOne, AccountIDTwo, ShareInfoCode);

    // internal static bool CreateNewCustomerAccount(string email, string password, string firstName, string lastName, string gender, int age, string phoneNumber)
    // {
    //     throw new NotImplementedException();
    // }

}