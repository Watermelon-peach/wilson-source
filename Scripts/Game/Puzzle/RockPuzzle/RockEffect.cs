using UnityEngine;

namespace Wilson.Game.Puzzle
{
    public class RockEffect : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float effectDuration = 5f;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            Destroy(gameObject, effectDuration);
        }
        #endregion
    }

}
