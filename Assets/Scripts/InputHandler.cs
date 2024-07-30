using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
 * InputHandler
 * 文件的主要功能是处理玩家的输入，并将这些输入传递给游戏中的其他系统，如相机控制和角色移动
 */
namespace SG
{
	public class InputHandler : MonoBehaviour
	{
		//存储玩家的水平和垂直移动输入
		public float horizontal;
		public float vertical;

		//存储玩家的移动量
		public float moveAmount;

		//存储鼠标的X和Y轴输入
		public float mouseX;
		public float mouseY;

		//存储玩家的滚动输入
		public bool b_Input;
		public bool rollFlag;
		public bool isInteracting;

		//存储玩家的输入操作
		PlayerControls inputActions;
		//相机控制器
		CameraHandler cameraHandler;
		
		//存储玩家的移动输入和相机输入
		Vector2 movementInnput;
		Vector2 cameraInnput;

		public void OnEnable()
		{
			if (inputActions == null) 
			{
				inputActions = new PlayerControls();
				inputActions.PlayerMovement.Movement.performed += inputActions => movementInnput = inputActions.ReadValue<Vector2>();
				inputActions.PlayerMovement.Camera.performed += i => cameraInnput = i.ReadValue<Vector2>();
			}

			inputActions.Enable();
		}

		private void Start()
		{
			cameraHandler = CameraHandler.singleton;
		}

		//相机输入一般放在FixedUpdate中
		private void FixedUpdate()
		{
			float delta = Time.fixedDeltaTime;
			if (cameraHandler != null)
			{
				cameraHandler.FollowTarget(delta);
				cameraHandler.HandleCameraRotation(delta,mouseX,mouseY);
			}
		}

		public void OnDisable()
		{
			inputActions.Disable();
		}

		//处理每帧的输入，调用 MoveInput 和 HandleRollInput 方法。
		public void TickInput(float delta) 
		{
			MoveInput(delta);
			HandleRollInput(delta);
		}

		public void MoveInput(float delta)
		{
			horizontal = movementInnput.x;
			vertical = movementInnput.y;
			moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
			mouseX = cameraInnput.x;
			mouseY = cameraInnput.y;
		}

		//处理玩家的滚动输入
		private void HandleRollInput(float delta)
		{
			b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

			if (b_Input) 
			{
				rollFlag = true;
			}
		}
	}
}