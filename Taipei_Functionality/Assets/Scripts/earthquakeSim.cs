using UnityEngine;

public class earthquakeSim : MonoBehaviour
{
    public float scale = 20f;
    public int amplitude = 20;
    public float frequency = 10;

    private int width = 256;
    private int height = 256;
    private float offset = 0;

    Terrain ground;
    
    void Start()
    {
        ground = GetComponent<Terrain>();
        ground.terrainData = GenTerrain(ground.terrainData);
    }
    
    void Update()
    {
        //Terrain ground = GetComponent<Terrain>();
        ground.terrainData = GenTerrain(ground.terrainData);
        //offset += Time.deltaTime * frequency;
    }

    TerrainData GenTerrain(TerrainData terraindata)
    {
        terraindata.heightmapResolution = width + 1;
        terraindata.size = new Vector3(width, amplitude, height);
        terraindata.SetHeights(0, 0, generate_heights());
        return terraindata;
    }

    float[,] generate_heights()
    {
        float[,] heights = new float[width, height];
        for (int i = 0; i<height;i++)
        {
            for (int j = 0; j<width;j++)
            {
                heights[i, j] = calculateHeights(i,j);
            }
        }
        return heights;
    }

    float calculateHeights(int x,int y)
    {
        float xCoord = (float)x / width * scale + offset;
        float yCoord = (float)y / height *scale + offset;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
