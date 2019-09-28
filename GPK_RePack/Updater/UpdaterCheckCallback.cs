using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPK_RePack.Updater
{
    interface UpdaterCheckCallback
    {
        void postUpdateResult(bool updateAvailable);
    }
}
