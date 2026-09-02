using System.Collections;
using UnityEngine;
using Wilson.Player;
using Wilson.Utility;
using Wilson.NPC;
using Wilson.UI;

namespace Wilson.Game
{
    //게임 내 시간 관리 및 연출을 담당하는 클래스
    public class TimeManager : MonoBehaviour
    {
        #region Variables
        //참조
        public InputHandler input; //밤낮 전환 키 인풋 (T)

        public Transform volleyBall;    //공
        public Transform playerSpawn;   //플레이어 스폰 포인트
    
        public GameObject playerSettings;
        public GameObject skyCamera;

        public ItemSpawner itemSpawner;
        private TimeLerp timeLerp;

        public NPCFSM npcFSM;

        private int day = 0;        //며칠 지났는지 저장할 변수

        //연출
        public SceneFader fader;            //페이더 프리팹
        public AnimationCurve curve;        //밤 리얼타임 연출용 애니메이션 커브
        private float switchingTime = 5f;   //밤낮 전환연출 시간

        //두번 전환 방지
        private bool isSwitched = false;
        #endregion

        #region Property
        public int Day { get { return day; } private set { day = value; } }         //UI, 게임 저장 정보 등에서 필요로할 속성
        public bool IsDay { get; private set; }     //true면 낮, false면 밤
        public float TimeLimit { get; set; }
        public float CountDown { get; private set; }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            timeLerp = GetComponent<TimeLerp>();
            
        }
        private void Start()
        {
            //초기화
            TimeLimit = 300f;
            SetDayEnvironment();
        }

        private void Update()
        {
            //낮에는 T키 인풋 활성화, 밤에는 x
            if (IsDay)
            {
                input.GetTimeSwitchInput();
                if (input.TimeSwitch && !isSwitched && npcFSM.CanChangeDay)
                {
                    isSwitched = true;
                    StartCoroutine(SwitchDayToNight());
                }
                else if(input.TimeSwitch)
                {
                    StartCoroutine(AlertBarUI.Instance.ShowAlert("토미가 행동중입니다"));
                    Invoke(nameof(HideMonolog), 1.5f);
                }
            }
            else
            {
#if UNITY_EDITOR
                //TODO : 치트키
                if (Input.GetKeyDown(KeyCode.K))
                {
                    StopAllCoroutines();
                    StartCoroutine(SwitchNightToDay());
                }
#endif
            }

        }
        #endregion

        #region Custom Method
        //낮 환경 세팅
        private void SetDayEnvironment()
        {
            isSwitched = false;
            IsDay = true;

            //날이 밝으면 1일 추가
            day++;
            timeLerp.TValue = 12;   //라이팅 12시로 설정

            //페이드 인
            fader.FadeStart();

            //플레이어 스폰 위치로
            volleyBall.position = playerSpawn.position;
        }

        //밤 환경 세팅 (제한시간 짧아지는 돌발 이벤트 가능성 열어둠)
        private IEnumerator SetNightEnvironment(float timeLimit = 300f)
        {
            timeLerp.TValue = 0f;
            IsDay = false;
            itemSpawner.SpawnItems();

            CountDown = timeLimit;
            while (true)
            {
                CountDown -= Time.deltaTime;

                //timeLerp.TValue += (6f / timeLimit) * Time.deltaTime;   //TimeLerp.TValue = 0~6까지 timeLimit동안 균일하게 증가
                float elapsedTime = timeLimit - CountDown;
                float t = Mathf.Clamp01(elapsedTime / timeLimit);
                timeLerp.TValue = curve.Evaluate(t) * 6f;


               

                if (CountDown <= 0.01f)
                {
                    StartCoroutine(SwitchNightToDay());
                    break;
                }
                yield return null;
            }
        }

        //낮 -> 밤 전환 연출
        private IEnumerator SwitchDayToNight()
        {
            timeLerp.TValue = 12f;

            //1.페이드 아웃 / 인
            fader.Blink(1f);

            yield return new WaitForSeconds(1f);

            //+카메라 교체 (플레이어 -> 하늘)
            playerSettings.SetActive(false);
            skyCamera.SetActive(true);

            yield return new WaitForSeconds(1f);

            //2. 낮 -> 밤 서서히 (TValue 12 -> 24) <<총 5초, 밤낮보다 속도가 빠름.
            while (timeLerp.TValue <= 23.9f)
            {
                timeLerp.TValue += (12f / switchingTime) * Time.deltaTime;
                yield return null;
            }
            timeLerp.TValue = 24f;

            //3.페이드 아웃 / 인
            fader.Blink(1f);

            yield return new WaitForSeconds(1f);

            //+카메라 교체 (하늘 -> 플레이어)
            playerSettings.SetActive(true);
            skyCamera.SetActive(false);

            yield return new WaitForSeconds(1f);

            //밤 시작
            StartCoroutine(SetNightEnvironment());
        }

        //밤 -> 낮 전환 연출
        private IEnumerator SwitchNightToDay()
        {
            timeLerp.TValue = 6f;
            //1.페이드 아웃 / 인
            fader.Blink(1f);

            yield return new WaitForSeconds(1f);

            //+카메라 교체 (플레이어 -> 하늘)
            playerSettings.SetActive(false);
            skyCamera.SetActive(true);

            yield return new WaitForSeconds(1f);

            //새벽 -> 낮 서서히 (TValue 6 -> 12) <<총 5초
            while (timeLerp.TValue <= 12f)
            {
                timeLerp.TValue += (6f / switchingTime) * Time.deltaTime;
                yield return null;
            }
            timeLerp.TValue = 12f;

            //2.페이드 아웃 / 인
            fader.Blink(1f);
            yield return new WaitForSeconds(1f);

            //+카메라 교체 (하늘 -> 플레이어)
            playerSettings.SetActive(true);
            skyCamera.SetActive(false);

            yield return new WaitForSeconds(1f);

            //3.낮 세팅
            SetDayEnvironment();
        }

        private void HideMonolog()
        {
            MonologUI.Instance.group.alpha = 0f;
        }
        #endregion
    }

}
