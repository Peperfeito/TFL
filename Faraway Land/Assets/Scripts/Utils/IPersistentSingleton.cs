using UnityEngine;
using UnityEngine.Assertions;

public class IPersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _uniqueInstance = null;
#if UNITY_EDITOR
    private static bool _isApplicationQuiting = false;
#endif
    public static T Instance
    {
        get
        {
            if (_uniqueInstance == null
#if UNITY_EDITOR && !UNITY_WEBGL
                && !_isApplicationQuiting
#endif
                )
            {
                _uniqueInstance = FindFirstObjectByType<T>();
                if (_uniqueInstance == null)
                {
                    GameObject SingletonPrefab = Resources.Load<GameObject>(typeof(T).Name);
                    if (SingletonPrefab)
                    {
                        GameObject SingletonObject = Instantiate<GameObject>(SingletonPrefab);
                        if (SingletonObject != null) { _uniqueInstance = SingletonObject.GetComponent<T>(); }
                    }
                    if (_uniqueInstance == null) { _uniqueInstance = new GameObject(typeof(T).Name).AddComponent<T>(); }
                }
            }
            return _uniqueInstance;
        }
        private set
        {
            if (null == _uniqueInstance)
            {
                _uniqueInstance = value;
                DontDestroyOnLoad(_uniqueInstance.gameObject);
            }
            else if (_uniqueInstance != value)
            {
#if UNITY_EDITOR && !UNITY_WEBGL
                Debug.LogError("[" + typeof(T).Name + "] Tentou instanciar uma segunda instancia da classe IPresistentSingleton.");
#endif
                DestroyImmediate(value.gameObject);
            }
        }
    }

    public static bool IsInitialized()
    {
        return _uniqueInstance != null;
    }

    // Awake is called when the script instance is being loaded
    protected virtual void Awake() => Instance = this as T;

    // This function is called when the MonoBehaviour will be destroyed
    protected virtual void OnDestroy()
    {
        if (_uniqueInstance == this) { _uniqueInstance = null; }
    }

    protected virtual void OnApplicationQuit()
    {
#if UNITY_EDITOR
        _isApplicationQuiting = true;
#endif
    }
}