using UnityEngine;
using Wilson.Utility;

namespace Wilson.Game
{
    //수정사항 : HotZone 기획 삭제
    public class UnlockManager : Singleton<UnlockManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        [Header("Zone 상태")]
        [SerializeField] private bool windZone = true;
        //[SerializeField] private bool hotZone = true;
        [SerializeField] private bool fearZone = true;

        [Header("Zone에 대응하는 GameObject")]
        [SerializeField] private GameObject windZoneObject;
        //[SerializeField] private GameObject hotZoneObject;
        [SerializeField] private GameObject fearZoneObject;

        [Header("금고 클리어 상태")]
        [SerializeField] private bool safeCleared = false;

        [Header("금고 오브젝트")]
        [SerializeField] private GameObject safeObject;

#if UNITY_EDITOR
        // 에디터에서 값이 바뀌었을 때 상태 반영
        private void OnValidate()
        {
            ApplyAllZoneStates();
        }
#endif



        private void ApplyAllZoneStates()
        {
            if (windZoneObject != null)
                windZoneObject.SetActive(windZone);
            //if (hotZoneObject != null)
            //    hotZoneObject.SetActive(hotZone);
            if (fearZoneObject != null)
                fearZoneObject.SetActive(fearZone);
            if (safeObject != null)
                safeObject.SetActive(!safeCleared);
        }

        public bool WindZone
        {
            get => windZone;
            set
            {
                windZone = value;
                if (windZoneObject != null)
                    windZoneObject.SetActive(windZone);
            }
        }

        //public bool HotZone
        //{
        //    get => hotZone;
        //    set
        //    {
        //        hotZone = value;
        //        if (hotZoneObject != null)
        //            hotZoneObject.SetActive(hotZone);
        //    }
        //}

        public bool FearZone
        {
            get => fearZone;
            set
            {
                fearZone = value;
                if (fearZoneObject != null)
                    fearZoneObject.SetActive(fearZone);
            }
        }

        public bool SafeCleared
        {
            get => safeCleared;
            set
            {
                safeCleared = value;
                if (safeObject != null)
                    safeObject.SetActive(!safeCleared);
            }
        }
    }
}
