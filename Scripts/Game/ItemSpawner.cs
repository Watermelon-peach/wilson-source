using UnityEngine;

namespace Wilson.Game
{
    //아이템 스폰 관리 클래스
    public class ItemSpawner : MonoBehaviour
    {
        [Header("아이템 설정")]
        [SerializeField] private GameObject[] itemPrefabs;
        [SerializeField] private int[] spawnChances = { 9, 1, 6, 6, 6, 18, 18, 18, 18 };

        [Header("스폰 영역별 설정")]
        [SerializeField] private SpawnArea[] spawnAreas;

        [SerializeField] private float spawnHeight = 30f;

        public void SpawnItems()
        {
            if (itemPrefabs.Length != spawnChances.Length)
            {
                Debug.LogError("아이템 수와 확률 수가 일치하지 않습니다!");
                return;
            }

            foreach (var area in spawnAreas)
            {
                for (int i = 0; i < area.spawnCount; i++)
                {
                    GameObject selectedItem = GetRandomItemByChance();
                    Vector3 spawnPos = GetRandomXZOnPlane(area.plane, spawnHeight);
                    Instantiate(selectedItem, spawnPos, Quaternion.identity);
                }
            }
        }

        private GameObject GetRandomItemByChance()
        {
            int total = 0;
            foreach (int c in spawnChances) total += c;

            int rand = Random.Range(0, total);
            int cumulative = 0;
            for (int i = 0; i < spawnChances.Length; i++)
            {
                cumulative += spawnChances[i];
                if (rand < cumulative)
                    return itemPrefabs[i];
            }

            return itemPrefabs[0]; // fallback
        }

        private Vector3 GetRandomXZOnPlane(Transform plane, float height)
        {
            Vector3 center = plane.position;
            Vector3 size = plane.localScale * 10f;

            float randX = Random.Range(-size.x / 2f, size.x / 2f);
            float randZ = Random.Range(-size.z / 2f, size.z / 2f);

            return new Vector3(center.x + randX, center.y + height, center.z + randZ);
        }
    }

    [System.Serializable]
    public struct SpawnArea
    {
        public Transform plane;
        public int spawnCount;
    }

}
