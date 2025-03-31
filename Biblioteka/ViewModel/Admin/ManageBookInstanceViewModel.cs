using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test_MVVM.ViewModel;

namespace Biblioteka.ViewModel.Admin
{
    internal class ManageBookInstanceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ManageBookInstanceViewModel()
        {
            // Load all books from database:
            _allBooks = DataAccess.BookRepository.GetAllBooks();
        }

        public void ReloadData()
        {
            _allBooks = DataAccess.BookRepository.GetAllBooks();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooks)));
        }

        #region Public binding properties
        private List<Model.Book> _allBooks;
        public List<Model.Book> AllBooks
        {
            get
            {
                return _allBooks;
            }
        }

        private int _allBooksSelectedIndex = 0;
        public int AllBooksSelectedIndex
        {
            get
            {
                return _allBooksSelectedIndex;
            }

            set
            {
                _allBooksSelectedIndex = value;
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
                AllBooksInstancesIndex = 0;
            }
        }

        public List<Model.BookInstance> AllBooksInstances
        {
            get
            {
                List<Model.BookInstance> _list = new List<Model.BookInstance>();
                if (_allBooksSelectedItem != null)
                {
                    _list = DataAccess.BookInstanceRepository.GetAllBooksInstances().Where((b) => b.Isbn == _allBooksSelectedItem.Isbn).ToList();
                    _list.Insert(0, new Model.BookInstance(null, DateTime.Now, 0, _allBooksSelectedItem.Isbn, null, null));
                }
                
                return _list;
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooksInstancesSelectedItem)));
            }
        }

        private int _allBooksInstancesIndex = 0;
        public int AllBooksInstancesIndex
        {
            get
            {
                return _allBooksInstancesIndex;
            }

            set
            {
                _allBooksInstancesIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooksInstancesIndex)));
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
        #endregion

        #region Commands
        private ICommand _submitBookInstanceChange;
        public ICommand SubmitBookInstanceChange
        {
            get
            {
                if (_submitBookInstanceChange == null)
                {
                    _submitBookInstanceChange = new RelayCommand(
                        (o) =>
                        {
                            string? error = DataAccess.BookInstanceRepository.ModifyBookInstance(_allBooksInstancesSelectedItem);

                            if (error != null)
                            {
                                ErrorLabel = error;
                            }
                            else
                            {
                                ErrorLabel = "";
                                SuccessLabel = "Instancja książki została zmodyfikowana pomyślnie!";
                                Task.Delay(3000).ContinueWith(_ => { SuccessLabel = ""; });
                            }

                            // Refresh books instances:
                            int previousIndex = AllBooksInstancesIndex;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooksInstances)));
                            AllBooksInstancesIndex = previousIndex == 0 ? AllBooksInstances.Count - 1 : previousIndex;
                        },
                        (o) =>
                        {
                            return true;
                        }
                    );
                }

                return _submitBookInstanceChange;
            }
        }

        private ICommand _deleteBookInstance;
        public ICommand DeleteBookInstance
        {
            get
            {
                if (_deleteBookInstance == null)
                {
                    _deleteBookInstance = new RelayCommand(
                        (o) =>
                        {
                            DataAccess.BookInstanceRepository.DeleteBookInstance(_allBooksInstancesSelectedItem);

                            // Refresh books instances:
                            int previousIndex = AllBooksInstancesIndex;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBooksInstances)));
                            AllBooksInstancesIndex = 0;

                            SuccessLabel = "Instancja książki została usunięta pomyślnie!";
                            Task.Delay(3000).ContinueWith(_ => { SuccessLabel = ""; });
                        },
                        (o) =>
                        {
                            if (_allBooksInstancesSelectedItem?.Id == null)
                            {
                                return false;
                            }

                            return true;
                        }
                    );
                }

                return _deleteBookInstance;
            }
        }
        #endregion
    }
}
