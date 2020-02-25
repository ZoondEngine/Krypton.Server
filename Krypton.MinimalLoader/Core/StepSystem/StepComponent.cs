using Krypton.MinimalLoader.Core.StepSystem.Contracts;
using Krypton.MinimalLoader.Core.StepSystem.Implements;
using Krypton.Support;
using System.Collections.Generic;

namespace Krypton.MinimalLoader.Core.StepSystem
{
    public class StepComponent : KryptonComponent<StepComponent>
    {
        private List<IStepElement> Steps { get; set; }

        public StepComponent()
        {
            Steps = new List<IStepElement>()
            {
                new SecurityStep(),
                new ConnectionStep(),
                new AuthorizeStep(),
                new PreparationStep(),
            };
        }

        public bool Check(out string message)
        {
            foreach(var step in Steps)
            {
                if(!step.Do(out message))
                {
                    return false;
                }
            }

            message = "";
            return true;
        }
    }
}
