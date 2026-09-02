using UnityEngine;
using UnityEngine.UI;
using Wilson.Item;
using TMPro;

namespace Wilson.UI
{
    public class ItemTooltip : MonoBehaviour
    {
        public static ItemTooltip Instance;

        [SerializeField] private GameObject tooltipPanel;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI extraInfoText;

        [SerializeField] private Vector3 offset;

        private void Awake()
        {
            Instance = this;
            tooltipPanel.SetActive(false);
        }

        public void ShowTooltip(ItemData item, Vector3 pos)
        {
            tooltipPanel.SetActive(true);
            tooltipPanel.transform.position = pos + offset;
            nameText.text = item.itemName;
            descriptionText.text = item.description;
            extraInfoText.text = item.GetExtraInfo();
        }

        public void HideTooltip()
        {
            tooltipPanel.SetActive(false);
        }
    }

}
