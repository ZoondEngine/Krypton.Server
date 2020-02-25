using Krypton.MinimalLoader.Core.CodeExecutor.Contracts;
using System;

namespace Krypton.MinimalLoader.Core.CodeExecutor.Implemented
{
    public class BaseCodeContainer : ICodeContainer
    {
        private string RawCode { get; set; }
        private dynamic DynamicObject { get; set; }

        public BaseCodeContainer(string code)
        {
            RawCode = code;
            if(!IsValid())
            {
                throw new InvalidOperationException("Received abnormal security code!");
            }
            else
            {
                DynamicObject = CSScriptLibrary.CSScript.LoadCode(code).CreateObject("*");
            }
        }

        public string Decrypt(string val)
            => Execute<string>("ExecuteDecrypt", val);

        public string Encrypt(string val)
            => Execute<string>("ExecuteEncrypt", val);

        public bool ExecuteInjection(string file_path)
            => Execute<bool>("ExecuteCode", file_path);

        public T Execute<T>(string method, params object[] args)
            => (T)(object)((object)GetDynamicObject()).GetType().GetMethod(method)?.Invoke(GetDynamicObject(), args);

        public dynamic GetDynamicObject()
            => DynamicObject;

        public string GetRawCode()
            => RawCode;

        public bool IsValid()
        {
            return true;
        }
    }
}
