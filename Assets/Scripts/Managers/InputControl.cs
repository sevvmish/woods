using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


public class InputControl : MonoBehaviour
{
    [Inject] private GameManager gm;
    [Inject] private Camera _camera;
    [Inject] private Joystick joystick;
    [Inject] private CameraControl cameraControl;
    [Inject] private PlayerControl playerControl;

    private readonly float XLimit = 10;
        
    [SerializeField] private PointerDownOnly jumpButton;
    [SerializeField] private PointerDownOnly useButton;
    [SerializeField] private PointerMoveOnly moverSurface;

    private Ray ray;
    private RaycastHit hit;
    private float cameraRayCast = 50f;

    private Vector3 markerPosition;
    
    public Vector3 GetMarkerPosition => markerPosition;
    
    private Transform mainPlayer;
    

    private LayerMask ignoreMask;
    private LayerMask blockMask;


    // Start is called before the first frame update
    void Start()
    {       
        ignoreMask = LayerMask.GetMask(new string[] { "player" });
        blockMask = LayerMask.GetMask(new string[] { "block" });

        if (!Globals.IsMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;       
            Cursor.visible = false;
            //Destroy(moverSurface.gameObject);
            //Destroy(jumpButton.gameObject);
            //Destroy(useButton.gameObject);
            Destroy(joystick.gameObject);
            Globals.WORKING_DISTANCE = 30;
        }
        else
        {            
            Globals.WORKING_DISTANCE = 20;

            moverSurface.gameObject.SetActive(true);
            jumpButton.gameObject.SetActive(true);
            useButton.gameObject.SetActive(true);
        }

        mainPlayer = playerControl.transform;
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Globals.IsMobile)
        {
            forMobile();
        }
        else
        {
            forPC();
        }                       
    }



    private void forMobile()
    {        

        playerControl.SetHorizontal(joystick.Horizontal);
        playerControl.SetVertical(joystick.Vertical);

        //TODEL        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            playerControl.SetHorizontal(horizontal);
            playerControl.SetVertical(vertical);
        }

        if (Input.GetKeyDown(KeyCode.Space) || jumpButton.IsPressed)
        {
            playerControl.SetJump();
        }

        
        Vector2 delta2 = moverSurface.DeltaPosition;
        Vector2 delta = delta2.normalized;

                
        if (delta2.x > 0 || delta2.x < 0)
        {

            int sign = delta2.x > 0 ? 1 : -1;
            playerControl.SetRotationAngle(4 * sign/*200 * sign * Time.deltaTime*/);

        }
        else if (delta2.x == 0)
        {
            playerControl.SetRotationAngle(0);
        }

        if (Mathf.Abs(delta.y) > 0)
        {
            cameraControl.ChangeCameraAngleX(delta.y * -70 * Time.deltaTime);
        }        
    }

    private void forPC()
    {        
        if (Input.mouseScrollDelta.magnitude > 0)
        {            
            cameraControl.ChangeZoom(Input.mouseScrollDelta.y);
        }

        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            playerControl.SetHorizontal(horizontal);
            playerControl.SetVertical(vertical);
        }

        
        if (Input.GetKeyDown(KeyCode.Space) || jumpButton.IsPressed)
        {
            playerControl.SetJump();
        }
          

        Vector3 mouseDelta = new Vector3(
            Input.GetAxis("Mouse X") * Globals.MOUSE_X_SENS * Time.deltaTime, 
            Input.GetAxis("Mouse Y") * Globals.MOUSE_Y_SENS * Time.deltaTime, 0);

                
        if ((mouseDelta.x > 0) || (mouseDelta.x < 0))
        {
            float koeff = mouseDelta.x * 20 * Time.deltaTime;

            if (koeff > XLimit)
            {
                koeff = XLimit;
            }
            else if (koeff < -XLimit)
            {
                koeff = -XLimit;
            }

            
            playerControl.SetRotationAngle(koeff);
        }        
        else
        {
            playerControl.SetRotationAngle(0);
        }

        
        if (Mathf.Abs(mouseDelta.y) > 0)
        {            
            cameraControl.ChangeCameraAngleX(mouseDelta.y * -7 * Time.deltaTime);
        }
    }

}
