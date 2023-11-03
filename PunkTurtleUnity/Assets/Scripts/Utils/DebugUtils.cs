using System;
using UnityEngine;

namespace Utils
{
    public static class DebugUtils
    {
        public static void DebugArea(Vector3 position, float distance, float duration = 3.0f)
        {
            // if (!GameManager.Singleton().Debug) return;
            Debug.DrawLine(position, position + distance * Vector3.right, Color.blue, duration);
            Debug.DrawLine(position, position + distance * Vector3.up, Color.green, duration);
            Debug.DrawLine(position, position + distance * Vector3.forward, Color.red, duration);
        }

        public static void DebugLogMsg(string msg)
        {
            // if (!GameManager.Singleton().Debug) return;
            Debug.Log(msg);
        }

        public static void DebugAssertion(bool condition, string msg)
        {
            // if (!GameManager.Singleton().Debug) return;
            Debug.Assert(condition, msg);
        }
        
        public static void DebugLogErrorMsg(string msg)
        {
            // if (!GameManager.Singleton().Debug) return;
            Debug.LogError(msg);
        }

        public static void DebugLogException(Exception exception)
        {
            // if (!GameManager.Singleton().Debug) return;
            Debug.LogError(exception.ToString());
            Debug.LogError(exception.StackTrace);
        }
    }
}