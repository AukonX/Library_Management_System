using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class BookDetails
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Isbn { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int Pages { get; set; }

        public string AuthorName { get; set; }
        public DateOnly AuthorBirthDate { get; set; }
        public DateOnly? AuthorDeathDate { get; set; }
        public string AuthorCountry { get; set; }

        public string GenreName { get; set; }
        public string GenreDescription { get; set; }

        public BookDetails(string title, string description, string isbn, DateOnly releaseDate, int pages, string authorName, DateOnly authorBirthDate, DateOnly? authorDeathDate, string authorCountry, string genreName, string genreDescription)
        {
            Title = title;
            Description = description;
            Isbn = isbn;
            ReleaseDate = releaseDate;
            Pages = pages;
            AuthorName = authorName;
            AuthorBirthDate = authorBirthDate;
            AuthorDeathDate = authorDeathDate;
            AuthorCountry = authorCountry;
            GenreName = genreName;
            GenreDescription = genreDescription;
        }

        public override string ToString()
        {
            return $"{Title} || {AuthorName} || {Isbn} || {ReleaseDate.ToShortDateString()} || {GenreName}";
        }

        public string GetDetails()
        {
            string output = $"Dane o książce: \nTytuł: {Title} \nOpis: {Description} \n ISBN: {Isbn} \n" +
                $"Data Wydania: {ReleaseDate.ToShortDateString()} \nIlość stron: {Pages} \n \n" +
                $"Dane o autorze: \nImię: {AuthorName} \nData Urodzenia: {AuthorBirthDate.ToShortDateString()} \n" +
                $"Data śmierci: {AuthorDeathDate.ToString()} \nNarodowość: {AuthorCountry} \n\n" +
                $"Dane o gatunku: \nGatunek: {GenreName} \nOpis: {GenreDescription}";

            return output;
        }
    }
}
