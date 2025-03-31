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
    internal class ManageGenresViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ManageGenresViewModel()
        {
            _allGenres = DataAccess.GenreRepository.GetAllGenres();
        }

        #region Public binding properties
        private List<Model.Genre> _allGenres;
        public List<Model.Genre> AllGenres
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

        private Model.Genre _selectedGenre;
        public Model.Genre SelectedGenre
        {
            get
            {
                return _selectedGenre;
            }

            set
            {
                _selectedGenre = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedGenre)));
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
        private bool _genresNoEmptyFields()
        {
            bool result = true;
            foreach (var genre in _allGenres)
            {
                if (genre.Name == "" || genre.Description == "")
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        #endregion

        #region Commands
        private ICommand _modifyGenres;
        public ICommand ModifyGenres
        {
            get
            {
                if (_modifyGenres == null)
                {
                    _modifyGenres = new RelayCommand(
                        (o) =>
                        {
                            if (!_genresNoEmptyFields())
                            {
                                ErrorLabel = "Pola z nazwą oraz opisem nie mogą być puste!";

                            }

                            else
                            {
                                DataAccess.GenreRepository.SaveGenres(_allGenres);

                                // Reload all genres after saving:
                                AllGenres = DataAccess.GenreRepository.GetAllGenres();

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

                return _modifyGenres;
            }
        }

        private ICommand _deleteGenre;
        public ICommand DeleteGenre
        {
            get
            {
                if (_deleteGenre == null)
                {
                    _deleteGenre = new RelayCommand(
                        (o) =>
                        {
                            if (SelectedGenre != null && SelectedGenre?.Id != null)
                            {
                                bool status = DataAccess.GenreRepository.DeleteGenre(SelectedGenre);

                                if (!status)
                                {
                                    ErrorLabel = "Nie można usunąć gatunku, ponieważ inny obiekt się do niego odwołuje!";
                                }

                                else
                                {
                                    ErrorLabel = "";
                                    SuccessLabel = "Pomyślnie usunięto gatunek.";
                                    Task.Delay(3000).ContinueWith(_ => { SuccessLabel = ""; });
                                }

                                // Reload all genres after saving:
                                AllGenres = DataAccess.GenreRepository.GetAllGenres();
                            }
                        },
                        (o) =>
                        {
                            if (SelectedGenre != null && SelectedGenre?.Id != null)
                            {
                                return true;
                            }

                            return false;
                        }
                    );
                }

                return _deleteGenre;
            }
        }
        #endregion

    }
}
