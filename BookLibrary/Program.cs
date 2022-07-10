global using BookLibrary;
global using System;
using static System.Console;

int bookIndex = 1;
int userIndex = 0;

while (true) {
    WriteLine();

    WriteLine("1. 책 추가");
    WriteLine("2. 책 목록");

    WriteLine("\r\n0. 끝내기");

    Write("\r\n선택 : ");

    switch (int.Parse(ReadLine())) {
        case 0:
            return;

        case 1:
            AddBook();
            break;

        case 2:
            BookList();
            break;

        default:
            continue;
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
    foreach (var book in Container.Instance.GetBooks()) {
        WriteLine();

        WriteLine("책 번호 : " + book.ID);
        WriteLine("책 제목 : " + book.Title);
        WriteLine("책 저자 : " + book.Author);
        WriteLine("책 출판사 : " + book.Publisher);
        WriteLine("책 설명 : " + book.Description);
    }
}