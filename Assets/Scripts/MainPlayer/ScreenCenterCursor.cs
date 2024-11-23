
using TMPro;
using UnityEngine;
using VContainer;

public class ScreenCenterCursor : MonoBehaviour
{
    [Inject] private Camera _camera;
    [Inject] private PlayerControl playerControl;

    [SerializeField] private TextMeshProUGUI aimSign;

    private Transform playerTransform;
    private LayerMask ignoreMask;    
    private RaycastHit hit;
    private float _timer;
    private float _cooldown = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        playerTransform = playerControl.transform;
        ignoreMask = LayerMask.GetMask(new string[] { "cell", "terrain", "player" });
        aimSign.text = "";
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
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward * 100, out hit, 20, ~ignoreMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject != null)
            {
                aimSign.text = hit.collider.gameObject.name;
            }
            else
            {
                aimSign.text = "";
            }
        }
        else
        {
            aimSign.text = "";
        }
    }
}
