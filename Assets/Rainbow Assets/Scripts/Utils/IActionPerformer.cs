namespace RainbowAssets.Utils
{
    public interface IActionPerformer
    {
        void PerformAction(EAction action, string[] parameters);
    }
}