using UnityEngine;
using Wilson.Item;

namespace Wilson.UI
{
    public class InventoryUI_Expanded : MonoBehaviour
    {
        [SerializeField] private GameObject panel; // 전체 패널
        [SerializeField] private InventorySlotUI[] slotUIs;
        [SerializeField] private GameObject background;

        private void Start()
        {
            //초기화
            panel.SetActive(false);
            RefreshUI(); //이거 100퍼 버그있음, 버그나면 지우셈
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                panel.SetActive(!panel.activeSelf);
                background.SetActive(panel.activeSelf);
                if (panel.activeSelf)
                {
                    RefreshUI();
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;

                }
                Cursor.visible = panel.activeSelf;
            }
        }

        public void RefreshUI()
        {
            var items = Inventory.Instance.Items;

            for (int i = 0; i < slotUIs.Length; i++)
            {
                slotUIs[i].SetItem(items[i]);
            }
        }
    }

}
