using System.Linq.Expressions;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IBackgroundJobService
{
    string Enqueue(Expression<Action> methodCall);
    string Enqueue<T>(Expression<Action<T>> methodCall);
    string Schedule(Expression<Action> methodCall, TimeSpan delay);
    string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);
    void Delete(string jobId);
    bool Requeue(string jobId);
}
