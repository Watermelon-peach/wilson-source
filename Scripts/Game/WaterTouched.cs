using Unity.VisualScripting;
using UnityEngine;
using Wilson.Utility;

namespace Wilson.Game
{
    public class WaterTouched : MonoBehaviour
    {
        #region Variables
        [Header("참조")]
        public SceneFader fader;
        #endregion

        #region Unity Event Method
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                fader.FadeTo("Distress");
            }
        }
        #endregion
    }

}
