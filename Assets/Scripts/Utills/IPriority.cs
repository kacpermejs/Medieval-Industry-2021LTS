using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utills
{
    public interface IPriority
    {
        int Priority { get; }
    }
    public class PriorityComparer : IComparer<IPriority>
    {
        public int Compare(IPriority x, IPriority y) => x.Priority > y.Priority ? 1 : x.Priority < y.Priority ? -1 : 0;

    }

}
