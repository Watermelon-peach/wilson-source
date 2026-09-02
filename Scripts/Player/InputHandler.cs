using UnityEngine;

namespace Wilson.Player
{
    public class InputHandler : MonoBehaviour
    {
        #region property
        public Vector3 MoveInput { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool RunHeld { get; private set; }
        public bool TimeSwitch { get; private set; }
        public bool Interacted { get; private set; }
        #endregion

        #region Unity Event Method
        void Start()
        {
            //커서 제어
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        #endregion

        #region Custom Method
        public void GetMoveInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            MoveInput = new Vector3(horizontal, 0f, vertical).normalized;
        }

        public void GetRunInput()
        {
            RunHeld = Input.GetKey(KeyCode.LeftShift);
        }

        public void GetJumpInput()
        {
            JumpPressed = Input.GetKey(KeyCode.Space);
        }

        public void GetTimeSwitchInput()
        {
            TimeSwitch = Input.GetKeyDown(KeyCode.T);
        }

        public void GetInteractionInput()
        {
            Interacted = Input.GetKeyDown(KeyCode.E);
        }
        #endregion
    }

}
