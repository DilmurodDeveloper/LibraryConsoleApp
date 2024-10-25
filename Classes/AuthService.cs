public class AuthService
{
    private readonly Library library;

    public AuthService(Library library)
    {
        this.library = library;
    }

    public Member Login()
    {
        Console.Write("Enter your login: ");
        string login = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();

        Member member = library.FindMember(login, password);
        if (member != null)
        {
            Console.WriteLine($"Welcome back, {member.FirstName} {member.LastName}!");
            return member; 
        }
        else
        {
            Console.WriteLine("Invalid login or password. Please try again.");
            return null;
        }
    }

    public Member Register()
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
        Console.WriteLine($"Registration successful! Welcome, {firstName} {lastName}!");
        return newMember; 
    }
}