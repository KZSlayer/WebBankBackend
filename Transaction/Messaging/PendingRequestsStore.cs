using System.Collections.Concurrent;

public class PendingRequestsStore
{
    public ConcurrentDictionary<string, TaskCompletionSource<int?>> PendingRequests { get; } = new();
    public void AddRequest(string correlationId, TaskCompletionSource<int?> tcs)
    {
        PendingRequests[correlationId] = tcs;
    }
    public bool TryRemove(string correlationId, out TaskCompletionSource<int?> tcs)
    {
        return PendingRequests.TryRemove(correlationId, out tcs);
    }
}