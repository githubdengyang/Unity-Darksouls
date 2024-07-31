using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
 * InputHandler
 * �ļ�����Ҫ�����Ǵ�����ҵ����룬������Щ���봫�ݸ���Ϸ�е�����ϵͳ����������ƺͽ�ɫ�ƶ�
 */
namespace SG
{
	public class InputHandler : MonoBehaviour
	{
		//�洢��ҵ�ˮƽ�ʹ�ֱ�ƶ�����
		public float horizontal;
		public float vertical;

		//�洢��ҵ��ƶ���
		public float moveAmount;

		//�洢����X��Y������
		public float mouseX;
		public float mouseY;

		//�洢��ҵĹ�������
		public bool b_Input;
		public bool rollFlag;
		public bool sprintFlag;
		public float rollInputTimer;
		

		//�洢��ҵ��������
		PlayerControls inputActions;
		
		
		//�洢��ҵ��ƶ�������������
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
			
		}

		public void OnDisable()
		{
			inputActions.Disable();
		}

		//����ÿ֡�����룬���� MoveInput �� HandleRollInput ������
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

		//������ҵĹ�������
		private void HandleRollInput(float delta)
		{
			b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

			if (b_Input) 
			{
				rollInputTimer += delta;
				sprintFlag = true;
			}
			else
			{
				if (rollInputTimer > 0 && rollInputTimer < 0.5f)
				{
					sprintFlag = false;
					rollFlag = true;
				}

				rollInputTimer = 0;
			}
		}
	}
}