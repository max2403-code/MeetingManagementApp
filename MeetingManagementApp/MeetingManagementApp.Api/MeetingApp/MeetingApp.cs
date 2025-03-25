using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;

namespace MeetingManagementApp.Api.MeetingApp
{
    internal class MeetingApp
    {
        private readonly ICommandRequestHandler _startHandler;
        private readonly ICommandRequestHandler _exceptionHandler;

        public MeetingApp(ICommandRequestHandler startHandler, ICommandRequestHandler exceptionHandler)
        {
            _startHandler = startHandler;
            _exceptionHandler = exceptionHandler;
        }

        public void Run()
        {
            var handler = _startHandler;
            string? commandResultValue = null;

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
                    handler = exCommandResult.NextCommandRequestHandler;
                    commandResultValue = exCommandResult.Result;
                }
            }

            //реализовать запись в хранилище
        }
    }
}
