using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float _jumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool _airControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask _groundLayer;							// A mask determining what is ground to the character
	[SerializeField] private Transform _groundCheck;							// A position marking where to check if the player is grounded.

	const float _groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool _grounded = true;            // Whether or not the player is grounded.
	private Rigidbody2D _rb;
	private bool _facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 _velocity = Vector3.zero;

	private Collider2D[] _groundColliders; // Cache Collider2D array used in Fixed Update to check for ground collision
	private float _movement = 0;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_groundColliders = new Collider2D[6];
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void Update()
	{
		_movement = Input.GetAxisRaw("Horizontal");

	}

	private void FixedUpdate()
	{
		// UpdateGroundStatus();
		Move(_movement, false);
	}
	
	void UpdateGroundStatus()
	{
		bool wasGrounded = _grounded;
		_grounded = false;
		
		int size = Physics2D.OverlapCircleNonAlloc(_groundCheck.position, _groundedRadius, _groundColliders, _groundLayer);
		for (int i = 0; i < size; i++)
		{
			if (_groundColliders[i].gameObject != gameObject)
			{
				_grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}
	
	public void Move(float move, bool jump)
	{
		if (_grounded || _airControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, _rb.velocity.y);
			// And then smoothing it out and applying it to the character
			_rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, _movementSmoothing);

			//Flip left or right
			if (move > 0 && !_facingRight)
				Flip();
			else if (move < 0 && _facingRight)
				Flip();
			
		}
		// If the player should jump...
		if (_grounded && jump)
		{
			// Add a vertical force to the player.
			_grounded = false;
			_rb.AddForce(new Vector2(0f, _jumpForce));
		}
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
}
