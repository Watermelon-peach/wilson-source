using System.Collections.Generic;
using UnityEngine;
using Wilson.UI;

namespace Wilson.Item
{
    public class FishTrap : MonoBehaviour, IInteractable
    {
        public FishTrapUI UI;

        public List<ItemData> storedItems = new();
        public List<ItemData> StoredItems => storedItems;

        public void Interact()
        {

            var inventory = Inventory.Instance;
            var items = inventory.Items;

            if (items[0] == null)
            {
                StartCoroutine(AlertBarUI.Instance.ShowAlert("넣을 아이템이 없습니다!!"));
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && items[i].itemType == ItemType.Consumable)
                {
                    storedItems.Add(items[i]);        // FishTrap에 보관
                    inventory.RemoveItem(items[i]);   // 플레이어 인벤토리에서 제거
                }
            }

            
            Debug.Log($"보관 완료: 현재 FishTrap에 {storedItems.Count}개의 소모 아이템이 있습니다.");

            //Sfx
            //풍덩...

            // UI 업데이트
            UI.RefreshUI();
            InventoryUI.Instance.RefreshUI();  
        }

        public void Clear()
        {
            storedItems.Clear();
        }

        public string GetPrompt() => "통발에 몰래 넣기";
    }
}
