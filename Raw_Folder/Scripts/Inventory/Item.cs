using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Thenexy/Inventory/Item")]
    public class GameItem : ScriptableObject
    {
        public string Id;
        public int MaxStack = 64;
    }
}