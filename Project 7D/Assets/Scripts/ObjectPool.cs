using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : SingleTon<ObjectPool>
{

    [System.Serializable]
    public class ResourcePool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [System.Serializable]
    public class ResourcePoolGroup
    {
        public List<ResourcePool> resourcePools;
    }

    public List<ResourcePoolGroup> resourcePoolGroups;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    protected new void Awake()
    {
        ResourcePooling();
    }


    private Dictionary<string, Transform> poolParents = new(); // 태그별 부모
    void ResourcePooling()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        poolParents = new Dictionary<string, Transform>();

        foreach (ResourcePoolGroup resourcePoolGroup in resourcePoolGroups)
        {
            
            foreach (ResourcePool resourcePool in resourcePoolGroup.resourcePools)
            {
                // 1. 각 리소스 타입별 부모 생성
                GameObject parentObj = new GameObject($"Resource({resourcePool.tag})");
                parentObj.transform.SetParent(this.transform); // ObjectPool 아래 정리
                poolParents[resourcePool.tag] = parentObj.transform;

                // 2. 풀 초기화
                Queue<GameObject> objectPool = new();

                for (int i = 0; i < resourcePool.size; i++)
                {
                    GameObject obj = Instantiate(resourcePool.prefab, parentObj.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(resourcePool.tag, objectPool);
            }
        }
    }


    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(obj);
        return obj;
    }
}
