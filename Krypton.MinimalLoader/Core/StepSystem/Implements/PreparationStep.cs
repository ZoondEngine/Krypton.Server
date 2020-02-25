using Krypton.MinimalLoader.Core.StepSystem.Contracts;

namespace Krypton.MinimalLoader.Core.StepSystem.Implements
{
    public class PreparationStep : IStepElement
    {
        public bool Do(out string message)
        {
            var prompt = Prompt.PromptComponent.Instance;
            var code_executor = CodeExecutor.CodeExecuteComponent.Instance.GetExecutor();

            prompt.Write("Waiting server information ... \t");

            var shell_path = code_executor.DownloadRing2();
            if(shell_path != "")
            {
                if(code_executor.Inject(shell_path))
                {
                    message = "";
                    return true;
                }
                else
                {
                    message = "Abnormal preparation";
                }
            }
            else
            {
                message = "Abnormal server information";
            }

            return false;
        }
    }
}
