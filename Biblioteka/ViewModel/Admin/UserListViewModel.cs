using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.ViewModel.Admin
{
    internal class UserListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public UserListViewModel()
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserFullBooksInstances)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserHistoricalBooksInstances)));
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

        private List<Model.HistoricalBookInstanceDetails> _userHistoricalBooksInstances;
        public List<Model.HistoricalBookInstanceDetails> UserHistoricalBooksInstances
        {
            get
            {
                _userHistoricalBooksInstances = new List<Model.HistoricalBookInstanceDetails>();

                if (_allUsersSelectedItem != null)
                {
                    _userHistoricalBooksInstances = DataAccess.HistoricalBookInstanceDetailsRepository.GetUsersHistoricalBookInstances(_allUsersSelectedItem);
                }

                return _userHistoricalBooksInstances;
            }
        }
        #endregion
    }
}
