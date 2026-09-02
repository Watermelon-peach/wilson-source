using UnityEngine;

namespace Wilson.Item
{
    [CreateAssetMenu(fileName = "PuzzleItemData", menuName = "Wilson Item/PuzzleItemData")]
    public class PuzzleItemData : ItemData
    {
        public string puzzleKey;

        private void OnEnable()
        {
            itemType = ItemType.Puzzle;
        }

        public override string GetExtraInfo()
        {
            return base.GetExtraInfo();
        }
    }

}
