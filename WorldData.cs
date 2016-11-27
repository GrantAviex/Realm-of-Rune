using UnityEngine;
using System.Collections;

[System.Serializable]
public struct biomeInfo
{
    public int id;
    public float rot;
    public int instance;
}

[System.Serializable]
public struct townInfo
{
    public float rot;
    public int exists;
}

[System.Serializable]
public class WorldData
{
    public townInfo[,] towns;
    public biomeInfo[,] world;
    public int size;
    public float biomeSize;
    public float biomeScale;
    public int chunkSize;

    public bool create;

	void Start ()
    {
        create = true;
        world = new biomeInfo[size, size];
        towns = new townInfo[size, size];
	}
}
