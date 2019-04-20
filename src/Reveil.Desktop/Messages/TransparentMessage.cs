using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.Messages
{
    public class TransparentMessage
    {
        public TransparentMessage(bool transparent)
        {
            Transparent = transparent;
        }

        public bool Transparent { get; set; }
    }
}
