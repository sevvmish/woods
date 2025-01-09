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
    private NavMeshAgent agent;
    private float speedLimit = 2f;
    private float minSpeed = 0.1f;
    private float _timer;
    private readonly float coolDown = 0.1f;


    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        agent = transform.parent.GetComponent<NavMeshAgent>();
        AnimationState = AnimationStates.None;
        idle();
    }

    public void Run()
    {
        checkMovement();
    }

    public void Idle()
    {
        checkMovement();
    }

    public async UniTask Hit(HitType _type)
    {
        //await hit(_type);
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
        if (IsGrounded && AnimationState != AnimationStates.Hit && AnimationState != AnimationStates.Collect)
        {
            float speed = agent.velocity.magnitude;
            
            if (speed <= speedLimit && speed > minSpeed)
            {
                walk();
            }
            else if (speed > speedLimit)
            {
                run();
            }
            else if (speed <= minSpeed)
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

    /*
    private async UniTask hit(HitType _type)
    {
        if (AnimationState == AnimationStates.Hit) return;

        AnimationState = AnimationStates.Hit;

        if (equipControl.RightHandItem == null)
        {
            playHitAnimationUnarmed();
        }
        else
        {
            switch (_type)
            {
                case HitType.Chop:
                    playHitAnimation1HMeleeHorizontal();
                    break;

                case HitType.Mine:
                    playHitAnimation1HMeleeVertical();
                    break;

                default:
                    switch (equipControl.RightHandItem.ItemType)
                    {
                        case ItemTypes.Axe1H:
                            playHitAnimation1HMeleeHorizontal();
                            break;

                        case ItemTypes.Flare1H:
                            playHitAnimation1HMeleeHorizontal();
                            break;

                        case ItemTypes.Pickaxe1H:
                            playHitAnimation1HMeleeVertical();
                            break;

                        default:
                            playHitAnimation1HMeleeHorizontal();
                            break;
                    }
                    break;
            }
        }



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
                pc.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 7f, ForceMode.Impulse);
                pc.Hits.MakeHit(pc.transform, Vector3.up * 1.2f, equipControl.RightHandItem);
            }
        }

        AnimationState = AnimationStates.Idle;
        checkMovement();
    }

    private void playHitAnimationUnarmed()
    {
        string anim = "";

        int rnd = UnityEngine.Random.Range(0, 2);

        switch (rnd)
        {
            case 0:
                anim = "Unarmed Hit R";

                break;

            case 1:
                anim = "Unarmed Hit L";
                break;
        }

        _animator.Play(anim);

        pc.Effects.PlayEffectAtLocation(0.1f, pc.Effects.PunchSwingPool, pc.transform.position + Vector3.up * 1.2f, Vector3.zero, Vector3.one, 0.5f);
    }

    private void playHitAnimation1HMeleeHorizontal()
    {
        string anim = "Hit 1H hor";

        _animator.Play(anim);

        pc.Effects.PlayEffectAtLocation(0.1f, pc.Effects.PunchSwingPool, pc.transform.position + Vector3.up * 1.2f, Vector3.zero, Vector3.one, 0.5f);
    }

    private void playHitAnimation1HMeleeVertical()
    {
        string anim = "Hit 1H vert";

        _animator.Play(anim);

        pc.Effects.PlayEffectAtLocation(0.1f, pc.Effects.PunchSwingPool, pc.transform.position + Vector3.up * 1.2f, Vector3.zero, Vector3.one, 0.5f);
    }

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
