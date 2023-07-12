using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _itemName;
    [SerializeField]
    private Image _image; 


    public void SetName(string text)
    {
        _itemName.text = text; 
    }

    public void SetImage(Sprite image)
    {
        _image.sprite = image; 
    }

    
}
