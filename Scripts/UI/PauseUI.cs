using UnityEngine;
using Wilson.Utility;

namespace Wilson.UI
{
    /// <summary>
    /// Pause화면 관리 클래스
    /// </summary>
    public class PauseUI : MonoBehaviour
    {
        #region Variables
        public GameObject pausePanel;
        public GameObject background;
        public SceneFader fader;
        public GameObject moveInput;

        private bool isPaused = false;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
                pause(isPaused);
            }
        }
        #endregion

        #region Custom Method
        private void pause(bool paused)
        {
            pausePanel.SetActive(paused);
            background.SetActive(paused);
            moveInput.SetActive(!paused);

            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = paused;
            Time.timeScale = paused ? 0f : 1f;
        }
        public void OnMainButton()
        {
            pause(false);
            fader.FadeTo("TitleScene");
        }

        public void OnOptions()
        {
            Debug.Log("옵션 열기");
        }

        public void OnCancel()
        {
            pause(false);
        }
        #endregion
    }

}
