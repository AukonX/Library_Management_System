using Biblioteka.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test_MVVM.ViewModel;

namespace Biblioteka.ViewModel.User
{
    internal class UserMainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Binded
        private string _userFirstName = SharedViewModel.LoggedUser.Name.Split(' ')[0];
        public string UserFirstName
        {
            get
            {
                return _userFirstName;
            }
        }

        private string _userName = SharedViewModel.LoggedUser.Name;
        public string UserName
        {
            get
            {
                return _userName;
            }
        }

        private DateTime _birthDate = SharedViewModel.LoggedUser.BirthDate;
        public DateTime BirthDate
        {
            get
            {
                return _birthDate;
            }
        }

        private string _userAdress = SharedViewModel.LoggedUser.Address;
        public string UserAdress
        {
            get
            {
                return _userAdress;
            }
        }

        private string _userEmail = SharedViewModel.LoggedUser.Email;
        public string UserEmail
        {
            get
            {
                return _userEmail;
            }
        }

        private string _userPhone = SharedViewModel.LoggedUser.Phone;
        public string UserPhone
        {
            get
            {
                return _userPhone;
            }
        }

        public List<String> UserOverdueBooks
        {
            get
            {
                return DataAccess.LoanedBooksRepository.GetUserOverdueBooks(SharedViewModel.LoggedUser);
            }
        }

        public List<string> UserLoanedBooks
        {
            get
            {
                return DataAccess.LoanedBooksRepository.GetUserLoanedBookInstances(SharedViewModel.LoggedUser);
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
