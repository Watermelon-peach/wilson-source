using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Wilson.Game;

namespace Wilson.UI
{
    //낮,밤과 관련된 UI들을 관리하는 클래스
    public class DayNightUI : MonoBehaviour
    {
        #region Variables
        //참조
        public TimeManager timeManager;

        public CanvasGroup timerGroup;      //타이머 ON / OFF <<알파값 조절
        public TextMeshProUGUI timerText;   //남은 시간 표시 텍스트

        public GameObject dayIcon;
        public GameObject nightIcon;
        public GameObject buttonIcon;

        public TextMeshProUGUI DayCountText;

        //타이머 라운드게이지
        public Image fillImg;
        
        private Color dayColor;
        private Color nightColor;

        private bool wasDay = false;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            dayColor = new Color(1f, 0.41f, 0.34f, 1f);
            nightColor = new Color(0.34f, 0.57f, 1f, 1f);
            DayCountText.text = "Day " + timeManager.Day.ToString();
        }
        private void Update()
        {
            UpdateDayNightIcon();
            UpdateTimer();

            wasDay = timeManager.IsDay;
        }
        #endregion

        #region Custom Method
        private void UpdateDayNightIcon()
        {
            //아이콘 세팅
            if (wasDay != timeManager.IsDay)
            {
                dayIcon.SetActive(timeManager.IsDay);
                buttonIcon.SetActive(timeManager.IsDay);
                nightIcon.SetActive(!timeManager.IsDay);
                DayCountText.text = "Day " + timeManager.Day.ToString();
            }
        }

        //타이머 표시
        private void UpdateTimer()
        {
            //변화 발생 시 alpha값 조정 >> 타이머 visual ON/OFF
            if (wasDay != timeManager.IsDay)
            {
                timerGroup.alpha = timeManager.IsDay ? 0f : 1f;
                if (timeManager.IsDay)
                {
                    fillImg.color = dayColor;
                    fillImg.fillAmount = 1;
                }
                wasDay = timeManager.IsDay;
            }

            if (timeManager.IsDay)
                return;

            fillImg.color = Color.Lerp(dayColor, nightColor, timeManager.CountDown / 300f);
            fillImg.fillAmount = 1 - Mathf.InverseLerp(0f, 300f, timeManager.CountDown);

            int minutes = Mathf.FloorToInt(timeManager.CountDown / 60f);
            float seconds = timeManager.CountDown % 60f;

            timerText.text = $"{minutes}:{seconds:00.0}";
        }
        #endregion
    }

}
