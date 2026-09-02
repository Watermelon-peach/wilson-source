    using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Wilson.Game;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Wilson.UI;

namespace Wilson.NPC
    {
        public enum NPCState { Sleep, Idle, Walk, WalkToPlayer, Gathering, LookArround, EatFood, DrinkWater, ThrowPlayer, PuttingDown, HoldingMove, HasItem, CheckNet }

    public class NPCFSM : MonoBehaviour
    {
        #region Variables
        //참조
        private NavMeshAgent agent;
        private Animator animator;
        public TimeManager timeManager;
        public Stats stats;

        //플레이어
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject palyerInput;
        //플레이어 시작 지점
        [SerializeField] private Transform playerSpawn;
        [SerializeField]private NPCInventory nPCInventory;

        [SerializeField] private FishTrapUI fishTrapUI;

        private Vector3 lastPlayerPos;
        //오른손 위치 
        public GameObject righthand;
        //플레이어의 현제 상태값 저장
        private NPCState currentState;
        //현제 진행중이 코루틴값
        private Coroutine currentRoutine;
        //그믈 조사하였는지 체크
        [SerializeField] private bool isCheckNet;
        //아이템 소지여부 체크
        public bool hasItems;

        //NPC한테 걸렸는지?
        [SerializeField] private bool isDetected = false;
        //구역밖을 이동하다 걸렸는지
        private bool isGoOut = false;
        //낮밤 변화
        private bool wasDayChange = true;
        //그물 위치
        [SerializeField] private Transform NetPos;
        //창고 위치
        [SerializeField] private Transform storagePos;
        //잠자는 위치
        [SerializeField] private Transform sleepPos;
        //이동 목표 지점
        private Vector3 targetPosition;
        //위치 못찾으면 반환하는 최후의 위치
        [SerializeField] private Transform CenterTargetPos;

        //손에 붙히기
        private bool catchPlayer = false;

        //아침구역 콜라이더
        [SerializeField] private GameObject NPCArea;

        public bool CanChangeDay;

        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            Rigidbody rb = player.GetComponent<Rigidbody>();


            //기본값 셋팅
            lastPlayerPos = player.transform.position;
            ChangeState(NPCState.Sleep);
            isDetected = false;
            NPCArea.SetActive(true);
        }

        private void Update()
        {
            CatchPlayer();

            if (currentState == NPCState.Idle || currentState == NPCState.Walk || currentState ==  NPCState.CheckNet || currentState == NPCState.HasItem )
            {
                CanChangeDay = true;
            }
            else
            {
                CanChangeDay = false;
            }

            if (timeManager.IsDay)
            {
                if (currentState == NPCState.Idle || currentState == NPCState.Walk)
                {
                    if (IsPlayerMovingInRange())
                    {
                        StopAllCoroutines(); // 현재 진행 중인 코루틴 모두 정지
                        agent.ResetPath();  //목표지점 초기회
                        animator.SetBool("Idle", false);
                        animator.SetBool("Walk", false);

                        ChangeState(NPCState.LookArround);
                    }
                    // lastPlayerPos 갱신 (매 프레임마다 추적용)
                    lastPlayerPos = player.transform.position;
                }
            }

            if (wasDayChange != timeManager.IsDay)
            { 
                IsDaychange();
                wasDayChange = timeManager.IsDay;

            }
        }

        #endregion

        #region Custom Method
        private void ChangeState(NPCState newState)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
            }

            currentState = newState;

            switch (newState)
            {
                case NPCState.Sleep:
                    currentRoutine = StartCoroutine(Sleep());
                    break;
                case NPCState.Idle:
                    currentRoutine = StartCoroutine(Idle());
                    break;
                case NPCState.Walk:
                    currentRoutine = StartCoroutine(Walk());
                    break;
                case NPCState.LookArround:
                    currentRoutine = StartCoroutine(LookArround());
                    break;
                case NPCState.WalkToPlayer:
                    currentRoutine = StartCoroutine(WalkToPlayer());
                    break;
                case NPCState.ThrowPlayer:
                    currentRoutine = StartCoroutine(ThrowPlayer());
                    break;
                case NPCState.HoldingMove:
                    currentRoutine = StartCoroutine(HoldingMove());
                    break;
                case NPCState.PuttingDown:
                    currentRoutine = StartCoroutine(PuttingDown());
                    break;
                case NPCState.CheckNet:
                    currentRoutine = StartCoroutine(CheckNet());
                    break;
                case NPCState.HasItem:
                    currentRoutine = StartCoroutine(HasItem());
                    break;
            }
        }

        private IEnumerator Sleep()
        {
            //잠을 자고이쓴 애니메이션 실행

            while (currentState == NPCState.Sleep)
            {
                if (timeManager.IsDay)
                {
                    isCheckNet = false;
                    animator.SetBool("isNight", false);
                    animator.SetBool("DayNightCycle", false);

                    //기상 애니메이션 실행 - 자동으로 됨
                    //기상 애니메이션 기다리기
                    yield return new WaitForSeconds(8.5f);

                    //기상
                    ChangeState(NPCState.Idle);
                }
                else
                {
                    animator.SetBool("DayNightCycle", true);
                    yield return null;
                }
            }
        }


        private IEnumerator Idle()
        {
            //Idle 애니메이션 실행 - 자동으로 됨
            animator.SetBool("Idle", true);

            NPCArea.SetActive(true);
            //잠시 대기
            yield return new WaitForSeconds(1.5f);

            ///타겟 포지션 결정
            if (!isCheckNet)
            {
                targetPosition = NetPos.position;

                ChangeState(NPCState.CheckNet);
            }
            else if (hasItems)
            {
                targetPosition = storagePos.position;

                ChangeState(NPCState.HasItem);
            }
            else
            {
                //랜덤 타겟 포지션 구하기
                targetPosition = GetRandomNavMeshPosition(transform.position, 50f);
                //걷기로 상태 변경
                ChangeState(NPCState.Walk);
            }
            animator.SetBool("Idle", false);
        }

        private IEnumerator Walk()
        {
            agent.SetDestination(targetPosition);

            yield return null;

            // 걷기 애니메이션 실행
            animator.SetBool("Walk", true);

            // 목적지 도달까지 대기
            while (currentState == NPCState.Walk && agent.remainingDistance > 0.1f)
            {
                yield return null;
            }

            animator.SetBool("Walk", false);
            ChangeState(NPCState.Idle);
        }

        private IEnumerator CheckNet()
        {
            animator.SetBool("Walk", true);
            agent.SetDestination(NetPos.position);
            yield return null;

            while (agent.remainingDistance > 0.5f)
            {
                yield return null;
            }
            animator.SetBool("Walk", false);

            animator.SetBool("Idle", true);
            //그물 이벤트 구현
            nPCInventory.FishtrapToNPC();

            fishTrapUI.RefreshUI();
             
            isCheckNet = true;

            //아이템 있는 것 처럼 구현
            hasItems = true;

            yield return new WaitForSeconds(2f);

            ChangeState(NPCState.Idle);
        }

        private IEnumerator HasItem()
        {

            agent.ResetPath();
            agent.SetDestination(storagePos.position);
            yield return null;

            //창고 위치 이동하기
            animator.SetBool("Walk", true);
            while (agent.remainingDistance > 0.3f)
            {
                yield return null;
            }

            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
            yield return new WaitForSeconds(3f);
            //창고 이벤트 구현
            nPCInventory.NPCToStorage();
            hasItems = false;
            yield return null;

            //밥먹기
            stats.EatFoodWater();

            animator.SetBool("Idle", false);

            ChangeState(NPCState.Idle);

        }

        private IEnumerator LookArround()
        {
            animator.SetBool("LookArround", true);

            yield return new WaitForSeconds(1.5f);

            float detectionDuration = 0f;

            while (true)
            {
                bool isInRange = IsPlayerMovingInRange();

                if (isInRange)
                {
                    detectionDuration += Time.deltaTime;

                    if (detectionDuration >= 3.5f)
                    {
                        animator.SetBool("LookArround", false);

                        palyerInput.SetActive(false);
                        Rigidbody rb = player.GetComponent<Rigidbody>();
                        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                        rb.useGravity = true;
                        rb.isKinematic = false;

                        isDetected = true;
                        detectionDuration = 0f;

                        ChangeState(NPCState.WalkToPlayer);
                        yield break;
                    }
                }
                else
                {
                    detectionDuration = 0f;
                    animator.SetBool("LookArround", false);
                    ChangeState(NPCState.Idle);
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator WalkToPlayer()
        {

            // 걷기 애니메이션 실행
            animator.SetBool("WalkToPlayer", true);
            NPCArea.SetActive(false);
            Vector3 targetPos = player.transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(targetPos, out hit, 7f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                yield return null;
            }
            else
            {
                yield break; // 목적지 설정 실패 시 중단
            }

            // 도착할 때까지 기다림
            while (agent.pathPending || agent.remainingDistance > 0.3f)
            {
                yield return null; // 프레임마다 체크
            }

            // 걷기 애니메이션 종료 후 줍기
            animator.SetBool("WalkToPlayer", false);

            // 줍기 시작
            animator.SetBool("PickUp", true);
            yield return new WaitForSeconds(5f);
            animator.SetBool("PickUp", false);

            if (isDetected)
            {
                ChangeState(NPCState.ThrowPlayer);
            }
            else if (isGoOut)
            {
                ChangeState(NPCState.HoldingMove);
            }
        }

        private IEnumerator ThrowPlayer()
        {
            // 던지기 방향으로 즉시 회전
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

            Rigidbody prb = player.GetComponent<Rigidbody>();
            prb.constraints = RigidbodyConstraints.None;
            bool wasThrow = true;

            // 던지기 애니메이션 시작
            animator.SetBool("Throw", true);

            while (wasThrow)
            {
                yield return new WaitForSeconds(0.8f);

                player.transform.SetParent(null);
                isDetected = false;

                if (prb != null)
                {
                    prb.isKinematic = false;
                    prb.useGravity = true;
                    catchPlayer = false;

                    Vector3 throwDir = Vector3.back + Vector3.up;
                    prb.AddForce(throwDir.normalized * 30f, ForceMode.Impulse);
                }
                yield return new WaitForSeconds(0.8f);
                wasThrow = false;
            }

            palyerInput.SetActive(true);
            animator.SetBool("Throw", false);
            ChangeState(NPCState.Idle); // 다음 상태로 전환
        }

        private IEnumerator HoldingMove()
        {
            animator.SetBool("isGoOut", true);

            isGoOut = false;

            agent.SetDestination(playerSpawn.position);
            yield return null;


            //yield return new WaitUntil(() => !agent.pathPending);

            while (agent.remainingDistance > 0.3f)
            {
                yield return null;
            }

            animator.SetBool("isGoOut", false);
            
            ChangeState(NPCState.PuttingDown);
        }

        private IEnumerator PuttingDown()
        {
            animator.SetBool("PuttingDown", true);

            yield return new WaitForSeconds(3f);

            catchPlayer = false;

            Rigidbody prb = player.GetComponent<Rigidbody>();
            if (prb != null)
            {
                prb.isKinematic = false;
                prb.useGravity = true;
                prb.constraints = RigidbodyConstraints.None;
            }
            palyerInput.SetActive(true);
            NPCArea.SetActive(true);

            animator.SetBool("PuttingDown", false);
            ChangeState(NPCState.Idle);

        }

        private void IsDaychange()
        {
            if (wasDayChange != timeManager.IsDay)
            {
                StopAllCoroutines();
                agent.ResetPath();

                // 애니메이션 종료
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("LookArround", false);
                animator.SetBool("Throw", false);
                animator.SetBool("PickUp", false);
                animator.SetBool("PuttingDown", false);
                animator.SetBool("isGoOut", false);
                animator.SetBool("WalkToPlayer", false);

                animator.SetBool("isNight", true);
                transform.position = sleepPos.position;
                isCheckNet = false;
                
                NPCArea.SetActive(!NPCArea.activeSelf);

                ChangeState(NPCState.Sleep);
            }
        }

        //네브메쉬 상의 모래 위의 랜덤한 포지션값 반환
        private Vector3 GetRandomNavMeshPosition(Vector3 center, float radius)
        {
            Terrain terrain = Terrain.activeTerrain;
            if (terrain == null) return CenterTargetPos.position;

            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPos = terrain.transform.position;

            // "layer_Soil" 레이어 인덱스 찾기 (정확한 이름 사용)
            int soilIndex = -1;
            for (int i = 0; i < terrainData.terrainLayers.Length; i++)
            {
                if (terrainData.terrainLayers[i] != null && terrainData.terrainLayers[i].name.Equals("Layer_Soil"))
                {
                    soilIndex = i;
                    break;
                }
            }

            if (soilIndex == -1)
            {
                return CenterTargetPos.position;
            }

            int maxAttempts = 30;

            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * radius;
                randomDirection.y = 0;
                Vector3 samplePos = center + randomDirection;

                if (NavMesh.SamplePosition(samplePos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
                {
                    Vector3 worldPos = hit.position;
                    float normalizedX = Mathf.InverseLerp(terrainPos.x, terrainPos.x + terrainData.size.x, worldPos.x);
                    float normalizedZ = Mathf.InverseLerp(terrainPos.z, terrainPos.z + terrainData.size.z, worldPos.z);

                    int mapX = Mathf.Clamp((int)(normalizedX * terrainData.alphamapWidth), 0, terrainData.alphamapWidth - 1);
                    int mapZ = Mathf.Clamp((int)(normalizedZ * terrainData.alphamapHeight), 0, terrainData.alphamapHeight - 1);

                    float[,,] alphamaps = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

                    float soilWeight = alphamaps[0, 0, soilIndex];
                    if (soilWeight > 0.5f)
                    {
                        return hit.position;
                    }
                }
            }

            // fallback
            if (NavMesh.SamplePosition(center, out NavMeshHit fallbackHit, 10f, NavMesh.AllAreas))
            {
                return fallbackHit.position;
            }

            return CenterTargetPos.position;
        }

        private bool IsPlayerMovingInRange()
        {
            // 탐지 거리 및 이동 감지 민감도
            float detectionRange = 5f;
            float movementThreshold = 0.01f;

            // 현재 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float movedDistance = Vector3.Distance(player.transform.position, lastPlayerPos);
            // 조건 검사
            return (distanceToPlayer < detectionRange && movedDistance > movementThreshold);
        }
        public void ChangeToWalkToPlayerFromTrigger()
        {
            if (currentState != NPCState.WalkToPlayer)
            {
                palyerInput.SetActive(false);
                Rigidbody rb = player.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                rb.useGravity = true;
                rb.isKinematic = false;

                isGoOut = true;

                StopAllCoroutines(); // 현재 진행 중인 코루틴 모두 정지



                agent.ResetPath();  //목표지점 초기회
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("LookArround", false);

                isGoOut = true;

                NPCArea.SetActive(timeManager.IsDay);

                ChangeState(NPCState.WalkToPlayer);
            }
        }

        private void OnCatchPlayer()
        {
            catchPlayer = true;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        private void CatchPlayer()
        {
            if(catchPlayer)
            {
                player.transform.position = righthand.transform.position;
                
            }
        }
        #endregion
        }
    }