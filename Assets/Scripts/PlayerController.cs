using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


enum PlayerStates
{
    Grounded,
    Jumping,
    Falling,
    Floating,
	Stuck,
}

public class PlayerController : MonoBehaviour
{
	private PlayerStates PlayerState = PlayerStates.Grounded;

	private Animator myAnimator;

	private Rigidbody2D myBody;

	private SpriteRenderer mySprite;

	private PlayerActions PlayerInputControls;

	[Header("Player Movement")]
	[SerializeField]
	private float GroundedSpeed = 1.5f;

	[SerializeField]
	private float AirSpeed = 0.9f;

	[SerializeField]
	private float JumpForce = 100f;

	[SerializeField]
	private float floatingGravity = 0.4f;

	[SerializeField]
	private float defaultGravity = 1f;

	[SerializeField]
	private float fallingGravity = 1.5f;

	[SerializeField]
	private float previousDirectionAirStrenght = 0.4f;

	[SerializeField]
	private float jumpStartTime = 0.25f;

	[SerializeField]
	private float coyoteeTime= .2f;

	bool bCanCoyotee = false;

    private float jumpTime;

	private Vector2 LastNonZeroDirection;

	private bool bHasJumpSetDirection;

	[Header("Ground Check")]
	[SerializeField]
	private LayerMask GroundLayer = 1;

	[SerializeField]
	private float CastSize = 3f;

	[SerializeField]
	private List<Transform> feetList = new List<Transform>();

	private static readonly int RunAnim = Animator.StringToHash("Running");

	private static readonly int IdleAnim = Animator.StringToHash("Idle");

	private static readonly int FloatingAnim = Animator.StringToHash("Floating");

	private static readonly int JumpingAnim = Animator.StringToHash("Jump");

	private static readonly int FallingAnim = Animator.StringToHash("Falling");

	private void Awake()
	{
		myAnimator = GetComponent<Animator>();
		myBody = GetComponent<Rigidbody2D>();
		mySprite = GetComponent<SpriteRenderer>();
		PlayerInputControls = new PlayerActions();
		PlayerInputControls.Enable();
	}

	private void Start()
	{
	}

	private void OnJump(InputAction.CallbackContext context)
	{
		Debug.Log("Jump");
		switch (PlayerState)
		{
			case PlayerStates.Grounded:
				ChangeState(PlayerStates.Jumping);
				myBody.gravityScale = defaultGravity;
				myBody.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
				break;
			case PlayerStates.Jumping:
				ChangeState(PlayerStates.Floating);
				myBody.gravityScale = floatingGravity;
				myBody.velocity = new Vector2(myBody.velocity.x, 0f);
				break;
			case PlayerStates.Floating:
				ChangeState(PlayerStates.Jumping);
				myBody.gravityScale = defaultGravity;
				myBody.velocity = new Vector2(myBody.velocity.x, myBody.gravityScale);
				break;
			case PlayerStates.Falling:
				break;
		}
	}

	private void FixedUpdate()
	{
		Vector2 vector = PlayerInputControls.Gameplay.Movement.ReadValue<Vector2>();
		if (vector != Vector2.zero && vector != Vector2.down && vector != Vector2.up)
		{
			if (PlayerState.Equals(PlayerStates.Floating) || PlayerState.Equals(PlayerStates.Falling))
			{
				bHasJumpSetDirection = true;
			}
			LastNonZeroDirection = vector;
		}
		switch (PlayerState)
		{
			case PlayerStates.Floating:
				if (bHasJumpSetDirection)
				{
					vector = LastNonZeroDirection * ((PlayerState == PlayerStates.Floating) ? previousDirectionAirStrenght : 1f);
				}
				vector *= AirSpeed;
				myBody.velocity = new Vector2(vector.x, myBody.velocity.y);
				break;
			case PlayerStates.Jumping:
			case PlayerStates.Falling:
				if (PlayerState != 0 && bHasJumpSetDirection)
				{
					vector = LastNonZeroDirection * ((PlayerState == PlayerStates.Floating) ? previousDirectionAirStrenght : 1f);
				}
				vector *= AirSpeed;
				myBody.velocity = new Vector2(vector.x, myBody.velocity.y);
				break;
			case PlayerStates.Grounded:
				{
					vector *= GroundedSpeed;
					myBody.velocity = new Vector2(vector.x, myBody.velocity.y);
					int stateHashName = ((Mathf.Abs(myBody.velocity.x) > 0f) ? RunAnim : IdleAnim);
					myAnimator.CrossFade(stateHashName, 0f, 0);
					break;
				}
		}
	}

