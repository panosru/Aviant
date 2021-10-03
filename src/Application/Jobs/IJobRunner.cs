namespace Aviant.DDD.Application.Jobs
{
    using System;
    using System.Threading.Tasks;
    using Hangfire.States;

    public interface IJob<in TJobOptions>
        where TJobOptions : class, IJobOptions
    {
        Task PerformAsync(TJobOptions jobOptions);
    }

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
}
