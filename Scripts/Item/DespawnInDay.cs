using UnityEngine;
using Wilson.Game;

namespace Wilson.Item
{
    public class DespawnInDay : MonoBehaviour
    {
        #region Variables
        private TimeManager time;
        #endregion

        private void Start()
        {
            // 씬 안에서 TimeManager를 자동으로 찾아옴 (한 번만)
            time = FindAnyObjectByType<TimeManager>();

            if (time == null)
            {
                Debug.LogWarning("TimeManager를 찾을 수 없습니다. DespawnInDay 비활성화됨.");
                enabled = false;
            }
        }

        private void Update()
        {
            if (time != null && time.IsDay)
            {
                Destroy(gameObject);
            }
        }
    }

}
