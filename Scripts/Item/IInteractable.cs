using UnityEngine;
using Wilson.Player;

namespace Wilson.Item
{
    public interface IInteractable
    {
        void Interact();  // 어떤 상호작용을 할지
        string GetPrompt();  // "E - 습득", "E - 열기" 같은 안내 텍스트
    }
}
