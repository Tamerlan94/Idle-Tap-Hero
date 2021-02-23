using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchHero
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public int amountToPool;
        public GameObject objectToPool;
        public bool shouldExpand;
    }


    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler SharedInstance;

        void Awake()
        {
            SharedInstance = this;
        }
        public List<ObjectPoolItem> itemsToPool;
        public List<GameObject> pooledObjects;
        public Transform parentTransform;        

        void Start()
        {
            pooledObjects = new List<GameObject>();
            foreach (ObjectPoolItem item in itemsToPool)
            {
                for (int i = 0; i < item.amountToPool; i++)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool, parentTransform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }
        }

        public GameObject GetPooledObject(string tag)
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].CompareTag(tag))
                {
                    return pooledObjects[i];
                }
            }
            foreach (ObjectPoolItem item in itemsToPool)
            {
                if (item.objectToPool.CompareTag(tag))
                {
                    if (item.shouldExpand)
                    {
                        GameObject obj = (GameObject)Instantiate(item.objectToPool);
                        obj.SetActive(false);
                        pooledObjects.Add(obj);
                        return obj;
                    }
                }
            }
            return null;
        }
        public void DisablePooledObject()
        {
            foreach (GameObject item in pooledObjects)
            {
                item.SetActive(false);
            }
        }

    }
}
