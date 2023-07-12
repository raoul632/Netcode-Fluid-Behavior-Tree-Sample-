using UnityEngine;
using CleverCrow.Fluid.ElasticInventory;

// This is a sample item definition. You can create as many of these as you want and edit them as you see fit.
// For customization details please visit the docs at https://github.com/ashblue/unity-elastic-inventory

// @IMPORTANT You may want to add a namespace here before creating definitions to avoid potential naming conflicts
// that could break your item definitions long term.

[ItemDefinitionDetails("Generic")]
public class ItemDefinition : ItemDefinitionBase {
    [InventoryCategory]
    [SerializeField]
    int _category;

    public int Cost;
    public Sprite Image; 

    // You can hard set a category name here instead if you don't want it to be selectable in the inspector
    public override string Category => GetCategoryByIndex(_category);
}
