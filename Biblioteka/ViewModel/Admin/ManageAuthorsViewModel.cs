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
    internal class ManageAuthorsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ManageAuthorsViewModel()
        {
            _allAuthors = DataAccess.AuthorRepository.GetAllAuthors();
        }

        #region Public binding properties
        private List<Model.Author> _allAuthors;
        public List<Model.Author> AllAuthors
        {
            get
            {
                return _allAuthors;
            }

            set
            {
                _allAuthors = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllAuthors)));
            }
        }

        private Model.Author _selectedAuthor;
        public Model.Author SelectedAuthor
        {
            get
            {
                return _selectedAuthor;
            }

            set
            {
                _selectedAuthor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAuthor)));
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
        private bool _authorsNoEmptyFields()
        {
            bool result = true;
            foreach (var author in _allAuthors)
            {
                if (author.Name == "" || author.Country == "")
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private bool _authorsDeathDateNotInFuture()
        {
            bool result = true;
            foreach (var author in _allAuthors)
            {
                if (author.DeathDate != null && author.DeathDate > DateTime.Now)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private bool _authorsBirthDateLessThanDeathDate()
        {
            bool result = true;
            foreach (var author in _allAuthors)
            {
                if (author.DeathDate != null && author.BirthDate > author.DeathDate)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        #endregion

        #region Commands
        private ICommand _modifyAuthors;
        public ICommand ModifyAuthors
        {
            get
            {
                if (_modifyAuthors == null)
                {
                    _modifyAuthors = new RelayCommand(
                        (o) =>
                        {
                            if (!_authorsNoEmptyFields())
                            {
                                ErrorLabel = "Pola z imieniem i nazwiskiem oraz z krajem pochodzenia nie mogą być puste!";

                            }

                            else if (!_authorsDeathDateNotInFuture())
                            {
                                ErrorLabel = "Data śmierci nie może być w przyszłości!";
                            }

                            else if (!_authorsBirthDateLessThanDeathDate())
                            {
                                ErrorLabel = "Data urodzenia musi być przed datą śmierci!";
                            }

                            else
                            {
                                DataAccess.AuthorRepository.SaveAuthors(_allAuthors);

                                // Reload all authors after saving:
                                AllAuthors = DataAccess.AuthorRepository.GetAllAuthors();

                                ErrorLabel = "";
                                SuccessLabel = "Pomyślnie zapisano zmiany.";
                                Task.Delay(3000).ContinueWith(_ => { SuccessLabel = ""; });
                            }
                        },
                        (o) =>
                        {
                            return true;
                        }
                    );
                }

                return _modifyAuthors;
            }
        }

        private ICommand _deleteAuthor;
        public ICommand DeleteAuthor
        {
            get
            {
                if (_deleteAuthor == null)
                {
                    _deleteAuthor = new RelayCommand(
                        (o) =>
                        {
                            if (SelectedAuthor != null && SelectedAuthor?.Id != null)
                            {
                                bool status = DataAccess.AuthorRepository.DeleteAuthor(SelectedAuthor);

                                if (!status)
                                {
                                    ErrorLabel = "Nie można usunąć autora, ponieważ inny obiekt się do niego odwołuje!";
                                }

                                else
                                {
                                    ErrorLabel = "";
                                    SuccessLabel = "Pomyślnie usunięto autora.";
                                    Task.Delay(3000).ContinueWith(_ => { SuccessLabel = ""; });
                                }

                                // Reload all authors after saving:
                                AllAuthors = DataAccess.AuthorRepository.GetAllAuthors();
                            }
                        },
                        (o) =>
                        {
                            if (SelectedAuthor != null && SelectedAuthor?.Id != null)
                            {
                                return true;
                            }
                            
                            return false;
                        }
                    );
                }

                return _deleteAuthor;
            }
        }
        #endregion
    }
}
