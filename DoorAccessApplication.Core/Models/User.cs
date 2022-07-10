using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public List<Lock> Locks { get; set; } = new List<Lock>();
    }
}
