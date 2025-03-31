using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.ViewModel.Admin
{
    internal class GlobalViewModel
    {
        #region Global variables
        private MainPageViewModel _mainPageViewModel;
        private ManageBookViewModel _manageBookViewModel;
        private ManageBookInstanceViewModel _manageBookInstanceViewModel;
        private ManageAuthorsViewModel _manageAuthorsViewModel;
        private ManageGenresViewModel _manageGenresViewModel;
        private LendBookViewModel _lendBookViewModel;
        private UserListViewModel _userListViewModel;
        private ManageUsersViewModel _manageUsersViewModel;
        #endregion

        #region Global constructor
        public GlobalViewModel()
        {
            _mainPageViewModel = new MainPageViewModel();
            _manageBookViewModel = new ManageBookViewModel();
            _manageBookInstanceViewModel = new ManageBookInstanceViewModel();
            _manageAuthorsViewModel = new ManageAuthorsViewModel();
            _manageGenresViewModel = new ManageGenresViewModel();
            _lendBookViewModel = new LendBookViewModel();
            _userListViewModel = new UserListViewModel();
            _manageUsersViewModel = new ManageUsersViewModel();
        }
        #endregion

        #region Separated ViewModels
        public MainPageViewModel MainPageViewModel
        {
            get
            {
                return _mainPageViewModel;
            }
        }

        public ManageBookViewModel ManageBookViewModel
        {
            get
            {
                return _manageBookViewModel;
            }
        }        

        public ManageBookInstanceViewModel ManageBookInstanceViewModel
        {
            get
            {
                return _manageBookInstanceViewModel;
            }
        }

        public ManageAuthorsViewModel ManageAuthorsViewModel
        {
            get
            {
                return _manageAuthorsViewModel;
            }
        }

        public ManageGenresViewModel ManageGenresViewModel
        {
            get
            {
                return _manageGenresViewModel;
            }
        }

        public LendBookViewModel LendBookViewModel
        {
            get
            {
                return _lendBookViewModel;
            }
        }

        public UserListViewModel UserListViewModel
        {
            get
            {
                return _userListViewModel;
            }
        }

        public ManageUsersViewModel ManageUsersViewModel
        {
            get
            {
                return _manageUsersViewModel;
            }
        }
        #endregion

        #region Reload data of specific tab when the tab is selected
        private int _tabControlSelectedIndex = 0;
        public int TabControlSelectedIndex
        {
            get
            {
                return _tabControlSelectedIndex;
            }

            set
            {
                _tabControlSelectedIndex = value;
                
                // Strona główna:
                if (_tabControlSelectedIndex == 0)
                {
                    _mainPageViewModel.ReloadData();
                }

                // Zarządzaj książkami:
                else if (_tabControlSelectedIndex == 1)
                {
                    _manageBookViewModel.ReloadData();
                }

                // Zarządzaj instancjami książek:
                else if (_tabControlSelectedIndex == 2)
                {
                    _manageBookInstanceViewModel.ReloadData();
                }

                // Wypożycz/odbierz książkę:
                else if (_tabControlSelectedIndex == 5)
                {
                    _lendBookViewModel.ReloadData();
                }

                // Spis użytkowników:
                else if (_tabControlSelectedIndex == 6)
                {
                    _userListViewModel.ReloadData();
                }

                // Zarządzaj użytkownikami:
                else if (_tabControlSelectedIndex == 7)
                {
                    _manageUsersViewModel.ReloadData();
                }
            }
        }
        #endregion
    }
}
