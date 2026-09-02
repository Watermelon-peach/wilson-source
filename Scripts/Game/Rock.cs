using UnityEngine;
using Wilson.UI;

namespace Wilson.Game
{
    //낙석 프리팹 클래스
    public class Rock : MonoBehaviour
    {
        #region Variable
        [SerializeField] private float extistanceDuration = 5f; //지속 시간
        private float knockBackForce = 30f;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //지속시간
            Destroy(gameObject, extistanceDuration);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                //튕겨나가기
                collision.rigidbody.AddForce(new Vector3(1, 1, 0).normalized * knockBackForce, ForceMode.Impulse);
                StartCoroutine(MonologUI.Instance.ShowMonolog("으악!"));
            }
        }
        #endregion
    }

}
