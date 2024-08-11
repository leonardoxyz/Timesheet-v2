﻿namespace Appointments.Models
{
    public class Project
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Project(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
