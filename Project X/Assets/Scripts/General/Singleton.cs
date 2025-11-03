using UnityEngine;
class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
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