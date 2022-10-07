#if PACKAGE_ADDRESSABLES
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Framework.Addressable
{

    abstract public class AddressablesHandler<T, K> where K : Data.AddressableData<T>
    {
        protected K data = null;
        public bool IsDone => handler.IsDone;
        public bool IsCompleted => handler.Status == AsyncOperationStatus.Succeeded;
        protected AsyncOperationHandle<T> handler;
        abstract public IEnumerator LoadAddressable();
        public void ReleaseObject() => UnityEngine.AddressableAssets.Addressables.Release(handler);
        public TT GetObject<TT>() where TT : class
        {
            if (data == null) return default;
            return data.GetObject<TT>();
        }
    }
}
#endif