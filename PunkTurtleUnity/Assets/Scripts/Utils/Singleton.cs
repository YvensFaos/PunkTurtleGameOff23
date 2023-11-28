using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour
    {
        #region Singleton
        private static T singleton;
        public static T GetSingleton() => singleton;
        private static GameObject singletonObject;
    
        protected void ControlSingleton()
        {
            if (singletonObject != null || singleton != null)
            {
                //Destroy the other singleton
                Destroy(gameObject);
                return;
            }
            //Set the singleton to be this object
            singleton = GetComponent<T>();
            singletonObject = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        private void Awake()
        {
            ControlSingleton();
        }
    }
}