using Biblioteka.Model;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
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
    internal class UserHistoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _filter = false;

        #region Properties

        

        private List<HistoricalBookInstanceDetails> _userBookHistory = DataAccess.HistoricalBookInstanceDetailsRepository.
            GetUsersHistoricalBookInstances(SharedViewModel.LoggedUser);
        public List<HistoricalBookInstanceDetails> UserBookHistory
        {
            get
            {
                return _userBookHistory;
            }
            set
            {
                _userBookHistory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserBookHistory)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputUserBookHistory)));
            }
        }

        public List<HistoricalBookInstanceDetails> OutputUserBookHistory
        {
            get
            {
                if(_filter == false)
                {
                    List<HistoricalBookInstanceDetails> output = new List<HistoricalBookInstanceDetails>();
                    foreach (var item in _userBookHistory)
                    {
                        output.Add(item);
                    }
                    return output;
                }
                else
                {
                    List<HistoricalBookInstanceDetails> temp = new List<HistoricalBookInstanceDetails>();
                    temp = Filter();
                    return temp;
                }
            }
        }

        #region Filter

        private List<HistoricalBookInstanceDetails> Filter()
        {
            List<HistoricalBookInstanceDetails> filtered = new List<HistoricalBookInstanceDetails>();
            switch (_filterGroup)
            {
                case 0:
                    foreach (var book in _userBookHistory)
                    {
                        if (book.Title == FilterCondition[_filterKey])
                        {
                            filtered.Add(book);
                        }
                    }
                    break;
                case 1:
                    foreach (var book in _userBookHistory)
                    {
                        if (book.Author == FilterCondition[_filterKey])
                        {
                            filtered.Add(book);
                        }
                    }
                    break;
                case 2:
                    foreach (var book in _userBookHistory)
                    {
                        if (book.Genre == FilterCondition[_filterKey])
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
                output.Add("Tytule");
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
                        foreach (var book in _userBookHistory)
                        {
                            if(!output.Contains(book.Title))
                            {
                                output.Add(book.Title);
                            }
                        }
                        break;
                    case 1:
                        foreach (var book in _userBookHistory)
                        {
                            if (!output.Contains(book.Author))
                            {
                                output.Add(book.Author);
                            }
                        }
                        break;
                    case 2:
                        foreach (var book in _userBookHistory)
                        {
                            if (!output.Contains(book.Genre))
                            {
                                output.Add(book.Genre);
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

        #endregion

        #region Commands

        private ICommand _historySortTitle;
        public ICommand HIstorySortTitle
        {
            get
            {
                if (_historySortTitle == null)
                {
                    _historySortTitle = new RelayCommand(
                        (o) =>
                        {
                            List<HistoricalBookInstanceDetails> temp = _userBookHistory;
                            temp.Sort((x, y) => x.Title.CompareTo(y.Title));
                            _userBookHistory = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputUserBookHistory)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                    
                }
                return _historySortTitle;
            }
        }

        private ICommand _historySortAuthor;
        public ICommand HistorySortAuthor
        {
            get
            {
                if (_historySortAuthor == null)
                {
                    _historySortAuthor = new RelayCommand(
                        (o) =>
                        {
                            List<HistoricalBookInstanceDetails> temp = _userBookHistory;
                            temp.Sort((x, y) => x.Author.CompareTo(y.Author));
                            _userBookHistory = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputUserBookHistory)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _historySortAuthor;
            }
        }

        private ICommand _historySortIsbn;
        public ICommand HistorySortIsbn
        {
            get
            {
                if (_historySortIsbn == null)
                {
                    _historySortIsbn = new RelayCommand(
                        (o) =>
                        {
                            List<HistoricalBookInstanceDetails> temp = _userBookHistory;
                            temp.Sort((x, y) => x.Isbn.CompareTo(y.Isbn));
                            _userBookHistory = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputUserBookHistory)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _historySortIsbn;
            }
        }

        private ICommand _historySortGenre;
        public ICommand HistorySortGenre
        {
            get
            {
                if (_historySortGenre == null)
                {
                    _historySortGenre = new RelayCommand(
                        (o) =>
                        {
                            List<HistoricalBookInstanceDetails> temp = _userBookHistory;
                            temp.Sort((x, y) => x.Genre.CompareTo(y.Genre));
                            _userBookHistory = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputUserBookHistory)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _historySortGenre;
            }
        }

        private ICommand _historySortBorrowDate;
        public ICommand HistorySortBorrowDate
        {
            get
            {
                if (_historySortBorrowDate == null)
                {
                    _historySortBorrowDate = new RelayCommand(
                        (o) =>
                        {
                            List<HistoricalBookInstanceDetails> temp = _userBookHistory;
                            temp.Sort((x, y) => x.BorrowDate.CompareTo(y.BorrowDate));
                            _userBookHistory = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputUserBookHistory)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _historySortBorrowDate;
            }
        }

        private ICommand _historySortReturnDate;
        public ICommand HistorySortReturnDate
        {
            get
            {
                if (_historySortReturnDate == null)
                {
                    _historySortReturnDate = new RelayCommand(
                        (o) =>
                        {
                            List<HistoricalBookInstanceDetails> temp = _userBookHistory;
                            temp.Sort((x, y) => x.ReturnDate.CompareTo(y.ReturnDate));
                            _userBookHistory = temp;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputUserBookHistory)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );

                }
                return _historySortReturnDate;
            }
        }

        private ICommand _historyFilterClick;
        public ICommand HistoryFilterClick
        {
            get
            {
                if( _historyFilterClick == null)
                {
                    _historyFilterClick = new RelayCommand(
                        (o) =>
                        {
                            _filter = true;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputUserBookHistory)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                }
                return _historyFilterClick;
            }
        }

        private ICommand _historyUnFilter;
        public ICommand HistoryUnFilter
        {
            get
            {
                if (_historyUnFilter == null)
                {
                    _historyUnFilter = new RelayCommand(
                        (o) =>
                        {
                            _filter = false;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputUserBookHistory)));
                        },
                        (o) =>
                        {
                            return true;
                        }
                        );
                }
                return _historyUnFilter;
            }
        }

        #endregion
    }
}
