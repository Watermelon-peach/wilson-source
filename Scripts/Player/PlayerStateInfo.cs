using UnityEngine;

namespace Wilson.Player
{
    public enum GroundType
    {
        Sand,
        Grass,
        Rock,
        Soil
    }
    //플레이어 상태를 관리하는 클래스
    public class PlayerStateInfo : MonoBehaviour
    {
        #region Variables
        public Terrain terrain;
        private Transform target; // 플레이어나 추적할 오브젝트

        public float checkDistance = 0.5f; // 이 거리 이상 이동해야 다시 체크
        private Vector3 lastCheckedPosition;
        private int lastTextureIndex = -1;

        //인스펙터 확인용
        [SerializeField] private GroundType groundType;
        #endregion
        #region Property
        public GroundType GroundState
        {
            get
            { 
                return groundType;
            }
            set
            {
                groundType = value;
            }
        }
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            target = transform;
            lastCheckedPosition = target.position;
            lastTextureIndex = GetMainTextureIndex(target.position);
            //Debug.Log($"[Start] Main texture: {lastTextureIndex}");
        }

        void Update()
        {
            if (Vector3.Distance(target.position, lastCheckedPosition) > checkDistance)
            {
                lastCheckedPosition = target.position;
                int newIndex = GetMainTextureIndex(target.position);

                if (newIndex != lastTextureIndex)
                {
                    lastTextureIndex = newIndex;
                    //Debug.Log($"[Moved] New main texture: {newIndex}");
                    switch (newIndex)
                    {
                        case 0:
                            GroundState = GroundType.Sand;
                            break;
                        case 1:
                            GroundState = GroundType.Grass;
                            break;
                        case 2:
                            GroundState = GroundType.Grass;
                            break;
                        case 3:
                            GroundState = GroundType.Soil;
                            break;
                        case 4:
                            GroundState = GroundType.Rock;
                            break;
                    }
                }
            }
        }
        #endregion

        #region Custom Method
        /// <summary>
        /// 월드 좌표에서 가장 많이 차지하는 텍스처의 인덱스를 반환
        /// </summary>
        int GetMainTextureIndex(Vector3 worldPos)
        {
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPos = worldPos - terrain.transform.position;

            int mapX = Mathf.FloorToInt((terrainPos.x / terrainData.size.x) * terrainData.alphamapWidth);
            int mapZ = Mathf.FloorToInt((terrainPos.z / terrainData.size.z) * terrainData.alphamapHeight);

            float[,,] map = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

            int maxIndex = 0;
            float maxMix = 0f;

            for (int i = 0; i < map.GetLength(2); i++)
            {
                if (map[0, 0, i] > maxMix)
                {
                    maxMix = map[0, 0, i];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }
        #endregion
    }

}
