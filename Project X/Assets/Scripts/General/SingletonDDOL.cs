using UnityEngine;
public class SingletonDDOL<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    public static T Instance => _instance;
    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }
}