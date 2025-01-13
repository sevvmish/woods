using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAI : MonoBehaviour
{    
    private NPCstats stats;
    private NPCManager npc;
    private Transform mainPLayer;
    private NPCsfx soundSFX;
    private Vector3 startPoint;
    private float radius;

    private Action onUpdate;
    private float timer;
    private float _timerCheckEnemy;
    private float cooldown;

    private Rigidbody _rb;

    private void OnEnable()
    {
        if (npc == null) npc = GetComponent<NPCManager>();
        if (stats == null) stats = GetComponent<NPCstats>();
        if (mainPLayer == null)
        {
            mainPLayer = GameObject.Find("MainPlayer").transform.GetChild(0);
        }
        if (soundSFX == null) soundSFX = GetComponent<NPCsfx>();

        radius = npc.Diameter / 2f;
        startPoint = transform.parent.position;
        cooldown = 0;
        timer = 0;

        onUpdate = null;
        onUpdate += checkEnemy;
        onUpdate += usualWalking;
    }

    // Update is called once per frame
    void Update()
    {
        onUpdate?.Invoke();
        //if ((mainPLayer.position - transform.position).magnitude < 10) print("timer: " + timer.ToString("f2") + ", cooldown: " + cooldown.ToString("f2") + ", _timerCheckEnemy: " + _timerCheckEnemy.ToString("f2"));
    }

    private void usualWalking()
    {
        if (timer >= cooldown)
        {
            timer = 0;
            Vector3 newPoint = startPoint + new Vector3(UnityEngine.Random.Range(-radius * 0.9f, radius * 0.9f), 0, UnityEngine.Random.Range(-radius * 0.9f, radius * 0.9f));
            float distance = (newPoint - transform.position).magnitude;
            if (npc.WalkToPoint(newPoint))
            {
                cooldown = distance / stats.WalkSpeed + UnityEngine.Random.Range(0.5f, 2f);
                soundSFX.PlayIdle();                
            }
            else
            {
                cooldown = 1.1f;
            }

        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void runAway()
    {                
        List<Vector3> positions = new List<Vector3>() {
                transform.position - new Vector3(6,0,0),
                transform.position - new Vector3(-6,0,0),
                transform.position - new Vector3(-6,0,-6),
                transform.position - new Vector3(6,0,6),
                transform.position - new Vector3(6,0,-6),
                transform.position - new Vector3(-6,0,6),
                transform.position - new Vector3(0,0,6),
                transform.position - new Vector3(0,0,-6)
            };

        float distance = 0;
        Vector3 bestPosition = Vector3.zero;

        for (int i = 0; i < positions.Count; i++)
        {
            if (isInBoundsOfNavMesh(positions[i]))
            {
                float currentDistance = (mainPLayer.position - positions[i]).magnitude;
                if (currentDistance > distance)
                {
                    distance = currentDistance;
                    bestPosition = positions[i];
                }
            }
        }

        if (bestPosition != Vector3.zero)
        {
            npc.RunToPoint(bestPosition);
        }
    }

    private void checkEnemy()
    {
        if (_timerCheckEnemy > 0)
        {
            _timerCheckEnemy -= Time.deltaTime;
            return;
        }

        _timerCheckEnemy = 0.1f;
        float distance = (mainPLayer.position - transform.position).magnitude;

        if (distance < stats.AgroRadius)
        {
            timer = 0;
            cooldown = 7f;
            _timerCheckEnemy = 2.5f;

            int rnd = UnityEngine.Random.Range(0, 10);
            if (rnd == 0)
            {
                soundSFX.PlayAttention();
            }
            else if (rnd == 1)
            {
                soundSFX.PlayIdle();
            }

            runAway();
        }
    }

    private bool isInBoundsOfNavMesh(Vector3 point)
    {
        if (point.x >= (startPoint.x - radius * 0.9f)
                && point.x <= (startPoint.x + radius * 0.9f)
                && point.z >= (startPoint.z - radius * 0.9f)
                && point.z <= (startPoint.z + radius * 0.9f)
                )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
