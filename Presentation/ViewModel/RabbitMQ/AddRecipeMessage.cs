namespace Presentation.ViewModel.RabbitMQ
{
    public class AddRecipeMessage :BaseMessage
    {
        public string RecipeName { get; set; }
        public string CategoryName { get; set; }


    }


}
