using System.Collections;
using UnityEngine;
using Wilson.UI;
using TMPro;

namespace Wilson.Game.Puzzle
{
    public class RockClicker : MonoBehaviour
    {
        #region Variable
        public Camera gameCamera;
        public GameObject realRock;
        public GameObject rewardEffect;
        public GameObject playerSettings;
        public GameObject uiManager;
        public FakeRock fakeRock;

        [Header("클릭 이펙트 프리팹")]
        public GameObject clickEffect;

        [Header("클릭으로 깨는 목표 횟수")]
        public int clearClickCount = 100;

        [Header("UI연결")]
        public TextMeshProUGUI countText;

        private int currentClickCount = 0;
        private bool isCleared = false;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //커서 활성화
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //게임 카메라 활성화
            gameCamera.gameObject.SetActive(true);

            //UI초기화
            countText.text = clearClickCount.ToString();
        }

        void Update()
        {
            if (isCleared) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Exit();
                gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == realRock)
                    {
                        // 이펙트 생성
                        if (clickEffect != null)
                            Instantiate(clickEffect, hit.point, Quaternion.identity);

                        // 클릭 수 증가
                        currentClickCount++;
                        countText.text = (clearClickCount - currentClickCount).ToString();
                        // 클리어 조건 검사
                        if (currentClickCount >= clearClickCount)
                        {
                            isCleared = true;
                            StartCoroutine(OnClear());
                        }
                    }
                }
            }
        }
        #endregion

        #region Custom Method
        private void Exit()
        {
            //게임초기화
            currentClickCount = 0;
            //커서 비활성화
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            gameCamera.gameObject.SetActive(false);

            playerSettings.SetActive(true);
            uiManager.SetActive(true);

            fakeRock.gameObject.SetActive(true);

        }

        private IEnumerator OnClear()
        {
            countText.enabled = false;
            // 간단한 클리어 처리 (추후 원하는 동작 추가 가능)
            Debug.Log("돌 깨기 완료!");
            realRock.SetActive(false);
            rewardEffect.SetActive(true);

            Exit();
            fakeRock.gameObject.SetActive(false);

            StartCoroutine(MonologUI.Instance.ShowMonolog("우왓 맛있어\n속이 든든해!"));
            yield return new WaitForSeconds(3f);

            //클리어 해금
            UnlockManager.Instance.WindZone = false;
            StartCoroutine(AlertBarUI.Instance.ShowAlert("돌풍 면역이 생겼습니다..."));
            fakeRock.IsCleared = true;
            Destroy(transform.parent.gameObject, 3f);
        }
        #endregion
    }

}
