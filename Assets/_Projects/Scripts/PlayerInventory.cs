using CleverCrow.Fluid.ElasticInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private InventoryHelper _inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        _inventory = GetComponent<InventoryHelper>();

        List<ItemDefinition> ItemsDef = new List<ItemDefinition>(); 
        var items = _inventory.Instance.GetAll();
        _inventory.Instance.Sort(items, sort: ItemSort.UpdatedAt, order: ItemOrder.Descending);
        //PrintItems(items);
        foreach(var item in items)
        {
            ItemsDef.Add(item.Definition as ItemDefinition); 
        }

        InventoryManager.Instance.AddItems(ItemsDef); 


    }

    void PrintItems(List<IItemEntryReadOnly> items)
    {
        foreach (var item in items)
        {
            Debug.Log("Name of the item " + item.Definition);
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
