using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test_MVVM.ViewModel;

namespace Biblioteka.ViewModel
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Public binded properties
        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
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
        #endregion

        #region Commands
        private ICommand _submitLoginForm;
        public ICommand SubmitLoginForm
        {
            get
            {
                if (_submitLoginForm == null)
                {
                    _submitLoginForm = new RelayCommand(
                        (o) =>
                        {
                            Model.User? user = DataAccess.UserRepository.GetUserByCredentials(Username, Password);

                            if (user == null)
                            {
                                ErrorLabel = "Login lub hasło są niepoprawne!";
                            }

                            else
                            {
                                ErrorLabel = "";

                                SharedViewModel.LoggedUser = user;

                                if (user.IsAdmin)
                                {
                                    View.MainWindow.MainFrame.Navigate(new Uri("View/AdminPage.xaml", UriKind.Relative));
                                }

                                else
                                {
                                    View.MainWindow.MainFrame.Navigate(new Uri("View/StandardUserPage.xaml", UriKind.Relative));
                                }
                            }
                        },
                        (o) =>
                        {
                            return true;
                        }
                    );
                }

                return _submitLoginForm;
            }
        }
        #endregion
    }
}
