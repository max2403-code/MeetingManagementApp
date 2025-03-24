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
            string? requestValue = null;

            while (handler != null) 
            {
                try
                {
                    var commandResult = handler.Execute(requestValue);

                    handler = commandResult?.NextCommandRequestHandler;
                    requestValue = commandResult?.Result;
                }
                catch(BusinessException ex)
                {
                    requestValue = ex.Value;
                    _exceptionHandler.Execute(ex.Message);
                }
                catch(Exception ex) 
                {
                    _exceptionHandler.Execute(ex.Message);
                }
            }

            //реализовать запись в хранилище
        }
    }
}
