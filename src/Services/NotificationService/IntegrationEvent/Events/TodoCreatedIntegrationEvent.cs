using EventBus.Base.Events;

namespace NotificationService.Api.Events
{
    public class TodoCreatedIntegrationEvent : IntegrationEvent
    {
        public int ToDoId { get; private set; }

        public string UserName { get; private set; }

        public string DueDate { get; private set; }

        public TodoCreatedIntegrationEvent(int toDoId, string userName, string dueDate)
        {
            ToDoId = toDoId;
            UserName = userName;
            DueDate = dueDate;
        }
    }
}
