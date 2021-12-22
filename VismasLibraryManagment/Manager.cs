using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;


namespace VismasLibraryManagment
{
    public static class Manager
    {
        public static Library GetJSonString(string path)
        {
            string jsonString = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Library>(jsonString);
        }

        public static void AddBook(ref Library existingLib, Book book)
        {
            AddBookToJson(ref existingLib, book, Library.path);
        }

        public static Book GetBookDataFromUser()
        {
            Book book = new Book();

            Console.WriteLine("Please enter name of the book: ");
            book.name = Console.ReadLine();
            Console.WriteLine("Please enter author of the book: ");
            book.author = Console.ReadLine();
            Console.WriteLine("Please enter category of the book: ");
            book.category = Console.ReadLine();
            Console.WriteLine("Please enter publication date of the book: ");
            book.publication_date = Console.ReadLine();
            Console.WriteLine("Please enter language of the book: ");
            book.language = Console.ReadLine();
            Console.WriteLine("Please enter ISBN of the book: ");
            book.ISBN = Console.ReadLine();

            return book;
        }

        public static bool isBookInputValid(Book book)
        {
            if (book.name == string.Empty ||
               book.author == string.Empty ||
               book.category == string.Empty ||
               book.publication_date == string.Empty ||
               book.language == string.Empty ||
               book.ISBN == string.Empty)
            {
                return false;
            }
            return true;
        }

        public static void AddBookToJson(ref Library existingLib, Book book, string path)
        {
            if (!File.Exists(path))
            {
                existingLib.AllBooks.Add(book);
                File.WriteAllText(path, JsonConvert.SerializeObject(existingLib.AllBooks, Formatting.Indented));
            }
            else
            {
                var library = GetJSonString(path);  
                library.AllBooks.Add(book);;
                File.WriteAllText(path, JsonConvert.SerializeObject(library, Formatting.Indented));
                JsonConvert.SerializeObject(library, Formatting.Indented);
            }
            Console.WriteLine("Book added!");
        }

        public static void RemoveBook(string name)
        {
            var library = GetJSonString(Library.path);
            var bookToRemove = new Book();
            bookToRemove = library.AllBooks.Find(r => r.name == name);

            if (bookToRemove != null)
            {
                library.AllBooks.Remove(bookToRemove);
                File.WriteAllText(Library.path, JsonConvert.SerializeObject(library, Formatting.Indented));
            } 
            else
            {
                Console.WriteLine("We don't have this book in a library yet.");
            }
        }

        public static void BorrowBook(string name, string whoBorrowed)
        {
            var library = GetJSonString(Library.path);
            var bookToBorrow = new Book();
            bookToBorrow = library.AllBooks.Find(r => r.name == name);
            ref var book = ref bookToBorrow;

            if (bookToBorrow != null)
            {
                if(bookToBorrow.IsBookAvailable == true)
                {
                    library.AllBooks.Find(r => r.IsBookAvailable = false);
                    book.BookTakenDate = DateTime.Now;
                    book.whoBorrowed = whoBorrowed;
                    File.WriteAllText(Library.path, JsonConvert.SerializeObject(library, Formatting.Indented));

                    Console.WriteLine($"You successfully checked out book {bookToBorrow.name}");
                }
                else
                {
                    Console.WriteLine("Book is taken.");
                }
            }
            else
            {
                Console.WriteLine("Book Title Couldn't be found!");
            }
        }

        public static void ReturnBook(string name)
        {
            var library = GetJSonString(Library.path);
            var bookToReturn = new Book();
            bookToReturn = library.AllBooks.Find(r => r.name == name);
            ref var book = ref bookToReturn;

            
            if (bookToReturn != null)
            {
                if (bookToReturn.IsBookAvailable != true)
                {
                    var returnDate = DateTime.Now;
                    var takenDate = bookToReturn.BookTakenDate;
                    var diff = ((returnDate.Year - takenDate.Year) * 12) + returnDate.Month - takenDate.Month;

                    if(diff >= 2)
                    {
                        Console.WriteLine("How dare you be late to return this book? As a punishment, you'll have to go sort books by color");
                    }

                    book.IsBookAvailable = true;
                    book.whoBorrowed = string.Empty;

                    File.WriteAllText(Library.path, JsonConvert.SerializeObject(library, Formatting.Indented));
                    Console.WriteLine($"You successfully returned book {bookToReturn.name}");
                }
                else
                {
                    Console.WriteLine("This book hasnt been taken.");
                }
            }
            else
            {
                Console.WriteLine("Book Title Couldn't be found!");
            }
        }

        public static void DisplayAllBooks()
        {
            var library = GetJSonString(Library.path);

            foreach(Book item in library.AllBooks.OrderBy(x=> x.name))
            {
                Console.WriteLine(item);
            }
        }

        public static void DisplayAllBooks(string filter)
        {
            var library = GetJSonString(Library.path);

            switch (filter)
            {
                case "name":
                    foreach (Book item in library.AllBooks.OrderBy(r => r.name))
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case "author":
                    foreach (Book item in library.AllBooks.OrderBy(r => r.author))
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case "category":
                    foreach (Book item in library.AllBooks.OrderBy(r => r.category))
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case "language":
                    foreach (Book item in library.AllBooks.OrderBy(r => r.language))
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case "availablebooks":
                    foreach (Book item in library.AllBooks.OrderBy(r => r.IsBookAvailable == true))
                    {
                        Console.WriteLine(item);
                    }
                    break;

                default:
                    Console.WriteLine("No such filter! Try again.");
                    break;
            }
        }
    }
}