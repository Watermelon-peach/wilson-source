using UnityEngine;
using Wilson.UI;

namespace Wilson.Game
{
    public class SuperJump : MonoBehaviour
    {
        #region Variable
        [SerializeField] private float superJumpForce = 50f;
        #endregion

        #region Unity Event Method
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                //슈퍼점프
                collision.rigidbody.AddForce(new Vector3(-1,1,1).normalized * superJumpForce, ForceMode.Impulse);
                StartCoroutine(MonologUI.Instance.ShowMonolog("끼얏호우!!"));
            }
        }
        #endregion
    }

}
