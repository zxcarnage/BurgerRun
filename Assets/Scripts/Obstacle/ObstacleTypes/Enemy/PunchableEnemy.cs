using UnityEngine;
using Random = System.Random;

public class PunchableEnemy : MonoBehaviour, IObstacle
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _hitPower;
    
    private Direction _direction;
    private static readonly int HitAssRight = Animator.StringToHash("HitAssRight");
    private static readonly int HitAssLeft = Animator.StringToHash("HitAssLeft");

    private void Start()
    {
        ChooseDirection();
    }

    private void ChooseDirection()
    {
        if (transform.position.x > 0)
            _direction = Direction.Right;
        else if (transform.position.x < 0)
            _direction = Direction.Left;
        else
        {
            Random rand = new Random();
            _direction = (Direction)rand.Next(0, 1);
        }
    }

    public void AffectPlayer(Player player)
    {
        player.AnimatePunch(_direction);
        AnimateHit();
        Hit();
    }

    private void Hit()
    {
        Vector3 hitVector;
        switch (_direction)
        {
            case Direction.Left:
                hitVector = Vector3.left;;
                break;
            case Direction.Right:
                hitVector = Vector3.right;
                break;
            default:
                hitVector = Vector3.zero;
                break;
        }
        _rigidbody.AddForce((hitVector + Vector3.up/2 + Vector3.forward/5) * _hitPower, ForceMode.Impulse);
    }

    public void AnimateHit()
    {
        if (_direction == Direction.Right)
            _animator.SetTrigger(HitAssRight);
        else
            _animator.SetTrigger(HitAssLeft);
        enabled = false;
    }
}
