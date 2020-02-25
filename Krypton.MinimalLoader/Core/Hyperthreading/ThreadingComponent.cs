using Krypton.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.MinimalLoader.Core.Hyperthreading
{
    public class ThreadingComponent : KryptonComponent<ThreadingComponent>
    {
        private Stack<Task> TaskPool { get; set; }
    }
}
