using UnityEngine;
using UnityEngine.UI;
using Wilson.Item;

namespace Wilson.UI
{
    public class CraftUIManager : MonoBehaviour
    {
        [Header("조합대 저장소")]
        [SerializeField] private CraftStorageSlotUI[] storageSlots;

        [Header("조합 슬롯")]
        [SerializeField] private InventorySlotUI[] craftSlots;

        [Header("조합 버튼")]
        [SerializeField] private Button craftButton;

        [Header("연결된 조합대 인벤토리")]
        [SerializeField] private CraftTableInventory craftInventory;

        private ItemData[] craftingItems = new ItemData[3];

        private void OnEnable()
        {
            RefreshStorageSlots();
            ClearCraftSlots();
        }

        public void RefreshStorageSlots()
        {
            var items = craftInventory.StorageItems;

            for (int i = 0; i < storageSlots.Length; i++)
            {
                if (i < items.Length && items[i] != null)
                {
                    storageSlots[i].SetItem(items[i]);
                }
                else
                {
                    storageSlots[i].ClearSlot();
                }
            }
        }

        public void OnClick_StorageSlot(int index)
        {
            var item = craftInventory.StorageItems[index];
            if (item == null) return;

            for (int i = 0; i < craftingItems.Length; i++)
            {
                if (craftingItems[i] == null)
                {
                    craftingItems[i] = item;
                    craftSlots[i].SetItem(item);
                    craftInventory.RemoveItem(item);
                    RefreshStorageSlots();
                    break;
                }
            }

            craftButton.interactable = CanCraft();
        }

        private bool CanCraft()
        {
            int count = 0;
            foreach (var item in craftingItems)
                if (item != null) count++;

            return count == 3;
        }

        public void OnClickCraft()
        {
            Debug.Log("조합 성공! (아이템 생성 로직 추가 예정)");

            ClearCraftSlots();
            craftButton.interactable = false;
        }

        private void ClearCraftSlots()
        {
            for (int i = 0; i < craftingItems.Length; i++)
            {
                craftingItems[i] = null;
                craftSlots[i].ClearSlot();
            }
        }
    }
}
