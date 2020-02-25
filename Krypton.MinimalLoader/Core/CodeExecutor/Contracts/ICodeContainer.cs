namespace Krypton.MinimalLoader.Core.CodeExecutor.Contracts
{
    public interface ICodeContainer
    {
        T Execute<T>(string method, params object[] args);
        bool IsValid();
        string GetRawCode();
        dynamic GetDynamicObject();
    }
}
