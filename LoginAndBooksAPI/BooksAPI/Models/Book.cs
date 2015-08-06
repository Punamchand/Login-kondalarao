using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Models
{
    public class Book
    {
        public int ID { get; set; }
        public String BookName { get; set; }
        public String AuthorName { get; set; }
        public String ISBN { get; set; }
    }
}
