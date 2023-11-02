using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private PlayerMover _playerMover;

    private float _actualSpeed;
    
    [SerializeField] private PlayerAnimationState _animationState = PlayerAnimationState.Idle;

    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int HitFall = Animator.StringToHash("HitFall");
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Up = Animator.StringToHash("StandUp");
    private static readonly int IsSlide = Animator.StringToHash("isSlide");
    private static readonly int TrampolineStart = Animator.StringToHash("TrampolineStart");
    private static readonly int TrampolineGround = Animator.StringToHash("TrampolineGround");
    private static readonly int TrampolineStop = Animator.StringToHash("TrampolineStop");
    private static readonly int Roll = Animator.StringToHash("Roll");
    private static readonly int BeamWalk = Animator.StringToHash("BeamWalk");
    private static readonly int BeamFallLeft = Animator.StringToHash("BeamFallLeft");
    private static readonly int BeamFallRight = Animator.StringToHash("BeamFallRight");
    private static readonly int PunchinWall = Animator.StringToHash("PunchinWall");
    private static readonly int PunchAssLeft = Animator.StringToHash("PunchAssLeft");
    private static readonly int PunchAssRight = Animator.StringToHash("PunchAssRight");
    private static readonly int HoleJump = Animator.StringToHash("HoleJump");
    private static readonly int HoleStuck = Animator.StringToHash("HoleGetStuck");
    private static readonly int GlassWalk = Animator.StringToHash("Sneak");
    private static readonly int DroneMount = Animator.StringToHash("DroneMount");
    private static readonly int DroneDismount = Animator.StringToHash("DroneDismount");
    private static readonly int DroneFly = Animator.StringToHash("DroneFly");
    private static readonly int DroneIdle = Animator.StringToHash("DroneIdle");
    private static readonly string DroneTurn = "DroneTurn";
    private static readonly int Slide = Animator.StringToHash("IsSlide");
    
    private bool _insideGlassArea;
    private bool _insideTreadmillArea;
    private bool _insideDrone;
    private bool _insideBeam;

    private int _runID = 1;
    private float _turningDirection;
    private PlayerSkinSwitcher _switcher;
    

    public float TurningDirection
    {
        get { return _turningDirection; }
        set
        {
            _turningDirection = value;
        }
    }

    private void Awake()
    {
        _switcher = GetComponent<PlayerSkinSwitcher>();
    }

    private void Start()
    {
        _actualSpeed = _animator.speed;
    }

    private void OnEnable()
    {
        _switcher.RigChanged += OnRigChanged;
        ServiceLocator.Player.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        _switcher.RigChanged -= OnRigChanged;
        ServiceLocator.Player.Died -= OnPlayerDied;
    }

    private void OnRigChanged(GameObject newRig)
    {
        _animator = newRig.GetComponent<Animator>();
    }
    

    private void OnPlayerDied()
    {
        _animator.SetTrigger(Fall);
    }

    private void LateUpdate()
    {
        AnimateRun();
    }

    private void AnimateRun()
    {
        if (_animationState == PlayerAnimationState.InMiniGame || _animationState == PlayerAnimationState.Other || _insideBeam)
            return;
        if (_rigidbody.velocity.z > 1f && _animationState != PlayerAnimationState.Running)
        {
            if (_insideDrone)
            {
                AnimateDroneMovement();
                return;
            }
            if (_insideGlassArea && _animationState != PlayerAnimationState.GlassWalk)
            {
                _animator.SetTrigger(GlassWalk);
                _animationState = PlayerAnimationState.GlassWalk;
                return;
            }
            if (_insideTreadmillArea && _animationState != PlayerAnimationState.TreadmillRun)
            {
                SetRunId(4);
                _animationState = PlayerAnimationState.TreadmillRun;
                return;
            }
            if (!_insideGlassArea && !_insideTreadmillArea)
            {
                SetRunningTrigger();
            }

        }
        else if (_rigidbody.velocity.z <= 1f && _animationState != PlayerAnimationState.Idle)
        {
            _animationState = PlayerAnimationState.Idle;
            _animator.SetTrigger(_insideDrone ? DroneIdle : Idle);
        }
    }

    public void SetRunningTrigger()
    {
        if (_animationState != PlayerAnimationState.Running && !_insideGlassArea && !_insideTreadmillArea)
        {
            SetRunId(1);
            _animationState = PlayerAnimationState.Running;
        }
        if (_runID == 1 && GameManager.Instance.ButtSize >= 0.15)
        {
            SetRunId(2);
        }
        else if (_runID == 2 && GameManager.Instance.ButtSize <= 0.15)
        {
            SetRunId(1);
        }
    }

    private void SetRunId(int runID)
    {
        _runID = runID;
        _animator.SetInteger("Run_id", _runID);
        _animator.SetTrigger(Run);
    }
    
    public void SetInsideGlass(bool value)
    {
        _insideGlassArea = value;
        if (_insideGlassArea)
        {
            _animator.SetTrigger(GlassWalk);
            _animationState = PlayerAnimationState.GlassWalk;
        }
        else
            SetRunningTrigger();
    }

    public void SetInsideTreadmill(bool value)
    {
        _insideTreadmillArea = value;
        if (_insideTreadmillArea)
        {
            SetRunId(4);
            _animationState = PlayerAnimationState.TreadmillRun;
        }
        else
        {
            SetRunningTrigger();
        }
    }

    public void SetInsideBeam(bool value)
    {
        if (value)
        {
            _animator.SetTrigger(BeamWalk);
            _animationState = PlayerAnimationState.Other;
        }
        else
            SetRunningTrigger();

        _insideBeam = value;
    }

    public void AnimateObstacleFall()
    {
        _animationState = PlayerAnimationState.Other;
        _animator.SetTrigger(HitFall);
    }

    public void AnimatePunch(Direction direction)
    {
        if(direction == Direction.Left)
            _animator.SetTrigger(PunchAssLeft);
        else
            _animator.SetTrigger(PunchAssRight);
    }

    public void StandUp()
    {
        _animator.SetTrigger(Up);
    }

    public void PlayCheeseAnimation()
    {
        _animationState = PlayerAnimationState.Other;
        _animator.SetTrigger(HoleJump);
    }

    public void SetIdle()
    {
        _animationState = PlayerAnimationState.Idle;
        _animator.SetTrigger(Idle);
    }

    public void UnlockMovement()
    {
        Debug.Log("In unlock movement");
        ServiceLocator.Player.UnlockMovement();
        Debug.Log("Unlocked player movement");
        _playerMover.Block(false);
        Debug.Log("Unlocked player mover movement");
        SetIdle();
        Debug.Log("Setted Idle");
    }

    public void StuckAnimation()
    {
        _animator.ResetTrigger(HoleJump);
        _animator.SetTrigger(HoleStuck);
        _animationState = PlayerAnimationState.Other;
    }

    public void AnimateBeamFall(float value)
    {
        if(value < 0)
            _animator.SetTrigger(BeamFallLeft);
        else
            _animator.SetTrigger(BeamFallRight);
    }
    
    public void StartJumpAnimation()
    {
        _animator.SetTrigger(TrampolineStart);
        _animationState = PlayerAnimationState.Other;
    }

    public void InAir()
    {
        
    }

    public void GroundedOnPad()
    {
        _animator.SetTrigger(TrampolineGround);
    }

    public void JumpEnded()
    {
        _animator.SetTrigger(TrampolineStop);
    }
    

    public void AnimateJumpToBeams()
    {
        if (_animationState != PlayerAnimationState.Other)
        {
            _animationState = PlayerAnimationState.Other;
            _animator.SetTrigger(TrampolineStart);
        }
    }

    public void TrampolineEnd()
    {
        _animator.SetTrigger(TrampolineStop);
    }

    public void Slow(float value)
    {
        _animator.speed = value;
    }

    public void UnSlow()
    {
        _animator.speed = _actualSpeed;
    }
    

    public void PlayerOnGround()
    {
        _animator.SetTrigger(TrampolineStop);
    }

    public void BreakingWall()
    {
        _animator.SetTrigger(PunchinWall);
    }

    public void PlayFallAnimation(int whichSideToFall)
    {
        _animator.SetTrigger(whichSideToFall == 1 ? BeamFallRight : BeamFallLeft);
    }

    public void MountDrone()
    {
        _animator.SetTrigger(DroneMount);
        _insideDrone = true;
        _rigidbody.useGravity = false;
    }

    public void DismountDrone()
    {
        StartCoroutine(DismountCoroutine());
    }

    private IEnumerator DismountCoroutine()
    {
        _animator.SetTrigger(DroneDismount);
        yield return WaitFor.Frames(30);
        _insideDrone = false;
        _rigidbody.useGravity = true;
        _playerMover.Block(false);
        _animationState = PlayerAnimationState.Idle;
    }

    public void AnimateDroneMovement()
    {
        if (_animationState != PlayerAnimationState.Drone)
        {
            _animationState = PlayerAnimationState.Drone;
            _animator.SetTrigger(DroneFly);
        }
        if (_animator.GetFloat(DroneTurn) != _turningDirection)
            _animator.SetFloat(DroneTurn, _turningDirection);
    }

    public void SetRollingTrigger()
    {
        _animator.SetTrigger(Roll);
        _animationState = PlayerAnimationState.Other;
    }

    public void SetPlayerAnimationState(PlayerAnimationState animationState)
    {
        _animationState = animationState;
    }

    public void AnimatePitFall()
    {
        _animationState = PlayerAnimationState.Other;
        _animator.SetTrigger(Fall);
        //GetComponent<CapsuleCollider>().enabled = false;

    }
}