	private void ChangeState(PlayerStates newState)
	{
		if (newState != PlayerState)
		{
			PlayerState = newState;
			Debug.Log(newState.ToString());
			int stateHashName = IdleAnim;
			switch (newState)
			{
				case PlayerStates.Jumping:
					stateHashName = JumpingAnim;
					break;
				case PlayerStates.Falling:
					myBody.gravityScale = fallingGravity;
					stateHashName = FallingAnim;
					break;
				case PlayerStates.Floating:
					stateHashName = FloatingAnim;
					break;
				case PlayerStates.Grounded:
					myBody.gravityScale = defaultGravity;
					bHasJumpSetDirection = false;
					break;
				default:
					stateHashName = IdleAnim;
					break;
			}
			myAnimator.CrossFade(stateHashName, 0f, 0);
		}
	}

	private void Update()
	{
		switch (PlayerState)
		{
			case PlayerStates.Floating:
				if (PlayerInputControls.Gameplay.Jump.triggered)
				{
					ChangeState(PlayerStates.Falling);
					myBody.gravityScale = defaultGravity;
				}
				break;
			case PlayerStates.Falling:
				if (PlayerInputControls.Gameplay.Jump.triggered)
				{
					ChangeState(PlayerStates.Floating);
					myBody.gravityScale = floatingGravity;
					myBody.velocity = new Vector2(myBody.velocity.x, 0f);
				}
				break;
			case PlayerStates.Jumping:
				if (PlayerInputControls.Gameplay.Jump.IsPressed())
				{
					if (jumpTime > 0f)
					{
						myBody.velocity = new Vector2(myBody.velocity.x, 1f * JumpForce);
						jumpTime -= Time.deltaTime;
					}
					else
					{
						ChangeState(PlayerStates.Falling);
					}
				}
				else
				{
					ChangeState(PlayerStates.Falling);
				}
				break;
			case PlayerStates.Grounded:
				if (PlayerInputControls.Gameplay.Jump.triggered)
				{
					ChangeState(PlayerStates.Jumping);
					myBody.velocity = new Vector2(myBody.velocity.x, Vector2.up.y * JumpForce);
					Debug.Log(myBody.velocity);
					jumpTime = jumpStartTime;
				}
				break;
		}
		CheckFlip();
		CheckGrounding();
	}

	private void CheckGrounding()
	{
		bool flag = false;
		foreach (Transform feet in feetList)
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(feet.transform.position, Vector2.down, CastSize, GroundLayer);
			Debug.DrawRay(feet.transform.position, Vector2.down * CastSize, Color.blue);
			if ((bool)raycastHit2D)
			{
				if (PlayerState == PlayerStates.Floating || myBody.velocity.y <= 0f || PlayerState == PlayerStates.Falling)
				{
					flag = true;
					myBody.gravityScale = defaultGravity;
					ChangeState(PlayerStates.Grounded);
					break;
				}
			}
			else
			{
				flag = false;
			}
		}
		if (!flag && !PlayerState.Equals(PlayerStates.Jumping) && !PlayerState.Equals(PlayerStates.Floating))
		{
			ChangeState(PlayerStates.Falling);
		}
	}

	private void CheckFlip()
	{
		if (Mathf.Abs(myBody.velocity.x) > 0f)
		{
			gameObject.transform.localScale= myBody.velocity.x < 0f ? Vector3.one : new Vector3(-1,1,1); 
		}
	}

	public bool GetIsGrounded()
	{
		return PlayerState.Equals(PlayerStates.Grounded);
	}

}
