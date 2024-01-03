using Hangfire.States;

namespace Aviant.Application.Jobs;

public interface IJobRunner
{
    string Run<TJob, TJobOptions>(Action<TJobOptions>? configureJobOptions = null)
        where TJobOptions : class, IJobOptions
        where TJob : IJob<TJobOptions>;

    string RunInState<TJob, TJobOptions>(IState state, Action<TJobOptions>? configureJobOptions = null)
        where TJobOptions : class, IJobOptions
        where TJob : IJob<TJobOptions>;

    string RunWithDelay<TJob, TJobOptions>(TimeSpan delay, Action<TJobOptions>? configureJobOptions = null)
        where TJobOptions : class, IJobOptions
        where TJob : IJob<TJobOptions>;

    string RunAtDateTime<TJob, TJobOptions>(DateTime dateTime, Action<TJobOptions>? configureJobOptions = null)
        where TJobOptions : class, IJobOptions
        where TJob : IJob<TJobOptions>;

    string RunAfter<TJob, TJobOptions>(string previousJobId, Action<TJobOptions>? configureJobOptions = null)
        where TJobOptions : class, IJobOptions
        where TJob : IJob<TJobOptions>;

    string RunRecurring<TJob, TJobOptions>(
        string               jobId,
        string               cron,
        Action<TJobOptions>? configureJobOptions = null)
        where TJobOptions : class, IJobOptions
        where TJob : IJob<TJobOptions>;

    void TriggerRecurringJob(string id);

    void RemoveRecurringJob(string id);
}
