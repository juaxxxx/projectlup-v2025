using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerBlackboard : MonoBehaviour
    {
        private Dictionary<WorkerBlackboardKey, object> data = new (); // 실제 데이터 저장소
        private Dictionary<string, WorkerBlackboardKey> keyRegistry = new (); // 키 캐싱 ( 같은 문자열인 키 생성 방지 )

        
        // 키가 이미 존재하면 기존 키 반환, 없으면 새로 생성하여 등록 후 반환
        public WorkerBlackboardKey GetOrRegisterKey(string keyName)
        {
            if (keyRegistry.TryGetValue(keyName, out var existingKey)) { return existingKey; }

            var newKey = new WorkerBlackboardKey(keyName);
            keyRegistry[keyName] = newKey;

            return newKey;
        }

        public void SetValue<T>(string keyName, T value)
        {
            WorkerBlackboardKey key = GetOrRegisterKey(keyName);
            SetValue(key, value);
        }

        public void SetValue<T>(WorkerBlackboardKey key, T value)
        {
            data[key] = value;
        }

        public T GetValue<T>(string keyName)
        {
            if (keyRegistry.TryGetValue(keyName, out var key)) { return GetValue<T>(key); }
            return default(T);
        }

        public T GetValue<T>(WorkerBlackboardKey key)
        {
            if (data.TryGetValue(key, out object val))
            {
                // 타입 캐스팅 (저장된게 int인데 float로 달라고 하면 에러나거나 기본값)
                if (val is T castedVal) return castedVal;
            }
            return default(T);
        }

        public bool HasKey(string keyName)
        {
            return keyRegistry.TryGetValue(keyName, out var key) && data.ContainsKey(key);
        }

        public bool TryGetValue<T>(string keyName, out T value)
        {
            if (keyRegistry.TryGetValue(keyName, out var key))
            {
                value = GetValue<T>(key);
    
                if (data.TryGetValue(key, out var v) && v is T t)
                {
                    value = t;
                    return true;
                }
            }
            
            value = default;
            return false;
        }


        public void Remove(string keyName)
        {
            if (keyRegistry.TryGetValue(keyName, out var key))
                data.Remove(key);
        }

    }
}

