using UnityEngine;

namespace Wilson.Game.Puzzle
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

       
        public SpawnPoint spawnPoint;
        [SerializeField] private float initialSpawnInterval = 1.5f;
        private float spawnInterval;

        
       
        
        public int maxMissCount = 10;
        private int missCount = 0;
        private int killCount = 0;
        private int currentWave = 1;
        private bool isGameOver = false;
        private bool isPuzzleActive = false;

        #region Unity Lifecycle
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            spawnInterval = initialSpawnInterval;
            spawnPoint.SetSpawnInterval(spawnInterval);
        }

        private void Update()
        {
            if (isPuzzleActive && Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("퍼즐 중단 (R 키)");
                RestartGame();
            }
        }
        #endregion

       

        #region 게임 로직
        public void OnEnemyMissed()
        {
            missCount++;
            Debug.Log($"[Missed] {missCount}/{maxMissCount}");

            if (missCount >= maxMissCount)
            {
                GameOver();
            }
        }

        public void OnEnemyKilled()
        {
            killCount++;
            if (killCount % 10 == 0)
            {
                IncreaseWave();
            }
        }

        private void IncreaseWave()
        {
            currentWave++;
            Debug.Log($"[Wave {currentWave}] 난이도 상승!");

            if (currentWave > 3)
            {
                Debug.Log("최종 웨이브 도달! 게임 종료.");
                GameOver();
                return;
            }

            spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.2f);
            spawnPoint.SetSpawnInterval(spawnInterval);
        }

        private void GameOver()
        {
            if (isGameOver) return;
            isGameOver = true;

            Debug.Log("게임 오버!");
            spawnPoint.StopSpawning();

           

            // 남은 적 제거
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
                Destroy(enemy);
        }

        public void RestartGame()
        {
            missCount = 0;
            killCount = 0;
            currentWave = 1;
            isGameOver = false;

            spawnInterval = initialSpawnInterval;

            // 적 제거
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
                Destroy(enemy);


            // 스폰 재시작
            spawnPoint.StopSpawning();
            spawnPoint.SetSpawnInterval(spawnInterval);
            spawnPoint.StartSpawning();

            Debug.Log("게임 재시작!");
        }
        #endregion
    }
}