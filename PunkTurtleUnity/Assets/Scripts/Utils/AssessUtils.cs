using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class AssessUtils
    {
        public static bool CheckRequirement<T>(ref T requirementType, MonoBehaviour owner, bool checkChildren = false)
        {
            if (requirementType != null) return true;
            
            if (owner.gameObject.TryGetComponent(out requirementType))
            {
                return true;
            }
            if (checkChildren)
            {
                var found = owner.gameObject.GetComponentInChildren<T>();
                if (found != null)
                {
                    requirementType = found;
                    return true;
                }
            }
            DebugUtils.DebugLogErrorMsg($"Error: Component of type ${requirementType.GetType()} not found in ${owner.gameObject.name}!");
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>If the index is bigger or equal to 0 and smaller than the list's length.</returns>
        public static bool CheckValidIndex<T>(ref List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        public static bool CheckValidIndex<T>(ref T[,] grid, int i, int j)
        {
            return i >= 0 && i < grid.GetLength(0) && 
                   j >= 0 && j < grid.GetLength(1);
        }

        public static void MakeValidIndex<T>(ref T[,] grid, ref int i, ref int j)
        {
            i = Mathf.Clamp(i, 0, grid.GetLength(0));
            j = Mathf.Clamp(j, 0, grid.GetLength(1));
        }
        
        /// <summary>
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>If the index is bigger or equal to 0 and smaller than the list's length.</returns>
        public static bool CheckValidIndex<T>(ref T[] list, int index)
        {
            return index >= 0 && index < list.Length;
        }
    }
}