using UnityEngine;

using Data;
using Inventory;

public class SubtractItem : MonoBehaviour
{
    [SerializeField] private GameItem _itemToSubtract;
    [SerializeField] private int _subtractAmount;

    public void _SubtractItem()
    {
        GameSession.Instance.SubtractItemFromInventory(_itemToSubtract, _subtractAmount);
    }
}