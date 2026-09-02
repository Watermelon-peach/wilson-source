using UnityEngine;

namespace Wilson.Player
{
    //바닥 체크 기능을 하는 클래스
    public class GroundCheck : MonoBehaviour
    {
        #region Property
        public bool IsGrounded { get; private set; }
        #endregion

        #region Unity Event Method
        private void OnTriggerStay(Collider other)
        {
            if (other.transform.tag == "Ground")
            {
                IsGrounded = true;
                //Debug.Log("닿았다");
            }
        }
        

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == "Ground")
            {
                IsGrounded = false;
                //Debug.Log("떨어졌다");
            }
        }
        #endregion

    }

}
