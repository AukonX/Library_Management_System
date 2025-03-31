using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.ViewModel
{
    internal class SharedViewModel
    {
        #region Public shared properties
        public static Model.User? LoggedUser { get; set; }
        #endregion
    }
}
