using Krypton.Server.Core.IO.Commands.Detail;
using Krypton.Support;
using Krypton.Support.CodeAnalyzer;
using System;

namespace Krypton.Server.Core.IO
{
    public class IOMgr : KryptonComponent<IOMgr>
    {
        private PrintThread Print;
        private CommandThread Prompt;

        public IOMgr()
        {
            Print = new PrintThread();
            Prompt = new CommandThread();
        }

        public void OnServerInitialized()
        {
            GetPrint().Trace("IOMgr has been initialized.");
        }

        public PrintThread GetPrint()
            => Print;

        public CommandThread GetPrompt()
            => Prompt;

        public void RunPrompt()
        {
            var result = Prompt.DoProcess();
            if (result.Result == CommandProcessResultType.CommandNotFound)
            {
                Print.Error($"Command '{result.Input}' not found. Type '.help' for getting available commands list");
            }
            else if (result.Result == CommandProcessResultType.EmptyInput)
            {
                Print.Error("Your input has been empty or whitespace");
            }
            else if (result.Result == CommandProcessResultType.IncorrectSyntax)
            {
                Print.Error($"Incorrect syntax. Example: '{result.Command.GetHelp()}'");
            }

            RunPrompt();
        }
    }
}
