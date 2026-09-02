using UnityEngine;
using Wilson.Utility;
using TMPro;
using System.Collections;

namespace Wilson.UI
{
    //경고문구 클래스
    public class AlertBarUI : Singleton<AlertBarUI>
    {
        #region Variables
        [Header("참조")]
        public TextMeshProUGUI alertMessageText;
        public CanvasGroup group;   //알파 조절용

        [Header("설정값")]
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float maxAlpha = 0.2f;

        [Header("알파 감소 커브 (t = 0~1)")]
        [SerializeField] private AnimationCurve fadeCurve;

        #endregion

        #region Unity Event Method
        protected override void Awake()
        {
            //싱글톤 불러오기
            base.Awake();
        }
        

        #endregion

        #region Custom Method
        //다른 스크립트에서 사용 가능한 경고문구 표시 메서드
        public IEnumerator ShowAlert(string message)
        {
            alertMessageText.text = message;
            group.alpha = maxAlpha;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                group.alpha = maxAlpha * fadeCurve.Evaluate(t);
                yield return null;
            }

            group.alpha = 0f;
        }
        #endregion
    }

}
