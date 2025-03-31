using Biblioteka.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using test_MVVM.ViewModel;

namespace Biblioteka.ViewModel.User
{
    internal class UserBookOpinionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<HistoricalBookInstanceDetails> _historicalBookInstanceDetails = DataAccess.HistoricalBookInstanceDetailsRepository.GetUsersHistoricalBookInstances(SharedViewModel.LoggedUser);
        public List<HistoricalBookInstanceDetails> HistoricalBookInstanceDetails
        {
            get
            {
                return _historicalBookInstanceDetails;
            }
        }

        private List<string> _historicalTitles;
        public List<string> HistoricalTitles
        {
            get
            {
                List<string> output = new List<string>();
                foreach (var book in _historicalBookInstanceDetails)
                {
                    if(!output.Contains(book.Title))
                    {
                        output.Add(book.Title);
                    }
                }
                return output;
            }
        }

        private List<string> _historicalIsbns;
        public  List<string> HistoricalIsbns
        {
            get
            {
                List<string> output = new List<string>();
                foreach (var book in _historicalBookInstanceDetails)
                {
                    if (!output.Contains(book.Title))
                    {
                        output.Add(book.Isbn);
                    }
                }
                return output;  
            }
        }

        private int _selectedItemIndex = 0;
        public int SelectedItemIndex
        {
            get
            {
                return _selectedItemIndex;
            }
            set
            {
                _selectedItemIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItemIndex)));
            }
        }

        private string _reviewTitle = null;
        public string ReviewTitle
        {
            get
            {
                return _reviewTitle;
            }
            set
            {
                _reviewTitle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReviewTitle)));
            }
        }

        private string _reviewBody = null;
        public string ReviewBody
        {
            get
            {
                return _reviewBody;
            }
            set
            {
                _reviewBody = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReviewBody)));
            }
        }

        public List<int> Rate
        {
            get
            {
                List<int> output = new List<int>();
                for(int i = 1; i < 11; i++)
                {
                    output.Add(i);
                }
                return output;
            }
        }

        private int _selectedRateIndex = 0;
        public int SelectedRateIndex
        {
            get
            {
                return _selectedRateIndex;
            }
            set
            {
                _selectedRateIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedRateIndex)));
            }
        }

        private ICommand _validate;
        public ICommand Validate
        {
            get
            {
                if( _validate == null)
                {
                    _validate = new RelayCommand(
                        (o) =>
                        {
                            Review temp = new Review(1, _selectedRateIndex + 1, _reviewTitle, _reviewBody, DateTime.Now, HistoricalIsbns[SelectedItemIndex], SharedViewModel.LoggedUser.Login);
                            DataAccess.ReviewRepository.AddReview(temp);
                            _reviewBody = null;
                            _reviewTitle = null;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReviewTitle)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReviewBody)));
                        },
                        (o)=>
                        {
                            if (_reviewBody != null && _reviewTitle != null)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        );
                }
                return _validate;
            }
        }
    }
}
