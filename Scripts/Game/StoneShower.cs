using UnityEngine;

namespace Wilson.Game
{
    public class StoneShower : MonoBehaviour
    {
        [Header("소환할 프리팹들")]
        public GameObject[] prefabs;

        [Header("소환 인터벌")]
        public float spawnInterval = 2f;

        private float planeWidth;
        private float planeHeight;

        private GameObject plane;

        private void Awake()
        {
            plane = gameObject;
        }
        void Start()
        {
            // Plane의 크기 계산
            MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();
            planeWidth = meshRenderer.bounds.size.x;
            planeHeight = meshRenderer.bounds.size.z;

            // 반복 소환 시작
            InvokeRepeating(nameof(SpawnRandomPrefab), 0f, spawnInterval);
        }

        void SpawnRandomPrefab()
        {
            if (prefabs.Length == 0 || plane == null) return;

            // 랜덤 위치 계산
            Vector3 planeCenter = plane.transform.position;
            float x = Random.Range(-planeWidth / 2f, planeWidth / 2f);
            float z = Random.Range(-planeHeight / 2f, planeHeight / 2f);
            Vector3 spawnPos = new Vector3(x, 0f, z) + planeCenter;

            // Y 좌표는 Plane 위로 약간 띄우기
            spawnPos.y += 0.5f;

            // 랜덤 프리팹 선택
            GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

            // 소환
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }
    }

}
