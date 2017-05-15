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
        public override void Initialize()
        {
            //CreatableTypes()
            //    .EndingWith("Service")
            //    .AsInterfaces()
            //    .RegisterAsLazySingleton();
            CreatableTypes()
                .EndingWith("Manager")
                .AsInterfaces()
                .RegisterAsSingleton();            

            RegisterAppStart<ViewModels.ShellViewModel>();
        }
    }
}
