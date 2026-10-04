using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTAU
{
    public static class CurrentSession
    {
        public static int SelectedTableId { get; set; }
        public static string OrderType { get; set; } //Whether dine-in or takeaway
    }
}
