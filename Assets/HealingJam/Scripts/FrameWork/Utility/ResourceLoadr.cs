using UnityEngine;

namespace HealingJam
{
    public static class ResourceLoader
    {
        public static T LoadOrigin<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public static T LoadAndInstaniate<T>(string path, Transform parent = null) where T : Object
        {
            T origin = LoadOrigin<T>(path);
            return Object.Instantiate(origin, parent);
        }
    }
}