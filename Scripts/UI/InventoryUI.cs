using UnityEngine;
using UnityEngine.UI;
using Wilson.Item;
using Wilson.Utility;

namespace Wilson.UI
{
    public class InventoryUI : Singleton<InventoryUI>
    {
        [SerializeField] private Transform slotParent;              // 슬롯 5개가 자식으로 있는 부모 오브젝트

        private Image[] slotImages;

        protected override void Awake()
        {
            base.Awake();

            // 자식 슬롯 Image 컴포넌트 가져오기
            slotImages = new Image[slotParent.childCount];

            for (int i = 0; i < slotImages.Length; i++)
            {
                slotImages[i] = slotParent.GetChild(i).GetComponent<Image>();
            }
        }

        private void Start()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            var items = Inventory.Instance.Items; // 아이템 배열 (ItemData[])

            for (int i = 0; i < slotImages.Length; i++)
            {
                if (i < items.Length && items[i] != null)
                {
                    slotImages[i].sprite = items[i].icon;
                    slotImages[i].enabled = true;
                }
                else
                {
                    slotImages[i].enabled = false;
                    //slotImages[i].sprite = emptySlotSprite;
                    //slotImages[i].enabled = true;
                }
            }
        }
    }

}
