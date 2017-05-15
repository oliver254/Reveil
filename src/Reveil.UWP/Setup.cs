using MvvmCross.Platform.IoC;
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

        #region Constructeurs
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {

        }
        #endregion

        #region Méthodes
        protected override void InitializeFirstChance()
        {
            CreatableTypes()
                .EndingWith("Manager")
                .AsInterfaces()
                .RegisterAsSingleton();

            base.InitializeFirstChance();

        }
        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
        #endregion
    }
}
