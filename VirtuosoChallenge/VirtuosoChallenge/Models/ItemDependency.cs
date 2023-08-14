namespace VirtuosoChallenge.Models
{
    /// <summary>
    /// This class is responsible for storing a single dependency of an Item
    /// </summary>
    public class ItemDependency
    {
        public string Item { get; set; }
        public string Dependency { get; set; }
    }
}
