using System.Collections.Generic;

namespace BookLibrary;

public class Container {
    private readonly List<Book> books;

    public static Container Instance { get; } = new();

    private Container() => books = new List<Book>();

    public Book[] GetAllBooks() => books.ToArray();

    public Book GetBook(int index) => books[index];

    public void AddBook(int id, string title, string author, string publisher, string description) => books.Add(new Book.Builder().ID(id).Title(title).Author(author).Publisher(publisher).Description(description).Build());

    public void AddBook(Book book) => books.Add(book);

    public void RemoveBook(int index) => books.RemoveAt(index);
}