using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private Transform location;
    public Transform Location { get => location; }


    private void setActivation(bool isActive)
    {
        location.gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            setActivation(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            setActivation(false);
        }
    }

    public bool IsInsideBounds(Vector3 pos)
    {
        float limit = 10.5f;

        if (pos.x >= (transform.position.x - limit) && pos.x <= (transform.position.x + limit) && pos.z >= (transform.position.z - limit) && pos.z <= (transform.position.z + limit))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}