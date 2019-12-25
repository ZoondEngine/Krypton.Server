using Krypton.Server.Core.IO.Contracts;

namespace Krypton.Server.Core.IO.Commands.Detail
{
    public class CommandDoResult
    {
        public ICommandElement Command;
        public string Input;
        public CommandProcessResultType Result;
    }
}
