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
    [Inject] private CharacterPanelUI characterPanel;

    private ActionControl actions;
    private readonly float XLimit = 10;
        
    [SerializeField] private PointerDownOnly jumpButton;
    [SerializeField] private PointerDownOnly inventoryButton;
    [SerializeField] private PointerDownOnly optionsButton;

    [SerializeField] private PointerDownOnly useButton;
    [SerializeField] private GameObject iconUseButtonUse;
    [SerializeField] private GameObject iconUseButtonHit;
    private bool isUseNotHit;

    [SerializeField] private PointerMoveOnly moverSurface;
    
        

    // Start is called before the first frame update
    void Start()
    {
        actions = playerControl.GetComponent<ActionControl>();



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
            playerControl.GetComponent<ActionControl>().SetInput(this);
            moverSurface.gameObject.SetActive(true);
            jumpButton.gameObject.SetActive(true);
            useButton.gameObject.SetActive(true);
            inventoryButton.gameObject.SetActive(true);
            optionsButton.gameObject.SetActive(true);

            ShowHitButton();
        }
                
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

    public void ShowUseButton()
    {
        iconUseButtonUse.gameObject.SetActive(true);
        iconUseButtonHit.gameObject.SetActive(false);
        isUseNotHit = true;
    }

    public void ShowHitButton()
    {
        iconUseButtonUse.gameObject.SetActive(false);
        iconUseButtonHit.gameObject.SetActive(true);
        isUseNotHit = false;
    }

    private void forMobile()
    {        

        playerControl.SetHorizontal(joystick.Horizontal);
        playerControl.SetVertical(joystick.Vertical);

        //TODEL        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (!Globals.IsOptions && (horizontal != 0 || vertical != 0))
        {
            playerControl.SetHorizontal(horizontal);
            playerControl.SetVertical(vertical);
        }

        
        if (!Globals.IsOptions && (Input.GetKeyDown(KeyCode.Space) || jumpButton.IsPressed))
        {
            playerControl.SetJump();
        }
        else if (!Globals.IsOptions && (useButton.IsPressed || (Globals.IsMobile && Input.GetKeyDown(KeyCode.Q))))
        {
            if (isUseNotHit)
            {
                actions.UsePressed();
            }
            else
            {
                actions.UseHit(HitType.None);
            }
        }
        else if (inventoryButton.IsPressed)
        {
            if (!characterPanel.IsMainPanelOpened)
            {
                characterPanel.OpenInventory();
            }            
        }

        if (Globals.IsOptions) return;

        Vector2 delta2 = moverSurface.DeltaPosition;
        Vector2 delta = delta2.normalized;

                
        if (delta2.x > 0 || delta2.x < 0)
        {            
            int sign = delta2.x > 0 ? 1 : -1;
            playerControl.SetRotationAngle(delta2.x * 22 * 1 * Time.deltaTime);
        }
        else if (delta2.x == 0)
        {
            playerControl.SetRotationAngle(0);
        }

        if (Mathf.Abs(delta.y) > 0)
        {
            cameraControl.ChangeCameraAngleX(delta.y * -90 * Time.deltaTime);
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
        if (!Globals.IsOptions && (horizontal != 0 || vertical != 0))
        {
            playerControl.SetHorizontal(horizontal);
            playerControl.SetVertical(vertical);
        }


        if (!Globals.IsOptions && Input.GetKeyDown(KeyCode.E))
        {
            actions.UsePressed();
        }
        else if (!Globals.IsOptions && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q)))
        {
            actions.UseHit(HitType.None);
        }
        else if (!Globals.IsOptions && (Input.GetKeyDown(KeyCode.Space) || jumpButton.IsPressed))
        {
            playerControl.SetJump();
        }        
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!characterPanel.IsMainPanelOpened)
            {
                characterPanel.OpenInventory();
            }
            else
            {
                characterPanel.CloseCharacterPanel();
            }
                
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (characterPanel.IsMainPanelOpened)
            {
                characterPanel.CloseCharacterPanel();                
            }                
        }

        if (Globals.IsOptions) return;

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
            cameraControl.ChangeCameraAngleX(mouseDelta.y * -10 * Time.deltaTime);
        }
    }

}
