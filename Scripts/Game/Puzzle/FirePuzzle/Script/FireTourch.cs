using UnityEngine;
using System.Collections;
//using AK.Wwise;   

namespace Wilson.Game.Puzzle
{
    public class FireTourch : MonoBehaviour
    {
        #region Variables
        private Animator animator;                             // 횃불 애니메이터
        [SerializeField] private GameObject vfxTorchLight;     // 불 이펙트 오브젝트
        private int alternateCount = 0;                         // Q/E 번갈아 누른 횟수
        private KeyCode lastKey = KeyCode.None;                 // 마지막 입력된 키
        private bool torchActivated = false;                    // 불이 켜졌는지 여부
        private float animationResetDelay = 0.1f;               // 애니메이션 리셋 딜레이

        public GameObject player;                               // 다시 활성화할 플레이어 오브젝트
        public GameObject firePickObject;                       // FirePick 오브젝트 (이벤트 종료 후 재활성화용)

        public delegate void TorchEventHandler();
        public event TorchEventHandler OnTorchEventCompleted;

        //public AK.Wwise.Event MakeFireWood1;
        //public AK.Wwise.Event MakeFireWood2;
        //public AK.Wwise.Event Fire;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            

            ResetFire(); // 오브젝트 활성화 시 상태 초기화
        }

        private void Update()
        {
            // R 키 입력은 항상 감지 → 이벤트 강제 종료용
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Torch Event forcefully exited by R key.");

                player.SetActive(true);               // 플레이어 다시 켜기
                gameObject.SetActive(false);          // FireTourch 종료

                if (firePickObject != null)
                    firePickObject.SetActive(true);   // FirePick 다시 활성화

                ResetFire();                          // 내부 상태 초기화
                OnTorchEventCompleted?.Invoke();      // 외부 알림
                return;
            }

            // Q/E 입력 감지는 불이 아직 켜지지 않았을 때만 수행
            if (!torchActivated)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    //MakeFireWood1.Post(gameObject);
                    CheckAlternatingInput(KeyCode.Q);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    //MakeFireWood2.Post(gameObject);
                    CheckAlternatingInput(KeyCode.E);
                }
            }
        }
        #endregion

        #region Logic

        private void PlayBoolAnimation(string boolName)
        {
            animator.SetBool(boolName, true);
            StartCoroutine(ResetBoolAfterDelay(boolName, animationResetDelay));
        }

        private IEnumerator ResetBoolAfterDelay(string boolName, float delay)
        {
            yield return new WaitForSeconds(delay);
            animator.SetBool(boolName, false);
        }

        private void CheckAlternatingInput(KeyCode currentKey)
        {
            if (lastKey == KeyCode.None)
            {
                lastKey = currentKey;
                return;
            }

            if (lastKey != currentKey)
            {
                alternateCount++;
                lastKey = currentKey;

                Debug.Log($"Alternating Count: {alternateCount}");

                // 25회 입력되면 불 점화
                if (alternateCount >= 25 && vfxTorchLight != null)
                {
                    //MakeFireWood2.Post(gameObject);
                    CompleteTorchEvent();
                }
            }
        }

        // 불 점화 후 이벤트 완료 상태로 전환
        private void CompleteTorchEvent()
        {
            vfxTorchLight.SetActive(true);    // 불 켜기
            
            torchActivated = true;            // 상태 전환
            Debug.Log("Torch Activated! Press R to exit.");
        }

        // 이벤트 리셋 (OnEnable 또는 강제 종료 시 호출)
        public void ResetFire()
        {
            if (vfxTorchLight != null)
                vfxTorchLight.SetActive(false);   // 불 끄기

            alternateCount = 0;
            lastKey = KeyCode.None;
            torchActivated = false;
        }

        #endregion
    }
}