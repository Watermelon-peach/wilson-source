using UnityEngine;
using Wilson.UI;

namespace Wilson.Game
{
    public class WindZone : MonoBehaviour
    {
        #region Variables
        [SerializeField] float windForce = 100f;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                Debug.Log("들어옴");
                StartCoroutine(MonologUI.Instance.ShowMonolog("바람이 너무 센 것 같아..\n든든하게 먹고 와야겠어."));
            }
        }
        private void OnTriggerStay(Collider other)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null && other.transform.CompareTag("Player"))
            {
                rb.AddForce(Vector3.back * windForce,ForceMode.Acceleration);
            }
        }
    }

}
