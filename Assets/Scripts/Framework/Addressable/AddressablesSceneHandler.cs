#if PACKAGE_ADDRESSABLES
using System.Collections;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Framework.Addressable
{
    public class AddressablesSceneHandler : AddressablesHandler<SceneInstance, AddressablesScene>
    {
        static public AddressablesSceneHandler MakeAddressablesSceneHandler(string _key, LoadSceneMode _loadMode = LoadSceneMode.Single, bool _activateOnLoad = true, int _priority = 100)
        {
            AddressablesSceneHandler handler = new AddressablesSceneHandler()
            {
                data = new AddressablesScene(_key, default, _loadMode, _activateOnLoad, _priority)
            };
            return handler;
        }
        static public AddressablesSceneHandler MakeAddressablesSceneHandler(AddressablesScene _data)
        {
            AddressablesSceneHandler handler = new AddressablesSceneHandler()
            {
                data = _data
            };
            return handler;
        }
        static public IEnumerator LoadAddressable(string _key, LoadSceneMode _loadMode = LoadSceneMode.Single, bool _activateOnLoad = true, int _priority = 100)
        {
            AddressablesSceneHandler handler = MakeAddressablesSceneHandler(_key, _loadMode, _activateOnLoad, _priority);
            yield return handler.LoadAddressable();
        }
        override public IEnumerator LoadAddressable()
        {
            handler = UnityEngine.AddressableAssets.Addressables.LoadSceneAsync(data.Key, data.LoadMode, data.ActivateOnLoad, data.Priority);
            yield return handler;

        }

    }
}
#endif