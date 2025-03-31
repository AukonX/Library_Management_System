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
    internal class ManageUsersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ManageUsersViewModel()
        {
            _allUsers = DataAccess.UserRepository.GetAllUsers();
        }

        public void ReloadData()
        {
            _allUsers = DataAccess.UserRepository.GetAllUsers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllUsers)));
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

        #region Validators
        private bool _noEmptyFields()
        {
            bool result = true;
            foreach (var user in _allUsers)
            {
                if (user.Login == "" || user.Password == "" || user.Name == "" || user.Address == "" || user.Email == "" || user.Phone == "")
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private bool _correctPhoneNumber()
        {
            bool result = true;
            string digits = "0123456789";
            foreach (var user in _allUsers)
            {
                if (user.Phone.Length != 9)
                {
                    result = false;
                    break;
                }

                bool notDigit = false;
                foreach (char digit in user.Phone)
                {
                    if (!digits.Contains(digit))
                    {
                        result = false;
                        notDigit = true;
                        break;
                    }
                }

                if (notDigit)
                {
                    break;
                }
            }
            return result;
        }

        private bool _dateNotInFuture()
        {
            bool result = true;
            foreach(var user in _allUsers)
            {
                if (user.BirthDate > DateTime.Now)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        #endregion

        #region Commands
        private ICommand _saveUsers;
        public ICommand SaveUsers
        {
            get
            {
                if (_saveUsers == null)
                {
                    _saveUsers = new RelayCommand(
                        (o) =>
                        {
                            if (!_noEmptyFields())
                            {
                                ErrorLabel = "Żadne pole nie może być puste!";
                            }

                            else if (!_correctPhoneNumber())
                            {
                                ErrorLabel = "Numer telefonu musi mieć 9 cyfr!";
                            }

                            else if (!_dateNotInFuture())
                            {
                                ErrorLabel = "Data urodzenia nie może być w przyszłości!";
                            }

                            else
                            {
                                DataAccess.UserRepository.SaveUsers(_allUsers);

                                ErrorLabel = "";
                                SuccessLabel = "Zmiany zostały pomyślnie zapisane!";
                                Task.Delay(3000).ContinueWith(_ => { SuccessLabel = ""; });

                                _allUsers = DataAccess.UserRepository.GetAllUsers();
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllUsers)));
                            }
                        },
                        (o) =>
                        {
                            return true;
                        }
                    );
                }

                return _saveUsers;
            }
        }

        private ICommand _deleteUser;
        public ICommand DeleteUser
        {
            get
            {
                if (_deleteUser == null)
                {
                    _deleteUser = new RelayCommand(
                        (o) =>
                        {
                            string status = DataAccess.UserRepository.DeleteUser(_allUsersSelectedItem);

                            if (status == "")
                            {
                                ErrorLabel = "";
                                SuccessLabel = "Uźytkownik został pomyślnie usunięty (wraz z historią wypożyczeń i wystawionymi ocenami)!";
                                Task.Delay(3000).ContinueWith(_ => { SuccessLabel = ""; });
                            }

                            else
                            {
                                ErrorLabel = status;
                            }

                            _allUsers = DataAccess.UserRepository.GetAllUsers();
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllUsers)));
                        },
                        (o) =>
                        {
                            if (_allUsersSelectedItem == null)
                            {
                                return false;
                            }

                            return true;
                        }
                    );
                }

                return _deleteUser;
            }
        }
        #endregion
    }
}
