using System.Collections.Generic;

namespace BookLibrary;

public class Container {
    private readonly List<Book> books;
    private readonly List<User> users;

    public static Container Instance { get; } = new();

    private Container() {
        books = new List<Book>();
        users = new List<User>();
    }

    public List<Book> GetBooks() => books;

    public void AddBook(int id, string title, string author, string publisher, string description) => books.Add(new Book.Builder().ID(id).Title(title).Author(author).Publisher(publisher).Description(description).Build());

    public void AddBook(Book book) => books.Add(book);
}