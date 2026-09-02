using UnityEngine;
using System.Linq;
using Wilson.Item;

namespace Wilson.UI
{
    public class FishTrapUI : MonoBehaviour
    {
        [SerializeField] private FishTrap fishTrap;
        [SerializeField] private FishTrapCounterSlotUI[] slots;  // 9칸 슬롯들 연결

        private void OnEnable()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            foreach (var slot in slots)
            {
                int count = fishTrap.StoredItems
                    .Where(item => item.itemType == ItemType.Consumable)
                    .Count(item => item == slot.Item);

                slot.SetCount(count);
            }
        }

    }
}