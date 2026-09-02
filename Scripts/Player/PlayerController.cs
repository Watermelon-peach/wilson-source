using UnityEngine;

namespace Wilson.Player
{
    //윌슨의 이동을 제어하는 클래스
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //참조
        public Rigidbody volleyball;            //배구공 강체
        public GroundCheck groundCheck;         //그라운드 체크 트리거
        public InputHandler input;             //플레이어 인풋 정보

        private Transform cameraTransform;

        //물리수치들
        [SerializeField] private float defaultMoveForce = 5f;   //이동 힘 기본값
        [SerializeField] private float runForceFactor = 3f;     //달리기 인수(계수)
        [SerializeField] private float maxSpeed = 10f;          //최대 속도
        private float moveForce;                                //윌슨에 최종적으로 가해지는 힘 (move)

        //차지 점프
        [SerializeField] private float jumpForce;          //점프 힘 (5~9)
        [SerializeField] private float minJumpForce = 3f;
        [SerializeField] private float maxJumpForce = 9f;
        [SerializeField] private float jumpChargeSharpness = 1f;
        private bool wasCharged = false;
        #endregion

        #region Property
        public bool IsCharging { get { return wasCharged; } }
        public float JumpForce { get { return jumpForce; } }

        public float MinJump { get { return minJumpForce; } }
        public float MaxJump { get { return maxJumpForce; } }
        #endregion
        #region Unity Event Method
        private void Start()
        {
            //참조
            cameraTransform = Camera.main.transform;
            //초기화
            jumpForce = minJumpForce;
        }

        private void Update()
        {
            transform.position = volleyball.transform.position;
            RotateWithAspect();     //카메라가 바라보고 있는 방향으로
            UpdateInput();          //인풋 키 받아오기

            //달리기 - 공중에서 갑자기 빨라지면 어색하니까 땅에서만 달릴 수 있게
            if (input.RunHeld && groundCheck.IsGrounded)    //땅에서 Shift 입력할 시
            {
                Run();
            }
            else
            {
                //이동속도 정상화
                moveForce = defaultMoveForce;
            }

            //점프 차지
            if (input.JumpPressed)
            {
                ChargeJumpGage();
            }
        }
        private void FixedUpdate()
        {
            ClampLinearVelocity();
            Move();
            if (wasCharged &&!input.JumpPressed)
            {
                Jump();
            }
            
        }
        #endregion

        #region Custom Method
        //Input Handler에서 Input 받아오기
        private void UpdateInput()
        {
            input.GetMoveInput();
            input.GetJumpInput();
            input.GetRunInput();
        }
        //받아온 인풋 값을 토대로 윌슨 굴리기
        private void Move()
        {
            if (volleyball.linearVelocity.z > maxSpeed || volleyball.linearVelocity.x > maxSpeed)
                return;

            //로컬 기준으로 방향 변환
            Vector3 localMove = transform.TransformDirection(input.MoveInput);
            volleyball.AddForce(localMove * moveForce, ForceMode.Force);
        }
        
        //카메라가 바라보는 방향으로 회전 (y축 무시)
        private void RotateWithAspect()
        {
            Vector3 dir = transform.position - cameraTransform.position;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        private void Run()
        {
            //가해지는 힘 연산
            moveForce = runForceFactor * defaultMoveForce;
            //비주얼 이펙트 (흑먼지 etc...)
            //...
        }

        //최대속도 제한
        void ClampLinearVelocity()
        {
            Vector3 velocity = volleyball.linearVelocity;

            // XZ 평면 속도 추출
            Vector3 flatVelocity = new Vector3(velocity.x, 0f, velocity.z);

            // 평면 속도가 max를 넘었으면 자르기
            if (flatVelocity.magnitude > maxSpeed)
            {
                flatVelocity = flatVelocity.normalized * maxSpeed;
                volleyball.linearVelocity = new Vector3(flatVelocity.x, velocity.y, flatVelocity.z);
            }

            //Debug.Log(flatVelocity.magnitude);
        }

        //점프 게이지 충전
        private void ChargeJumpGage()
        {
            //TODO : 게이지 애니메이션 <<UI 클래스에서 구현
            jumpForce += jumpChargeSharpness * Time.deltaTime;
            jumpForce = Mathf.Clamp(jumpForce, minJumpForce, maxJumpForce);
            wasCharged = true;
        }
        private void Jump()
        {
            if (!groundCheck.IsGrounded)
                return;

            volleyball.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //비주얼 이펙트
            //...
            //jumpForce 초기화
            jumpForce = minJumpForce;
            wasCharged = false;
        }
        #endregion
    }

}

/*
25-06-20 : 최대속도 실험
이론상 가속하면 무한정 올라갈 것 같은데 어느 순간 속도가 안올라가는 것 같음 (아니면 매우 더디게 올라가거나)
그래서 속도제한이 딱히 필요없을 수도??
*/