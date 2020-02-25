using Krypton.MinimalLoader.Core.StepSystem.Contracts;
using System;

namespace Krypton.MinimalLoader.Core.StepSystem.Implements
{
    public class ConnectionStep : IStepElement
    {
        public bool Do(out string message)
        {
            var console = Prompt.PromptComponent.Instance;
            console.Write("Connecting ... \t\t\t");
            if(Network.NetworkComponent.Instance.Connect())
            {
                console.Write(ConsoleColor.Green, "OK", true);

                message = "";
                return true;
            }

            console.Write(ConsoleColor.Red, "ERROR", true);
            message = "Unable to connect, check your internet connection and try again";
            return false;
        }
    }
}
