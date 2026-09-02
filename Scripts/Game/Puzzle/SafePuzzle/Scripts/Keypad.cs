using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Wilson.Game;


namespace Wilson.Game.Puzzle
{
    public class Keypad : MonoBehaviour
    {
        #region 변수 선언
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;   // 접근 성공 시 호출될 이벤트
        [SerializeField] private UnityEvent onAccessDenied;    // 접근 실패 시 호출될 이벤트

        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private int keypadCombo = 12345;      // 정답으로 설정된 패스코드

        // 외부 접근용 이벤트 프로퍼티
        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";  // 성공 시 표시될 텍스트
        [SerializeField] private string accessDeniedText = "Denied";    // 실패 시 표시될 텍스트

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f; // 결과 텍스트 표시 시간
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f; // 디스플레이 발광 강도

        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); // 기본 발광 색상 (주황)
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f);            // 실패 시 색상 (빨강)
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f);         // 성공 시 색상 (초록)

        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;    // 버튼 클릭 사운드
        [SerializeField] private AudioClip accessDeniedSfx;     // 실패 사운드
        [SerializeField] private AudioClip accessGrantedSfx;    // 성공 사운드

        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;            // 키패드 디스플레이 (Emission 제어)
        [SerializeField] private TMP_Text keypadDisplayText;    // 현재 입력된 문자열을 표시하는 텍스트
        [SerializeField] private AudioSource audioSource;       // 사운드 재생용 오디오 소스

        private string currentInput;           // 현재까지 입력된 숫자 문자열
        private bool displayingResult = false; // 결과 텍스트를 표시 중인지 여부
        private bool accessWasGranted = false; // 성공 여부 플래그
        /*------------------------------------------------------------------------------------------------*/
        private bool readyToExit = false; // E키 입력 대기 상태

        [SerializeField] private Camera cam;           // 입력 감지를 위한 카메라
        [SerializeField] private LayerMask buttonLayer; // 버튼만 감지하기 위한 레이어 마스크
        [SerializeField] private GameObject safeCloseObject;       // safe_close 전체 오브젝트
        [SerializeField] private GameObject keypadCameraObject;    // safe 안의 카메라
        [SerializeField] private SlidingDoor door;                 // 연결된 슬라이딩 도어
        [SerializeField] private GameObject safeObjectToDestroy; // 퍼즐 완료 후 파괴할 Safe 오브젝트
        [SerializeField] private GameObject rewardItem;         //퍼즐 완료 보상

        public GameObject player;                          // 다시 활성화할 플레이어 오브젝트
        public GameObject safePickObject; // 인스펙터에서 SafePick 오브젝트 연결

        private bool isExiting = false; // 중복 종료 방지
        private bool exitEarly = false; // 퍼즐 성공 전에 나감 여부

        // 외부에서 이벤트 종료 알림을 받을 수 있는 사용자 정의 델리게이트 이벤트
        public delegate void KeypadEventHandler();
        public event KeypadEventHandler OnKeypadEventCompleted;
        #endregion

        private void Awake()
        {
            // 키패드 초기화
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        private void OnEnable()
        {
            // 활성화 시점에 마우스 커서 보이도록 설정 (1프레임 지연 필요)
            StartCoroutine(EnableCursorAfterFrame());
        }

        private IEnumerator EnableCursorAfterFrame()
        {
            yield return null; // 1프레임 대기
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            // 키패드 버튼 클릭 처리
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 5f, buttonLayer))
                {
                    if (hit.collider.TryGetComponent(out KeypadButton button))
                    {
                        button.PressButton(); // 버튼 클릭
                    }
                }
            }

            // 퍼즐 도중 언제든 E키로 포기 가능
            if (!isExiting && Input.GetKeyDown(KeyCode.E))
            {
                if (!accessWasGranted)
                {
                    ExitKeypad();//퍼즐종료
                }
                else if (!accessWasGranted)
                {
                    exitEarly = true; // 퍼즐 도중 포기
                    ExitKeypad(); // 동일한 종료 루틴 사용
                }
            }
        }

        // 버튼으로부터 입력을 받을 때 호출됨
        public void AddInput(string input)
        {
            audioSource.PlayOneShot(buttonClickedSfx); // 클릭 사운드 재생

            if (displayingResult || accessWasGranted)
                return; // 결과 표시 중 또는 이미 성공했다면 입력 무시

            switch (input)
            {
                case "enter":
                    CheckCombo(); // "enter" 입력이면 정답 확인
                    break;
                default:
                    if (currentInput != null && currentInput.Length == 9)
                        return; // 9자리 초과 방지

                    currentInput += input; // 입력 추가
                    keypadDisplayText.text = currentInput;
                    break;
            }
        }

        // 입력값과 정답 코드 비교
        public void CheckCombo()
        {
            if (int.TryParse(currentInput, out var currentKombo))
            {
                bool granted = currentKombo == keypadCombo;

                if (!displayingResult)
                {
                    StartCoroutine(DisplayResultRoutine(granted)); // 결과 표시 루틴 시작
                }
            }
            else
            {
                Debug.LogWarning("입력값을 숫자로 변환할 수 없습니다.");
            }
        }

        // 결과 표시 코루틴
        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            // [이벤트 시작]
            if (granted)
                AccessGranted(); // 성공 이벤트 및 표시
            else
                AccessDenied();  // 실패 이벤트 및 표시
            // [이벤트 종료]

            yield return new WaitForSeconds(displayResultTime); // 일정 시간 대기
            displayingResult = false;

            if (granted) yield break; // 성공 시에는 입력 유지

            ClearInput(); // 실패 시 입력 초기화
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity); // 기본 색상 복원
        }

        // 접근 실패 처리
        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText;
            onAccessDenied?.Invoke(); // Unity 이벤트 실행
            panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity); // 색상 변경
            audioSource.PlayOneShot(accessDeniedSfx); // 사운드 재생


        }

        // 입력 초기화
        private void ClearInput()
        {
            currentInput = "";
            keypadDisplayText.text = currentInput;
        }

        // 접근 성공 이벤트처리
        private void AccessGranted()
        {

            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            onAccessGranted?.Invoke(); // Unity 이벤트 실행 연결된 SlidingDoor.OpenDoor() 실행
            panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity); // 색상 변경
            audioSource.PlayOneShot(accessGrantedSfx); // 사운드 재생


            //  Safe 오브젝트 파괴
            if (door != null)
            {
                door.OpenDoor();
            }

            if (safeObjectToDestroy != null)
            {
                Instantiate(rewardItem, transform.position, Quaternion.identity);
                Destroy(safeObjectToDestroy);
            }

            //  플레이어 복원
            if (player != null)
            {
                player.SetActive(true);
            }

            //  커서 비활성화
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            // E 키 대기 시작
            readyToExit = true;
            OnKeypadEventCompleted?.Invoke();



        }
        //E키 입력받아 빠져나오기
        private void ExitKeypad()
        {
            if (isExiting) return;
            isExiting = true;

            // 퍼즐 중도 포기라면 입력 초기화
            if (exitEarly)
            {
                ClearInput(); // 숫자 입력 초기화
                panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
            }

            // 플레이어 복귀
            if (player != null)
                player.SetActive(true);

            // 카메라 비활성화
            if (keypadCameraObject != null)
                keypadCameraObject.SetActive(false);

            // 금고 전체 비활성화
            if (safeCloseObject != null)
                safeCloseObject.SetActive(false);

            // 가짜 금고 다시 활성화
            if (safePickObject != null)
                safePickObject.SetActive(true);

            // 문은 정답일 때만 열렸으므로 닫을 필요 없음 (or CloseDoor로 안전 처리)
            if (door != null)
                door.CloseDoor();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            readyToExit = false;
            OnKeypadEventCompleted?.Invoke();

            Debug.Log(exitEarly ? "Keypad exited early with E key." : "Keypad exited after solving.");
        }
    }
}