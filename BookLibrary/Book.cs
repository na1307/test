namespace BookLibrary;

public sealed class Book {
    private Book(int id, string title, string author, string publisher, string description) {
        ID = id;
        Title = emptyCheck(title, "제목 없음");
        Author = emptyCheck(author, "저자 없음");
        Publisher = emptyCheck(publisher, "출판사 없음");
        Description = !string.IsNullOrEmpty(description) ? description : "(설명 없음)";
        IsBorrowed = false;

        string emptyCheck(string value, string message) => !string.IsNullOrEmpty(value) ? value : throw new ProgramException(message);
    }

    public int ID { get; }
    public string Title { get; }
    public string Author { get; }
    public string Publisher { get; }
    public string Description { get; }
    public bool IsBorrowed { get; private set; }

    public void Borrow() {
        if (IsBorrowed) throw new ProgramException("이미 빌렸음");

        IsBorrowed = true;
    }

    public void Return() {
        if (!IsBorrowed) throw new ProgramException("빌리지 않음");

        IsBorrowed = false;
    }

    public sealed class Builder {
        private int id;
        private string title;
        private string author;
        private string publisher;
        private string description;

        public Builder ID(int id) {
            this.id = id;

            return this;
        }

        public Builder Title(string title) {
            this.title = title;

            return this;
        }

        public Builder Author(string author) {
            this.author = author;

            return this;
        }

        public Builder Publisher(string publisher) {
            this.publisher = publisher;

            return this;
        }

        public Builder Description(string description) {
            this.description = description;

            return this;
        }

        public Book Build() => new(id, title, author, publisher, description);
    }
}