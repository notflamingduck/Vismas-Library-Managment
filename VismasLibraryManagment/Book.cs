using System;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace VismasLibraryManagment
{
    public class Book
    {
        public string name { get; set; }
        public string author { get; set; }
        public string category { get; set; }
        public string language { get; set; }
        public string publication_date { get; set; }
        public string ISBN { get; set; }
        public bool IsBookAvailable { get; set; } = true;
        public string whoBorrowed { get; set; }
        public DateTime BookTakenDate { get; set; }

        public override string ToString()
        {
            return string.Format("Title:\t{0}\nAuthor:\t{1}\nCategory:\t{2}\nLanguage:\t{3}\nPublication Date:\t{4}\nISBN:\t{5}\n", name, author, category, language, publication_date, ISBN);
        }
    }
}
