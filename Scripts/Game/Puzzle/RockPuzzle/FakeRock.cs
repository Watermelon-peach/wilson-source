using UnityEngine;
using Wilson.Item;
using Wilson.UI;

namespace Wilson.Game.Puzzle
{
    public class FakeRock : MonoBehaviour, IInteractable
    {
        #region Variables
        public GameObject puzzleRock;
        public GameObject playerSettings;
        public GameObject uiManager;
        #endregion

        #region Property
        public bool IsCleared { get; set; }
        #endregion
        public string GetPrompt() => "돌 깨기";

        public void Interact()
        {
            if (IsCleared)
            {
                AlertBarUI.Instance.ShowAlert("이미 클리어했습니다.");
                return;
            }

            puzzleRock.SetActive(true);
            uiManager.SetActive(false);
            playerSettings.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
