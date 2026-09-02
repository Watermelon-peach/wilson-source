using UnityEngine;
using Wilson.Player;

namespace Wilson.Item
{
    public class WorldItem : MonoBehaviour,IInteractable
    {
        [SerializeField] private ItemData itemData;

        public void Interact()
        {
            if (Inventory.Instance.AddItem(itemData))
            {
                ItemDetector.Instance.RemoveTarget(this);   // 이 오브젝트를 리스트에서 제거
                Destroy(gameObject);
            }
            else
            {
                //인터랙션 인터페이스 비활성화 (흐리게)
            }
        }

        public string GetPrompt() => $"<color=#FDFFB7><b>'{itemData.itemName}'</b></color> 줍기";
    }

}
