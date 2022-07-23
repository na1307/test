global using System;
using BookLibrary;
using static System.Console;

int bookIndex = 1;

while (true) {
    WriteLine();

    WriteLine("1. 책 추가");
    WriteLine("2. 책 목록");
    WriteLine("3. 책 제거");

    WriteLine("\r\n0. 끝내기");

    Write("\r\n선택 : ");

    try {
        switch (int.Parse(ReadLine())) {
            case 0:
                return;

            case 1:
                AddBook();
                break;

            case 2:
                BookList();
                break;

            case 3:
                RemoveBook();
                break;

            default:
                OutOfLange();
                continue;
        }
    } catch (FormatException) {
        OutOfLange();
    }
}

void AddBook() {
    WriteLine("\r\n" + bookIndex + "번째 책");

    Write("책 제목 : ");
    var title = ReadLine();

    Write("책 저자 : ");
    var author = ReadLine();

    Write("책 출판사 : ");
    var publisher = ReadLine();

    Write("책 설명 : ");
    var desctiption = ReadLine();

    Container.Instance.AddBook(bookIndex, title, author, publisher, desctiption);

    bookIndex++;
}

void BookList() {
    foreach (var book in Container.Instance.GetAllBooks()) {
        WriteLine();

        WriteLine("책 번호 : " + book.ID);
        WriteLine("책 제목 : " + book.Title);
        WriteLine("책 저자 : " + book.Author);
        WriteLine("책 출판사 : " + book.Publisher);
        WriteLine("책 설명 : " + book.Description);
    }
}

void RemoveBook() {
    Write("\r\n제거할 책의 번호를 입력 : ");

    try {
        Container.Instance.RemoveBook(int.Parse(ReadLine()) - 1);
    } catch (FormatException) {
        WriteLine("숫자만 입력하세요");
    }
}

void OutOfLange() => WriteLine("\r\n0부터 3까지의 숫자를 입력하세요");