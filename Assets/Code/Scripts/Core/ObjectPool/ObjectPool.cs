using StormDreams;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour, IObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private List<T> instanceList = new List<T>();
    private Stack<T> reusableInstanceStack = new Stack<T>();

    [SerializeField]
    private T _prefab;

    public T GetPrefabInstance()
    {
        T instance;

        if (reusableInstanceStack.Count == 0)
        {
            instance = Instantiate(_prefab);

            instanceList.Add(instance);
        }
        else
        {
            instance = reusableInstanceStack.Pop();

            instance.transform.SetParent(null);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = Vector3.one;
            instance.transform.localEulerAngles = Vector3.zero;

            instance.gameObject.SetActive(true);
        }

        instance.Pool = this;
        instance.PrepareToUse();

        return instance;
    }

    public void ReturnToPool(T instance)
    {
        instance.gameObject.SetActive(false);

        instance.transform.SetParent(transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;
        instance.transform.localEulerAngles = Vector3.zero;


        reusableInstanceStack.Push(instance);
    }

    public void ReturnToPool(object instance)
    {
        if (instance is T)
        {
            ReturnToPool(instance as T);
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < instanceList.Count; i++)
        {
            if (instanceList[i] != null)
            {
                instanceList[i].Dispose();
            }
        }

        instanceList.Clear();
    }

    public bool Exist()
    {
        return this != null;
    }
}
