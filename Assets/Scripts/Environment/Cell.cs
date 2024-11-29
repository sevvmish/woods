using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private Transform locationFar;
    [SerializeField] private Transform locationClose;
    public Transform LocationFar { get => locationFar; }
    public Transform LocationClose { get => locationClose; }

    //public List<GameObject> closeObjects = new List<GameObject>();

    private void OnEnable()
    {
        setActivationFar(false);
        setActivationClose(false);
    }

    private void setActivationFar(bool isActive)
    {
        locationFar.gameObject.SetActive(isActive);
    }

    private void setActivationClose(bool isActive)
    {
        locationClose.gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            setActivationFar(true);
        }

        if (other.CompareTag("MainCameraClose"))
        {
            setActivationClose(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            setActivationFar(false);
        }

        if (other.CompareTag("MainCameraClose"))
        {
            setActivationClose(false);
        }
    }

    public bool IsInsideBounds(Vector3 pos)
    {
        float limit = 12.9f;//10.5f;

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
