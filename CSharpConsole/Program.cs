using System.Diagnostics.CodeAnalysis;
using static System.Console;

Book book = new(1, "사딸라", "김두한", 4000);

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

WriteLine(book2 with { CloneId = 2 });

#pragma warning disable CA1050,S3903
public sealed record class Book {
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string Author { get; init; }
    public required int Price { get; init; }
    public required int CloneId { get; init; }
    public bool Borrowed { get; private set; } = false;

    public Book() { }

    [SetsRequiredMembers]
    public Book(int id, string title, string author, int price) : this(id, title, author, price, 1) { }

    [SetsRequiredMembers]
    public Book(int id, string title, string author, int price, int cloneid) {
        Id = id;
        Title = title;
        Author = author;
        Price = price;
        CloneId = cloneid;
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

namespace System.Runtime.CompilerServices {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RequiredMemberAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public sealed class CompilerFeatureRequiredAttribute : Attribute {
        public string FeatureName { get; }

        public CompilerFeatureRequiredAttribute(string featureName) => FeatureName = featureName;
    }
}

namespace System.Diagnostics.CodeAnalysis {
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class SetsRequiredMembersAttribute : Attribute { }
}