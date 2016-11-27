using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class CraftingManager : MonoBehaviour 
{
    public GameObject RecipePanelPrefab;
    public Text myStationName;
    public GameObject RecipeBucket;
    public ItemDatabase database;
    public Inventory inv;

    public RecipePanel ProductPanel;
    public int selected = -1;
    public int amount = 1;
    public Text CreateAmount;
    RecipePanel selectedPanel;
    public Button craftButton;
    
    public GameObject ingredientPanelPrefab;
    public GameObject ingredientBucket;

    List<GameObject> Recipes = new List<GameObject>();

    List<RecipePanel> ingredients = new List<RecipePanel>();
    CraftingStation myStation;
    void Start()
    {
        GameObject inventory = GameObject.Find("Inventory");
        database = inventory.GetComponent<Loader>().ic;
        inv = inventory.GetComponent<Inventory>();
    }
    public void LoadStation(CraftingStation station)
    {
        database = GameObject.Find("Inventory").GetComponent<Loader>().ic;
        myStation = station;
        myStationName.text = station.myStation.Name;
        int count = 0;
        foreach(Recipe recipe in station.myStation.Recipes)
        {
            GameObject recipePanel = Instantiate(RecipePanelPrefab);
            recipePanel.GetComponent<RecipePanel>().Setup(database.FetchItemByID(recipe.Product.itemID), recipe.Product.quantity,this, count,recipe);
            recipePanel.transform.SetParent(RecipeBucket.transform);
            Recipes.Add(recipePanel);
            count++;
        }
    }
    public void SelectSlot(int id)
    {
        RecipePanel oldSelected = Recipes[selected].GetComponent<RecipePanel>();
        if(oldSelected)
        {
            oldSelected.myBackground.color = new Color(1, 1, 1, 0.0f);
        }
        selected = id;
        selectedPanel = Recipes[id].GetComponent<RecipePanel>();
        selectedPanel.myBackground.color = new Color(1, 1, 1, 0.2f);
        ProductPanel.Setup(selectedPanel.myItem, selectedPanel.amount, this, 0, null);
        ProductPanel.gameObject.SetActive(true);
        ProductPanel.id = -1;
        foreach(RecipePanel ingredient in ingredients)
        {
            Destroy(ingredient.gameObject);
        }
        ingredients.Clear();
        bool ingredientCheck = true;
        foreach(ItemCluster ingredient in selectedPanel.myRecipe.ingredients)
        {
            GameObject ingredientPanel = Instantiate(ingredientPanelPrefab);
            RecipePanel newingredient=ingredientPanel.GetComponent<RecipePanel>();
            newingredient.Setup(database.FetchItemByID(ingredient.itemID), ingredient.quantity, this, -1, null);
            ingredientPanel.transform.SetParent(ingredientBucket.transform);
            ingredients.Add(newingredient);
            if (inv.CheckHowManyIHave(ingredient.itemID) < ingredient.quantity * amount)
            {
                ingredientCheck = false;
            }
        }
        craftButton.interactable = ingredientCheck;
        amount = 1;
        CreateAmount.text = amount.ToString();
    }
    public void AmountChanged()
    {
        CreateAmount.text = amount.ToString();
        ProductPanel.ChangeQuantity(amount);
        foreach(RecipePanel ingredient in ingredients)
        {
            ingredient.ChangeQuantity(amount);
        }
        bool ingredientCheck = true;
        foreach (ItemCluster ingredient in selectedPanel.myRecipe.ingredients)
        {
            if (inv.CheckHowManyIHave(ingredient.itemID) < ingredient.quantity * amount)
            {
                ingredientCheck = false;
            }
        }
        craftButton.interactable = ingredientCheck;
    }
    public void IncreaseAmount()
    {
        amount++;
        AmountChanged();
    }
    public void DecreaseAmount()
    {
        amount--;
        if(amount < 0)
        {
            amount = 0;
        }
        AmountChanged();
    }
    public void Craft()
    {
        foreach (ItemCluster ingredient in selectedPanel.myRecipe.ingredients)
        {
            inv.RemoveItem(ingredient.itemID, ingredient.quantity * amount);
        }
        inv.AddItem(ProductPanel.myItem.ID, ProductPanel.amount * amount);
    }
}
