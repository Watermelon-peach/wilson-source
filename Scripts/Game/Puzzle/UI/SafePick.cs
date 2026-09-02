using UnityEngine;
using Wilson.Item;
using Wilson.Player;
namespace Wilson.Game.Puzzle
{

    public class SafePick : MonoBehaviour, IInteractable
    {
        public GameObject safeObject;// 금고 전체
        public GameObject safeCameraObject;       // safe_close 내부 SafeCamera
        public Keypad keypad;// Key 스크립트
        public GameObject playerSettingsObject;   // 비활성화할 PlayerSettings 오브젝트
        #region IInteractable Implementation
        public void Interact()
        {
            //Safe 켜기
            safeObject.SetActive(true);
            // SafeCamera 별도 켜기
            if (safeCameraObject != null)
                safeCameraObject.SetActive(true);

            //플레이어 비활성화
            playerSettingsObject.SetActive(false);

            //키패드이벤트 연결
            keypad.OnKeypadEventCompleted -= OnKeypadComplete;
            keypad.OnKeypadEventCompleted += OnKeypadComplete;
            keypad.enabled = true;

            // FakeSafe 끄기
            gameObject.SetActive(false);

        }

        public string GetPrompt() => "금고 열람";

        private void OnKeypadComplete()
        {
            //  다시 재시작할 수 있도록 해제
            keypad.OnKeypadEventCompleted -= OnKeypadComplete;
        }
        private void OnDisable()
        {
            ItemDetector.Instance?.RemoveTarget(this); // 직접 리스트에서 제거
        }
        #endregion

    }
}