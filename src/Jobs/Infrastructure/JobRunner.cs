using Aviant.Application.Jobs;
using Aviant.Core.Timing;
using Hangfire;
using Hangfire.States;

namespace Aviant.Infrastructure.Jobs;

public class JobRunner : IJobRunner
{
    public JobRunner(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
    {
        BackgroundJobClient = backgroundJobClient;
        RecurringJobManager = recurringJobManager;
    }

    private IBackgroundJobClient BackgroundJobClient { get; }

    private IRecurringJobManager RecurringJobManager { get; }

    #region IJobRunner Members

    /// <inheritdoc />
    public string Run<TJob, TJobOptions>(Action<TJobOptions>? configureJobOptions = null)
        where TJob : IJob<TJobOptions>
        where TJobOptions : class, IJobOptions
    {
        return BackgroundJobClient.Enqueue<TJob>(job => job.PerformAsync(BuildOptions(configureJobOptions)));
    }

    /// <inheritdoc />
    public string RunInState<TJob, TJobOptions>(IState state, Action<TJobOptions>? configureJobOptions = null)
        where TJob : IJob<TJobOptions>
        where TJobOptions : class, IJobOptions
    {
        return BackgroundJobClient.Create<TJob>(
            job =>
                job.PerformAsync(BuildOptions(configureJobOptions)),
            state);
    }

    /// <inheritdoc />
    public string RunWithDelay<TJob, TJobOptions>(TimeSpan delay, Action<TJobOptions>? configureJobOptions = null)
        where TJob : IJob<TJobOptions>
        where TJobOptions : class, IJobOptions
    {
        return BackgroundJobClient.Schedule<TJob>(
            job =>
                job.PerformAsync(BuildOptions(configureJobOptions)),
            Clock.Now + delay);
    }

    /// <inheritdoc />
    public string RunAtDateTime<TJob, TJobOptions>(DateTime dateTime, Action<TJobOptions>? configureJobOptions = null)
        where TJob : IJob<TJobOptions>
        where TJobOptions : class, IJobOptions
    {
        return BackgroundJobClient.Schedule<TJob>(
            job =>
                job.PerformAsync(BuildOptions(configureJobOptions)),
            dateTime);
    }

    /// <inheritdoc />
    public string RunAfter<TJob, TJobOptions>(string previousJobId, Action<TJobOptions>? configureJobOptions = null)
        where TJob : IJob<TJobOptions>
        where TJobOptions : class, IJobOptions
    {
        return BackgroundJobClient.ContinueJobWith<TJob>(
            previousJobId,
            job => job.PerformAsync(BuildOptions(configureJobOptions)));
    }

    /// <inheritdoc />
    public string RunRecurring<TJob, TJobOptions>(
        string               jobId,
        string               cron,
        Action<TJobOptions>? configureJobOptions = null)
        where TJob : IJob<TJobOptions>
        where TJobOptions : class, IJobOptions
    {
        RecurringJobManager.AddOrUpdate<TJob>(
            jobId,
            job => job.PerformAsync(BuildOptions(configureJobOptions)),
            cron);

        return jobId;
    }

    /// <inheritdoc />
    public void TriggerRecurringJob(string id) => RecurringJobManager.Trigger(id);

    /// <inheritdoc />
    public void RemoveRecurringJob(string id) => RecurringJobManager.RemoveIfExists(id);

    #endregion

    private static TJobOptions BuildOptions<TJobOptions>(Action<TJobOptions>? configureJobOptions)
        where TJobOptions : class, IJobOptions
    {
        var jobOptions = Activator.CreateInstance<TJobOptions>();

        configureJobOptions?.Invoke(jobOptions);

        return jobOptions;
    }
}
