#if PACKAGE_ADDRESSABLES
using System;
using UnityEngine;

namespace Framework.Addressable.Data
{
    [Serializable]
    public class AddressableData<T>
    {
        [SerializeField] private string key;
        public string Key => key;
        public AddressableData() { }
        public AddressableData(string _key, T _addressableObject) : this()
        {
            key = _key;
            addressableObject = _addressableObject;
        }
        private T addressableObject;

        public void SetObject(T obj) => addressableObject = obj;
        public K GetObject<K>() where K :class
        {
            if (addressableObject == null) return default;
            return addressableObject as K;
        }
    }
}
#endif