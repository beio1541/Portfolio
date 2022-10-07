#if PACKAGE_ADDRESSABLES
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Framework.Addressable
{
    public class AddressablesScene : Data.AddressableData<SceneInstance>
    {
        public AddressablesScene(LoadSceneMode _loadMode = LoadSceneMode.Single, bool _activateOnLoad = true, int _priority = 100)
        {
            SetData(_loadMode, _activateOnLoad, _priority);
        }

        public AddressablesScene(string _key, SceneInstance _addressableObject, LoadSceneMode _loadMode = LoadSceneMode.Single, bool _activateOnLoad = true, int _priority = 100) : base(_key, _addressableObject)
        {
            SetData(_loadMode, _activateOnLoad, _priority);
        }
        private void SetData(LoadSceneMode _loadMode = LoadSceneMode.Single, bool _activateOnLoad = true, int _priority = 100)
        {
            LoadMode = _loadMode;
            ActivateOnLoad = _activateOnLoad;
            Priority = _priority;
        }

        public LoadSceneMode LoadMode { get; private set; } = LoadSceneMode.Single;
        public bool ActivateOnLoad { get; private set; } = true;
        public int Priority { get; private set; } = 100;
    }
}
#endif