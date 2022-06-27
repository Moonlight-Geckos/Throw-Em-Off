using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator _animator;
    private int _rand;

    private Queue<AttackDirection> _nextAttacksQueue;
    private AttackDirection _atkDir;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _nextAttacksQueue = new Queue<AttackDirection>();
        _atkDir = AttackDirection.None;
        EventsPool.UserSwipedEvent.AddListener(RegisterAttack);
    }
    private void AttackWithAnimation(AttackDirection dir)
    {
        _rand = Random.Range(1, 5);
        _animator.SetInteger("Attack", _rand);
        if (dir == AttackDirection.Right)
            transform.localEulerAngles = new Vector3(0, 90, 0);
        else
            transform.localEulerAngles = new Vector3(0, -90, 0);
    }
    private void RegisterAttack(AttackDirection dir)
    {
        if(_atkDir == AttackDirection.None)
        {
            _atkDir = dir;
            AttackWithAnimation(dir);
        }
        else if(_atkDir != dir)
        {
            _atkDir = dir;
            _nextAttacksQueue.Enqueue(dir);
        }
    }
    public void CanAttack()
    {
        if (_nextAttacksQueue.Count > 0)
            AttackWithAnimation(_nextAttacksQueue.Dequeue());
        else
        {
            _atkDir = AttackDirection.None;
            _animator.SetInteger("Attack", 0);
        }
    }
}
