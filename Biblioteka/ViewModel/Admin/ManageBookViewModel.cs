using Biblioteka.DataAccess;
using Biblioteka.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using test_MVVM.ViewModel;
using ZstdSharp.Unsafe;

namespace Biblioteka.ViewModel.Admin
{
    class ManageBookViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void ReloadData()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllAuthors)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllGenres)));
        }

        private bool _isEnabled = true;
        public bool isEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isEnabled)));
            }
        }

        public List<Model.Book> AllBooks
        {
            get
            {
                List<Book> temp = DataAccess.BookRepository.GetAllBooks();
                temp.Insert(0, new Book(null, "Nowa Książka", null, DateTime.Now, 0));
                return temp;
            }
        }

        private List<string> AllIsbn
        {
            get
            {
                List<string> temp = new List<string>();
                foreach(var book in AllBooks)
                {
                    temp.Add(book.Isbn);
                }
                return temp;
            }
        }

        private Model.Book _allBooksSelectedItem;
        public Model.Book AllBooksSelectedItem
        {
            get
            {
                return _allBooksSelectedItem;
            }

            set
            {
                _allBooksSelectedItem = value;
                if (value != null && value.Isbn != null)
                {
                    _allAuthorsSelectedIndex = DataAccess.AuthorRepository.GetBookAuthor(value.Isbn).Id - 1 ?? default(int);
                    _allGenresSelectedIndex = DataAccess.GenreRepository.GetBookGenres(value.Isbn).Id - 1 ?? default(int);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllAuthorsSelectedIndex)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllGenresSelectedIndex)));

                    BookTitle = value.Title;
                    Bookisbn = value.Isbn;
                    Bookdesc = value.Description;
                    PagesCount = value.Pages;
                    ReleaseDate = value.ReleaseDate;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooksSelectedItem)));

            }
        }

        private int _allBooksSelectedIndex;
        public int AllBooksSelectedIndex
        {
            get
            {
                return _allBooksSelectedIndex;
            }
            set
            {
                _allBooksSelectedIndex = value;
                if(value != 0)
                {
                    isEnabled = false;
                }
                else
                {
                    isEnabled = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooksSelectedIndex)));
            }
        }

        public List<Author> AllAuthors
        {
            get
            {
                return DataAccess.AuthorRepository.GetAllAuthors();
            }
        }

        public List<Genre> AllGenres
        {
            get
            {
                return DataAccess.GenreRepository.GetAllGenres();
            }
        }

        private int _allAuthorsSelectedIndex;
        public int AllAuthorsSelectedIndex
        {
            get
            {
                return _allAuthorsSelectedIndex;
            }
            set
            {
                _allAuthorsSelectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllAuthorsSelectedIndex)));
            }
        }

        private int _allGenresSelectedIndex = 0;
        public int AllGenresSelectedIndex
        {
            get
            {
                return _allGenresSelectedIndex;
            }
            set
            {
                _allGenresSelectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllGenresSelectedIndex)));
            }
        }

        private string _bookTitle;
        public string BookTitle
        {
            get
            {
                return _bookTitle;
            }
            set
            {
                _bookTitle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BookTitle)));
            }
        }

        private string _bookisbn;
        public string Bookisbn
        {
            get { return _bookisbn; }
            set
            {
                _bookisbn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Bookisbn)));
            }
        }

        private string _bookdesc;
        public string Bookdesc
        {
            get
            {
                return _bookdesc;
            }
            set
            {
                _bookdesc = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Bookdesc)));
            }
        }

        private int _pagesCount;
        public int PagesCount
        {
            get => _pagesCount;
            set
            {
                _pagesCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PagesCount)));
            }
        }

        private DateTime _releaseDate;
        public DateTime ReleaseDate
        {
            get { return _releaseDate; }
            set
            {
                _releaseDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReleaseDate)));
            }
        }

        private string _errorLabel;
        public string ErrorLabel
        {
            get
            {
                return _errorLabel;
            }

            set
            {
                _errorLabel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLabel)));
            }
        }

        private string _successLabel;
        public string SuccessLabel
        {
            get
            {
                return _successLabel;
            }

            set
            {
                _successLabel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SuccessLabel)));
            }
        }

        private ApiConnection _ac = new ApiConnection();

        private ICommand _save;
        public ICommand Save
        {
            get
            {
                if (_save == null)
                {
                    _save = new RelayCommand(
                        (o) =>
                        {
                            if(_allBooksSelectedIndex == 0 && !AllIsbn.Contains(_bookisbn))
                            {
                                Book temp = new Book(_bookisbn, _bookTitle, _bookdesc, _releaseDate, _pagesCount);
                                string? error = DataAccess.BookRepository.AddBook(temp);
                                string? error2 = DataAccess.BookRepository.AddBookAuthor(_bookisbn, _allAuthorsSelectedIndex + 1);
                                string? error3 = DataAccess.BookRepository.AddBookGenre(_bookisbn, AllGenres[_allGenresSelectedIndex].Id ?? default(int));

                                if (error != null)
                                {
                                    ErrorLabel = error;
                                }
                                else
                                {
                                    if (error2 != null)
                                    {
                                        ErrorLabel = error2;
                                    }
                                    else
                                    {
                                        if (error3 != null)
                                        {
                                            ErrorLabel = error3;
                                        }
                                        else
                                        {
                                            ErrorLabel = "";
                                            SuccessLabel = "Książka zmodyfikowana pomyślnie!";
                                            Task.Delay(3000).ContinueWith(_ => { SuccessLabel = ""; });
                                        }
                                    }
                                }

                                int previousIndex = _allBooksSelectedIndex;
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooks)));
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllIsbn)));
                                _allBooksSelectedIndex = previousIndex == 0 ? AllBooks.Count - 1 : previousIndex;
                            }
                            else
                            {
                                Book temp = new Book(_bookisbn, _bookTitle, _bookdesc, _releaseDate, _pagesCount);
                                string? error = DataAccess.BookRepository.ModifyBook(temp);
                                string? error2 = DataAccess.BookRepository.UpdateBookAuthor(_bookisbn, _allAuthorsSelectedIndex + 1);
                                string? error3 = DataAccess.BookRepository.UpdateBookGenre(_bookisbn, AllGenres[_allGenresSelectedIndex].Id ?? default(int));

                                if (error != null)
                                {
                                    ErrorLabel = error;
                                }
                                else
                                {
                                    if (error2 != null)
                                    {
                                        ErrorLabel = error2;
                                    }
                                    else
                                    {
                                        if (error3 != null)
                                        {
                                            ErrorLabel = error3;
                                        }
                                        else
                                        {
                                            ErrorLabel = "";
                                            SuccessLabel = "Książka zmodyfikowana pomyślnie!";
                                            Task.Delay(3000).ContinueWith(_ => { SuccessLabel = ""; });
                                        }
                                    }
                                }

                                int previousIndex = AllBooksSelectedIndex;
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooks)));
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllIsbn)));
                                _allBooksSelectedIndex = previousIndex == 0 ? AllBooks.Count - 1 : previousIndex;
                            }    
                        },
                        (o) =>
                        {
                        if (_bookTitle != null && _bookisbn != null && _bookdesc != null && _pagesCount != null)
                            {
                                return true;
                            }
                        else
                            return false;
                        }

                        );
                }  
                return _save;
            }
        }

        private ICommand _delete;
        public ICommand Delete
        {
            get
            {
                if( _delete == null )
                {
                    _delete = new RelayCommand(
                        (o) =>
                        {
                            DataAccess.BookRepository.DeleteBooks(_bookisbn);
                            int previousIndex = AllBooksSelectedIndex;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooks)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllIsbn)));
                            _allBooksSelectedIndex = previousIndex == 1 ? AllBooks.Count - 1 : previousIndex;
                        },
                        (o) =>
                        {
                            if(_allBooksSelectedIndex == 0)
                            {
                                return false;
                            }
                            else
                            { return true; }
                        }
                    );
                }
                return _delete;
            }
        }

        private ICommand _importFromApi;
        public ICommand ImportFromApi
        {
            get
            {
                if(_importFromApi == null)
                {
                    _importFromApi = new RelayCommand(
                        async (o) =>
                        {
                            BookData? bookdata = await _ac.GetVolumeInfoDataAsync(_bookisbn);
                            if(bookdata != null )
                            {
                                _bookTitle = bookdata?.items?[0].VolumeInfo?.Title;
                                //_releaseDate = DateTime.Parse(bookdata?.items?[0].VolumeInfo?.publishedDate);
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BookTitle)));
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReleaseDate)));

                            }
                        },
                        (o) =>
                        {
                            if (_bookisbn != null && _allBooksSelectedIndex == 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        );
                }
                return _importFromApi;
            }
        }
    }
}
