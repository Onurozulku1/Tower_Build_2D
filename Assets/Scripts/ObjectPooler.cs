using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton
    public static ObjectPooler instance;
    private void Awake()
    {
        instance = this;

        CreateQueue();
    }
    #endregion


    public GameObject SpawnFromPool(string tag,Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log(tag + " mevcut deðil");
            return null;
        }

        GameObject spawnObject = poolDictionary[tag].Dequeue();
        spawnObject.SetActive(true);
        spawnObject.transform.position = position;
        spawnObject.transform.rotation = rotation;
        poolDictionary[tag].Enqueue(spawnObject);

        return spawnObject;
    }

    void CreateQueue()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            Transform parentGo = new GameObject(pool.tag + " Parent").transform;
            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.prefab, parentGo);
                go.SetActive(false);
                objectPool.Enqueue(go);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
}
