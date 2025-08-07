using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "Scriptable Objects/ItemList")]
public class ItemList : ScriptableObject
{
    [SerializeField]
    private List<Item> _fullItemList;
    public List<Item> FullItemList { get { return _fullItemList; } }

    public List<Item> RollNextItems(int numItems)
    {
        List<Item> itemsCopy = new List<Item>(_fullItemList);
        List<Item> chosenItems = new List<Item>();
        
        for (int i = 0; i < numItems; i++)
        {
            int chosenItemIndex = UnityEngine.Random.Range(0, itemsCopy.Count);
            Item chosenItem = itemsCopy[chosenItemIndex];
            chosenItems.Add(chosenItem);
            itemsCopy.RemoveAt(chosenItemIndex);
        }
        return chosenItems;
    }
}
