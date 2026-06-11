namespace Core.SaveSystem.Interfaces
{
    public interface ISavable
    {
        object Save();
        void Load(object state);
    }
}