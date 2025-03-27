using System.Linq.Expressions;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IBackgroundJobService
{
    string Enqueue(Expression<Action> job);
    string Enqueue<T>(Expression<Action<T>> job);
    string Schedule(Expression<Action> job, TimeSpan delay);
    string Schedule<T>(Expression<Action<T>> job, TimeSpan delay);
    void RecurringJob(string jobId, Expression<Action> job, string cronExpression);
    void RecurringJob<T>(string jobId, Expression<Action<T>> job, string cronExpression);
    bool Delete(string jobId);
    bool DeleteRecurringJob(string jobId);
}
