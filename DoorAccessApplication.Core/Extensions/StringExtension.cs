using DoorAccessApplication.Core.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Core.Extensions
{
    public static class StringExtension
    {
        public static StatusType GetStatus(this string status)
        {
            if (!Enum.TryParse(status, out StatusType myStatus))
                return StatusType.Unknown;
            return myStatus;
        }
    }
}
