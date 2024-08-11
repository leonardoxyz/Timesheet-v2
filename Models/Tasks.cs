namespace Appointments.Models
{
    public class Tasks
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int ProjectId { get; private set; }

        public Tasks(int id, string name, int projectId)
        {
            Id = id;
            Name = name;
            ProjectId = projectId;
        }
    }

}
