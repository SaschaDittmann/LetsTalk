using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace LetsTalk.Common
{
    public class BusyIndicatorServiceFilter : IServiceFilter
    {
        private int _callCount;
        private readonly object _countLock = new object();
        private readonly Action<bool> _busyIndicator;

        public BusyIndicatorServiceFilter(Action<bool> busyIndicator)
        {
            _busyIndicator = busyIndicator;
        }

        public Windows.Foundation.IAsyncOperation<IServiceFilterResponse> Handle(IServiceFilterRequest request, IServiceFilterContinuation continuation)
        {
            return HandleAsync(request, continuation).AsAsyncOperation();
        }

        private async Task<IServiceFilterResponse> HandleAsync(IServiceFilterRequest request, IServiceFilterContinuation continuation)
        {
            var invokeBusy = false;

            lock (_countLock)
            {
                if (_callCount == 0)
                {
                    invokeBusy = true;
                }
                _callCount++;
            }

            if (invokeBusy)
            {
                _busyIndicator.Invoke(true);
            }

            var response = await continuation.Handle(request).AsTask();
            var invokeIdle = false;

            lock (_countLock)
            {
                if (_callCount == 1)
                {
                    invokeIdle = true;
                }
                _callCount--;
            }

            if (invokeIdle)
            {
                _busyIndicator.Invoke(false);
            }

            return response;
        }
    }        
}