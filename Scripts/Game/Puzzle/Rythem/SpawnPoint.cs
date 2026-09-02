using UnityEngine;
using System.Collections;

namespace Wilson.Game.Puzzle
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private GameObject[] spawnPrefabs;
        [SerializeField] private float repeatInterval = 1f;

        private Coroutine spawnRoutine;

        public void StartSpawning()
        {
            StopSpawning(); // 항상 기존 루틴을 멈추고 다시 시작

            if (spawnPrefabs.Length > 0)
                spawnRoutine = StartCoroutine(SpawnLoop());
        }

        public void StopSpawning()
        {
            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                SpawnRandom();
                yield return new WaitForSeconds(repeatInterval);
            }
        }

        private void SpawnRandom()
        {
            if (spawnPrefabs.Length == 0)
            {
                Debug.LogWarning("SpawnPrefabs가 비어 있습니다!");
                return;
            }

            int index = Random.Range(0, spawnPrefabs.Length);
            GameObject prefab = spawnPrefabs[index];
            Debug.Log("Prefab 선택됨: " + prefab.name);
            SpawnObject(prefab);
        }

        public GameObject SpawnObject(GameObject prefab)
        {
            if (prefab != null)
            {
                Vector3 pos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                GameObject obj = Instantiate(prefab, pos, transform.rotation);
                Debug.Log("Spawned object at: " + pos);
                return obj;
            }
            else
            {
                Debug.LogError("Null prefab 전달됨");
                return null;
            }
        }

        public void SetSpawnInterval(float interval)
        {
            repeatInterval = interval;

            // 즉시 반영되도록 재시작
            StartSpawning();
        }
    }
}