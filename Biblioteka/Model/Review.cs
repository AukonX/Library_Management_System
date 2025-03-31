using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class Review
    {
        public int Id { get; set; }
        public int Rating {  get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string Isbn { get; set; }
        public string Login { get; set; }

        public Review(int id, int rating, string title, string content, DateTime time, string isbn, string login)
        {
            Id = id;
            Rating = rating;
            Title = title;
            Content = content;
            Time = time;
            Isbn = isbn;
            Login = login;
        }

        public override string ToString()
        {
            return $"{Title} || {Rating} || {Content} || {Time} || {Isbn} || {Login}";
        }

        public string GetDetails()
        {
            return $"{Title}\n {Content}\n {Time}\n\n";
        }
    }
}
