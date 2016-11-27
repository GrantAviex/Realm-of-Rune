using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class RecipePanel : MonoBehaviour, IPointerClickHandler 
{
    public Text ProductName;
    public Image productIcon;
    public Text productAmount;
    public Text productDescription;
    CraftingManager myManager;
    public int id;
    public Item myItem;
    public int amount;
    public Image myBackground;
    public Recipe myRecipe;
    int createAmount;
    public void Setup(Item item, int quantity, CraftingManager manager, int myID, Recipe recipe)
    {
        createAmount = 1;
        myRecipe = recipe;
        myItem = item;
        amount = quantity;
        if(ProductName != null)
            ProductName.text = item.Title;
        productIcon.sprite = item.sprite;
        if(productAmount != null)
            productAmount.text = quantity.ToString();
        myManager = manager;
        id = myID;
        if (productDescription != null)
            productDescription.text = item.Description.Description;
    }
    public void ChangeQuantity(int newCreateAmount)
    {
        createAmount = newCreateAmount;
        productAmount.text = (amount * createAmount).ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (id != -1)
        {
            if (myManager.selected != id)
            {
                myManager.SelectSlot(id);
            }
        }
    }
}
