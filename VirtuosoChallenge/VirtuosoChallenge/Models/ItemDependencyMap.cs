namespace VirtuosoChallenge.Models
{
    /// <summary>
    /// This class is responsible for storing set of dependencies for an Item
    /// </summary>
    public class ItemDependencyMap
    {
        public string Item { get; set; }
        public HashSet<string> Dependencies { get; set; }
    }
}
