using UnityEngine;

namespace Wilson.Item
{
    [CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Wilson Item/ConsumableItemData")]
    public class ConsumableItemData : ItemData
    {
        public float hungerRestore;
        public float thirstRestore;
        public float happinessRestore;

        private void OnEnable()
        {
            itemType = ItemType.Consumable;
        }

        public override string GetExtraInfo()
        {
            string stats = "";
            if (hungerRestore != 0)
            {
                stats += $"허기 회복 +{hungerRestore} \n";
            }
            if (thirstRestore != 0)
            {
                stats += $"갈증회복 +{thirstRestore}";
            }
            return stats;
        }
    }

}
