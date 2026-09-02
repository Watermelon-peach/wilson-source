using UnityEngine;

namespace Sample.Item
{
    //소비 아이템 정보
    [CreateAssetMenu(fileName = "Item_Protion_", menuName = "Inventory System/Item Data/Portion", order = 3)]
    public class PortionItemData : CountableItemData
    {
        public float Value => _value;
        [SerializeField] private float _value;
        public override Item CreateItem()
        {
            return new PortionItem(this);
        }
    }

}
