using Krypton.MinimalLoader.Core.CodeExecutor.Contracts;

namespace Krypton.MinimalLoader.Core.CodeExecutor
{
    public class CodeExecutor
    {
        private ICodeContainer Container { get; set; }

        public CodeExecutor(ICodeContainer container)
        {
            Container = container;
        }

        public string Encrypt(string clean_str)
            => GetInterface().Execute<string>("Encrypt", clean_str);

        public string Decrypt(string encrypted_str)
            => GetInterface().Execute<string>("Decrypt", encrypted_str);

        public bool Inject(string file_path)
            => GetInterface().Execute<bool>("Inject", file_path);

        public string DownloadRing2()
            => GetInterface().Execute<string>("DownloadRing2");

        public string GetInjectionProcess()
            => GetInterface().Execute<string>("GetProcess");

        public bool Validate()
            => GetInterface().Execute<bool>("Validate");

        public ICodeContainer GetInterface()
            => Container;
    }
}
