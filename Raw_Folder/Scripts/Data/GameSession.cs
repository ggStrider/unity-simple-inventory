using UnityEngine;

using Inventory;
using Data.Observers;

using System.Collections.Generic;

namespace Data
{
    public class GameSession : MonoBehaviour
    {
        /// <summary>
        /// Inventory data
        /// </summary>
        [SerializeField] private PlayerData _data;
        
        /// <summary>
        /// Represent of actual data (read-only)
        /// </summary>
        public PlayerData Data => _data;
        
        private readonly List<IItemAdded> _itemAddedObservers = new List<IItemAdded>();
        
        /// <summary>
        /// This instance (of GameSession, singleton) 
        /// </summary>
        public static GameSession Instance {get; private set;}
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else if(Instance != this) Destroy(gameObject);
        }

        /// <summary>
        /// Creates or adds item in inventory (depends on conditions)
        /// </summary>
        /// <param name="item">item to add to inventory</param>
        /// <param name="amount">amount of item which needs to add to inventory</param>
        public void AddItemToInventory(GameItem item, int amount)
        {
            if (amount <= 0) { Debug.Log("Amount cannot be <= 0"); return; }
            
            // If inventory already has this 'item' in inventory
            if (DataManager.IsInventoryContainItem(item, out var itemData))
            {
                // If 'amount' of item < possible stack
                if (DataManager.CanFitInSlot(itemData, amount))
                { 
                    itemData.ItemAmount += amount;
                }
                
                // if 'amount' > than possible stack
                else
                {
                    FillAndCreateRemainAmount(item, amount);
                }
            }
            
            // Inventory doesn't have 'item' in inventory
            else
            {
                if (amount > item.MaxStack)
                {
                    FillAndCreateRemainAmount(item, amount);
                }
                else
                {
                    _data.CreateItem(item, amount);
                }
            }
            
            // Notifying all observers
            NotifyItemAddedObservers(item, amount);
        }

        public void SubtractItemFromInventory(GameItem itemToSubtract, int amountToSubtract)
        {
            if(!DataManager.CanSubtractItem(itemToSubtract, amountToSubtract)) return;

            foreach (var slot in _data.GetCertainItemSlots(itemToSubtract))
            {
                var subtractDelta = Mathf.Min(slot.ItemAmount, amountToSubtract);
                    
                slot.ItemAmount -= subtractDelta;
                amountToSubtract -= subtractDelta;
                
                if(slot.ItemAmount <= 0) _data.DeleteSlot(slot);
                if(amountToSubtract <= 0) break;
            }
        }

        /// <summary>
        /// Fills all slots with this item and creates stacks
        /// </summary>
        /// <param name="item">item to add to inventory</param>
        /// <param name="amount">amount of items which needs to add to inventory</param>
        private void FillAndCreateRemainAmount(GameItem item, int amount)
        {
            DataManager.FillExistSlots(item, ref amount);
            if(amount <= 0) return;
            
            var stacks = amount / item.MaxStack;
            if(stacks > 0) DataManager.CreateStacks(item, stacks);
                    
            var remains = amount % item.MaxStack;
            if(remains > 0) _data.CreateItem(item, remains);
        }
        
        #region Observer stuff

        /// <summary>
        /// Subscribes observer on item added event (this gives item and amount)
        /// </summary>
        /// <param name="observer">observer to subscribe</param>
        public void SubscribeItemAdded(IItemAdded observer)
        {
            if(_itemAddedObservers.Contains(observer)) return;
            _itemAddedObservers.Add(observer);
        }
        
        private void OnDestroy()
        {
            // Clears all observers
            _itemAddedObservers.Clear();
        }

        /// <summary>
        /// Notifies all observers when item added to inventory (created or added)
        /// </summary>
        /// <param name="addedItem">Added/created item to/in inventory</param>
        /// <param name="amount">amount of added/created</param>
        private void NotifyItemAddedObservers(GameItem addedItem, int amount)
        {
            if(_itemAddedObservers.Count == 0) return;

            foreach (var observer in _itemAddedObservers)
            {
                observer.OnItemAdded(addedItem, amount);
            }
        }
        
        #endregion
    }
}
