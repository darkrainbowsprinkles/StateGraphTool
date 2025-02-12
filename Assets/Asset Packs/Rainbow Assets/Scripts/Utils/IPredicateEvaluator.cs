namespace RainbowAssets.Utils
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(EPredicate predicate, string[] parameters);
    }
}