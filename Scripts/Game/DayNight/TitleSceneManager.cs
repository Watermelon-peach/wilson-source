using UnityEngine;
using Wilson.Utility;

namespace Wilson.Game
{
    public class TitleSceneManager : MonoBehaviour
    {
        #region Variables
        //참조
        public TimeLerp timeLerp;
        public SceneFader fader;
        public GameObject CreditPanel;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            fader.FadeStart();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        private void Update()
        {
            timeLerp.TValue += Time.deltaTime;
        }
        #endregion

        #region Custom Method
        public void OnStartButton()
        {
            fader.FadeTo("PlayScene");
        }
        
        public void OnExitButton()
        {
            Application.Quit();
        }

        public void OnCreditButton()
        {
            //크레딧버튼, 나가기버튼에 연결
            CreditPanel.SetActive(!CreditPanel.activeSelf);
        }
        #endregion
    }

}
