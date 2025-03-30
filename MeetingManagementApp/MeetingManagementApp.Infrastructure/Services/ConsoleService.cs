using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.Services
{
    internal class ConsoleService : IPrinterService
    {
        private readonly Mutex _mutex = new Mutex();

        public CommandResult PrinterExecute(string? value,
            Func<string?, CommandResult> func,
            IReadOnlyDictionary<string, ICommandRequestHandler>? handlers = null,
            Func<string?, IReadOnlyCollection<string>>? allowCommandFunc = null)
        {
            try
            {
                _mutex.WaitOne();

                var funcValue = func(value);

                _mutex.ReleaseMutex();
                // Так сделано для того, чтобы уведомление прилетало либо после выполнения команды, либо после выбора команды
                _mutex.WaitOne();

                if (handlers != null && handlers.Count > 0 && allowCommandFunc != null)
                {
                    var allowCommands = allowCommandFunc(funcValue.ResultValue);

                    funcValue.Command = GetUserCommand(handlers, allowCommands);
                }

                return funcValue;
            }
            catch
            {
                throw;
            }
            finally
            {
                _mutex.ReleaseMutex(); 
            }
        }

        /// <summary>
        /// Метод для корректного и однотипного вызова команд.
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        private string? GetUserCommand(IReadOnlyDictionary<string, ICommandRequestHandler> handlers, IReadOnlyCollection<string> commands) 
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine();

            Console.WriteLine("Выберите команду:");

            foreach (var command in commands)
            {
                if (handlers.TryGetValue(command, out var handler))
                {
                    Console.WriteLine();
                    Console.WriteLine($"{command} - {handler.GetCommandDescription()}");
                }
            }
                
            Console.WriteLine();

            Console.WriteLine("Команда:");

            return Console.ReadLine();
        }
    }
}
