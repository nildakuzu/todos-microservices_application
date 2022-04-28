using EventBus.Base.Events;

namespace ToDoManagementService.Api.Events
{
    public class TodoCreatedIntegrationEvent : IntegrationEvent
    {
        public int ToDoId { get; set; }

        public string UserName { get; set; }

        public string DueDate { get; set; }

        public TodoCreatedIntegrationEvent(int toDoId, string userName, string dueDate)
        {
            ToDoId = toDoId;
            UserName = userName;
            DueDate = dueDate;
        }
    }
}
