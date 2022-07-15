using DoorAccessApplication.Core.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Core.Models
{
    public class LockHistoryEntry
    {
        public int Id { get; set; }
        public int LockId { get; set; }
        public string UserId { get; set; }

        public DateTime DateTime { get; set; }

        // open/close Action
        public StatusType Status { get; set; }

    }
}
