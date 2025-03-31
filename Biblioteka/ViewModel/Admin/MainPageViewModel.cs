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
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void ReloadData()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Privileges)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StandardUserCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdminCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BookCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BookInstanceCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GenreCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AuthorCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoanedBookInstanceCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsersWithOverdueBooks)));
        }

        #region Public binded properties
        private string _name = SharedViewModel.LoggedUser.Name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        private string _privileges = SharedViewModel.LoggedUser.Privileges;
        public string Privileges
        {
            get
            {
                return _privileges;
            }
        }

        public int StandardUserCount
        {
            get
            {
                return DataAccess.UserRepository.GetAllUsers().Where((u) => u.IsAdmin == false).Count();
            }
        }

        public int AdminCount
        {
            get
            {
                return DataAccess.UserRepository.GetAllUsers().Where((u) => u.IsAdmin == true).Count();
            }
        }

        public int BookCount
        {
            get
            {
                return DataAccess.BookRepository.GetAllBooks().Count();
            }
        }

        public int BookInstanceCount
        {
            get
            {
                return DataAccess.BookInstanceRepository.GetAllBooksInstances().Count();

            }
        }

        public int GenreCount
        {
            get
            {
                return DataAccess.GenreRepository.GetAllGenres().Count();

            }
        }

        public int AuthorCount
        {
            get
            {
                return DataAccess.AuthorRepository.GetAllAuthors().Count();
            }
        }

        public int LoanedBookInstanceCount
        {
            get
            {
                return DataAccess.BookInstanceRepository.GetAllBooksInstances().Where((b) => b.Login != null).Count();
            }
        }

        public List<string> UsersWithOverdueBooks
        {
            get
            {
                return DataAccess.UserRepository.GetUsersWithOverdueBooks();
            }
        }
        #endregion

        #region Commands
        private ICommand _logout;
        public ICommand Logout
        {
            get
            {
                if (_logout == null)
                {
                    _logout = new RelayCommand(
                        (o) =>
                        {
                            View.MainWindow.MainFrame.Navigate(new Uri("View/LoginPage.xaml", UriKind.Relative));
                            SharedViewModel.LoggedUser = null;
                        },
                        (o) =>
                        {
                            return true;
                        }
                    );
                }

                return _logout;
            }
        }
        #endregion
    }
}
