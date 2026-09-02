using UnityEngine;
using TMPro;
using Wilson.Game;
using UnityEngine.UI;

namespace Wilson.UI
{
    public class SurvivalRulesUI : MonoBehaviour
    {
        #region Variables
        [Header("참조")]
        public GameObject rulePanel;
        public GameObject background;
        public GameObject keyButtonIcon;
        public TextMeshProUGUI ruleText;
        public TimeManager timeManager;

        public Button nextButton;
        public Button previousButton;

        public int showingDay = 0;

        [Header("메시지")]
        public string[] messages = new string[3];
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            rulePanel.SetActive(false);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                rulePanel.SetActive(!rulePanel.activeSelf);
                background.SetActive(rulePanel.activeSelf);
                keyButtonIcon.SetActive(!rulePanel.activeSelf);

                if (rulePanel.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;

                }
                Cursor.visible = rulePanel.activeSelf;

                //실행시 현재날짜 기준
                showingDay = (timeManager.Day > 3)? 4: timeManager.Day;
                UpdateButtons();
                //스포방지
                UpdateByCurrentDate();

                PrintMessage(showingDay);
            }
        }
        #endregion

        #region Custom Method
        private void UpdateByCurrentDate()
        {
            if (showingDay >= timeManager.Day)
            {
                showingDay = timeManager.Day;
                nextButton.interactable = false;
            }
        }

        private void UpdateButtons()
        {
            switch (showingDay)
            {
                case 1:
                    nextButton.interactable = true;
                    previousButton.interactable = false;
                    break;

                default:
                    nextButton.interactable = true;
                    previousButton.interactable = true;
                    break;

                case 4:
                    nextButton.interactable = false;
                    previousButton.interactable = true;
                    break;
            }
        }
        private void PrintMessage(int dayCount)
        {
            ruleText.text = $"<{dayCount}일차>\n{Message(dayCount)}";
        }
        private string Message(int day)
        {
            string rules = "";
            //default 처리 대신 4로 고정 <<페이지 관리하기 편하게
            if (day > 3)
                day = 4;

            switch (day)
            {
                case 1:
                    rules = messages[0];
                    break;
                case 2:
                    rules = messages[1];
                    break;
                case 3:
                    rules = messages[2];
                    break;
                case 4:
                    rules = " 이제 이런 일기 같은 걸 쓸 시간이 없어. 부품 세개를 찾아서 토미와 함께 빠져나가야돼!!";
                    break;
            }
            return rules;
        }

        public void NextPage()
        {
            showingDay++;
            UpdateButtons();
            UpdateByCurrentDate();
            PrintMessage(showingDay);
        }

        public void PreviousPage()
        {
            showingDay--;
            UpdateButtons();
            UpdateByCurrentDate();
            PrintMessage(showingDay);
        }
        #endregion
    }

}
