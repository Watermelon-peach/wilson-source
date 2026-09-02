using UnityEngine;
using TMPro;
using Wilson.Item;
using UnityEngine.UI;

namespace Wilson.UI
{
    public class FishTrapCounterSlotUI : MonoBehaviour
    {
        [SerializeField] private ItemData representedItem;           // 이 슬롯이 담당할 아이템
        [SerializeField] private TextMeshProUGUI countText;          // 개수 텍스트
        [SerializeField] private Image iconImage;                    // 아이콘 이미지

        [Header("아이콘 색상 설정")]
        private Color normalColor = Color.white;
        private Color disabledColor = new Color(0.3f, 0.3f, 0.3f, 1f);

        public void SetCount(int count)
        {
            countText.text = count > 0 ? count.ToString() : "";

            // 색상 조절
            iconImage.color = count > 0 ? normalColor : disabledColor;
        }

        public ItemData Item => representedItem;
    }

}
