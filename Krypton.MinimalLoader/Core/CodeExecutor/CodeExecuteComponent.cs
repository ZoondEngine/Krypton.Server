using Krypton.MinimalLoader.Core.CodeExecutor.Implemented;
using Krypton.Support;

namespace Krypton.MinimalLoader.Core.CodeExecutor
{
    public class CodeExecuteComponent : KryptonComponent<CodeExecuteComponent>
    {
        private CodeExecutor Executor { get; set; }

        public CodeExecuteComponent()
        { }

        public void ApplyScript(string script_from)
            => Executor = new CodeExecutor(new BaseCodeContainer(script_from));

        public string GetRawScript()
            => GetExecutor().GetInterface().GetRawCode();

        public dynamic GetDynamicObject()
            => GetExecutor().GetInterface().GetDynamicObject();

        public CodeExecutor GetExecutor()
            => Executor;

        public T To<T>()
            => (T)(object)GetDynamicObject();

        public bool RunScriptCheck()
            => GetExecutor().GetInterface().IsValid();
    }
}
