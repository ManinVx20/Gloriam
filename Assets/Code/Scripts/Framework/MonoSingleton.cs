using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).ToString();
                    _instance = go.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}

public class PersistentMonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            _instance = FindObjectOfType<T>();

            if (_instance == null)
            {
                GameObject go = new GameObject();
                go.name = typeof(T).ToString();
                _instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }
}
