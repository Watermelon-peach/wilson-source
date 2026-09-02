using UnityEngine;
using Wilson.Player;
using Wilson.Utility;

namespace Wilson.UI
{
    //게이지, 인터랙티브 키 디스플레이가 포함된 캔버스를 컨트롤하는 클래스
    public class ChargingGageUI : MonoBehaviour
    {
        #region Variables
        //월드스페이스 캔버스
        public Transform gageCanvas;

        public CanvasGroup gageGroup;
        private PlayerController controller;
        private Transform playerCamera;

        private bool wasCharging = false;

        //차징 게이지
        public ImgsFillDynamic ImgFD;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            controller = GetComponent<PlayerController>();
            playerCamera = Camera.main.transform;
        }

        private void Start()
        {
            //초기화
            gageGroup.alpha = 0f;
        }

        private void Update()
        {
            UpdateGage();
            LookAtCamera();
            OnOffGage();

            wasCharging = controller.IsCharging;
        }
        #endregion

        #region Custom Method
        private void UpdateGage()
        {
            if (!controller.IsCharging)
            {
                ImgFD.SetValue(0, true);
                return;
            }

            ImgFD.SetValue(Mathf.InverseLerp(controller.MinJump, controller.MaxJump, controller.JumpForce),true);
        }

        private void LookAtCamera()
        {
            Vector3 dir = (playerCamera.position - gageCanvas.position).normalized;
            gageCanvas.rotation = Quaternion.LookRotation(dir);
        }

        private void OnOffGage()
        {
            if (wasCharging != controller.IsCharging)
            {
                gageGroup.alpha = controller.IsCharging ? 1 : 0;
            }
            
        }
        #endregion

    }

}
