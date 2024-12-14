using UnityEngine;

using Inventory;

namespace Data
{
    public static class DataManager
    {
        public static bool CanSubtractItem(GameItem itemToSubtract, int amountToSubtract)
        {
            if (amountToSubtract <= 0)
            {
                Debug.LogWarning("Amount cannot be <= 0");
                return false;
            }

            if (!IsInventoryContainItem(itemToSubtract, out _))
            {
                Debug.Log($"Inventory doesn't contain {itemToSubtract.Id}");
                return false;
            }

            if (CalculateSpecificItemAmount(itemToSubtract) < amountToSubtract) 
            {
                Debug.Log($"Inventory doesn't contain enough amount of {itemToSubtract.Id}");
                return false;
            }

            return true;
        } 
        
        /// <summary>
        /// Checks if inventory contain specific item in inventory
        /// </summary>
        /// <param name="item">item to search</param>
        /// <param name="slotDataRef">reference to slot with item</param>
        /// <returns>bool is inventory contain item</returns>
        public static bool IsInventoryContainItem(GameItem item, out PlayerData.SlotData slotDataRef)
        {
            slotDataRef = null;
            
            foreach (var itemData in GameSession.Instance.Data.SlotsData)
            {
                if(itemData.Item != item) continue;
                
                slotDataRef = itemData;
                break;
            }

            return slotDataRef != null;
        }

        public static int CalculateSpecificItemAmount(GameItem item)
        {
            var amount = 0;
            var itemSlots = GameSession.Instance.Data.GetCertainItemSlots(item);

            foreach (var slot in itemSlots)
            {
                if(slot.Item != item) continue;
                amount += slot.ItemAmount;
            }

            return amount;
        }

        /// <summary>
        /// Checks if item (current amount) + (add amount) lesser or equals to max stack of item
        /// </summary>
        /// <param name="slotData">slot with item to check</param>
        /// <param name="amount">amount to add to current amount of item</param>
        /// <returns>bool is (current amount) + (amount to add) lesser or equals to max stack</returns>
        public static bool CanFitInSlot(PlayerData.SlotData slotData, int amount)
        {
            return ((slotData.ItemAmount + amount) <= slotData.Item.MaxStack);
        }

        /// <summary>
        /// Checks if slot has available space
        /// </summary>
        /// <param name="slot">slot to check</param>
        /// <param name="availableSpace">available space in 'slot'</param>
        /// <returns>bool is slot has available space</returns>
        public static bool IsSlotSpaceAvailable(PlayerData.SlotData slot, out int availableSpace)
        {
            if (slot == null || slot.ItemAmount >= slot.Item.MaxStack)
            {
                availableSpace = 0;
                return false;
            }
            
            availableSpace = slot.Item.MaxStack - slot.ItemAmount;
            return true;
        }

        /// <summary>
        /// Fills exist slots
        /// </summary>
        /// <param name="item">item to search in inventory for filling</param>
        /// <param name="amount">amount of item which needs to fill (if inventory has space for rhis)</param>
        public static void FillExistSlots(GameItem item, ref int amount)
        {
            if(amount <= 0) return;
                
            var slotsToFill = GameSession.Instance.Data.GetCertainItemSlots(item);
            foreach (var slotToFill in slotsToFill)
            {
                if(!IsSlotSpaceAvailable(slotToFill, out var availableSpace)) continue;

                var addAmount = Mathf.Min(availableSpace, amount);
                slotToFill.ItemAmount += addAmount;
                
                amount -= addAmount;
                
                if(amount <= 0) break;
            }
        }

        /// <summary>
        /// Creates stacks of item
        /// </summary>
        /// <param name="item">item to stack create</param>
        /// <param name="stacksAmount">amount of stacks to create
        /// e.g. Item.MaxStack = 64, if stacksAmount = 5, it would create 5 slots with 'item' with MaxStack</param>
        public static void CreateStacks(GameItem item, int stacksAmount)
        {
            if (stacksAmount <= 0) return;
            var gameSession = GameSession.Instance;
            
            for (var i = 1; i <= stacksAmount; i++)
            {
                gameSession.Data.CreateItem(item, item.MaxStack);
            }
        }
    }
}