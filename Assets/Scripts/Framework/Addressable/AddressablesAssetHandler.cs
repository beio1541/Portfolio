#if PACKAGE_ADDRESSABLES
using System.Collections;

namespace Framework.Addressable
{
    public class AddressablesAssetHandler : AddressablesHandler<UnityEngine.Object, AddressablesAssets>
    {
        static public AddressablesAssetHandler MakeAddressablesAssetHandler(AddressablesAssets _data)
        {
            AddressablesAssetHandler handler = new AddressablesAssetHandler()
            {
                data = _data
            };
            return handler;
        }
        override public IEnumerator LoadAddressable()
        {
            handler = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.Object>(data.Key);
            yield return handler;
            if (IsDone && IsCompleted)
            {
                data.SetObject(handler.Result);
            }

        }
    }
}
#endif