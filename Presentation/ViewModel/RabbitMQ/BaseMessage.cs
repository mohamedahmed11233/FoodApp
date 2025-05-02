namespace Presentation.ViewModel.RabbitMQ
{
    public class BaseMessage
    {
        public DateTime Date { get; set; }
        public string Publisher { get; set; }
        public string Action { get; set; }
        public string Type { get; set; } 

    }
}
