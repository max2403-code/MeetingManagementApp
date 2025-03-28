using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;

namespace MeetingManagementApp.Api.MeetingApp
{
    public class MeetingApp
    {
        private readonly ICommandRequestHandler _startHandler;
        private readonly ICommandRequestHandler _exceptionHandler;
        private readonly IBackgroundService _notificationSender;

        public MeetingApp( IEnumerable<ICommandRequestHandler> handlers, IBackgroundService notificationSender)
        {
            _startHandler = handlers.FirstOrDefault(x => x.GetCommand().Equals("m"));
            _exceptionHandler = handlers.FirstOrDefault(x => x.GetCommand().Equals("ex"));
            _notificationSender = notificationSender;
        }

        public void Run()
        {
            var handler = _startHandler;
            string? commandResultValue = null;

            Task.Run(_notificationSender.Run);

            while (handler != null) 
            {
                try
                {
                    var commandResult = handler.Execute(commandResultValue);

                    handler = commandResult.NextCommandRequestHandler;
                    commandResultValue = commandResult.Result;
                }
                catch(BusinessException ex)
                {
                    commandResultValue = ex.Value;
                    _exceptionHandler.Execute(ex.Message);
                }
                catch(Exception ex) 
                {
                    var exCommandResult = _exceptionHandler.Execute(ex.Message);
                    handler = _startHandler; //exCommandResult.NextCommandRequestHandler;
                    commandResultValue = null;
                }
            }

            //реализовать запись в хранилище
        }
    }
}
