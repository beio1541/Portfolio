namespace Framework.Interface
{
    public interface IDestroyable
    {
        bool IsDestroyed { get; }
        void Destroy();
    }
}