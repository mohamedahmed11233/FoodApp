namespace Presentation.ViewModel.RabbitMQ
{
    public interface IBaseMessage<T> where T : BaseMessage
    {
       public Task ConsumeAsync(T message);
    }
}
