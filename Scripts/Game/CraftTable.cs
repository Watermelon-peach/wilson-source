using UnityEngine;

namespace Wilson.Item
{
    public class CraftTable : MonoBehaviour, IInteractable
    {
        #region Variable
        public GameObject craftUI;
        #endregion
        public string GetPrompt() => "조합대 열기";

        public void Interact()
        {
            craftUI.SetActive(true);
        }
    }

}
