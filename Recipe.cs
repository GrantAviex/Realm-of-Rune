using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class Recipe
{
    [XmlElement("ID")]
    public int ID { get; set; }


    [XmlElement("ingredient")]
    public ItemCluster[] ingredients
    {
        get
        {
            if (_ingredientList == null)
            {
                _ingredientList = new List<ItemCluster>();
            }
            return _ingredientList.ToArray();
        }

        set
        {
            if (_ingredientList == null)
            {
                _ingredientList = new List<ItemCluster>();
            }

            if (value != null)
            {
                _ingredientList.AddRange(value);
            }
        }
    }
    private List<ItemCluster> _ingredientList = new List<ItemCluster>();

    [XmlElement("Product")]
    public ItemCluster Product { get; set; }
}
public class ItemCluster
{
    [XmlElement("itemID")]
    public int itemID { get; set; }
    [XmlElement("Quantity")]
    public int quantity { get; set; }

}
public class Station
{
    [XmlAttribute("Name")]
    public string Name { get; set; }
    [XmlElement("ID")]
    public int ID { get; set; }

    [XmlElement("Recipe")]
    public Recipe[] Recipes
    {
        get
        {
            if (_recipeList == null)
            {
                _recipeList = new List<Recipe>();
            }
            return _recipeList.ToArray();
        }

        set
        {
            if (_recipeList == null)
            {
                _recipeList = new List<Recipe>();
            }

            if (value != null)
            {
                _recipeList.AddRange(value);
            }
        }
    }
    private List<Recipe> _recipeList = new List<Recipe>();

    public Station()
    {
    }
    public Station(Station other)
    {
        Name = other.Name;
        ID = other.ID;
        Recipes = (Recipe[])other.Recipes.Clone();
    }
}