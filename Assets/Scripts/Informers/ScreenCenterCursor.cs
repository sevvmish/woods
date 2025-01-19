using UnityEngine;
using VContainer;

public class ScreenCenterCursor : MonoBehaviour
{
    [Inject] private Camera _camera;
    [Inject] private PlayerControl playerControl;
    [Inject] private AimInformerUI UI;


    private ActionControl actions;
    private Transform playerTransform;
    private LayerMask aimMask;    
    private RaycastHit hit;
    private float _timer;
    private float _cooldown = 0.15f;
    private bool isZeroText;


    // Start is called before the first frame update
    void Start()
    {
        if (Globals.IsMobile) this.enabled = false;

        actions = playerControl.GetComponent<ActionControl>();

        playerTransform = playerControl.transform;
        aimMask = LayerMask.GetMask(new string[] { "interactable" });        
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > _cooldown)
        {
            _timer = 0;
            checkAim();
        }
        else
        {
            _timer += Time.deltaTime;
        }        
    }

    private void checkAim()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 16, aimMask))
        {
            if (hit.collider.gameObject != null)
            {
                UI.ShowAimCursorText(hit.collider.gameObject);
                actions.SetAim(hit.collider.gameObject);
                isZeroText = false;
            }            
        }       
        else if (!isZeroText)
        {
            isZeroText = true;
            actions.SetAim(null);
            UI.ShowAimCursorText("");
        }
    }
}
