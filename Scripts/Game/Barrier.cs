using System.Collections;
using UnityEngine;
using Wilson.UI;

namespace Wilson.Game
{
    //투명벽 클래스
    public class Barrier : MonoBehaviour
    {
        #region Variables
        public string blockingMessage ="";
        public GameObject InputController;

        private bool isBlocked = false;
        #endregion

        #region Unity Event Method
        private void OnCollisionEnter(Collision collision)
        {
            if (isBlocked)
                return;

            if (collision.transform.CompareTag("Player"))
            {
                isBlocked = true;
                StartCoroutine(MonologUI.Instance.ShowMonolog(blockingMessage));
                StartCoroutine(StopInput());
            }
            
        }

        IEnumerator StopInput()
        {
            InputController.SetActive(false);

            yield return new WaitForSeconds(3f);

            InputController.SetActive(true);
            isBlocked = false;
        }
        #endregion
    }

}
