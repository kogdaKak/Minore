using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an object pool for managing and reusing GameObjects efficiently.
/// </summary>
public class ObjectPool
{
    private readonly GameObject prefab; // The prefab used to create new GameObject instances.
    private readonly List<GameObject> free = new(); // List of available (free) GameObject instances.
    private readonly List<GameObject> busy = new(); // List of busy (in use) GameObject instances.
    private readonly int enlargePoolMinimumAmount; // Minimum amount to enlarge the pool when there are no free objects available.

    /// <summary>
    /// Event triggered when an object is created in the pool.
    /// </summary>
    public event Action ObjectCreat;

    /// <summary>
    /// Event triggered when an object is retrieved from the pool.
    /// </summary>
    public event Action ObjectGet;

    /// <summary>
    /// Event triggered when an object is returned to the pool.
    /// </summary>
    public event Action ObjectReturn;

    /// <summary>
    /// Initializes a new instance of the ObjectPool class.
    /// </summary>
    /// <param name="prefab">The prefab used to create new GameObject instances.</param>
    /// <param name="initialPoolSize">Initial size of the pool.</param>
    /// <param name="enlargePoolMinimumAmount">Minimum amount to enlarge the pool when there are no free objects available (default is 1).</param>
    public ObjectPool(GameObject prefab, int initialPoolSize, int enlargePoolMinimumAmount = 1)
    {
        this.prefab = prefab;
        this.enlargePoolMinimumAmount = enlargePoolMinimumAmount > 0 ? enlargePoolMinimumAmount : 1;
        PopulatePool(initialPoolSize);
    }

    /// <summary>
    /// Populates the object pool with the specified number of initial objects.
    /// </summary>
    /// <param name="initialPoolSize">The number of objects to pre-populate the pool with.</param>
    private void PopulatePool(int initialPoolSize)
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateObject();
        }
    }

    /// <summary>
    /// Creates a new GameObject instance and adds it to the pool.
    /// </summary>
    /// <returns>The newly created GameObject instance.</returns>
    private GameObject CreateObject()
    {
        GameObject obj = UnityEngine.Object.Instantiate(prefab);
        obj.SetActive(false);
        free.Add(obj);
        ObjectCreat?.Invoke();
        return obj;
    }

    /// <summary>
    /// Retrieves an available GameObject instance from the pool.
    /// </summary>
    /// <returns>An available GameObject instance from the pool.</returns>
    public GameObject Get()
    {
        if (free.Count <= 0)
        {
            for (int index = 0; index < enlargePoolMinimumAmount; index++)
            {
                CreateObject();
            }
        }

        GameObject obj = free[0];
        free.Remove(obj);
        busy.Add(obj);
        obj.SetActive(true);
        ObjectGet?.Invoke();
        return obj;
    }

    /// <summary>
    /// Returns a GameObject instance to the pool.
    /// </summary>
    /// <param name="obj">The GameObject instance to return to the pool.</param>
    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        busy.Remove(obj);
        free.Add(obj);
        ObjectReturn?.Invoke();
    }

    /// <summary>
    /// Returns all busy GameObject instances to the pool.
    /// </summary>
    public void ReturnAll()
    {
        foreach (var item in busy)
        {
            free.Add(item);
        }
        busy.Clear();
    }
}
