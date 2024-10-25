public class Member
{
    public int Id { get; private set; } 
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public int Age { get; set; } 
    public string Login { get; set; } 
    public string Password { get; set; } 
    public List<Book> BorrowedBooks { get; set; }

    public Member(int id, string firstName, string lastName, int age, string login, string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Login = login;
        Password = password;
        BorrowedBooks = new List<Book>(); 
    }
}