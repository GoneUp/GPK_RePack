using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Saver;

namespace GPK_RePack.Classes.Interfaces
{
    interface IProgress
    {
        //double instead?
        Status GetStatus();
    }
}
