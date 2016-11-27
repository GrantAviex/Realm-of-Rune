using UnityEngine;
using System.Collections;

public class Biome : MonoBehaviour 
{
    public int id;
    public GameObject TownPrefab;
    public int locationX;
    public int locationY;
    public World world;
    public GameObject myTown = null;
    public Vector3 spawnPoint;
    public uint myID;
    public int instance;
}
