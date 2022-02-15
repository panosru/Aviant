namespace Aviant.Application.Jobs;

public interface IJob<in TJobOptions>
    where TJobOptions : class, IJobOptions
{
    Task PerformAsync(TJobOptions jobOptions);
}