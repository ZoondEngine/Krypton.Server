namespace Krypton.Server.Core.IO.Contracts
{
    public interface ICommandElement
    {
        string Name { get; set; }
        string Description { get; set; }
        int Level { get; set; }

        bool IsHandlable(string line);
        bool Run(string line);
        string GetHelp();
    }
}
