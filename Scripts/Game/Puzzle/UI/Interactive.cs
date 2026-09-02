//using TMPro;
//using Wilson.Item;
//using UnityEngine;

//namespace Wilson.Game.Puzzle
//{
//    //인터랙티브 액션의 부모 클래스
//    public class Interactive : MonoBehaviour
//    {
//        #region Variables
//        //오브젝트과와 플레이어와의 거리
//        protected float theDistance;

//        //액션 UI
//        public GameObject actionUI;
//        public TextMeshProUGUI actionText;



//        //인터렉티브 기능 사용 여부
//        [SerializeField]
//        protected bool unInteractive = false;

//        [SerializeField]
//        protected string action = "Do Interactive Action";
//        public Transform player;  // 인스펙터에 드래그하여 할당
//        #endregion

//        private void Start()
//        {

//        }
//        #region Unity Event Method
//        private void Update()
//        {
//            theDistance = Vector3.Distance(player.position, transform.position);
//        }

//        private void OnTriggerEnter(Collider other)
//        {
//            if (unInteractive)
//                return;

//            if (other.CompareTag("Player"))
//            {

//                ShowActionUI();
//            }
//        }

//        private void OnTriggerExit(Collider other)
//        {
//            if (other.CompareTag("Player"))
//            {

//                HideActionUI();
//            }
//        }
//        #endregion

//        #region Custom Method
//        //Action UI 보여주기
//        protected void ShowActionUI()
//        {
//            actionUI.SetActive(true);
//            actionText.text = action;
//        }

//        //Action UI 숨기기
//        protected void HideActionUI()
//        {
//            actionUI.SetActive(false);
//            actionText.text = "";
//        }

//        //액션 함수
//        protected virtual void DoAction()
//        {

//        }
//        #endregion
//    }
//}
