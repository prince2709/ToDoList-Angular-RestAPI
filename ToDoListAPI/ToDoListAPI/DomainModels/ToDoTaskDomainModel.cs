using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAPI.DomainModels
{
    public class ToDoTaskDomainModel
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string DueDate { get; set; }
        public string CompletedDate { get; set; }
        public string Status { get; set; }
        public int? AssignedToUserID { get; set; }
        public UserDomainModel AssignedTo { get; set; }
    }
}
