using Oiski.School.ProjectVault.App;
using System.IO;

const string SLASH_PLACEHOLDER = "!_^_!";

//  This program uses 'goto' statements because I thought it would be fun to try and it keeps the source-code shorter (Slightly more confusing as well...)

program_Start:  //  The beginning of the program (This is a reset point)

#region INITIAL SIGN IN PROMPT
ConsoleHelper.BuildBaseLayout();

Console.WriteLine("Sign in to decrypt file");
string username = ConsoleHelper.GetStringInput("Username").ToLower();
string password = ConsoleHelper.GetStringInput("Passsword", hideInput: true);
Console.WriteLine();
#endregion

#region CREATE NEW USER
if (username.StartsWith("new "))
{
    if (UserManager.StoreUser(username.Replace("new ", string.Empty), password))
    {
        Console.WriteLine("User Successfully stored!");
        Thread.Sleep(2000);
        goto program_Start; //  Resets the menu
    }

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("User already exists!");
    Thread.Sleep(2000);
    Console.ResetColor();
    goto program_Start; //  Resets the menu
}
#endregion

#region SIGN IN
if (!UserManager.SignInUser(username, password))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Password or username is incorrect!");
    Thread.Sleep(2000);
    goto program_Start; //  Reset the menu
}
#endregion

#region CHOICE MENU
choice_Menu: //  Marks the beginning of the choice menu (This is a reset point)
string choice;

do
{
    ConsoleHelper.BuildBaseLayout(username);
    Console.WriteLine("What do you wanna do?");
    choice = ConsoleHelper.GetStringInput("[E] Encrypt | [D] Decrypt | [S] Switch Account").ToLower();

} while (string.IsNullOrWhiteSpace(choice) && (choice != "e" || choice != "d"));

if (choice == "s") { goto program_Start; /* Resetting menu */ }
#endregion

#region FILE FETCH
string fileData = null;
string filePath;
string fileName;

do
{
    ConsoleHelper.BuildBaseLayout(username);
    Console.WriteLine($"{((choice == "e") ? ("Encrypt") : ("Decrypt"))} File");
    filePath = ConsoleHelper.GetStringInput("File Path");
    fileName = ConsoleHelper.GetStringInput($"File Name {((choice == "e") ? (string.Empty) : ("(Without hash)"))}");

    Console.WriteLine();

    if (!string.IsNullOrWhiteSpace(filePath))
    {
        try
        {
            fileData = File.ReadAllText($"{filePath}/{fileName}");
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Something went wrong... Try again!");
            Thread.Sleep(1000);
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("You must provide a path");
        Thread.Sleep(1000);
    }

} while (fileData == null);
#endregion

#region ENCRYPT
if (choice == "e")
{
    var crypto = new OiskiCipher();

    crypto.GenerateKeys();
    var hashPass = OiskiHasher.HashPassword(password, username).Replace("/", SLASH_PLACEHOLDER);
    string fullPath = $"{filePath}/{hashPass}_{fileName}";

    File.WriteAllBytes(fullPath, crypto.Encrypt(fileData));
    UserManager.StoreKey(username, password, crypto.GetPrivateKeyAsXml());

    Console.Write("File Encrypted and stored at: ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write(fullPath);
    Console.ResetColor();
    Thread.Sleep(2000);

    goto choice_Menu;   //  Reset menu
}
#endregion
#region DECRYPT
else if (choice == "d")
{
    string keys = UserManager.RetrieveKeys(username, password);

    var hashedPass = OiskiHasher.HashPassword(password, username).Replace("/", SLASH_PLACEHOLDER);

    var path = $"{filePath}/{hashedPass}_{fileName}";

    if (keys != null && File.Exists(path))
    {
        var crypto = new OiskiCipher(keys);

        byte[] ciphertext = File.ReadAllBytes($"{filePath}/{hashedPass}_{fileName}");

        var plaintext = crypto.Decrypt(ciphertext);

        var plaintextHandler = new FileHandler(filePath, $"Decrypted_{fileName}");
        plaintextHandler.WriteLine(plaintext);

        Console.Write("File Decrypted and stored at: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(plaintextHandler.FilePath);
        Console.ResetColor();
        Thread.Sleep(3000);

        goto choice_Menu;    //  Reset Menu
    }

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("[!Access Denied!]");
    Thread.Sleep(3000);

    goto choice_Menu;    //  Reset menu
}
#endregion

Console.Read();
