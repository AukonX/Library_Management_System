using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test_MVVM.ViewModel;

namespace Biblioteka.ViewModel.User
{
    internal class UserGlobalViewModel
    {
        public UserMainPageViewModel UserMainPageViewModel
        {
            get
            {
                return new UserMainPageViewModel();
            }
        }

        public UserMiscViewModel UserMiscViewModel
        {
            get
            {
                return new UserMiscViewModel();
            }
        }

        public UserHistoryViewModel UserHistoryViewModel
        {
            get
            {
                return new UserHistoryViewModel();
            }
        }

        public UserBookOpinionViewModel UserBookOpinionViewModel
        {
            get
            {
                return new UserBookOpinionViewModel();
            }
        }
    }
}
