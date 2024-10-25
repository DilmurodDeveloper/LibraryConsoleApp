public class Program
{
    private static Library library = new Library(); 

    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Library System");
        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");

            Console.Write("Choose: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Login(); 
                    break;
                case "2":
                    Register(); 
                    break;
                case "3":
                    return; 
                default:
                    Console.WriteLine("Invalid choice. Please try again."); 
                    break;
            }
        }
    }

    private static void Login()
    {
        Console.Write("Enter your login: ");
        string login = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();

        Member member = library.FindMember(login, password);
        if (member != null)
        {
            Console.WriteLine($"Welcome back, {member.FirstName}!");
            MemberMenu(member); 
        }
        else
        {
            Console.WriteLine("Invalid login or password.");
        }
    }

    private static void Register()
    {
        Console.Write("Enter your first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter your last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter your age: ");
        int age = int.Parse(Console.ReadLine());
        Console.Write("Enter your login: ");
        string login = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();

        Member newMember = new Member(library.GetNextMemberId(), firstName, lastName, age, login, password);
        library.AddMember(newMember); 
        Console.WriteLine("Registration successful!"); 
    }

    private static void MemberMenu(Member member)
    {
        while (true)
        {
            Console.WriteLine("\nMember Menu:");
            Console.WriteLine("1. View Books");
            Console.WriteLine("2. Borrow Book");
            Console.WriteLine("3. Return Book");
            Console.WriteLine("4. Show Borrowed Books and Members");
            Console.WriteLine("5. Logout");
            Console.Write("Choose: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    library.DisplayBooks(); 
                    break;
                case "2":
                    Console.Write("Enter the number of the book to borrow: ");
                    library.SelectBooks(member);
                    break;
                case "3":
                    library.ShowMemberBorrowedBooks(member);
                    if (member.BorrowedBooks.Count > 0) 
                    {
                        Console.Write("Enter the ID of the book to return: ");
                        library.ReturnBooks(member); 
                    } 
                    break;
                case "4":
                    library.ShowBorrowedBooks(); 
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again."); 
                    break;
            }
        }
    }
}