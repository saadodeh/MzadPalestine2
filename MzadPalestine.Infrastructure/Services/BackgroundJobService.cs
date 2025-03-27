using System.Linq.Expressions;
using Hangfire;
using MzadPalestine.Core.Interfaces.Services;

namespace MzadPalestine.Infrastructure.Services;

public class BackgroundJobService : IBackgroundJobService
{
    public string Enqueue(Expression<Action> job)
    {
        return BackgroundJob.Enqueue(job);
    }

    public string Enqueue<T>(Expression<Action<T>> job)
    {
        return BackgroundJob.Enqueue<T>(job);
    }

    public string Schedule(Expression<Action> job, TimeSpan delay)
    {
        return BackgroundJob.Schedule(job, delay);
    }

    public string Schedule<T>(Expression<Action<T>> job, TimeSpan delay)
    {
        return BackgroundJob.Schedule<T>(job, delay);
    }

    public void RecurringJob(string jobId, Expression<Action> job, string cronExpression)
    {
        Hangfire.RecurringJob.AddOrUpdate(jobId, job, cronExpression);
    }

    public void RecurringJob<T>(string jobId, Expression<Action<T>> job, string cronExpression)
    {
        Hangfire.RecurringJob.AddOrUpdate<T>(jobId, job, cronExpression);
    }

    public bool Delete(string jobId)
    {
        try
        {
            BackgroundJob.Delete(jobId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteRecurringJob(string jobId)
    {
        try
        {
            Hangfire.RecurringJob.RemoveIfExists(jobId);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
