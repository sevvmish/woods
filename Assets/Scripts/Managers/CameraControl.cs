using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class CameraControl : MonoBehaviour
{
    [Inject] private GameManager gm;
    [Inject] private Camera _camera;
    [Inject] private FOVControl fovControl;
    [Inject] private PlayerControl playerControl;


    private Transform mainPlayer;
    private Transform mainCamera;
    private Transform mainCamTransformForRaycast;
    private Transform outerCamera;
    private Vector3 outerCameraShiftVector = Vector3.zero;
    private Vector3 baseCameraBodyPosition = Vector3.zero;

    private float currentZoom;
    private float zoomTimer;
    private bool isZooming;

    private bool isUpdate = true;
    private float _timer;    
    private float _timerCooldown;
    private LayerMask mainMask;
    private LayerMask ignoreMask;
    private Ray ray;
    private RaycastHit hit;

    private float xLimitUp = 40;
    private float xLimitDown = 270;

    private bool isCameraIn;


    private Dictionary<MeshRenderer, Material> changedMeshRenderers = new Dictionary<MeshRenderer, Material>();
    private HashSet<MeshRenderer> renderers = new HashSet<MeshRenderer>();

    private HashSet<MeshRenderer> renderersToReturn = new HashSet<MeshRenderer>();

    private float defaultCameraDistance;
    private float currentCameraDistance;
    private WaitForSeconds fixedDelta = new WaitForSeconds(0.02f);
    private float previousDistance;

    public Vector3 BasePositionShiftForCamera;

    private void Awake()
    {
        fovControl.SetFOV();
    }

    private void Start()
    {
        mainCamTransformForRaycast = _camera.transform;
        mainPlayer = playerControl.transform;
        playerControl = mainPlayer.GetComponent<PlayerControl>();
        mainCamera = _camera.transform.parent;
        outerCamera = mainCamera.parent;

        if (Globals.IsMobile)
        {
            BasePositionShiftForCamera = Globals.BasePositionMob;
        }
        else
        {
            BasePositionShiftForCamera = Globals.BasePositionPC;
        }
        

        mainCamera.localPosition = BasePositionShiftForCamera;
        mainCamera.localEulerAngles = Globals.BaseRotation;
        baseCameraBodyPosition = BasePositionShiftForCamera;

        ignoreMask = LayerMask.GetMask(new string[] { "trigger", "player", "ragdoll", "danger" });
        mainMask = LayerMask.GetMask(new string[] { "terrain" });

        currentZoom = Globals.MainPlayerData.Zoom;
        Zoom(Globals.MainPlayerData.Zoom);

        outerCamera.eulerAngles += new Vector3(-25, 0, 0);
        defaultCameraDistance = (mainPlayerPoint - mainCamTransformForRaycast.position).magnitude;
    }

    public void SwapControlBody(Transform newTransform)
    {
        mainPlayer = newTransform;
        isUpdate = false;
        StartCoroutine(playSwap());
    }
    private IEnumerator playSwap()
    {
        outerCamera.DOMove(mainPlayer.position/* + basePosition*/, 0.1f);
        yield return new WaitForSeconds(0.1f);
        isUpdate = true;
    }

    public void ChangeZoom(float koeff)
    {
        if (koeff < 0 && Globals.MainPlayerData.Zoom <= Globals.ZOOM_LIMIT_HIGH)
        {
            Globals.MainPlayerData.Zoom += Globals.ZOOM_DELTA;
        }
        else if (koeff > 0 && Globals.MainPlayerData.Zoom > -Globals.ZOOM_LIMIT_LOW)
        {
            Globals.MainPlayerData.Zoom -= Globals.ZOOM_DELTA;
        }

        checkCorrectZoom();
    }

    private void checkCorrectZoom()
    {
        if (Globals.MainPlayerData.Zoom > Globals.ZOOM_LIMIT_HIGH)
        {
            Globals.MainPlayerData.Zoom = Globals.ZOOM_LIMIT_HIGH;
            Zoom(Globals.MainPlayerData.Zoom);
        }
        else if (Globals.MainPlayerData.Zoom < Globals.ZOOM_LIMIT_LOW)
        {
            Globals.MainPlayerData.Zoom = Globals.ZOOM_LIMIT_LOW;
            Zoom(Globals.MainPlayerData.Zoom);
        }
        else
        {
            Zoom(Globals.MainPlayerData.Zoom);
        }
    }

    public void Zoom(float koeff)
    {
        _camera.fieldOfView = koeff;
    }

    public void RotateCameraOnVector(Vector3 vec, float _time)
    {
        outerCamera.DORotate(outerCamera.eulerAngles + vec, _time).SetEase(Ease.Linear);
    }

    public void ResetCameraOnRespawn()
    {
        outerCamera.eulerAngles = Vector3.zero;
    }

    public void ResetCameraOnRespawn(Vector3 vec)
    {
        outerCamera.eulerAngles = new Vector3(/*outerCamera.localEulerAngles.x + */outerCameraShiftVector.x, vec.y, 0);
    }

    public void ChangeCameraAngleY(float angleY)
    {

        outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, playerControl.angleYForMobile + angleY, outerCamera.eulerAngles.z);
    }

    public void ChangeCameraAngleX(float angleX)
    {
        if (!Globals.IsMobile && Mathf.Abs(angleX) > 5) return;


        if (angleX > 0 && outerCamera.localEulerAngles.x > (xLimitUp - 10) && outerCamera.localEulerAngles.x < xLimitUp) return;
        if (angleX < 0 && outerCamera.localEulerAngles.x < (xLimitDown + 10) && outerCamera.localEulerAngles.x > xLimitDown) return;


        outerCamera.localEulerAngles = new Vector3(outerCamera.localEulerAngles.x + angleX * 2, outerCamera.localEulerAngles.y, outerCamera.localEulerAngles.z);

    }

    private void changeCurrentCameraDistance(float val)        
    {
        currentCameraDistance += val;
        if (currentCameraDistance > 0.9f)
        {
            currentCameraDistance = 1;
        }
        else if (currentCameraDistance < 0)
        {
            currentCameraDistance = 0;
        }

        mainCamera.localPosition = Vector3.Lerp(baseCameraBodyPosition, new Vector3(0, 1f, 0.7f), currentCameraDistance);
    }

    private void setCurrentCameraDistance(float val)
    {
        currentCameraDistance = val;
        if (currentCameraDistance > 1)
        {
            currentCameraDistance = 1;
        }
        else if (currentCameraDistance <= 0.1f)
        {
            currentCameraDistance = 0;
        }
               
        /*
        if (currentCameraDistance < 0.25f)
        {
            skinControl.SetSkin(false);
        }
        else if(currentCameraDistance > 0.3f)
        {
            skinControl.SetSkin(true);
        }*/



        Vector3 newVector = Vector3.Lerp(new Vector3(0,0,defaultCameraDistance), Vector3.zero, currentCameraDistance);
        mainCamTransformForRaycast.DOLocalMove(newVector, 0.05f).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {        
        mainCamera.localPosition = BasePositionShiftForCamera;
        baseCameraBodyPosition = BasePositionShiftForCamera;


        if (zoomTimer > 10)
        {
            zoomTimer = 0;

            if (currentZoom != Globals.MainPlayerData.Zoom)
            {
                currentZoom = Globals.MainPlayerData.Zoom;
                SaveLoadManager.Save();
            }
        }
        else
        {
            zoomTimer += Time.deltaTime;
        }


        outerCamera.position = mainPlayer.position + Vector3.up * 2;
        outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, playerControl.angleYForMobile, outerCamera.eulerAngles.z);
                
    }

    
    private void FixedUpdate()
    {
        
        if (_timer > _timerCooldown)
        {            
            _timer = 0;
            _timerCooldown = 0.05f;
            newSystem();
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }
    

    private Vector3 mainPlayerPoint => mainPlayer.position + Vector3.up * 1.2f;


    private void newSystem()
    {
        if (isZooming) return;

        Vector3 playerPoint = mainPlayerPoint;

        if (Physics.Raycast(playerPoint + Vector3.down * 0.4f, (mainCamera.position - playerPoint).normalized, out hit, defaultCameraDistance, mainMask))
        {
            float distToBarrier = (playerPoint - hit.point).magnitude;

            float distKoeff = distToBarrier / defaultCameraDistance;
            setCurrentCameraDistance(distKoeff);
        }
        else
        {
            if (currentCameraDistance < 1) setCurrentCameraDistance(1);
        }

    }
    private IEnumerator playZoom(int koeff)
    {
        isZooming = true;

        for (int i = 0; i < 200; i++)
        {            
            ChangeZoom(koeff);
            yield return fixedDelta;

            if (koeff > 0)
            {
                if (Physics.Raycast(mainPlayerPoint, (mainCamTransformForRaycast.position - mainPlayerPoint).normalized, out hit, defaultCameraDistance, ~ignoreMask, QueryTriggerInteraction.Ignore))
                {
                    //
                }
                else
                {
                    break;
                }
            }
            else
            {
                float currentDistance = (mainPlayerPoint - mainCamTransformForRaycast.position).magnitude;
                if ((defaultCameraDistance - currentDistance) <= 0.5f)
                {
                    break;
                }

                if (Physics.Raycast(mainPlayerPoint, (mainCamTransformForRaycast.position - mainPlayerPoint).normalized, out hit, defaultCameraDistance, ~ignoreMask, QueryTriggerInteraction.Ignore))
                {
                    break;
                }                
            }
        }

        isZooming = false;
        _timerCooldown = 0.5f;
    }

}
