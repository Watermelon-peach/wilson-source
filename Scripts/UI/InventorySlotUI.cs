using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Wilson.Item;

namespace Wilson.UI
{
    public class InventorySlotUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler   // 클릭 핸들러 추가
    {
        [SerializeField] private Image iconImage;
        private ItemData currentItem;

        // 클릭 시 호출될 콜백
        private System.Action onClick;

        public void SetItem(ItemData item, System.Action clickAction = null)
        {
            currentItem = item;
            iconImage.enabled = item != null;
            iconImage.sprite = item ? item.icon : null;
            onClick = (item != null) ? clickAction : null;
        }

        public void ClearSlot()
        {
            SetItem(null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (currentItem != null)
                ItemTooltip.Instance.ShowTooltip(currentItem, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemTooltip.Instance.HideTooltip();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 상품이 있을 때만 콜백 실행
            onClick?.Invoke();
        }
    }
}
