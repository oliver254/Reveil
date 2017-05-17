using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.Messages
{
    public class RingPathMessage
    {
        #region Propriétés
        public string Path
        {
            get;
            set;
        }
        #endregion
       
        #region Méthodes
        public RingPathMessage(string path)
        {
            this.Path = path;
        }
        #endregion

    }
}
