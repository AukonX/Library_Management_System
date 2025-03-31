using Biblioteka.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using test_MVVM.ViewModel;

namespace Biblioteka.ViewModel.User
{
    internal class UserMiscViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region List of authors

        #region Properties

        private bool _toFilter = false;

        private List<Author> _allAuthorsList = DataAccess.AuthorRepository.GetAllAuthors();
        public List<Author> AllAuthorsList
        {
            get
            {
                return _allAuthorsList;
            }
            set
            {
                _allAuthorsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllAuthorsList)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AuthorsOutput)));
            }
        }

        private List<string> _authorsOutput;
        public List<string> AuthorsOutput
        {
            get
            {
                if (_toFilter == false)
                {
                    string temp;
                    List<string> output = new List<string>();
                    foreach (Author author in _allAuthorsList)
                    {
                        temp = $"{author.Name} || {author.Country} || {author.BirthDate} || {author.DeathDate}";
                        output.Add(temp);
                    }
                    return output;
                }
                else
                {
                    List<string> output = FilterAuthors(_allAuthorNationalities[_selectedNationalityID]);
                    return output;
                }
                
            }
        }

        private int _selectedNationalityID = 0;
        public int SelectedNationalityID
        {
            get
            {
                return _selectedNationalityID;
            }
            set
            {
                _selectedNationalityID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNationalityID)));
            }
        }

        private List<string> _allAuthorNationalities = DataAccess.AuthorRepository.GetAuthorsNatiotalities();
        public List<string> AllAuthorNationalities
        {
            get
            {
                return _allAuthorNationalities;
            }
        }

        private List<string> FilterAuthors(string Key)
        {
            List<string> temp = new List<string>();
            foreach (Author author in _allAuthorsList)
            {
                if(author.Country == Key)
                {
                    temp.Add($"{author.Name} || {author.Country} || {author.BirthDate} || {author.DeathDate}");
                }
            }

            return temp;
        }


        #endregion

        #region Commands

        private ICommand _filter;
        public ICommand Filter
        {
            get
            {
                if (_filter == null)
                {
                    _filter = new RelayCommand(
                        (o) =>
                        {
                            _toFilter = true;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AuthorsOutput)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                }
                return _filter;
            }
        }

        private ICommand _unFilter;
        public ICommand UnFilter
        {
            get
            {
                if (_unFilter == null)
                {
                    _unFilter = new RelayCommand(
                        (o) =>
                        {
                            _toFilter = false;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AuthorsOutput)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                }
                return _unFilter;
            }
        }


        private ICommand _sortCountry;
        public ICommand SortCountry
        {
            get
            {
                if (_sortCountry == null)
                {
                    _sortCountry = new RelayCommand(
                        (o) =>
                        {
                            List<Author> temp = _allAuthorsList;
                            temp.Sort((x, y) => x.Country.CompareTo(y.Country));
                            _allAuthorsList = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AuthorsOutput)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                }
                return _sortCountry;
            }
        }

        private ICommand _sortName;
        public ICommand SortName
        {
            get
            {
                if (_sortName == null)
                {
                    _sortName = new RelayCommand(
                        (o) =>
                        {
                            List<Author> temp = _allAuthorsList;
                            temp.Sort((x, y) => x.Name.CompareTo(y.Name));
                            _allAuthorsList = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AuthorsOutput)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                }
                return _sortName;
            }
        }

        private ICommand _sortBirthDate;
        public ICommand SortBirthDate
        {
            get
            {
                if (_sortBirthDate == null)
                {
                    _sortBirthDate = new RelayCommand(
                        (o) =>
                        {
                            List<Author> temp = _allAuthorsList;
                            temp.Sort((x, y) => x.BirthDate.CompareTo(y.BirthDate));
                            _allAuthorsList = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AuthorsOutput)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                }
                return _sortBirthDate;
            }
        }

        #endregion

        #endregion

        #region List of books

        private bool _bookFilter = false;

        #region Properties

        private List<BookDetails> _allBookDetails = DataAccess.BookDetialsRepository.GetAllBookDetails();
        public List<BookDetails> AllBookDetails
        {
            get
            {
                return _allBookDetails;
            }
            set
            {
                _allBookDetails = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBookDetails)));
            }
        }

        public List<BookDetails> OutBookDetials
        {
            get
            {
                if( _bookFilter == false)
                {
                    List<BookDetails> output = new List<BookDetails>();
                    foreach(BookDetails book in _allBookDetails)
                    {
                        output.Add(book);
                    }
                    return output;
                }
                else
                {
                    List<BookDetails> output = new List<BookDetails>();
                    output = BookFilter();
                    return output;
                }    
            }
        }

        #region BookFilrer
        private List<BookDetails> BookFilter()
        {
            List<BookDetails> filtered = new List<BookDetails>();
            switch (_filterGroup)
            {
                case 0:
                    foreach (var book in _allBookDetails)
                    {
                        if (book.AuthorName == FilterCondition[_filterKey])
                        {
                            filtered.Add(book);
                        }
                    }
                    break;
                case 1:
                    foreach (var book in _allBookDetails)
                    {
                        if (book.GenreName == FilterCondition[_filterKey])
                        {
                            filtered.Add(book);
                        }
                    }
                    break;
            }


            return filtered;
        }

        public List<string> FilterOptions
        {
            get
            {
                List<string> output = new List<string>();
                output.Add("Autorze");
                output.Add("Gatunku");
                return output;
            }
        }

        private int _filterGroup = 0;
        public int FilterGroup
        {
            get { return _filterGroup; }
            set
            {
                _filterGroup = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterGroup)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterCondition)));
                _filterKey = 0;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterKey)));
            }
        }

        private List<string> _filterCondition;
        public List<string> FilterCondition
        {
            get
            {
                List<string> output = new List<string>();
                switch (_filterGroup)
                {
                    case 0:
                        foreach (var book in _allBookDetails)
                        {
                            if (!output.Contains(book.AuthorName))
                            {
                                output.Add(book.AuthorName);
                            }
                        }
                        break;
                    case 1:
                        foreach (var book in _allBookDetails)
                        {
                            if (!output.Contains(book.GenreName))
                            {
                                output.Add(book.GenreName);
                            }
                        }
                        break;
                }
                return output;
            }
        }

        private int _filterKey = 0;
        public int FilterKey
        {
            get
            {
                return _filterKey;
            }
            set
            {
                _filterKey = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterKey)));

            }
        }

        #endregion

        private int _selectedBookIndex = -1;
        public int SelectedBookIndex
        {
            get { return _selectedBookIndex; } 
            set
            {
                _selectedBookIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBookIndex)));
            }
        }

        #endregion

        #region Commands

        private ICommand _bookDetailsSortTitle;
        public ICommand BookDetailsSortTitle
        {
            get
            {
                if (_bookDetailsSortTitle == null)
                {
                    _bookDetailsSortTitle = new RelayCommand(
                        (o) =>
                        {
                            List<BookDetails> temp = _allBookDetails;
                            temp.Sort((x, y) => x.Title.CompareTo(y.Title));
                            _allBookDetails = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutBookDetials)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _bookDetailsSortTitle;
            }
        }

        private ICommand _bookDetailsSortAuthor;
        public ICommand BookDetailsSortAuthor
        {
            get
            {
                if (_bookDetailsSortAuthor == null)
                {
                    _bookDetailsSortAuthor = new RelayCommand(
                        (o) =>
                        {
                            List<BookDetails> temp = _allBookDetails;
                            temp.Sort((x, y) => x.AuthorName.CompareTo(y.AuthorName));
                            _allBookDetails = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutBookDetials)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _bookDetailsSortAuthor;
            }
        }

        private ICommand _bookDetailsSortIsbn;
        public ICommand BookDetailsSortIsbn
        {
            get
            {
                if (_bookDetailsSortIsbn == null)
                {
                    _bookDetailsSortIsbn = new RelayCommand(
                        (o) =>
                        {
                            List<BookDetails> temp = _allBookDetails;
                            temp.Sort((x, y) => x.Isbn.CompareTo(y.Isbn));
                            _allBookDetails = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutBookDetials)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _bookDetailsSortIsbn;
            }
        }

        private ICommand _bookDetailsSortRDate;
        public ICommand BookDetailsSortRDate
        {
            get
            {
                if (_bookDetailsSortRDate == null)
                {
                    _bookDetailsSortRDate = new RelayCommand(
                        (o) =>
                        {
                            List<BookDetails> temp = _allBookDetails;
                            temp.Sort((x, y) => x.ReleaseDate.CompareTo(y.ReleaseDate));
                            _allBookDetails = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutBookDetials)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _bookDetailsSortRDate;
            }
        }

        private ICommand _bookDetailsSortGenre;
        public ICommand BookDetailsSortGenre
        {
            get
            {
                if (_bookDetailsSortGenre == null)
                {
                    _bookDetailsSortGenre = new RelayCommand(
                        (o) =>
                        {
                            List<BookDetails> temp = _allBookDetails;
                            temp.Sort((x, y) => x.GenreName.CompareTo(y.GenreName));
                            _allBookDetails = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutBookDetials)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _bookDetailsSortGenre;
            }
        }

        private ICommand _bookDetailsFilterClick;
        public ICommand BookDetailsFilterClick
        {
            get
            {
                if (_bookDetailsFilterClick == null)
                {
                    _bookDetailsFilterClick = new RelayCommand(
                        (o) =>
                        {
                            _bookFilter = true;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutBookDetials)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                }
                return _bookDetailsFilterClick;
            }
        }

        private ICommand _bookDetailsUnFilter;
        public ICommand BookDetailsUnFilter
        {
            get
            {
                if (_bookDetailsUnFilter == null)
                {
                    _bookDetailsUnFilter = new RelayCommand(
                        (o) =>
                        {
                            _bookFilter = false;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutBookDetials)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                }
                return _bookDetailsUnFilter;
            }
        }

        private ICommand _moreDetails;
        public ICommand MoreDetails
        {
            get
            {
                if( _moreDetails == null)
                {
                    _moreDetails = new RelayCommand(
                        (o) =>
                        {
                            MessageBox.Show(OutBookDetials[SelectedBookIndex].GetDetails());
                        },
                        (o) =>
                        {
                            if (SelectedBookIndex == -1)
                            {
                                return false;
                            }
                            else
                                return true;
                        }
                        );

                }
                return _moreDetails;
            }
        }

        private ICommand _opinions;
        public ICommand Opinions
        {
            get
            {
                if (_opinions == null)
                {
                    _opinions = new RelayCommand(
                        (o) =>
                        {
                            List<Review> temp = DataAccess.ReviewRepository.GetReviews(OutBookDetials[SelectedBookIndex].Isbn);
                            string output = "";
                            foreach(var review in temp)
                            {
                                output += review.GetDetails();
                            }
                            MessageBox.Show(output);
                        },
                        (o) =>
                        {
                            if (SelectedBookIndex == -1)
                            {
                                return false;
                            }
                            else
                                return true;
                        }
                        );

                }
                return _opinions;
            }
        }

        #endregion

        #endregion

        #region List of genres

        #region Properties

        private List<Genre> _allGenres = DataAccess.GenreRepository.GetAllGenres();
        public List <Genre> AllGenres
        {
            get
            {
                return _allGenres;
            }
            set
            {
                _allGenres = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllGenres)));
            }
        }

        private List<string> _outGenreList;
        public List<string> OutGenreList
        {
            get
            {
                string temp;
                List<string> output = new List<string>();
                foreach (var genre in _allGenres)
                {
                    temp = $"{genre.Name} || {genre.Description}";
                    output.Add(temp);
                }
                return output;
            }
        }

        #endregion

        #region Commands

        #endregion

        #endregion
    }
}
