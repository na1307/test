using static System.Console;

Book book = new(1, "심영의 모험", "김두한", 4000);

WriteLine(book);

book.Borrow();
WriteLine(book);

book.Return();
WriteLine(book);

Book book2 = new() {
    Id = 10,
    Title = "C# 프로그래밍",
    Author = "몰?루",
    Price = 5000,
    CloneId = 1
};

WriteLine(book2);

Book book3 = book2 with { CloneId = 2 };

WriteLine(book3);

#pragma warning disable S3903
#pragma warning disable CA1050
public record Book {
    public int Id { get; init; }
    public string Title { get; init; }
    public string Author { get; init; }
    public int Price { get; init; }
    public int CloneId { get; init; }
    public bool Borrowed { get; private set; }

    public Book() {
        Id = 0;
        Title = string.Empty;
        Author = string.Empty;
        Price = 0;
        CloneId = 1;
        Borrowed = false;
    }

    public Book(int id, string title, string author, int price) : this(id, title, author, price, 1) { }

    public Book(int id, string title, string author, int price, int cloneid) {
        Id = id;
        Title = title;
        Author = author;
        Price = price;
        CloneId = cloneid;
        Borrowed = false;
    }

    public void Borrow() {
        if (Borrowed) throw new InvalidOperationException("이미 빌렸음");

        Borrowed = true;
        WriteLine("대출 성공");
    }

    public void Return() {
        if (!Borrowed) throw new InvalidOperationException("빌려지지 않음");

        Borrowed = false;
        WriteLine("반납 성공");
    }
}