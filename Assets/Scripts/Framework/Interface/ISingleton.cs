using UnityEngine;

namespace Framework.Interface
{
    public interface ISingleton : ICreatable
    {
        void Initialize(MonoBehaviour behaviour);
    }
}