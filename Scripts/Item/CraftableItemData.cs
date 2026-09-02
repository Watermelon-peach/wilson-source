using UnityEngine;

namespace Wilson.Item
{
    [CreateAssetMenu(fileName = "CraftableItemData", menuName = "Wilson Item/CraftableItemData")]
    public class CraftableItemData : ItemData
    {
        public string recipeID;

        private void OnEnable()
        {
            itemType = ItemType.Craftable;
        }

        public override string GetExtraInfo()
        {
            return base.GetExtraInfo();
        }
    }

}
