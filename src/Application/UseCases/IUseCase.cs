namespace Aviant.DDD.Application.UseCases
{
    using System.Threading.Tasks;

    public interface IUseCase<in TUseCaseOutput>
        where TUseCaseOutput : class, IUseCaseOutput
    {
        public Task Execute();
        
        public void SetOutput(TUseCaseOutput output);
    }
}