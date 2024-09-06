using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		
		public Vector2 move;
		public bool moveDisabled;
		public bool horizontal_move_disabled;
        public Vector2 forced_move = Vector2.zero;
		public bool force_sprint = false;
        public Vector2 look;
		public bool jump;
		public bool jumpDisabled;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			if(moveDisabled)
				move = Vector2.zero;
            else if (forced_move != Vector2.zero)
                MoveInput(forced_move);
            else if (horizontal_move_disabled)
                MoveInput(new Vector2(0, value.Get<Vector2>().y));
			else
                MoveInput(value.Get<Vector2>());
		}

        public bool isMoving()
		{
            return move != Vector2.zero;
        }

        public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
			else
			{
                LookInput(Vector2.zero);
            }
		}

		public void OnJump(InputValue value)
		{
			if(!jumpDisabled)
                JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
                move = newMoveDirection;
        } 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			if(force_sprint == false)
				sprint = newSprintState;
            else
				sprint = force_sprint;
        }
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}