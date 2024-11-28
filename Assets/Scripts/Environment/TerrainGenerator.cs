using Cysharp.Threading.Tasks;
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
    [Inject] private WorldGenerator worldGenerator;

    private Transform mainPlayer;
    private Transform terrainLocation;
    
    //making terrain in update
    private float _currentTimer = 0;
    private float _cooldown = 2;
    private float distanceForCheking = 100;

    private Dictionary<Vector3, GameObject> readyTerrains = new Dictionary<Vector3, GameObject>();
    private List<Vector3> currentlyActivePoints = new List<Vector3>();

    private Queue<TerrainData> terrainsToClear = new Queue<TerrainData>();
    private bool isCleaningTerrain;
    

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

        if (!isCleaningTerrain && terrainsToClear.Count > 0)
        {
            cleanTerrain(terrainsToClear.Dequeue()).Forget();
        }
    }
    private async UniTaskVoid cleanTerrain(TerrainData data)
    {
        isCleaningTerrain = true;

        data.ReleaseCells(assetManager);

        await UniTask.Delay(500);
        isCleaningTerrain = false;
    }

    private void checkAround()
    {
        List<Vector3> regions = new List<Vector3>();

        //center
        regions.Add(new Vector3(Mathf.RoundToInt(mainPlayer.position.x / 100f) * 100, 0, Mathf.RoundToInt(mainPlayer.position.z / 100f) * 100));
        //left up
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.position.x - distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.position.z + distanceForCheking) / 100f) * 100));
        //left down
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.position.x - distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.position.z - distanceForCheking) / 100f) * 100));
        //up
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.position.x) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.position.z + distanceForCheking) / 100f) * 100));
        //down
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.position.x) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.position.z - distanceForCheking) / 100f) * 100));
        //left
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.position.x - distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.position.z) / 100f) * 100));
        //right
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.position.x + distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.position.z) / 100f) * 100));
        //right up
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.position.x + distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.position.z + distanceForCheking) / 100f) * 100));
        //right down
        regions.Add(new Vector3(Mathf.RoundToInt((mainPlayer.position.x + distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((mainPlayer.position.z - distanceForCheking) / 100f) * 100));

        
        if (!Globals.IsLowFPS)
        {
            Vector3 farPos = pc.FarMarker.position;

            regions.Add(new Vector3(Mathf.RoundToInt(farPos.x / 100f) * 100, 0, Mathf.RoundToInt(farPos.z / 100f) * 100));

            if (Mathf.Abs(farPos.z - mainPlayer.position.z) > Mathf.Abs(farPos.x - mainPlayer.position.x))
            {
                //left
                regions.Add(new Vector3(Mathf.RoundToInt((farPos.x - distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((farPos.z) / 100f) * 100));
                //right
                regions.Add(new Vector3(Mathf.RoundToInt((farPos.x + distanceForCheking) / 100f) * 100, 0, Mathf.RoundToInt((farPos.z) / 100f) * 100));
            }
            else
            {
                //up
                regions.Add(new Vector3(Mathf.RoundToInt((farPos.x) / 100f) * 100, 0, Mathf.RoundToInt((farPos.z + distanceForCheking) / 100f) * 100));
                //down
                regions.Add(new Vector3(Mathf.RoundToInt((farPos.x) / 100f) * 100, 0, Mathf.RoundToInt((farPos.z - distanceForCheking) / 100f) * 100));
            }

        }

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

        List<Vector3> keysToRemove = new List<Vector3>();
        foreach (Vector3 key in readyTerrains.Keys)
        {
            if (!readyTerrains[key].activeSelf)
            {
                float distance = (key - mainPlayer.position).magnitude;
                if (distance > 200)
                {
                    //readyTerrains[key].GetComponent<TerrainData>().ReleaseCells(assetManager);
                    TerrainData data = readyTerrains[key].GetComponent<TerrainData>();
                    if (!terrainsToClear.Contains(data)) terrainsToClear.Enqueue(data);
                    assetManager.ReturnAsset(readyTerrains[key]);                    
                    keysToRemove.Add(key);
                }
            }
        }

        for (int i = 0; i < keysToRemove.Count; i++)
        {
            if (readyTerrains.ContainsKey(keysToRemove[i])) readyTerrains.Remove(keysToRemove[i]);
        }
    }

    private void makeTerrainChunk(Vector3 pos)
    {        
        GameObject g = assetManager.GetAssetByTerrainType(AssetTypes.terrain_flat, worldGenerator.GetTerrainIndex(pos));
        g.transform.parent = terrainLocation;
        g.transform.position = pos;
        g.transform.eulerAngles = Vector3.zero;
        g.SetActive(true);
        TerrainData data = g.GetComponent<TerrainData>();

        for (float x = -37.5f; x <= 37.7f; x+=25)
        {
            for (float z = -37.5f; z <= 37.5f; z+=25)
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
