using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;

namespace MeetingManagementApp.Api.MeetingApp
{
    public class MeetingApp
    {
        private readonly IBackgroundService _notificationSender;
        private readonly IReadOnlyDictionary<string, ICommandRequestHandler> _handlers;

        public MeetingApp(IEnumerable<ICommandRequestHandler> handlers, IBackgroundService notificationSender)
        {
            _notificationSender = notificationSender;
            _handlers = handlers.ToDictionary(k => k.GetCommand());
        }

        /// <summary>
        /// Запуск инструкций приложения.
        /// </summary>
        public void Run()
        {
            var startHandler = _handlers["m"];
            var exceptionHandler = _handlers["ex"];
            var userInputExceptionHandler = _handlers["uex"];

            var handler = startHandler;
            
            string? commandResultValue = null;

            Task.Run(_notificationSender.Run);

            while (handler != null) 
            {
                try
                {
                    var commandResult = handler.Execute(commandResultValue, _handlers);

                    handler = commandResult.NextCommandRequestHandler;
                    commandResultValue = commandResult.Result;
                }
                catch(UserInputException ex) // При ошибке ввода можно либо выйти в главное меню либо повторить текущую команда с сохраненными корректными данными ввода.
                {
                    var exCommandResult = userInputExceptionHandler.Execute(ex.Message, _handlers);

                    if (exCommandResult.NextCommandRequestHandler != null) 
                    {
                        handler = exCommandResult.NextCommandRequestHandler;
                        commandResultValue = exCommandResult.Result;
                    }
                   else
                        commandResultValue = ex.Value;
                }
                catch(Exception ex) // Сразу выход в главное меню.
                {
                    exceptionHandler.Execute(ex.Message, _handlers);
                    handler = startHandler;
                    commandResultValue = null;
                }
            }


        }
    }
}
