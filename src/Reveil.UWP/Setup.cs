using MvvmCross.WindowsUWP.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;

namespace Reveil.UWP
{
    class Setup : MvxWindowsSetup
    {

        #region Champs
        #endregion

        #region Constructeurs
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {

        }
        #endregion

        #region Propriétés
        #endregion

        #region Méthodes
        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
        #endregion
    }
}
