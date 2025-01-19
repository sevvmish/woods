using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class WorldGenerator : MonoBehaviour
{
    [Inject] private AssetManager assetManager;    
    [Inject] private PlayerControl pc;

    private const int MAX_TERRAIN_INDEX = 2;
      


    public int GetTerrainIndex(Vector3 vector)
    {
        System.Random rnd = new System.Random(Globals.MainPlayerData.TerrainSeed);

        for (int x = -5000; x <= 5000; x += 100)
        {
            for (int y = -5000; y <= 5000; y += 100)
            {
                Vector2 vec = new Vector2(x, y);
                int result = rnd.Next(0, MAX_TERRAIN_INDEX);
                if (vec == new Vector2(vector.x, vector.z))
                {
                    return result;
                }
            }
        }

        return 0;
    }

    public bool IsEmptyPlace(Vector3 vector)
    {
        System.Random rnd = new System.Random(Globals.MainPlayerData.TerrainSeed+1);

        for (int x = -5000; x <= 5000; x += 100)
        {
            for (int y = -5000; y <= 5000; y += 100)
            {
                Vector2 vec = new Vector2(x, y);
                int result = rnd.Next(0, 7);
                if (vec == new Vector2(vector.x, vector.z))
                {                    
                    return result < 3;
                }
            }
        }

        return false;
    }

    public int GetRandomIndex(int from, int index, int maxValue, int seed)
    {
        System.Random rnd = new System.Random(seed);
        int result = 0;
        
        for (int i = 0; i <= index; i++)
        {
            result = rnd.Next(from, maxValue);

            if (i == index)
            {                
                return result;
            }
        }
        print("ejfwkebfwef");
        return result;
    }

    public int GetRandomIndexTest(int from, int index, int maxValue, int seed)
    {
        System.Random rnd = new System.Random(seed);
        int result = 0;

        for (int i = 0; i <= index; i++)
        {
            result = rnd.Next(from, maxValue);

            if (i == index)
            {
                return result;
            }
        }

        return result;
    }


}
