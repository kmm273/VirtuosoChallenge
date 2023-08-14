using VirtuosoChallenge.Models;

///////////////////////////////////////////////////////////////////////////////////////
/// Main Program
var input = new string[,]
    {
				//dependency    //item
				{"t-shirt",     "dress shirt"},
                {"dress shirt", "pants"},
                {"dress shirt", "suit jacket"},
                {"tie",         "suit jacket"},
                {"pants",       "suit jacket"},
                {"belt",        "suit jacket"},
                {"suit jacket", "overcoat"},
                {"dress shirt", "tie"},
                {"suit jacket", "sun glasses"},
                {"sun glasses", "overcoat"},
                {"left sock",   "pants"},
                {"pants",       "belt"},
                {"suit jacket", "left shoe"},
                {"suit jacket", "right shoe"},
                {"left shoe",   "overcoat"},
                {"right sock",  "pants"},
                {"right shoe",  "overcoat"},
                {"t-shirt",     "suit jacket"}
    };

List<ItemDependency> itemDependencies = ConvertInputToItemDependencies(input);

List<ItemDependencyMap> itemDependencyMaps = BuildItemDependencyMaps(itemDependencies);

List<List<string>> dependencyOrder = GetDependencyOrder(itemDependencyMaps);


Console.WriteLine("Output: ");
foreach (List<string> items in dependencyOrder)
{
    Console.WriteLine(string.Join(",", items));
}
///////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Converts 2D String Array into List of ItemDependency Objects
/// </summary>
List<ItemDependency> ConvertInputToItemDependencies(string[,] input)
{

    List<ItemDependency> itemDependencies = new List<ItemDependency>();

    for (int i = 0; i < input.GetLength(0); i++)
    {
        string dependency = input[i, 0];
        string item = input[i, 1];

        itemDependencies.Add(new ItemDependency()
        {
            Item = item,
            Dependency = dependency
        });
    }
    return itemDependencies;
}


/// <summary>
/// Builds List of all Dependencies for each Item and returns List of ItemDependencyMap Objects
/// </summary>
List<ItemDependencyMap> BuildItemDependencyMaps(List<ItemDependency> itemDependencies)
{
    //Unique list of items store in HashSet for O(1) search time
    HashSet<string> items = new HashSet<string>();

    List<ItemDependencyMap> itemDependencyMaps = new List<ItemDependencyMap>();

    foreach (ItemDependency itemDependency in itemDependencies)
    {
        if (items.Contains(itemDependency.Item))
        {
            //Search dependency list for item and add dependency
            itemDependencyMaps.Single(d => d.Item == itemDependency.Item).Dependencies.Add(itemDependency.Dependency);
        }
        else
        {
            //Add initial dependency for item
            itemDependencyMaps.Add(new ItemDependencyMap()
            {
                Item = itemDependency.Item,
                Dependencies = new HashSet<string>() { itemDependency.Dependency }
            });
        }
        items.Add(itemDependency.Item);

        //Add dependency as an item with dependencies TBD
        if (!items.Contains(itemDependency.Dependency))
        {
            itemDependencyMaps.Add(new ItemDependencyMap()
            {
                Item = itemDependency.Dependency,
                Dependencies = new HashSet<string>()
            });
        }
        items.Add(itemDependency.Dependency);
    }

    return itemDependencyMaps;
}

/// <summary>
/// Builds List of all Dependencies for each Item and returns List of ItemDependencyMap Objects
/// </summary>
List<List<string>> GetDependencyOrder(List<ItemDependencyMap> itemDependencyMaps)
{

    List<List<string>> dependencyOrder = new List<List<string>>();
    HashSet<string> handledItems = new HashSet<string>();


    void AddItemsToDependencyOrder(List<List<string>> dependencyOrder, HashSet<string> handledItems, List<string> items)
    {
        //Alphabetize and Add Items to Dependency Order
        dependencyOrder.Add(items.OrderBy(i => i).ToList());
        //Mark Items as Handled
        handledItems.UnionWith(items);
    }

    //Get Items with No dependencies 
    List<string> itemsWithNoDependencies = itemDependencyMaps.Where(d => d.Dependencies.Count == 0).Select(d => d.Item).ToList();
    //Add as First line item in Dependency Order
    AddItemsToDependencyOrder(dependencyOrder, handledItems, itemsWithNoDependencies);

    do
    {
        //Filter out handled items from dependency maps
        itemDependencyMaps = itemDependencyMaps.Where(d => !handledItems.Contains(d.Item)).ToList();

        List<string> itemsWithHandledDependencies = itemDependencyMaps.Where(d => d.Dependencies.IsSubsetOf(handledItems)).Select(i => i.Item).ToList();
        AddItemsToDependencyOrder(dependencyOrder, handledItems, itemsWithHandledDependencies);
    } while (itemDependencyMaps.Count > 0);

    return dependencyOrder;
}
