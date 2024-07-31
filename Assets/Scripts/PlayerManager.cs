using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace SG
{
	public class PlayerManager : MonoBehaviour
    {
		InputHandler inputHandler;
        Animator animator;

		//相机控制器
		CameraHandler cameraHandler;
		PlayerLocomotion playerLocomotion;

		[Header("Player Flags")]
		public bool isSprinting;
		public bool isInteracting;
		public bool isInAir;
		public bool isGrounded;

		private void Awake()
		{
			cameraHandler = CameraHandler.singleton;
		}

		void Start()
        {
            inputHandler = GetComponent<InputHandler>();
			animator = GetComponentInChildren<Animator>();
			playerLocomotion = GetComponent<PlayerLocomotion>();
		}

        void Update()
        {
			float delta = Time.deltaTime;

			isInteracting = animator.GetBool("isInteracting");
			
			
			inputHandler.TickInput(delta);
			playerLocomotion.HandleMovement(delta);
			playerLocomotion.HandleRollingAndSprinting(delta);
			playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
		}

		//相机输入一般放在FixedUpdate中
		private void FixedUpdate()
		{
			float delta = Time.fixedDeltaTime;
			if (cameraHandler != null)
			{
				cameraHandler.FollowTarget(delta);
				cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
			}
		}

		private void LateUpdate()
		{
			inputHandler.rollFlag = false;
			inputHandler.sprintFlag = false;
			isSprinting = inputHandler.b_Input;

			if (isInAir)
			{
				playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
			}
		}
	}
}
