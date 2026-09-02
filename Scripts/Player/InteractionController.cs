using UnityEngine;

namespace Wilson.Player
{

    public class InteractionController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private InputHandler input;
        [SerializeField] private ItemDetector itemDetector;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            input.GetInteractionInput();
            if (input.Interacted)
            {
                itemDetector.TryInteract();
            }
        }
        #endregion
    }

}
