using UnityEngine;

//Quick singleton initialization for managers
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; protected set; }

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = (T)this;
        else
            Destroy(gameObject);
    }
}
