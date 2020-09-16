namespace Aviant.DDD.Application.UseCases
{
    using System.Threading.Tasks;

    public interface IUseCase
    { }
    
    public interface IUseCase<in TUseCaseOutput> : IUseCase
        where TUseCaseOutput : class, IUseCaseOutput
    {
        public Task Execute<TInputData>(TUseCaseOutput output, TInputData data)
            where TInputData : class;
    }
}