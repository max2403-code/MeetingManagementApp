using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;

namespace MeetingManagementApp.Api.MeetingApp
{
    public class MeetingApp
    {
        private readonly IBackgroundService _notificationSender;
        private readonly IReadOnlyDictionary<string, ICommandRequestHandler> _handlers;
        private readonly IReadOnlyCollection<(string command, string? description)> _commands;

        public MeetingApp(IEnumerable<ICommandRequestHandler> handlers, IBackgroundService notificationSender)
        {
            _notificationSender = notificationSender;
            _handlers = handlers.ToDictionary(k => k.GetCommand());
            _commands = handlers.Select(x => (x.GetCommand(), x.GetCommandDescription())).ToArray();
        }

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
                    var commandResult = handler.Execute(commandResultValue, _handlers, _commands);

                    handler = commandResult.NextCommandRequestHandler;
                    commandResultValue = commandResult.Result;
                }
                catch(UserInputException ex)
                {
                    var exCommandResult = userInputExceptionHandler.Execute(ex.Message, _handlers, _commands);

                    if (exCommandResult.NextCommandRequestHandler != null) 
                    {
                        handler = exCommandResult.NextCommandRequestHandler;
                        commandResultValue = exCommandResult.Result;
                    }
                   else
                        commandResultValue = ex.Value;
                }
                catch(Exception ex) 
                {
                    exceptionHandler.Execute(ex.Message, _handlers, _commands);
                    handler = startHandler; //exCommandResult.NextCommandRequestHandler;
                    commandResultValue = null;
                }
            }

            //реализовать запись в хранилище
        }
    }
}
