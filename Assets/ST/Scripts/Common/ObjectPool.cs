using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace ST
{

    public class ObjectPool<T> where T : Component
    {
        private T prefab;
        private Transform poolParent;
        private Queue<T> availableObjects = new Queue<T>();
        private List<T> allObjects = new List<T>();

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            this.prefab = prefab;

            // Pool 부모 오브젝트 생성
            GameObject poolObject = new GameObject($"{prefab.name}_Pool");
            poolParent = poolObject.transform;
            if (parent != null)
                poolParent.SetParent(parent);

            // 초기 오브젝트 생성
            for (int i = 0; i < initialSize; i++)
            {
                CreateNewObject();
            }
        }

        // 새 오브젝트 생성
        private T CreateNewObject()
        {
            T newObj = GameObject.Instantiate(prefab, poolParent);
            newObj.gameObject.SetActive(false);
            availableObjects.Enqueue(newObj);
            allObjects.Add(newObj);
            return newObj;
        }

        // 풀에서 가져오기
        public T Get(Vector3 position, Quaternion rotation)
        {
            T obj;

            // 사용 가능한 오브젝트가 없으면 새로 생성
            if (availableObjects.Count == 0)
            {
                obj = CreateNewObject();
                Debug.LogWarning($"Pool exhausted! Creating new {prefab.name}");
            }
            else
            {
                obj = availableObjects.Dequeue();
            }

            // 위치 설정
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.gameObject.SetActive(true);

            // IPoolable 인터페이스가 있으면 OnSpawn 호출
            IPoolable poolable = obj.GetComponent<IPoolable>();
            poolable?.OnSpawn();

            return obj;
        }

        // 풀로 반환
        public void Return(T obj)
        {
            // IPoolable 인터페이스가 있으면 OnDespawn 호출
            IPoolable poolable = obj.GetComponent<IPoolable>();
            poolable?.OnDespawn();

            obj.gameObject.SetActive(false);
            availableObjects.Enqueue(obj);
        }

        // 모든 오브젝트 반환
        public void ReturnAll()
        {
            foreach (T obj in allObjects)
            {
                if (obj.gameObject.activeSelf)
                {
                    Return(obj);
                }
            }
        }

        // 통계
        public int TotalCount => allObjects.Count;
        public int ActiveCount => allObjects.Count - availableObjects.Count;
        public int AvailableCount => availableObjects.Count;
    }
}