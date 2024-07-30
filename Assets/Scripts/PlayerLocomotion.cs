using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
	public class PlayerLocomotion : MonoBehaviour
	{
		#region Field
		InputHandler inputHandler;
		[HideInInspector]
		public AnimatorHandler animatorHandler;

		Transform cameraObject;
		[HideInInspector]
		public Transform myTransform;
		public new Rigidbody rigidbody;
		public GameObject normalCamera;

		[Header("Stats")]
		[SerializeField]
		float movementSpeed = 5;
		[SerializeField]
		float rotationSpeed = 10;

		Vector3 moveDirection;
		Vector3 normalVector;
		Vector3 targetPosition;
		#endregion


		void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
			inputHandler = GetComponent<InputHandler>();
			animatorHandler = GetComponentInChildren<AnimatorHandler>();
			cameraObject = Camera.main.transform;
			myTransform = transform;
			animatorHandler.Initialize();
		}

		public void Update()
		{
			float delta = Time.deltaTime;

			inputHandler.TickInput(delta);
			HandleMovement(delta);
			HandleRollingAnddSprinting(delta);
		}

		#region Movement
		private void HandleMovement(float delta)
		{
			moveDirection = cameraObject.forward * inputHandler.vertical;
			moveDirection += cameraObject.right * inputHandler.horizontal;
			moveDirection.Normalize();
			moveDirection.y = 0;

			float speed = movementSpeed;
			moveDirection *= speed;

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
			rigidbody.velocity = projectedVelocity;

			animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

			if (animatorHandler.canRotate)
			{
				HandleRotation(delta);
			}
		}

		private void HandleRotation(float delta)
		{
			Vector3 targetDir = Vector3.zero;
			float moveOverride = inputHandler.moveAmount;

			targetDir = cameraObject.forward * inputHandler.vertical;
			targetDir += cameraObject.right * inputHandler.horizontal;

			targetDir.Normalize();
			targetDir.y = 0;

			if (targetDir == Vector3.zero)
				targetDir = myTransform.forward;

			float rs = rotationSpeed;

			Quaternion tr = Quaternion.LookRotation(targetDir);
			Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

			myTransform.rotation = targetRotation;
		}

		

		private void HandleRollingAnddSprinting(float delta)
		{
			if(animatorHandler.anim.GetBool("isInteracting"))
			{
				return;
			}

			if (inputHandler.rollFlag)
			{
				moveDirection = cameraObject.forward * inputHandler.vertical;
				moveDirection += cameraObject.right * inputHandler.horizontal;

				if (inputHandler.moveAmount > 0)
				{
					Debug.Log("Rolling");
					animatorHandler.PlayTargetAnimation("Rolling", true);
					moveDirection.y = 0;
					Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
					myTransform.rotation = rollRotation;
				}
				else
				{
					animatorHandler.PlayTargetAnimation("Backstep", true);
				}
			}
	
		}
		#endregion
	}
}
