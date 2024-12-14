using Inventory;

namespace Data.Observers
{
    public interface IItemAdded
    {
        /// <summary>
        /// Executed when item added/created to/in inventory
        /// </summary>
        /// <param name="addedItem">added item to inventory</param>
        /// <param name="amount">amount of added/created item</param>
        public void OnItemAdded(GameItem addedItem, int amount);
    }
}