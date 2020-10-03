namespace GPK_RePack.Core.Updater
{
    public interface IUpdaterCheckCallback
    {
        void PostUpdateResult(bool updateAvailable);
    }
}
