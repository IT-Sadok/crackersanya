
using LibraryApp.Models;

namespace LibraryApp.Builders
{
    public class BookBuilder
    {
        private Guid _id;

        private string _title = string.Empty;

        private string _author = string.Empty;

        private DateTime? _releaseDate;

        private string _code = string.Empty;

        private bool _isAvailable = true;

        public BookBuilder WithId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("Id is required");

            _id = id;

            return this;
        }

        public BookBuilder WithTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("Book title cannot be empty");

            _title = title;

            return this;
        }

        public BookBuilder WithAuthor(string author)
        {
            if (string.IsNullOrEmpty(author))
                throw new ArgumentNullException("Book author cannot be empty");

            _author = author;

            return this;
        }

        public BookBuilder WithReleaseDate(DateTime? releaseDate)
        {
            _releaseDate = releaseDate;

            return this;
        }

        public BookBuilder WithCode(string code)
        {
            _code = code;

            return this;
        }

        public BookBuilder WithIsAvailable(bool isAvailable)
        {
            _isAvailable = isAvailable;

            return this;
        }

        public Book Build()
        {
            return new Book
            {
                Id = _id,
                Title = _title,
                Author = _author,
                ReleaseDate = _releaseDate,
                Code = _code,
                IsAvailable = _isAvailable
            };
        }
    }
}
