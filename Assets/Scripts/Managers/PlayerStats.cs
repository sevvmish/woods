using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IInteractable
{
    [SerializeField] private float maxHP = 50;
    public float CurrentHP { get; private set; }

    [SerializeField] private float maxSpeed = 7;
    public float MaxSpeed => maxSpeed;

    [SerializeField] private float walkSpeed = 2;
    public float WalkSpeed => walkSpeed;
    public Asset CurrentAsset { get; private set; }
    

    public void GetHit(float damage)
    {
        print("player got: " + damage);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
