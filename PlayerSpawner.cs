using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour 
{
    PlayerData data;
    public GameObject player;
    public void Spawn(PlayerData spawnData)
    {
        data = spawnData;
        Vector3 spawnPos = new Vector3(spawnData.locationX, spawnData.locationY, spawnData.locationZ);
        if(spawnPos == Vector3.zero)
        {
            spawnPos = transform.position;
        }
        RaycastHit hitInfo;
        if (Physics.Raycast((spawnPos + Vector3.up * 10f), Vector3.down, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                spawnPos = hitInfo.point;
            }
        }
        player.name = "Character";
        player.transform.position = spawnPos;
        player.SetActive(true);
        Invoke("IntializePlayer", 0.3f);

    }
    void IntializePlayer()
    {
        EquipmentManager manager = GameObject.Find("Equipment Manager").GetComponent<EquipmentManager>();
        GearSystem gearSys = player.GetComponent<GearSystem>();
        GameObject obj = gearSys.transform.GetChild(0).GetChild(0).gameObject;
        manager.offhandLeftWep = obj;
        obj = gearSys.transform.GetChild(0).GetChild(1).gameObject;
        manager.offhandRightWep = obj;
        for (int i = 0; i < data.equips.Count; i++)
        {
            manager.EquipItemID(data.equips[i]);
        }
        Invoke("SetUpOffhandWeps", .5f);
        Inventory inv = manager.GetInventory();
        for (int i = 0; i < data.inventory.Count; i++)
        {
            inv.AddItem(data.inventory[i], data.inventoryAmounts[i]);
        }
        gearSys.SetHairColor(new Color(data.hairColorR, data.hairColorG, data.hairColorB));
        gearSys.SetEyeColor(new Color(data.eyeColorR, data.eyeColorG, data.eyeColorB));
        gearSys.SetGender(data.gender);
        gearSys.SwitchTribe(data.tribe);
        gearSys.SwitchSkinTone(data.skinTone);
        gearSys.SetHairstyle(data.hairstyle);
    }
    void SetUpOffhandWeps()
    {
        EquipmentManager manager = GameObject.Find("Equipment Manager").GetComponent<EquipmentManager>();
        for (int i = 0; i < data.sideWeps.Count; i++)
        {
            manager.SetOffhandWep(data.sideWeps[i], i);
        }
    }
}
