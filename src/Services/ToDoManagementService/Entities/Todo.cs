namespace ToDoManagementService.Api.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public int? Priority { get; set; }
        public string DueDate { get; set; }
        public bool? Done{ get; set; }
        public string Description { get; set; }
    }
}