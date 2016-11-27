using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class EquipmentManager : MonoBehaviour
{
    public GearSystem modelEquip;
    private Inventory inv;
    public List<GameObject> equipSlots;
    public List<EquippedItemData> equippedItems;
    public int hat, armor, gloves, pants, boots, cape, rightWep, leftWep, sideRightWep, sideLeftWep;
    public int activeTab = 1;
    public Image tab1, tab2;
    public GameObject offhandLeftWep;
    public GameObject offhandRightWep;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();

        for (int i = 0; i < transform.childCount; i++ )
        {
            string name = transform.GetChild(i).name;
            if(name == "HatSlot")
            {
                hat = i;
            }
            else if(name == "ArmorSlot")
            {
                armor = i;
            }
            else if (name == "PantsSlot")
            {
                pants = i;
            }
            else if (name == "BootsSlot")
            {
                boots = i;
            }
            else if (name == "RightWeaponSlot")
            {
                rightWep = i;
            }
            else if (name == "LeftWeaponSlot")
            {
                leftWep = i;
            }
            else if (name == "CapeSlot")
            {
                cape = i;
            }
            else if (name == "GlovesSlot")
            {
                gloves = i;
            }
            else if (name == "SideRightWeaponSlot")
            {
                sideRightWep = i;
            }
            else if (name == "SideLeftWeaponSlot")
            {
                sideLeftWep = i;
            }
            equipSlots.Add(transform.GetChild(i).gameObject);
            equippedItems.Add(transform.GetChild(i).GetChild(0).GetComponent<EquippedItemData>());
        }
        equipSlots[sideLeftWep].SetActive(false);
        equipSlots[sideRightWep].SetActive(false);
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (activeTab == 1)
            {
                WeaponTab2Activate();
                WeaponTab1Deactivate();
            }  

            else if (activeTab == 2)
            {
                WeaponTab1Activate();
                WeaponTab2Deactivate();
            }
        }
    }
    public Inventory GetInventory()
    {
        return inv;
    }
    public void EquipItemID(int id)
    {
        if (inv == null)
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        Item item = inv.database.FetchItemByID(id);
        EquipItem(item, item.Eqp.Type, 1);
    }
    public void SetOffhandWep(int id, int hand)
    {
      if (inv == null)
          inv = GameObject.Find("Inventory").GetComponent<Inventory>();
      Item item = inv.database.FetchItemByID(id);
      string name = "WeaponsOffhand/" + item.Name;
      if(item.Eqp.Type == 1 || item.Eqp.Type == 2 || item.Eqp.Type == 10)
      {
          GameObject newOffhand = Resources.Load<GameObject>(name);
          if (newOffhand != null)
          {
              GameObject spawnOffhand = (GameObject)Instantiate(newOffhand);
              spawnOffhand.transform.parent = offhandRightWep.transform;
              Vector3 localPos = spawnOffhand.transform.position;
              spawnOffhand.transform.position = offhandRightWep.transform.position;
              spawnOffhand.transform.localPosition = localPos;
              Quaternion localRot = spawnOffhand.transform.rotation;
              spawnOffhand.transform.localRotation = Quaternion.identity;
              spawnOffhand.transform.localRotation = localRot;
              equippedItems[sideRightWep].SetGear(item);
          }
      }
      else
      {
          if(hand == 0)
          {
              GameObject newOffhand = Resources.Load<GameObject>(name);
              if (newOffhand != null)
              {
                  GameObject spawnOffhand = (GameObject)Instantiate(newOffhand);
                  spawnOffhand.transform.parent = offhandRightWep.transform;
                  Vector3 localPos = spawnOffhand.transform.position;
                  spawnOffhand.transform.position = offhandRightWep.transform.position;
                  spawnOffhand.transform.localPosition = localPos;
                  Quaternion localRot = spawnOffhand.transform.rotation;
                  spawnOffhand.transform.localRotation = Quaternion.identity;
                  spawnOffhand.transform.localRotation = localRot;
                  equippedItems[sideRightWep].SetGear(item);
              }
          }
          else
          {
              GameObject newOffhand = Resources.Load<GameObject>(name);
              if (newOffhand != null)
              {
                  GameObject spawnOffhand = (GameObject)Instantiate(newOffhand);
                  spawnOffhand.transform.parent = offhandLeftWep.transform;
                  Vector3 localPos = spawnOffhand.transform.position;
                  spawnOffhand.transform.position = offhandLeftWep.transform.position;
                  spawnOffhand.transform.localPosition = localPos;
                  Quaternion localRot = spawnOffhand.transform.rotation;
                  spawnOffhand.transform.localRotation = Quaternion.identity;
                  spawnOffhand.transform.localRotation = localRot;
                  equippedItems[sideLeftWep].SetGear(item);
              }
          }
      }
    }
    public void FindModelEquip()
    {
        modelEquip = GameObject.Find("Character").GetComponent<GearSystem>();
    }
    public bool EquipItem(Item item, int type, int amount)
    {
        if(modelEquip == null)
        {
            FindModelEquip();
        }
        Skills skillLevels = GameManager.Instance.gameData.data.skillLevels;
        Debug.Log(skillLevels);
        foreach (LevelReq req in item.Eqp.LvlsReq)
        {
            if(skillLevels.skills[req.skill].level < req.level)
            {
                DialogueManager.ShowAlert("You do not meet the required levels");
                //return false;
            }
        }
        Debug.Log(item.Eqp.Type);
        switch (item.Eqp.Type)
        {
            case 0:
                {
                    if(activeTab == 1)
                    {
                        if (equippedItems[rightWep].item == null)
                        {
                            equippedItems[rightWep].SetGear(item);
                            modelEquip.SwitchWeapon(1, item);
                        }
                        else if (equippedItems[leftWep].item == null && equippedItems[rightWep].item.Eqp.Type == 0)
                        {
                            equippedItems[leftWep].SetGear(item);
                            modelEquip.SwitchWeapon(0, item);
                        }
                        else
                        {
                            equippedItems[rightWep].SetGear(item);
                            modelEquip.SwitchWeapon(1, item);
                        }
                    }
                    else if(activeTab == 2)
                    {
                        if (equippedItems[sideRightWep].item == null)
                        {
                            equippedItems[sideRightWep].SetGear(item);
                            modelEquip.SwitchWeapon(1, item);
                        }
                        else if (equippedItems[sideLeftWep].item == null && equippedItems[sideRightWep].item.Eqp.Type == 0)
                        {
                            equippedItems[sideLeftWep].SetGear(item);
                            modelEquip.SwitchWeapon(0, item);
                        }
                        else
                        {
                            equippedItems[sideRightWep].SetGear(item);
                            modelEquip.SwitchWeapon(1, item);
                        }
                    }
                    break;
                }
            case 1:
                {
                    if(activeTab == 1)
                    {
                        equippedItems[rightWep].SetGear(item);
                        equippedItems[leftWep].SetGear(null);
                        modelEquip.SwitchWeapon(1, item);
                        modelEquip.SwitchWeapon(0, null);
                    }
                    else if (activeTab == 2)
                    {
                        equippedItems[sideRightWep].SetGear(item);
                        equippedItems[sideLeftWep].SetGear(null);
                        modelEquip.SwitchWeapon(1, item);
                        modelEquip.SwitchWeapon(0, null);
                    }
                    break;
                }
            case 2:
                {
                    if (activeTab == 1)
                    {
                        equippedItems[rightWep].SetGear(item);
                        equippedItems[leftWep].SetGear(null);
                        modelEquip.SwitchWeapon(1, item);
                        modelEquip.SwitchWeapon(0, null);
                    }
                    else if (activeTab == 2)
                    {
                        equippedItems[sideRightWep].SetGear(item);
                        equippedItems[sideLeftWep].SetGear(null);
                        modelEquip.SwitchWeapon(1, item);
                        modelEquip.SwitchWeapon(0, null);
                    }
                    break;
                }
            case 3:
                {
                    equippedItems[armor].SetGear(item);
                    modelEquip.SwitchArmor(item.Name);
                    break;
                }
            case 4:
                {
                    equippedItems[hat].SetGear(item);
                    modelEquip.SwitchHat(item.Name, item);
                    break;
                }
            case 5:
                {
                    equippedItems[pants].SetGear(item);
                    modelEquip.SwitchPants(item.Name);
                    break;
                }
            case 6:
                {
                    equippedItems[boots].SetGear(item);
                    modelEquip.SwitchBoots(item.Name);
                    break;
                }
            case 7:
                {
                    equippedItems[gloves].SetGear(item);
                    modelEquip.SwitchGloves(item.Name);
                    break;
                }
            case 8:
                {
                    equippedItems[cape].SetGear(item);
                    modelEquip.SwitchCape(item.Name);
                    break;
                }
            case 9:
                {
                    equippedItems[hat].SetGear(item);
                    modelEquip.SwitchHat(item.Name, item);
                    break;
                }
            case 10:
                {
                    if (activeTab == 1)
                    {
                        equippedItems[leftWep].SetGear(item);
                        modelEquip.SwitchWeapon(0, item);
                        if (equippedItems[rightWep].item != null)
                        {
                            if (equippedItems[rightWep].item.Eqp.Type != 0)
                            {

                                equippedItems[rightWep].SetGear(null);
                                modelEquip.SwitchWeapon(1, null);
                            }
                        }
                    }
                    else if (activeTab == 2)
                    {
                        equippedItems[sideLeftWep].SetGear(item);
                        modelEquip.SwitchWeapon(0, item);
                        if (equippedItems[sideRightWep].item != null)
                        {
                            if (equippedItems[sideRightWep].item.Eqp.Type != 0)
                            {
                                equippedItems[sideRightWep].SetGear(null);
                                modelEquip.SwitchWeapon(1, null);
                            }
                        }
                    }
                    break;
                }
            case 11:
                {
                    if (activeTab == 1)
                    {
                        equippedItems[rightWep].SetGear(item);
                        equippedItems[leftWep].SetGear(null);
                        modelEquip.SwitchWeapon(1, item);
                        modelEquip.SwitchWeapon(0, null);
                        equippedItems[rightWep].SetAmout(amount);
                        modelEquip.SetThrownWepInfo(equippedItems[rightWep]);
                    }
                    else if (activeTab == 2)
                    {
                        equippedItems[sideRightWep].SetGear(item);
                        equippedItems[sideLeftWep].SetGear(null);
                        modelEquip.SwitchWeapon(1, item);
                        modelEquip.SwitchWeapon(0, null);
                        equippedItems[sideRightWep].SetAmout(amount);
                    }
                    break;
                }
            case 12:
                {
                    if (activeTab == 1)
                    {
                        equippedItems[rightWep].SetGear(item);
                        equippedItems[leftWep].SetGear(null);
                        modelEquip.SwitchWeapon(1, item);
                        modelEquip.SwitchWeapon(0, null);
                        equippedItems[rightWep].SetAmout(amount);
                    }
                    else if (activeTab == 2)
                    {
                        equippedItems[sideRightWep].SetGear(item);
                        equippedItems[sideLeftWep].SetGear(null);
                        modelEquip.SwitchWeapon(1, item);
                        modelEquip.SwitchWeapon(0, null);
                        equippedItems[sideRightWep].SetAmout(amount);
                    }
                    break;
                }
        }
        return true;

    }
    public void UnquipItem(GameObject equipSlot, int type)
    {
        switch (type)
        {
            case 0:
            case 1:
            case 2:
            case 11:
                {
                    if (equipSlot.name == equipSlots[rightWep].name)
                    {
                        modelEquip.SwitchWeapon(1, null);
                    }
                    else if(equipSlot.name == equipSlots[leftWep].name)
                    {
                        modelEquip.SwitchWeapon(0, null);
                    }
                    else if (equipSlot.name == equipSlots[sideRightWep].name)
                    {
                        modelEquip.SwitchWeapon(1, null);
                    }
                    else
                    {
                        modelEquip.SwitchWeapon(0, null);
                    }
                    break;
                }
            case 3:
                {
                    modelEquip.SwitchArmor(null);
                    break;
                }
            case 4:
                {
                    modelEquip.SwitchHat(null,null);
                    //hat
                    break;
                }
            case 5:
                {
                    modelEquip.SwitchPants(null);
                    //pants
                    break;
                }
            case 6:
                {
                    modelEquip.SwitchBoots(null);
                    //boots
                    break;
                }
            case 7:
                {
                    //gloves
                    modelEquip.SwitchGloves(null);
                    break;
                }
            case 8:
                {
                    modelEquip.SwitchCape(null);
                    //cape
                    break;
                }
            case 9:
                {
                    //helmet
                    modelEquip.SwitchHat(null, null);
                    break;
                }
            case 10:
                {
                    Debug.Log("unequipping shield");
                    modelEquip.SwitchWeapon(0, null);
                    break;
                }
            case 12:
                {
                    Debug.Log("unequipping tool");
                    modelEquip.SwitchWeapon(1, null);
                    break;
                }
        }
    }

    public void WeaponTab1Activate()
    {
        if(activeTab == 2)
        {
            if (equippedItems[leftWep].item != null)
            {
                modelEquip.SwitchWeapon(0, equippedItems[leftWep].item);
            }
            if (equippedItems[rightWep].item != null)
            {
                modelEquip.SwitchWeapon(1, equippedItems[rightWep].item);
            }
            equipSlots[leftWep].SetActive(true);
            equipSlots[rightWep].SetActive(true);
            Color tempColor = tab2.color;
            tab2.color = tab1.color;
            tab1.color = tempColor;
        }
    }
    public void WeaponTab1Deactivate()
    {
        if (activeTab == 1)
        {
            activeTab = 2;
            if (offhandLeftWep.transform.childCount != 0)
            {
                Destroy(offhandLeftWep.transform.GetChild(0).gameObject);
            }
            if (equippedItems[leftWep].item != null)
            {
                UnquipItem(equipSlots[leftWep], equippedItems[leftWep].item.Eqp.Type);
                string name = "WeaponsOffhand/" + equippedItems[leftWep].item.Name;
                GameObject newOffhand = Resources.Load<GameObject>(name);
                if (newOffhand != null)
                {
                    GameObject spawnOffhand = (GameObject)Instantiate(newOffhand);
                    spawnOffhand.transform.parent = offhandLeftWep.transform;
                    Vector3 localPos = spawnOffhand.transform.position;
                    spawnOffhand.transform.position = offhandLeftWep.transform.position;
                    spawnOffhand.transform.localPosition = localPos;
                    Quaternion localRot = spawnOffhand.transform.rotation;
                    spawnOffhand.transform.localRotation = Quaternion.identity;
                    spawnOffhand.transform.localRotation = localRot;
                }
            }
            if (offhandRightWep.transform.childCount != 0)
            {
                Destroy(offhandRightWep.transform.GetChild(0).gameObject);
            }
            if (equippedItems[rightWep].item != null)
            {
                UnquipItem(equipSlots[rightWep], equippedItems[rightWep].item.Eqp.Type);
                string name = "WeaponsOffhand/" + equippedItems[rightWep].item.Name;
                GameObject newOffhand = Resources.Load<GameObject>(name);
                if(newOffhand != null)
                {
                    GameObject spawnOffhand = (GameObject)Instantiate(newOffhand);
                    spawnOffhand.transform.parent = offhandRightWep.transform;
                    Vector3 localPos = spawnOffhand.transform.position;
                    spawnOffhand.transform.position = offhandRightWep.transform.position;
                    spawnOffhand.transform.localPosition = localPos;
                    Quaternion localRot = spawnOffhand.transform.rotation;
                    spawnOffhand.transform.localRotation = Quaternion.identity;
                    spawnOffhand.transform.localRotation = localRot;
                }
            }
            equipSlots[leftWep].SetActive(false);
            equipSlots[rightWep].SetActive(false);
        }
    }
    public void WeaponTab2Activate()
    {
        if(activeTab == 1)
        {
            Debug.Log(equippedItems[sideLeftWep].item);
            Debug.Log(equippedItems[sideRightWep].item);
            if (equippedItems[sideLeftWep].item != null)
            {
                modelEquip.SwitchWeapon(0, equippedItems[sideLeftWep].item);
            }
            if (equippedItems[sideRightWep].item != null)
            {
                modelEquip.SwitchWeapon(1, equippedItems[sideRightWep].item);
            }
            equipSlots[sideLeftWep].SetActive(true);
            equipSlots[sideRightWep].SetActive(true);

            Color tempColor = tab2.color;
            tab2.color = tab1.color;
            tab1.color = tempColor;
        }
    }
    public void WeaponTab2Deactivate()
    {
        if (activeTab == 2)
        {
            activeTab = 1;
            if (offhandLeftWep.transform.childCount != 0)
            {
                Destroy(offhandLeftWep.transform.GetChild(0).gameObject);
            }
            if (equippedItems[sideLeftWep].item != null)
            {
                UnquipItem(equipSlots[sideLeftWep], equippedItems[sideLeftWep].item.Eqp.Type);
                string name = "WeaponsOffhand/" + equippedItems[sideLeftWep].item.Name;
                GameObject newOffhand = Resources.Load<GameObject>(name);
                if (newOffhand != null)
                {
                    GameObject spawnOffhand = (GameObject)Instantiate(newOffhand);
                    spawnOffhand.transform.parent = offhandLeftWep.transform;
                    Vector3 localPos = spawnOffhand.transform.position;
                    spawnOffhand.transform.position = offhandLeftWep.transform.position;
                    spawnOffhand.transform.localPosition = localPos;
                    Quaternion localRot = spawnOffhand.transform.rotation;
                    spawnOffhand.transform.localRotation = Quaternion.identity;
                    spawnOffhand.transform.localRotation = localRot;
                }
            }
            if (offhandRightWep.transform.childCount != 0)
            {
                Destroy(offhandRightWep.transform.GetChild(0).gameObject);
            }
            if (equippedItems[sideRightWep].item != null)
            {
                UnquipItem(equipSlots[sideRightWep], equippedItems[sideRightWep].item.Eqp.Type);
                string name = "WeaponsOffhand/" + equippedItems[sideRightWep].item.Name;
                GameObject newOffhand = Resources.Load<GameObject>(name);
                if (newOffhand != null)
                {
                    GameObject spawnOffhand = (GameObject)Instantiate(newOffhand);
                    spawnOffhand.transform.parent = offhandRightWep.transform;
                    Vector3 localPos = spawnOffhand.transform.position;
                    spawnOffhand.transform.position = offhandRightWep.transform.position;
                    spawnOffhand.transform.localPosition = localPos;
                    Quaternion localRot = spawnOffhand.transform.rotation;
                    spawnOffhand.transform.localRotation = Quaternion.identity;
                    spawnOffhand.transform.localRotation = localRot;
                }
            }
            equipSlots[sideLeftWep].SetActive(false);
            equipSlots[sideRightWep].SetActive(false);
        }
    }

    public void FillInPlayerEquips(PlayerData data)
    {
        for (int i = 0; i < equippedItems.Count; i++)
        {
            EquippedItemData equip = equippedItems[i];
            if(i != sideLeftWep && i != sideRightWep)
            {
                if (equip.item != null)
                    data.equips.Add(equip.item.ID);
            }
            else
            {
                Debug.Log(data.sideWeps);
                if (equip.item != null)
                    data.sideWeps.Add(equip.item.ID);
            }
        }
    }
}
