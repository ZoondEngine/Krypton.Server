using Krypton.MinimalLoader.Core.StepSystem.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.MinimalLoader.Core.StepSystem.Implements
{
    public class SecurityStep : IStepElement
    {
        public bool Do(out string message)
        {
            var console = Prompt.PromptComponent.Instance;

            console.Write("Security checking ... \t\t");
            if(Security.SecurityComponent.Instance.Check(out message))
            {
                console.Write(ConsoleColor.Green, "OK", true);
                return true;
            }

            console.Write(ConsoleColor.Red, "ERROR", true);
            return false;
        }
    }
}
