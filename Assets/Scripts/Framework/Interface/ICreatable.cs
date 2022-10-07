namespace Framework.Interface
{
    public interface ICreatable
    {
        bool IsInitialized { get; }
        void Initialize();
    }
}

