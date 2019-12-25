namespace Krypton.Server.Core.IO.Commands.Detail
{
    public enum CommandProcessResultType
    {
        Success = 0,
        CommandNotFound = 1,
        IncorrectSyntax = 2,
        EmptyInput = 3,
    }
}
