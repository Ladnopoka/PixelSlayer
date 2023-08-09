using UnityEngine;

/*A static instance is similar to a singleton, but instead of destroying any new
 instances, it overrides the current instance. This is handy for resetting the state 
 and saves you doing it manually*/
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
      Instance = null;
      Destroy(gameObject);
    }
}

/* This transforms the static instance into a basic singleton. This will destroy any new
 versions created, leaving the original instance intact*/
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour{
    protected override void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        base.Awake();
    }
}

/*Finally we have persistent version of the singleton. This will survive through scene
 loads. Perfect for system classes which require stateful, persistent data. Or audio sources
 where music plays through loading screens, etc*/
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
