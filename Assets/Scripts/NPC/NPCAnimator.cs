using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    public AnimationStates AnimationState { get; private set; }
    public bool IsGrounded = true;

    private Animator _animator;
    private NPCManager npc;
    private NPCstats stats;
    private NPCsfx sfx;
    private IInteractable interactable;
    private float speedLimit = 1.5f;
    private float minSpeed = 0.1f;
    private float _timer;
    private readonly float coolDown = 0.1f;


    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        npc = transform.parent.GetComponent<NPCManager>();
        stats = npc.GetComponent<NPCstats>();
        sfx = npc.GetComponent<NPCsfx>();
        interactable = npc.GetComponent<IInteractable>();
        AnimationState = AnimationStates.None;
        idle();
    }

    public void Run()
    {
        checkMovement();
    }

    public void Dead()
    {
        sfx.PlayDeath();
        AnimationState = AnimationStates.Dead;
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            _animator.Play("Die");
        }

        this.enabled = false;
    }

    public void GetHit()
    {
        sfx.PlayGetHit();
        getHit();
    }

    public void Idle()
    {
        checkMovement();
    }

    public async UniTask Hit()
    {
        await hit();
    }

    public async UniTask Collect(Asset asset)
    {
        //await collect(asset);        
    }

    public void JumpStart()
    {
        _animator.Play("JumpStart");
        AnimationState = AnimationStates.Fly;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        _timer = coolDown;

        checkMovement();
    }

    private void checkMovement()
    {
        if (npc.IsDead) return;

        if (IsGrounded && AnimationState != AnimationStates.Hit)
        {
            float speed = npc.CurrentSpeed;

            
            if (speed <= speedLimit && speed > minSpeed)
            {
                walk();
            }
            else if (speed > speedLimit)
            {
                run();
            }
            else if (speed <= minSpeed && AnimationState != AnimationStates.GetHit)
            {
                idle();
            }
        }
        else if (!IsGrounded)
        {
            fly();
        }

    }

    private void fly()
    {
        if (AnimationState == AnimationStates.Fly) return;
        AnimationState = AnimationStates.Fly;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLoop")
                && !_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpStart")) _animator.Play("JumpLoop");
    }

    private void idle()
    {
        if (AnimationState == AnimationStates.Idle) return;
        AnimationState = AnimationStates.Idle;

        if (IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _animator.Play("Idle");
        }
        else if (!IsGrounded)
        {
            fly();
        }
    }

    private void getHit()
    {
        if (AnimationState == AnimationStates.GetHit || AnimationState != AnimationStates.Idle) return;
        AnimationState = AnimationStates.GetHit;

        if (IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
        {
            _animator.Play("GetHit");
        }
        else if (!IsGrounded)
        {
            fly();
        }
    }

    private void run()
    {
        if (AnimationState == AnimationStates.Run) return;
        AnimationState = AnimationStates.Run;

        if (IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            _animator.Play("Run");
        }
        else if (!IsGrounded)
        {
            fly();
        }
    }

    private void walk()
    {
        if (AnimationState == AnimationStates.Walk) return;
        AnimationState = AnimationStates.Walk;

        if (IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            _animator.Play("Walk");
        }
        else if (!IsGrounded)
        {
            fly();
        }
    }

    
    private async UniTask hit()
    {
        if (AnimationState == AnimationStates.Hit) return;
                
        AnimationState = AnimationStates.Hit;
        
        _animator.Play("Attack");

        int awaited = 0;
        bool isHitted = false;

        await UniTask.Delay(100);

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            awaited += 20;
            await UniTask.Delay(20);


            if (awaited > 100 && !isHitted)
            {
                isHitted = true;
                npc.HitControl.MakeHit(interactable, npc.transform, Vector3.up * 0.5f + npc.transform.forward * stats.BodyHitDistance, null);
                sfx.PlayAttack();
            }
        }

        //await UniTask.Delay((int)(stats.AttackSpeed * 1000));
        //await UniTask.Delay(100);

        AnimationState = AnimationStates.Idle;
        checkMovement();
    }


    private void playHitAnimation1HMeleeHorizontal()
    {
        string anim = "Hit 1H hor";

        _animator.Play(anim);
    }

    private void playHitAnimation1HMeleeVertical()
    {
        string anim = "Hit 1H vert";

        _animator.Play(anim);
    }

    /*
    private async UniTask collect(Asset asset)
    {
        if (AnimationState == AnimationStates.Collect) return;

        AnimationState = AnimationStates.Collect;

        string anim = "Loot fast";


        switch (Asset.GetBodyLevelByAsset(asset.AssetType))
        {
            default:
                anim = "Loot fast";
                break;

            case BodyLevel.Low:
                anim = "Loot fast";
                break;

            case BodyLevel.Medium:
                anim = "Loot middle";
                break;

            case BodyLevel.High:
                anim = "Loot fast";
                break;
        }

        _animator.Play(anim);
        int awaited = 0;
        bool isHitted = false;

        await UniTask.Delay(100);


        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            awaited += 20;
            await UniTask.Delay(20);


            if (awaited > 100 && !isHitted)
            {
                isHitted = true;
                if (asset.TryGetComponent(out Interactable i))
                {
                    i.GetHit(1000);
                }
            }
        }

        AnimationState = AnimationStates.Idle;
        checkMovement();
    }*/
}
