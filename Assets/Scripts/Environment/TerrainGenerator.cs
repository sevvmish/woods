using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class TerrainGenerator : MonoBehaviour
{
    [Inject] private PlayerControl pc;
    [Inject] private AssetManager assetManager;
    [Inject] private NatureGenerator natureGenerator;

    private Transform mainPlayer;
    private Transform terrainLocation;
    
    //making terrain in update
    private float _currentTimer = 0;
    private float _cooldown = 5;
    private float distanceForCheking = 100;

    private Dictionary<Vector3, GameObject> readyTerrains = new Dictionary<Vector3, GameObject>();
    private List<Vector3> currentlyActivePoints = new List<Vector3>();

    void Start()
    {
        mainPlayer = pc.transform;
        terrainLocation = transform;
        checkAround();
    }

    private void Update()
    {
        if (_currentTimer > _cooldown)
        {
            _currentTimer = 0;

            checkAround();
        }
        else
        {
            _currentTimer += Time.deltaTime;
        }
    }

    private void checkAround()
    {
        List<Vector3> regions = new List<Vector3>();

        //center
        regions.Add(new Vector3(Mathf.RoundToInt(mainPlayer.transform.position.x / 100f) * 100, 0, Mathf.RoundToInt(mainPlayer.transform.position.z / 100f) * 100));
        //left up
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.transform.position.x - distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.transform.position.z + distanceForCheking) / 100f) * 100));
        //left down
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.transform.position.x - distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.transform.position.z - distanceForCheking) / 100f) * 100));
        //up
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.transform.position.x) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.transform.position.z + distanceForCheking) / 100f) * 100));
        //down
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.transform.position.x) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.transform.position.z - distanceForCheking) / 100f) * 100));
        //left
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.transform.position.x - distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.transform.position.z) / 100f) * 100));
        //right
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.transform.position.x + distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.transform.position.z) / 100f) * 100));
        //right up
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.transform.position.x + distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.transform.position.z + distanceForCheking) / 100f) * 100));
        //right down
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.transform.position.x + distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.transform.position.z - distanceForCheking) / 100f) * 100));

        
        arrangeTerrains(regions);
    }

    private void arrangeTerrains(List<Vector3> regions)
    {
        HashSet<Vector3> newPoints = regions.ToHashSet();

        for (int i = 0; i < currentlyActivePoints.Count; i++)
        {
            if (!newPoints.Contains(currentlyActivePoints[i]))
            {
                readyTerrains[currentlyActivePoints[i]].SetActive(false);
                currentlyActivePoints.Remove(currentlyActivePoints[i]);
            }
        }


        for (int i = 0; i < regions.Count; i++)
        {            
            if (!readyTerrains.ContainsKey(regions[i]))
            {
                makeTerrainChunk(regions[i]);
            }
            else if (!readyTerrains[regions[i]].activeSelf)
            {
                readyTerrains[regions[i]].SetActive(true);
            }
        }
    }

    private void makeTerrainChunk(Vector3 pos)
    {
        GameObject g = Instantiate(assetManager.GetAssetByTerrainType(TerrainTypes.flat), terrainLocation);
        g.transform.position = pos;
        g.transform.eulerAngles = Vector3.zero;
        g.SetActive(true);
        TerrainData data = g.GetComponent<TerrainData>();

        for (int x = -40; x <= 40; x+=20)
        {
            for (int z = -40; z <= 40; z+=20)
            {
                Cell cell = Instantiate(assetManager.Cell, g.transform);
                cell.transform.localPosition = new Vector3(x, 0, z);
                cell.transform.eulerAngles = Vector3.zero;
                cell.gameObject.SetActive(true);
                data.AddCell(cell);
            }
        }

        readyTerrains.Add(pos, g);
        currentlyActivePoints.Add(pos);

        natureGenerator.GenerateNatureInTerrainChunk(g);
    }

}
