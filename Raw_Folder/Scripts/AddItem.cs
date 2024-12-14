using Data;
using Inventory;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    [SerializeField] private GameItem _itemToAdd;
    [SerializeField] private int _amount;

    public void _Add()
    {
        GameSession.Instance.AddItemToInventory(_itemToAdd, _amount);
    }
}
