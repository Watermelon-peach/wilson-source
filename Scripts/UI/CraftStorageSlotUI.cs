using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Wilson.Item;

namespace Wilson.UI
{
    public class CraftStorageSlotUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image iconImage;
        private ItemData currentItem;
        private int slotIndex;

        private CraftUIManager manager;

        public void Init(int index, CraftUIManager managerRef)
        {
            slotIndex = index;
            manager = managerRef;
        }

        public void SetItem(ItemData item)
        {
            currentItem = item;
            iconImage.enabled = item != null;
            iconImage.sprite = item ? item.icon : null;
        }

        public void ClearSlot()
        {
            currentItem = null;
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (currentItem != null)
                manager.OnClick_StorageSlot(slotIndex); // 조합 슬롯으로 보내기
        }
    }
}
