using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utills
{
    public static class HelperMethods
    {
        public static bool HasComponent<T>(this GameObject obj) where T : class
        {
            return obj.GetComponent<T>() != null;
        }
    }
}
