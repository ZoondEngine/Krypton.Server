using Krypton.Support;
using System;

namespace Krypton.MinimalLoader.Core.Prompt
{
    public class PromptComponent : KryptonComponent<PromptComponent>
    {
        public bool TryGetInput<T>(out T result)
        {
            result = GetInput<T>();

            return result != null;
        }

        public T GetInput<T>()
        {
            var line = Console.ReadLine();
            var converted = Convert.ChangeType(line, typeof(T));
            if (converted != null)
            {
                return (T)converted;
            }

            return default;
        }

        public void SetTitle(string title)
            => Console.Title = title;

        public T ExceptGetInput<T>()
        {
            if(TryGetInput(out T result))
            {
                if (result == null)
                    throw new ArgumentNullException("Input result has been null or abnormal for casting");

                return result;
            }

            return default;
        }

        public void Write(string line, bool new_line = false)
            => Console.Write(new_line == true ? line + Environment.NewLine : line);

        public void Write(ConsoleColor foreground, string line, bool new_line = false)
        {
            Console.ForegroundColor = foreground;
            Write(line, new_line);
            Console.ResetColor();
        }

        public void Exception(Exception ex)
        {
            if(Security.SecurityComponent.Instance.IsDebug())
            {
                Write(ConsoleColor.Red, $"EXCEPTION: {ex.Message}", true);
                Write(ConsoleColor.Red, $"Stack trace: ", true);
                Write(ConsoleColor.Red, ex.StackTrace, true);
            }
            else
            {
                Write(ConsoleColor.Red, "Internal error!", true);
                Write(ConsoleColor.Red, "Please contact with administrator", true);
            }
        }
    }
}
