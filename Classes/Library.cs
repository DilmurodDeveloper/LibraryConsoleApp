using Newtonsoft.Json;

public class Library
{
    private List<Book> books;
    private List<Member> members;
    private const string BooksFilePath = "books.json";
    private const string MembersFilePath = "members.json";

    public Library()
    {
        books = LoadBooks();
        members = LoadMembers();
    }

    private List<Book> LoadBooks()
    {
        if (!File.Exists(BooksFilePath))
        {
            return new List<Book>();
        }

        var json = File.ReadAllText(BooksFilePath);
        return JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
    }

    private List<Member> LoadMembers()
    {
        if (!File.Exists(MembersFilePath))
        {
            return new List<Member>();
        }

        var json = File.ReadAllText(MembersFilePath);
        return JsonConvert.DeserializeObject<List<Member>>(json) ?? new List<Member>();
    }

    public void DisplayBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("No books available in the library.");
            return;
        }

        Console.WriteLine("Available Books:");
        for (int i = 0; i < books.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {books[i].Title} by {books[i].Author} (Available: {books[i].AvailableQuantity})");
        }
    }

    public void SelectBooks(Member member)
    {
        DisplayBooks();

        Console.Write("Enter the numbers of the books you want to borrow (comma-separated): ");
        string input = Console.ReadLine();
        var bookIds = input.Split(',');

        foreach (var id in bookIds)
        {
            if (int.TryParse(id.Trim(), out int bookIndex) && bookIndex > 0 && bookIndex <= books.Count)
            {
                Book selectedBook = books[bookIndex - 1];
                if (selectedBook.AvailableQuantity > 0)
                {
                    selectedBook.BorrowedBy.Add($"{member.FirstName} {member.LastName}"); 
                    selectedBook.AvailableQuantity--; 
                    member.BorrowedBooks.Add(selectedBook); 

                    Console.WriteLine($"You have successfully borrowed '{selectedBook.Title}' by {selectedBook.Author}.");
                }
                else
                {
                    Console.WriteLine($"Book '{selectedBook.Title}' by {selectedBook.Author} is currently unavailable.");
                }
            }
            else
            {
                Console.WriteLine($"Invalid book ID: {id.Trim()}"); 
            }
        }

        SaveBooks();
        SaveMembers();
    }

    public void ReturnBooks(Member member)
    {
        if (member.BorrowedBooks.Count == 0)
        {
            Console.WriteLine("You have no borrowed books to return.");
            return;
        }

        Console.WriteLine("Your borrowed books:");
        foreach (var book in member.BorrowedBooks)
        {
            Console.WriteLine($"- {book.Id}: '{book.Title}' by {book.Author}");
        }

        Console.Write("Enter the IDs of the books you want to return (comma-separated): ");
        string input = Console.ReadLine();
        var bookIds = input.Split(',');

        bool anyReturned = false;
        List<Book> booksToRemove = new List<Book>(); 

        foreach (var idStr in bookIds)
        {
            if (int.TryParse(idStr.Trim(), out int bookId))
            {
                Book bookToReturn = member.BorrowedBooks.Find(b => b.Id == bookId);
                if (bookToReturn != null)
                {
                    Book libraryBook = books.Find(b => b.Id == bookToReturn.Id);

                    if (libraryBook != null)
                    {
                        libraryBook.AvailableQuantity++;
                        libraryBook.BorrowedBy.Remove($"{member.FirstName} {member.LastName}"); 
                    }

                    booksToRemove.Add(bookToReturn); 
                    Console.WriteLine($"Successfully returned '{bookToReturn.Title}'.");
                    anyReturned = true;
                }
                else
                {
                    Console.WriteLine($"Book with ID {bookId} was not borrowed by you or does not exist.");
                }
            }
            else
            {
                Console.WriteLine($"Invalid book ID: {idStr}");
            }
        }

        foreach (var book in booksToRemove)
        {
            member.BorrowedBooks.Remove(book);
        }

        if (anyReturned)
        {
            SaveBooks();   
            SaveMembers(); 
        }
        else
        {
            Console.WriteLine("No valid books were returned.");
        }
    }



    public void ShowBorrowedBooks()
    {
        bool hasBorrowedBooks = false;
        Console.WriteLine("Borrowed Books:");
        foreach (var book in books)
        {
            if (book.BorrowedBy.Count > 0)
            {
                hasBorrowedBooks = true; 
                Console.WriteLine($"'{book.Title}' by {book.Author} is borrowed by: {string.Join(", ", book.BorrowedBy)}");
            }
        }
         if (!hasBorrowedBooks) 
        {
            Console.WriteLine("There are no bookings."); 
        }
    }

    public void ShowMemberBorrowedBooks(Member member)
    {
        if (member.BorrowedBooks.Count == 0) 
        {
            Console.WriteLine("You have not made a reservation.");
        }
    }

    public void AddMember(Member member)
    {
        members.Add(member); 
        SaveMembers(); 
    }

    public Member FindMember(string login, string password)
    {
        return members.Find(m => m.Login == login && m.Password == password);
    }

    private void SaveMembers()
    {
        File.WriteAllText(MembersFilePath, JsonConvert.SerializeObject(members, Formatting.Indented)); 
    }

    private void SaveBooks()
    {
        File.WriteAllText(BooksFilePath, JsonConvert.SerializeObject(books, Formatting.Indented)); 
    }
    
    public int GetNextMemberId()
    {
        return members.Count > 0 ? members[^1].Id + 1 : 1; 
    }
}