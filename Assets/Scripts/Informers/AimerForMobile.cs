using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class AimerForMobile : MonoBehaviour
{
    [Inject] private AimInformerUI UI;

    
    private HashSet<GameObject> informers = new HashSet<GameObject>();
        
    private void OnTriggerEnter(Collider other)
    {
        /*
        if (!informers.Contains(other.gameObject) && other.TryGetComponent(out Asset asset))
        {
            UI.ShowMobile(asset);
            informers.Add(other.gameObject);
        }*/

        if (!informers.Contains(other.gameObject) && other.TryGetComponent(out IInteractable i))
        {            
            UI.ShowMobile(other.gameObject);
            informers.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*
        if (informers.Contains(other.gameObject) && other.TryGetComponent(out Asset asset))
        {
            UI.HideMobile(asset);
            informers.Remove(other.gameObject);
        }*/

        if (informers.Contains(other.gameObject) && other.TryGetComponent(out IInteractable i))
        {
            UI.HideMobile(other.gameObject);
            informers.Remove(other.gameObject);
        }
    }
}
