using System;
using System.Reflection;

public abstract class Singleton<T> where T : class
{
    private static T instance = null;
    private static object syncobj = new object();

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
        lock (syncobj)
        {
            if (instance == null)
            {
                Type t = typeof(T);

                // Ensure there are no public constructors...  
                ConstructorInfo[] ctors = t.GetConstructors();
                if (ctors.Length > 0)
                {
                    throw new InvalidOperationException(String.Format("{0} has at least one accesible ctor making it impossible to enforce singleton behaviour", t.Name));
                }

                // Create an instance via the private constructor  
                instance = (T)Activator.CreateInstance(t, true);
            }
        }
    }
}
