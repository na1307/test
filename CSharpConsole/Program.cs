using System.Diagnostics.CodeAnalysis;
using static System.Console;

WriteLine("1. Book");
WriteLine("2. 가위 바위 보");

while (true) {
    Write("\r\n선택 : ");

    var line = ReadLine();

    WriteLine();

    switch (line) {
        case "1":
            booktest();
            return;

        case "2":
            rps();
            return;

        default:
            continue;
    }
}

void booktest() {
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
}

void rps() {
    while (true) {
        WriteLine("가위 바위 보\r\n");

        WriteLine("1. 가위");
        WriteLine("2. 바위");
        WriteLine("3. 보");

        WriteLine();
        WriteLine("0. 끝내기");

        Write("\r\n당신의 선택은? : ");

        try {
            var input = int.Parse(ReadLine()!);

            if (input is < 0 or > 3) continue;

            if (input == 0) break;

            var choice = (you: (RockPaperScissors)input, com: (RockPaperScissors)Random.Shared.Next(1, 4));

            WriteLine($"\r\n당신 {choice.you} / {choice.com} 컴퓨터\r\n\r\n" + choice switch {
                (RockPaperScissors.가위, RockPaperScissors.보) => true,
                (RockPaperScissors.보, RockPaperScissors.가위) => false,
                _ => choice.you != choice.com ? choice.you > choice.com : (bool?)null
            } switch {
                true => "당신이 이겼습니다!",
                false => "컴퓨터가 이겼습니다!",
                null => "비겼습니다!"
            } + "\r\n");
        } catch (Exception) {
            //
        }
    }
}

#pragma warning disable CA1050, S3903
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

public enum RockPaperScissors {
    가위 = 1,
    바위,
    보
}
#pragma warning restore

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