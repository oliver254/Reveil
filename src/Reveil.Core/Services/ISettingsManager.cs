using Reveil.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.Core.Services
{
    public interface ISettingsManager
    {
        Settings Get();
        void Save(Settings settings);
    }
}
