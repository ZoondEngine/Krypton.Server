using Krypton.MinimalLoader.Core.StepSystem.Contracts;
using System;

namespace Krypton.MinimalLoader.Core.StepSystem.Implements
{
    public class AuthorizeStep : IStepElement
    {
        public bool Do(out string message)
        {
            var hardware = Hardware.HardwareComponent.Instance;
            var prompt = Prompt.PromptComponent.Instance;
            var network = Network.NetworkComponent.Instance;
            var code_executor = CodeExecutor.CodeExecuteComponent.Instance;

            prompt.Write("Enter your key: ");
            if(prompt.TryGetInput(out string key))
            {
                prompt.Write("Authorize key ... \t\t")
                var packet = new GetKeyAuth()
                {
                    Hardware = hardware.GetHardwareId(),
                    Key = key,
                    LocaleCode = hardware.GetLocale().GetLocaleCode(),
                    LocaleShort = hardware.GetLocale().GetShortLocale(),
                    ActivateDate = DateTime.Now
                };

                var reply_task = network.SendAndWait(packet);
                reply_task.Wait();

                var reply = reply_task.Result.PacketContent.Convert<SetKeyAuth>();
                if(reply.Result)
                {
                    try
                    {
                        code_executor.ApplyScript(reply.Script);

                        prompt.Write(ConsoleColor.Green, "OK", true);
                        prompt.Write(ConsoleColor.Cyan, $"Expiration date: {reply.RemainingTime}", true);

                        message = "";
                        return true;
                    }
                    catch(Exception ex)
                    {
                        prompt.Exception(ex);

                        message = "Server invalid challenge token. Please contact with administrator";
                        return false;
                    }
                }
                else
                {
                    message = reply.Message;
                }
            }
            else
            {
                message = "Invalid key format";
            }

            prompt.Write(ConsoleColor.Red, "ERROR", true);
            return false;
        }
    }
}
