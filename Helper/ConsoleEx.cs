using System;

namespace PersonDBGenerator.Helper
{
    public static class ConsoleEx
    {
        /// <summary>
        ///     Captura la linea que el usuario ingresa, y valida los datos ingresados.
        /// </summary>
        /// <returns></returns>
        public static T ReadLineAndValidate<T>(
            Func<string, bool> validate,
            string invalidMsg = "Por favor, introduzca un valor:",
            bool clearConsole = false,
            ConsoleColor alertColor = ConsoleColor.Red
        ) where T : IConvertible
        {
            if (validate is null)
                throw new ArgumentNullException(nameof(validate));

            string read = null;

            while (validate(read!))
            {
                read = Console.ReadLine();

                if (validate(read!))
                {
                    if (clearConsole)
                        Console.Clear();
                    WriteColor(invalidMsg, alertColor);
                    continue;
                }
            }

            try
            {
                return (T)Convert.ChangeType(read!, typeof(T));
            }
            catch (Exception ex)
            {
                WriteColor(ex, ConsoleColor.Red);
                return default!;
            }
        }

        public static void WriteColor(object msg, ConsoleColor consoleColor = ConsoleColor.Red)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(msg);
            Console.ResetColor();
        }

        public static void WriteLineColor(object msg, ConsoleColor consoleColor = ConsoleColor.Red)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static void WriteLineInfo(object msg)
        {
            WriteLineColor(string.Format("[{1}] {0}", msg, DateTime.Now), ConsoleColor.Cyan);
        }
    }
}
