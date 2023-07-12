using UnityEngine;
using CleverCrow.Fluid.ElasticInventory;
using static UnityEditor.Progress;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject _item;

    [SerializeField]
    private GameObject _listOfItemsContent;

    private List<ItemManager> _items; 

    public static InventoryManager Instance; 

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return; 
        }

        Instance = this;
        _items = new List<ItemManager>(); 

    }

    public void AddItem(ItemDefinition item)
    {
        var itemManager = _item.GetComponent<ItemManager>();
        itemManager.SetName(item.DisplayName);
        itemManager.SetImage(item.Image);

        _items.Add(Instantiate(itemManager, _listOfItemsContent.transform));
    }
    public void AddItems(List<ItemDefinition> items)
    {

        foreach (var item in items)
        {
            
            var itemManager = _item.GetComponent<ItemManager>();
            itemManager.SetName(item.DisplayName);
            itemManager.SetImage(item.Image);

            _items.Add(Instantiate(itemManager, _listOfItemsContent.transform));
        }
    }

    

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
