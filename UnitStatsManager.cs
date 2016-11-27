using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitStatsManager : MonoBehaviour 
{
    Dictionary<EntityIDs,List<UnitStats>> entityPool;
	void Start () 
    {
        entityPool = new Dictionary<EntityIDs, List<UnitStats>>();
	}
	    
	void Update () 
    {
	
	}

    public void AddEntity(UnitStats entity)
    {
        entityPool[entity.GetID()].Add(entity);
    }
    public void RemoveEntity(UnitStats entity)
    {
        entityPool[entity.GetID()].Remove(entity);
    }
}
