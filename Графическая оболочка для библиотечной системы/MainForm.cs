using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Графическая_оболочка_для_библиотечной_системы
{
    public partial class MainForm : Form
    {
        private Lib _library;
        public MainForm()
        {
            InitializeComponent();
            _library = new Lib("books.txt");
            LoadBooksToGrid();
        }
        private void LoadBooksToGrid()
        {
            dataGridView1.Rows.Clear();
            foreach (var book in _library.GetBooks())
            {
                dataGridView1.Rows.Add(book.Id, book.Title, book.Author, book.Year);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public class Lib
        {
            public List<Book> GetBooks()
            {
                return _books.ToList(); // Возвращаем копию списка
            }
            private List<Book> _books = new List<Book>();
            private string _filePath;
            public Lib(string _filePath)
            {
                this._filePath = _filePath;
                Load_Books_From_File();
            }
            public void Add_to_lib(Book _book)
            {
                _books.Add(_book);
                Save_Books_To_File();
                Console.WriteLine("Книга успешно добавлена!");
            }
            public void List_of_lib()
            {
                if (_books.Count == 0)
                {
                    Console.WriteLine("Библиотека пуста.");
                }
                else
                {
                    Console.WriteLine("Список книг в библиотеке:");
                    _books.ForEach(book => Console.WriteLine($"ID: {book.Id}, Название: {book.Title}, Автор: {book.Author}, Год издания: {book.Year}"));
                }
            }

            public string Search_in_lib(string _stroka)
            {
                string _result = "";
                foreach (var book in _books)
                {
                    if (book.Title == _stroka || book.Author == _stroka)
                    {
                        Console.WriteLine("Результат поиска:");
                        _result = ($"ID: {book.Id}, Название: {book.Title}, Автор: {book.Author}, Год издания: {book.Year}");
                    }
                }
                return _result;
            }
            public void Delete_from_lib(string _stroka)
            {
                try
                {
                    int id = Convert.ToInt32(_stroka);
                    var _removebook = _books.Find(_book => _book.Id == id);
                    if (_removebook != null)
                    {
                        _books.Remove(_removebook);
                        Save_Books_To_File();
                        Console.WriteLine("Книга удалена.");
                    }
                    else
                    {
                        Console.WriteLine("Указанная книга не найдена.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: ID книги должен быть числом.");
                }
            }
            private void Save_Books_To_File()
            {
                File.WriteAllLines(_filePath, _books.ConvertAll(_book => _book.ToString()));
            }
            private void Load_Books_From_File()
            {
                if (File.Exists(_filePath))
                {
                    string[] _lines = File.ReadAllLines(_filePath);
                    foreach (var _line in _lines)
                    {
                        try
                        {
                            _books.Add(Book.FromString(_line));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Ошибка загрузки книги: {ex.Message}");
                        }
                    }
                }
            }
        }
        public class Book
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public int Year { get; set; }
            public int Id { get; set; }
            public Book(string _title, string _author, int _year, int _id)
            {
                Title = _title;
                Author = _author;
                Year = _year;
                Id = _id;
            }
            public override string ToString()
            {
                return $"{Id} {Title} {Author} {Year}";
            }
            public static Book FromString(string data)
            {
                var _parts = data.Split(' ');
                if (_parts.Length == 4)
                {
                    return new Book(_parts[1], _parts[2], Convert.ToInt32(_parts[3]), Convert.ToInt32(_parts[0]));
                }
                throw new FormatException("Некорректный формат данных книги.");
            }
        }

        private void Load_Data_button_Click(object sender, EventArgs e)
        {
            LoadBooksToGrid();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string _title = titleTextBox.Text;
            string _author = authorTextBox.Text;
            int _year = int.Parse(yearTextBox.Text);
            int _id = int.Parse(idTextBox.Text);

            Book _newBook = new Book(_title, _author, _year, _id);
            _library.Add_to_lib(_newBook);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            string _id = deleteTextBox.Text;
            _library.Delete_from_lib(_id);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string _search = searchTextBox.Text;
            infoForBook.Text = _library.Search_in_lib(_search);
        }
    }
}
