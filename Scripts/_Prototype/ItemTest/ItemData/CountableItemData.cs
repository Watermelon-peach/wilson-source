using UnityEngine;

namespace Sample.Item
{
    //셀 수 있는 아이템 데이터
    public abstract class CountableItemData : ItemData
    {
        public int MaxAmount => _maxAmount;
        [SerializeField] private int _maxAmount = 99;
    }

}
