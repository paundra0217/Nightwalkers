using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace RDCT
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        [SerializeField] private SpriteRenderer _sprite;
        public Animator _animator;
        bool _enableMove = true;
        public bool _counterAttack = false;
        private bool isJumping, isFalling;
        

        private float inputAxis;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _timeStamp;
        private float _time;
        private float _counterTime;

        private float _realCounterTime;
        private float moveDelay;


        private PlayerCombat playerCombat;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            _animator = GetComponent<Animator>();
            playerCombat = GetComponent<PlayerCombat>();

            moveDelay = _timeStamp * 1.2f;

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _timeStamp = Time.deltaTime;
            _time += Time.deltaTime;
            _counterTime += Time.deltaTime;

            if (playerCombat.IsDashing)
            {
                return;
            }
            spriteFlip();
            GatherInput();
            if(_enableMove)
            {
                HandleJump();
                ApplyMovement();
                HandleDirection();
            }
            
            instantInput();
        }

        private void instantInput()
        {
            bool counter = Input.GetKeyDown(KeyCode.P);
            bool attack = Input.GetKey(KeyCode.O);

            if (counter && _grounded && _frameVelocity.x == 0)
            {
                _counterAttack = _counterTime < _realCounterTime * .5f;
                _realCounterTime = _counterTime;
                _animator.SetBool("attack", true);
                _enableMove = false;
            }
            else if (_timeStamp > moveDelay)
            {
                _enableMove = true;
                _animator.SetBool("attack", false);
                _counterAttack = false;
                _realCounterTime = 0;
            }

            if (attack)
            {
                _animator.SetBool("isAttacking", attack);
            }

            if (_frameInput.JumpDown)
            {
                isJumping = true;
                _animator.SetBool("isJumping", isJumping);
            } else isJumping = false;
            _animator.SetBool("isJumping", isJumping);


        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                isJumping = true;
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();
            HandleGravity();

            
        }

        #region Collisions

        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);
            

            //bool groundHit = Physics2D.Raycast(transform.position, Vector2.up, _stats.GrounderDistance);
            //bool ceilingHit = Physics2D.Raycast(transform.position, Vector2.down, _stats.GrounderDistance);

            // Hit a Ceiling
            if (ceilingHit)
            {
                _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
                Debug.Log("Tembok atas");
            }


            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                
                isJumping = false;
                _grounded = true;
                _animator.SetBool("isLand", true);
                _animator.SetBool("isFalling", isFalling);
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                Debug.Log(_rb.velocity.y);
                _grounded = false;
                _frameLeftGrounded = _time;
                _animator.SetBool("isLand", false);
                _animator.SetBool("isFalling", isFalling);
                GroundedChanged?.Invoke(false, 0);
            }

            if(_rb.velocity.y > 0) isFalling = true;    else isFalling = false;

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            isJumping = false;

            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote)
            {
                ExecuteJump();
                
            }


            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {

            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }


        }

        #endregion

        #region Speed
        private void spriteFlip()
        {
            if (Input.GetAxisRaw("Horizontal") < 0) transform.localEulerAngles = new Vector3(0, 180, 0);
            else if (Input.GetAxisRaw("Horizontal") > 0) transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        #endregion

        private void ApplyMovement()
        {
            float animController = _rb.velocity.x;
            if(animController < 0) animController = 1;           
            
            

            _animator.SetFloat("Speed", animController);
            _rb.velocity = _frameVelocity;
        }
    }



    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}