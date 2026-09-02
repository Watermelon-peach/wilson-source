using UnityEngine;

namespace Wilson.Item
{
    //아이템 타입
    public enum ItemType
    {
        Consumable, 
        Craftable, 
        Puzzle 
    }

    //아이템 데이터 부모클래스
    public abstract class ItemData : ScriptableObject
    {
        //아이템 공통 정보
        public string itemName;                 //이름
        public Sprite icon;                     //아이콘
        [TextArea] public string description;   //설명
        public ItemType itemType;               //아이템 타입
        public string itemCode;                 //아이템 코드

        //아이템 타입별 추가 정보 반환
        public virtual string GetExtraInfo() => "";
    }

}
