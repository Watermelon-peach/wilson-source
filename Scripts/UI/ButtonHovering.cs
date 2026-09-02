using UnityEngine;
using UnityEngine.EventSystems;

namespace Wilson.UI
{
    public class ButtonHovering : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Variables
        private Animator animator;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            animator.SetBool("IsHovering", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animator.SetBool("IsHovering", false);
        }

        #endregion
    }

}
