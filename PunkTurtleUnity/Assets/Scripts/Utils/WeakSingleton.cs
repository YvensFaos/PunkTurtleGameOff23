using UnityEngine;

namespace Utils
{
    public class WeakSingleton<T> : MonoBehaviour
    {
        #region Singleton
        private static T singleton;
        public static T GetSingleton() => singleton;
        private static GameObject singletonObject;
    
        protected void ControlSingleton()
        {
            if (singleton != null)
            {
                //Destroy the current singleton, so the new one takes its place
                Destroy(singletonObject);
                return;
            }
            //Set the singleton to be this object
            singleton = GetComponent<T>();
            singletonObject = gameObject;
        }
        #endregion

        private void Awake()
        {
            ControlSingleton();
        }
    }
}