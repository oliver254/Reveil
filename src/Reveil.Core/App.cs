using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.Core
{
    public class App : MvxApplication
    {
        #region Champs
        #endregion

        #region Constructeurs
        #endregion

        #region Propriétés
        #endregion

        #region Méthodes
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.ShellViewModel>();
        }
        #endregion
    }
}
