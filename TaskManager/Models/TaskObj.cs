namespace TaskManager.Models {
    public class TaskObj
    {
        public int Id { get; set; }
        public int DueDay { get; set; }
        public int Priority { get; set; }
        public string UserEmail { get; set; }="user_email@gmail.com";
        public string Description { get; set; }

        public TaskObj()
        {
            
        }
    }
}