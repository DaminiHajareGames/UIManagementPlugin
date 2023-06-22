using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.UIPackage
{
    public static class MyExtention 
    {
        public static void Open(this GameObject obj)
        {
            if(obj != null)
            {
                obj.SetActive(true);
            }
        }

        public static void Close(this GameObject obj)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}