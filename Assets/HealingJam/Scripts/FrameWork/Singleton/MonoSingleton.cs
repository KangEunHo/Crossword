using UnityEngine;

namespace HealingJam
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance = null;
        private static object syncobj = new object();
        public static bool Instantiated { get; private set; } = false;
        public static bool AppIsQuitting { get; private set; } = false;

        public bool Persistent = true;
        public bool LazyInit = true;

        public static T Instance
        {
            get
            {
                lock (syncobj)
                {
                    if (instance == null)
                    {
                        CreateInstance();
                    }
                    return instance;
                }
            }
        }

        private static void CreateInstance()
        {
            if (Instantiated) return;

            var type = typeof(T);
            var objects = FindObjectsOfType<T>();
            if (objects.Length > 0)
            {
                if (objects.Length > 1)
                {
                    Debug.LogWarning("There is more than one instance of Singleton of type \"" + type +
                                     "\". Keeping the first one. Destroying the others.");
                    for (var i = 1; i < objects.Length; i++) Destroy(objects[i].gameObject);
                }
                instance = objects[0];
            }
            else
            {
                instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
            }

            instance.Init();

            Instantiated = true;
        }

        protected virtual void OnApplicationQuit()
        {
            AppIsQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            instance = null;

            if (AppIsQuitting == false && Persistent == false)
                Instantiated = false;
        }

        protected virtual void Awake()
        {
            if (Instantiated && instance != null && instance != this)
            {
                Debug.LogWarning("There is more than one instance of Singleton of type \"" + typeof(T) +
                 "\". Keeping the first one. Destroying the others.");
                Destroy(gameObject);
                return;
            }

            if (LazyInit == false)
            {
                CreateInstance();
            }

            if (Persistent)
                DontDestroyOnLoad(gameObject);
        }

        public virtual void Init() { }
    }
}