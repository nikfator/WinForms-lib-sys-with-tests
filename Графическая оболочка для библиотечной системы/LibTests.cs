using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Графическая_оболочка_для_библиотечной_системы.MainForm;

namespace Графическая_оболочка_для_библиотечной_системы
{
    [TestFixture]
    class LibTests
    {
        private string testFilePath = "test_books.txt";
        private Lib _library;

        [SetUp]
        public void Setup()
        {
            // Создаем временный файл для тестов
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }

            _library = new Lib(testFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            // Удаляем временный файл после тестов
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

        [Test]
        public void Add_to_lib_ShouldAddBook()
        {
            var book = new Book("TestTitle", "TestAuthor", 2020, 1);
            _library.Add_to_lib(book);

            var books = _library.GetBooks();
            Assert.That(books.Count, Is.EqualTo(1));
            Assert.That(books[0].Title, Is.EqualTo("TestTitle"));
        }

        [Test]
        public void List_of_lib_ShouldReturnBooksList()
        {
            var book1 = new Book("Title1", "Author1", 2021, 1);
            var book2 = new Book("Title2", "Author2", 2022, 2);
            _library.Add_to_lib(book1);
            _library.Add_to_lib(book2);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _library.List_of_lib();
                var output = sw.ToString();

                Assert.That(output.Contains("Title1"));
                Assert.That(output.Contains("Title2"));
            }
        }

        [Test]
        public void Search_in_lib_ShouldReturnCorrectBookInfo()
        {
            var book = new Book("SearchTitle", "SearchAuthor", 2023, 3);
            _library.Add_to_lib(book);

            var result = _library.Search_in_lib("SearchTitle");
            Assert.That(result.Contains("SearchTitle"));
            Assert.That(result.Contains("SearchAuthor"));
        }

        [Test]
        public void Delete_from_lib_ShouldRemoveBook()
        {
            var book = new Book("DeleteTitle", "DeleteAuthor", 2024, 4);
            _library.Add_to_lib(book);

            _library.Delete_from_lib("4");

            var books = _library.GetBooks();
            Assert.That(books.Count, Is.EqualTo(0));
        }

        [Test]
        public void Load_Books_From_File_ShouldLoadBooks()
        {
            File.WriteAllLines(testFilePath, new[]
            {
                "1 TestTitle1 TestAuthor1 2020",
                "2 TestTitle2 TestAuthor2 2021"
            });

            var newLibrary = new Lib(testFilePath);
            var books = newLibrary.GetBooks();

            Assert.That(books.Count, Is.EqualTo(2));
            //Assert.That( ,Is.EqualTo());
            Assert.That(books[0].Title, Is.EqualTo("TestTitle1"));
            Assert.That(books[1].Title, Is.EqualTo("TestTitle2"));
        }

        [Test]
        public void Save_Books_To_File_ShouldPersistBooks()
        {
            var book = new Book("PersistTitle", "PersistAuthor", 2025, 5);
            _library.Add_to_lib(book);

            var lines = File.ReadAllLines(testFilePath);
            Assert.That(lines.Length, Is.EqualTo(1));
            Assert.That(lines[0].Contains("PersistTitle"));
        }
    }

    [TestFixture]
    public class BookTests
    {
        [Test]
        public void FromString_ShouldParseValidString()
        {
            var bookString = "1 TestTitle TestAuthor 2020";
            var book = Book.FromString(bookString);

            Assert.That(book.Id, Is.EqualTo(1));
            Assert.That(book.Title, Is.EqualTo("TestTitle"));
            Assert.That(book.Author, Is.EqualTo("TestAuthor"));
            Assert.That(book.Year, Is.EqualTo(2020));
        }

        [Test]
        public void FromString_ShouldThrowExceptionOnInvalidString()
        {
            var invalidString = "InvalidString";
            Assert.Throws<FormatException>(() => Book.FromString(invalidString));
        }

        [Test]
        public void ToString_ShouldReturnValidFormat()
        {
            var book = new Book("ToStringTitle", "ToStringAuthor", 2022, 6);
            var result = book.ToString();

            Assert.That(result, Is.EqualTo("6 ToStringTitle ToStringAuthor 2022"));
        }
    }
}
