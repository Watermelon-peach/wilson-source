using UnityEngine;
using UnityEngine.UI;

namespace Wilson.Utility
{
    public class FillRound : MonoBehaviour
    {
        float distance;
        public Image ImgFill, ImgStartDot, ImgEndDot;
        public ImgsFillDynamic ImgsFD;
        private void Start()
        {
            if (ImgStartDot != null)
                this.distance = ImgStartDot.transform.localPosition.magnitude;
        }

        public void SetFill(float _amount)
        {
            this.ImgFill.fillAmount = _amount;
            this.RefreshAngle();
        }

        void RefreshAngle()
        {
            float ratio = this.ImgsFD != null ? this.ImgsFD.Factor : this.ImgFill.fillAmount;

            // 회전 각도 계산
            float angle = ratio * 360f;
            float radian = angle * Mathf.Deg2Rad;

            // 원의 반지름 (distance)를 이용한 위치 계산
            Vector3 endPos = new Vector3(
                Mathf.Sin(radian),
                Mathf.Cos(radian),
                0f
            ) * this.distance;

            this.ImgEndDot.transform.localPosition = endPos;

            // 시작점은 항상 위쪽
            this.ImgStartDot.transform.localPosition = new Vector3(0, this.distance, 0);
        }

        float GetAngle(float _amount)
        {
            return _amount * 360F;
        }
    }

}