using UnityEngine;
using UnityEngine.Playables;
using Wilson.Utility;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Wilson.Game
{
    /// <summary>
    /// 엔딩씬 연출부분
    /// </summary>
    public class EndingScene : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        public PlayableDirector director;
        public string nextSceneName;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            fader.FadeStart();
            StartCoroutine(WaitAndStop());
            if (director != null)
            {
                director.stopped += OnTimelineStopped;
            }
        }
        #endregion
        private IEnumerator WaitAndStop()
        {
            yield return new WaitForSeconds(20f);
            director.Stop();
        }
        private void OnTimelineStopped(PlayableDirector obj)
        {
            //Debug.Log("타임라인 끝");
            // Timeline 종료 시 호출됨
            fader.FadeTo("TitleScene");
        }

        private void OnDestroy()
        {
            if (director != null)
            {
                director.stopped -= OnTimelineStopped;
            }
        }
    }

}
