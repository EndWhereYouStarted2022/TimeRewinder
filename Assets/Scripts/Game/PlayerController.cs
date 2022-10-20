using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float _jumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool _airControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask _groundLayer;							// A mask determining what is ground to the character
	[SerializeField] private Transform _groundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private LayerMask _ladderLayer;
	[SerializeField] private Animator _anim;
	const float _groundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	const float _ladderCheckRadius = .16f; // Radius of the overlap circle to determine if grounded
	private bool _grounded = true;            // Whether or not the player is grounded.
	private Rigidbody2D _rb;
	private Collider2D _collider;
	private bool _facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 _velocity = Vector3.zero;

	private Collider2D[] _cacheColliders; // Cache Collider2D array used in Fixed Update to check for ground collision
	private Vector2 _movement = Vector2.zero;
	private bool _jump;
	private bool _clickBtnJump;
	private bool _isClimbing;
	private bool _isOnLadder;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	private static readonly int IsGround = Animator.StringToHash("isGround");
	private static readonly int IsRunning = Animator.StringToHash("isRunning");
	private static readonly int Jump = Animator.StringToHash("Jump");
	private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
	private static readonly int IsRewinding = Animator.StringToHash("isRewinding");
	private static readonly int Climb = Animator.StringToHash("Climb");

	protected void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
		_anim = GetComponent<Animator>();
		_cacheColliders = new Collider2D[6];
		_anim.SetBool(IsRewinding,false);
	}

	private void Update()
	{
		if(GameMgr.Instance.IsRewinding) return;
		_movement.x = Input.GetAxisRaw("Horizontal");
		_movement.y = Input.GetAxisRaw("Vertical");
		_jump = Input.GetAxisRaw("Jump") > 0 || _clickBtnJump;
		//play running animation 
		_anim.SetBool(IsRunning,Mathf.Abs(_movement.x) > 0);
	}

	protected void FixedUpdate()
	{
		UpdateLadderStatus();
		UpdateGroundStatus();
		Move(_movement, _jump);
	}
	
	void UpdateGroundStatus()
	{
		bool wasGrounded = _grounded;
		_grounded = false;
		
		int size = Physics2D.OverlapCircleNonAlloc(_groundCheck.position, _groundedRadius, _cacheColliders, _groundLayer);
		for (int i = 0; i < size; i++)
		{
			if (_cacheColliders[i].gameObject != gameObject)
			{
				_grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
		//set animator parameter
		_anim.SetBool(IsGround, _grounded);
	}
	void UpdateLadderStatus()
	{
		_isOnLadder = false;
		
		int size = Physics2D.OverlapCircleNonAlloc(transform.position, _ladderCheckRadius, _cacheColliders, _ladderLayer);
		for (int i = 0; i < size; i++)
		{
			if (_cacheColliders[i].gameObject != gameObject)
			{
				_isOnLadder = true;
				break;
			}
		}

		if (!_isOnLadder && _isClimbing)
		{
			OnEndClimb();
		}
	}

	public void ClickJump()
	{
		if (!_clickBtnJump)
		{
			_clickBtnJump = true;
		}
	}

	private void Move(Vector2 move, bool jump)
	{
		//On a ladder, not yet climbing but has Y input, Start to climb
		if (_isOnLadder && Mathf.Abs(_movement.y) > 0 && (!_isClimbing))
		{
			_isClimbing = true;
			OnStartClimb();
		}

		//When climbing, fetch Y input into action
		if (_isClimbing)
		{
			ClimbMove(move.y);

			//if not grounded, skip reading x input; else still read x
			if (!_grounded)
				return;
		}
		
		if (_grounded || _airControl)
		{
			HorizontalMove(_movement.x);
		}
		
		// If the player should jump...
		if (_grounded && jump)
		{
			// Add a vertical force to the player.
			_grounded = false;
			_rb.AddForce(new Vector2(0f, _jumpForce));
			_anim.SetTrigger(Jump);
			_clickBtnJump = false;
		}
	}

	void ClimbMove(float yMove)
	{
		SmoothMove(new Vector2(0, yMove * 3f));
	}

	void HorizontalMove(float xMove)
	{
		SmoothMove(new Vector2(xMove * 6f, _rb.velocity.y));
		if (xMove > 0 && !_facingRight)
			Flip();
		else if (xMove < 0 && _facingRight)
			Flip();
	}

	void SmoothMove(Vector2 velocity)
	{
		_rb.velocity = Vector3.SmoothDamp(_rb.velocity, velocity, ref _velocity, _movementSmoothing);
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_facingRight = !_facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnStartClimb()
	{
		Debug.Log("On Start Climb");
		_movement = Vector2.zero;
		_rb.gravityScale = 0;
		ToggleFloorCollision(true);
		_anim.SetTrigger(Climb);
		_anim.SetBool(IsClimbing, _isClimbing);
	}

	void OnEndClimb()
	{
		Debug.Log("On End Climb");
		_isClimbing = false;
		_rb.gravityScale = Config.GameConfig.GravityScale;
		ToggleFloorCollision(false);
		_anim.SetBool(IsClimbing, _isClimbing);
	}
	
	public void OnRewindStart()
	{
		_movement = Vector2.zero;
		_anim.SetBool(IsRunning,true);
		// _anim.Play("Player_Run");
		_anim.SetBool(IsRewinding,true);
	}
	public void OnRewindStop()
	{
		_anim.SetBool(IsRewinding,false);
		Reset();
	}

	void Reset()
	{
		//Reset player scale
		_facingRight = true;
		Vector3 theScale = transform.localScale;
		theScale.x = 1;
		transform.localScale = theScale;
	}

	void ToggleFloorCollision(bool flag)
	{
		Physics2D.IgnoreLayerCollision(3, 9, flag);
	}
}
