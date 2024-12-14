using UnityEngine;
using Inventory;

using System;
using System.Linq;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Contains all data about player (inventory, etc.)
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        /// <summary>
        /// Info about slot
        /// </summary>
        public List<SlotData> SlotsData =  new List<SlotData>();
        
        /// <summary>
        /// Max inventory items
        /// </summary>
        public const int INVENTORY_CAPACITY = 3;

        /// <summary>
        /// Creates item in inventory
        /// </summary>
        /// <param name="item">item to create</param>
        /// <param name="amount">amount of item</param>
        public void CreateItem(GameItem item, int amount)
        {
            if (SlotsData.Count >= INVENTORY_CAPACITY) { Debug.Log("Inventory is full"); return; }
            
            var itemData = new SlotData(item, amount);
            SlotsData.Add(itemData);
        }

        /// <summary>
        /// Checks is slot full
        /// </summary>
        /// <param name="slotData">slot to check</param>
        /// <returns>bool is slot full</returns>
        private bool CheckIsSlotFull(SlotData slotData)
        {
            return slotData.ItemAmount >= slotData.Item.MaxStack;
        }

        /// <summary>
        /// Gives slots with certain item
        /// </summary>
        /// <param name="itemToSearch">item to search in inventory</param>
        /// <returns>slots with 'itemToSearch'</returns>
        public SlotData[] GetCertainItemSlots(GameItem itemToSearch)
        {
            return SlotsData.Where(itemData => itemData.Item == itemToSearch).ToArray();
        }

        public void DeleteSlot(SlotData slot)
        {
            if(!SlotsData.Contains(slot)) return;
            SlotsData.Remove(slot);
        }

        /// <summary>
        /// Data of slot
        /// </summary>
        [Serializable]
        public class SlotData
        {
            /// <summary>
            /// Item in this slot
            /// </summary>
            public GameItem Item;
            
            /// <summary>
            /// Amount of 'Item' in this slot
            /// </summary>
            public int ItemAmount;
            
            public SlotData(GameItem item, int itemAmount)
            {
                Item = item;
                ItemAmount = itemAmount;
            }
        }
    }
}