using Oiski.School.ProjectVault.App;
//  This program uses 'goto' statements because I thought it would be fun to try and it keeps the source-code shorter

start:  //  The beginning of the program (This is a reset point)

ConsoleHelper.BuildBaseLayout();

Console.WriteLine("Sign in to decrypt file");
string username = ConsoleHelper.GetStringInput("Username").ToLower();
string password = ConsoleHelper.GetStringInput("Passsword", hideInput: true);
Console.WriteLine();

if (username.StartsWith("new "))
{
    if (UserManager.StoreUser(username.Replace("new ", string.Empty), password))
    {
        Console.WriteLine("User Successfully stored!");
        Thread.Sleep(2000);
        goto start; //  Resets the menu
    }

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("User already exists!");
    Thread.Sleep(2000);
    Console.ResetColor();
    goto start; //  Resets the menu
}

if (!UserManager.SignInUser(username, password))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Password or username is incorrect!");
    Thread.Sleep(2000);
    goto start; //  Reset the menu
}

choiceMenu: //  Marks the beginning of the choice menu (This is a reset point)
string choice;

do
{
    ConsoleHelper.BuildBaseLayout();
    Console.WriteLine("What do you wanna do?");
    choice = ConsoleHelper.GetStringInput("[E] Encrypt | [D] Decrypt | [S] Switch Account").ToLower();

} while (string.IsNullOrWhiteSpace(choice) && (choice != "e" || choice != "d"));

if (choice == "s") { goto start; /* Resetting menu */ }

string fileData = null;
string filePath;
string fileName;

do
{
    ConsoleHelper.BuildBaseLayout(username);
    Console.WriteLine($"{((choice == "E") ? ("Encrypt") : ("Decrypt"))} File");
    filePath = ConsoleHelper.GetStringInput("File Path");
    fileName = ConsoleHelper.GetStringInput("File Name");
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

if (choice == "e")
{
    var crypto = new OiskiCipher();

    crypto.GenerateKeys();

    var handler = new FileHandler(filePath, $"Encrypted_{fileName}");
    //handler.WriteLine(OiskiHasher.HashPassword(password, username));
    handler.Write(crypto.Encrypt(fileData), append: true);
    UserManager.StoreKey(username, password, crypto.GetPrivateKeyAsXml());

    Console.Write("File Encrypted and stored at: ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write(handler.FilePath);
    Console.ResetColor();
}
else if (choice == "d")
{
    string keys = UserManager.RetrieveKeys(username, password);

    var cipherHandler = new FileHandler(filePath, fileName);

    var filePassword = cipherHandler.ReadLines()[0];
    var hashedPass = OiskiHasher.HashPassword(password, username);

    //if (keys != null && filePassword == hashedPass)
    //{
    var crypto = new OiskiCipher(keys);

    var ciphertext = cipherHandler.ReadAll().Replace($"{hashedPass}{Environment.NewLine}", string.Empty);

    var plaintext = crypto.Decrypt(ciphertext);

    var plaintextHandler = new FileHandler(filePath, $"Encrypted_{fileName}");
    plaintextHandler.WriteLine(plaintext);

    Console.Write("File Decrypted and stored at: ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write(plaintextHandler.FilePath);
    Console.ResetColor();
    //}

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("[!Access Denied!]");
    Thread.Sleep(2000);

    goto choiceMenu;    //  Reset menu
}

Console.Read();
