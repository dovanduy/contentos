using BatchjobService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Models
{
    public class AlgorithmDataBeforeModel
    {
        public int IdUser { get; set; }
        public int IdTag { get; set; }
        public double TimeInTeraction { get; set; }
    }
    public class ListTaskModel 
    {
        public int IdUser { get; set; }
        public List<TaskInterModel> IdTask { get; set; }
    }
    public class TaskInterModel : IEquatable<TaskInterModel>
    {
        public int Id { get; set; }
        public double Interaction { get; set; }

        public bool Equals(TaskInterModel other)
        {
            if (other is null)
                return false;

            return this.Id == other.Id && this.Interaction == other.Interaction;
        }

        public override bool Equals(object obj) => Equals(obj as TaskInterModel);
        public override int GetHashCode() => (Id, Interaction).GetHashCode();
    }
}
