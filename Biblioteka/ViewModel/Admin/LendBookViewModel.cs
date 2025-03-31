using Biblioteka.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test_MVVM.ViewModel;

namespace Biblioteka.ViewModel.Admin
{
    internal class LendBookViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LendBookViewModel()
        {
            _allUsers = DataAccess.UserRepository.GetAllUsers();
            _allBooks = DataAccess.BookRepository.GetAllBooks();
        }

        public void ReloadData()
        {
            _allUsers = DataAccess.UserRepository.GetAllUsers();
            _allBooks = DataAccess.BookRepository.GetAllBooks();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllUsers)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooks)));
        }

        #region Public binding properties
        private List<Model.User> _allUsers;
        public List<Model.User> AllUsers
        {
            get
            {
                return _allUsers;
            }

            set
            {
                _allUsers = value;
            }
        }

        private Model.User _allUsersSelectedItem;
        public Model.User AllUsersSelectedItem
        {
            get
            {
                return _allUsersSelectedItem;
            }

            set
            {
                _allUsersSelectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserFullBooksInstances)));
            }
        }

        private List<Model.FullBookInstance> _userFullBooksInstances;
        public List<Model.FullBookInstance> UserFullBooksInstances
        {
            get
            {
                _userFullBooksInstances = new List<Model.FullBookInstance>();

                if (_allUsersSelectedItem != null)
                {
                    _userFullBooksInstances = DataAccess.FullBookInstanceRepository.GetAllFullBooksInstances().Where((f) => f.Login == _allUsersSelectedItem.Login).ToList();
                }
                
                return _userFullBooksInstances;
            }

            set
            {
                _userFullBooksInstances = value;
            }
        }

        private Model.FullBookInstance _userFullBooksInstancesSelectedItem;
        public Model.FullBookInstance UserFullBooksInstancesSelectedItem
        {
            get
            {
                return _userFullBooksInstancesSelectedItem;
            }

            set
            {
                _userFullBooksInstancesSelectedItem = value;
            }
        }

        private string _errorLabel1;
        public string ErrorLabel1
        {
            get
            {
                return _errorLabel1;
            }

            set
            {
                _errorLabel1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLabel1)));
            }
        }

        private string _successLabel1;
        public string SuccessLabel1
        {
            get
            {
                return _successLabel1;
            }

            set
            {
                _successLabel1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SuccessLabel1)));
            }
        }

        private List<Model.Book> _allBooks;
        public List<Model.Book> AllBooks
        {
            get
            {
                return _allBooks;
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooksInstances)));
            }
        }

        private List<Model.BookInstance> _allBooksInstances;
        public List<Model.BookInstance> AllBooksInstances
        {
            get
            {
                _allBooksInstances = new List<Model.BookInstance>();
                if (_allBooksSelectedItem != null)
                {
                    _allBooksInstances = DataAccess.BookInstanceRepository.GetAllBooksInstances().Where((b) => b.Isbn == _allBooksSelectedItem.Isbn && b.Login == null).ToList();

                }
                return _allBooksInstances;
            }
        }

        private Model.BookInstance _allBooksInstancesSelectedItem;
        public Model.BookInstance AllBooksInstancesSelectedItem
        {
            get
            {
                return _allBooksInstancesSelectedItem;
            }

            set
            {
                _allBooksInstancesSelectedItem = value;
            }
        }

        private string _errorLabel2;
        public string ErrorLabel2
        {
            get
            {
                return _errorLabel2;
            }

            set
            {
                _errorLabel2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLabel2)));
            }
        }

        private string _successLabel2;
        public string SuccessLabel2
        {
            get
            {
                return _successLabel2;
            }

            set
            {
                _successLabel2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SuccessLabel2)));
            }
        }
        #endregion

        #region Commands
        private ICommand _retrieveBookInstance;
        public ICommand RetrieveBookInstance
        {
            get
            {
                if (_retrieveBookInstance == null)
                {
                    _retrieveBookInstance = new RelayCommand(
                        (o) =>
                        {
                            Model.BookInstance bookInstance = new Model.BookInstance(
                                _userFullBooksInstancesSelectedItem.Id,
                                _userFullBooksInstancesSelectedItem.PurchaseDate,
                                _userFullBooksInstancesSelectedItem.MaxBorrowDays,
                                _userFullBooksInstancesSelectedItem.Isbn,
                                _userFullBooksInstancesSelectedItem.Login,
                                _userFullBooksInstancesSelectedItem.BorrowDate
                            );

                            DataAccess.BookInstanceRepository.RemoveUserFromBookInstance(bookInstance);

                            ErrorLabel1 = "";
                            SuccessLabel1 = "Książka została odebrana pomyślnie!";
                            Task.Delay(3000).ContinueWith(_ => { SuccessLabel1 = ""; });

                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserFullBooksInstances)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooksInstances)));
                        },
                        (o) =>
                        {
                            if (_allUsersSelectedItem == null || _userFullBooksInstancesSelectedItem == null)
                            {
                                return false;
                            }

                            return true;
                        }
                    );
                }

                return _retrieveBookInstance;
            }
        }

        private ICommand _lendBookInstance;
        public ICommand LendBookInstance
        {
            get
            {
                if (_lendBookInstance == null)
                {
                    _lendBookInstance = new RelayCommand(
                        (o) =>
                        {
                            Model.BookInstance bookInstance = _allBooksInstancesSelectedItem;
                            bookInstance.Login = _allUsersSelectedItem.Login;
                            DataAccess.BookInstanceRepository.LendBookInstance(bookInstance);

                            ErrorLabel2 = "";
                            SuccessLabel2 = "Książka została wypożyczona pomyślnie!";
                            Task.Delay(3000).ContinueWith(_ => { SuccessLabel2 = ""; });

                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserFullBooksInstances)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooksInstances)));
                        },
                        (o) =>
                        {
                            if (_allUsersSelectedItem == null || _allBooksSelectedItem == null || _allBooksInstancesSelectedItem == null)
                            {
                                return false;
                            }

                            return true;
                        }
                    );
                }

                return _lendBookInstance;
            }
        }
        #endregion
    }
}
