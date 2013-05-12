using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LetsTalk.Common
{
    public class BusyHandler : DelegatingHandler
    {
        private int _callCount;
        private readonly Action<bool> _busyIndicator;

        public BusyHandler(Action<bool> busyIndicator)
        {
            _busyIndicator = busyIndicator;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // update the count by one in a single atomic operation. 
            // If we get a 1 back, we know we just went 'busy'
            var outgoingCount = Interlocked.Increment(ref _callCount);
            if (outgoingCount == 1)
            {
                _busyIndicator(true);
            }

            var response = await base.SendAsync(request, cancellationToken);

            // decrement the count by one in a single atomic operation.
            // If we get a 0 back, we know we just went 'idle'
            var incomingCount = Interlocked.Decrement(ref _callCount);
            if (incomingCount == 0)
            {
                _busyIndicator(false);
            }

            return response;
        }
    }
}