using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public const string recipePath = "Recipes";
    public RecipeDatabase rc { get; set; }
    public const string itemPath = "Items";
    public ItemDatabase ic { get; set; }
    public const string resourcePath = "Resources";
    public ResourceDatabase sc { get; set; }
	void Start () 
    {
        rc = RecipeDatabase.Load(recipePath);
        rc.ConstructRecipeDatabase();
        ic = ItemDatabase.Load(itemPath);
        ic.ConstructItemDatabase();
        sc = ResourceDatabase.Load(resourcePath);
        sc.ConstructResourceDatabase();

	}
}
