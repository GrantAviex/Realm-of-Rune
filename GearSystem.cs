using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Finger
{
    public GameObject Knuckle;
    public GameObject Tip;
    public Finger(GameObject finger)
    {
        Knuckle = finger;
        Tip = finger.transform.GetChild(0).gameObject;
    }
}
public class Glove
{
    public GameObject Hand;
    public List<Finger> fingers = new List<Finger>();

    public Glove(GameObject gameObject)
    {
        Hand = gameObject;
        for (int i = 0; i < 4; i++)
        {
            fingers.Add(new Finger(Hand.transform.GetChild(i).gameObject));
        }

    }
}

public class GearSystem : MonoBehaviour
{
    public bool charSelectionScreen = false;
   
    public int currGender;
    public int currHairstyle;
    public string currArmor;
    public string currHat;
    public string currBoots;
    public string currGloves;
    public string currPants;
    public string currCape;
    public int currTribe;
    public int currSkinTone = 1;

    public string tribePath;
    public int maleHairstylesCount;
    public int femaleHairstylesCount;
    private int[] hairstyles;
    public GameObject[] Armor;
    public GameObject[] Pants;
    public GameObject[] Gloves;
    public Glove[] Hands;
    public GameObject[] Boots;
    public GameObject[] Cape;
    public GameObject Hat;
    public GameObject Hair;
    public GameObject[] Helmet;
    public GameObject[] Weapons;
    public GameObject Eyes;
    public GameObject[] sideWeapons;
    public Color hairColor;
    public Color eyeColor;

    public SkinToneManager skinManager;

    public PlayerCollectingController m_collector;

    // Use this for initialization
    void Start()
    {
        tribePath = "Tribeless";
        currArmor = null;
        currPants = null;
        hairstyles = new int[2];
        Hands = new Glove[2];
        for (int i = 0; i < 2; i++)
        {
            Hands[i] = new Glove(Gloves[i]);
        }
        hairstyles[0] = maleHairstylesCount;
        hairstyles[1] = femaleHairstylesCount;
        hairColor = Hair.transform.GetChild(0).GetComponent<Renderer>().material.color;
        eyeColor = Eyes.transform.GetComponent<Renderer>().material.color;
        //hairColor = ;
    }

    // Update is called once per frame
    void Update()
    {            
    }

    public void SwitchSkinTone(int tone)
    {
        if(tone != currSkinTone)
        {
            currSkinTone = tone;
            if(currSkinTone > 5 || currSkinTone < 1)
            {
                currSkinTone = 1;
            }
            skinManager.UpdateTone(currSkinTone, currGender);
        }
    }
    public void SwitchTribe(int tribe)
    {   
        if(tribe != currTribe)
        {
            currTribe = tribe;
            switch (currTribe)
            {
                case 0:
                    {
                        tribePath = "Tribeless";
                        break;
                    }
                case 1:
                    {
                        tribePath = "Islander";
                        break;
                    }
                case 2:
                    {
                        tribePath = "Woodsmen";
                        break;
                    }
                case 3:
                    {
                        tribePath = "Inuit";
                        break;
                    }
                case 4:
                    {
                        tribePath = "Merchant";
                        break;
                    }
            }
            SwitchArmor(null);
            SwitchPants(null);
            SwitchGloves(null);
            SwitchBoots(null);  
        }

    }
    public void SwitchArmor(string path)
    {
        if (path == null)
            path = tribePath + "Shirt";
        

        currArmor = path;
        for (int i = 0; i < 3; i++)
        {
            GameObject parent = Armor[i];
            GameObject temp = parent.transform.GetChild(parent.transform.childCount - 1).gameObject;
            if (temp != null)
            {
                string name = "Armor/" + path + "/";
                if (currGender == 1 && temp.name == "Torso")
                    name += "Female";
                name += temp.name;
                GameObject newGear = Resources.Load<GameObject>(name);
                Replace(temp, newGear);
            }
        }
    }
    public void SwitchArmorVarient()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject parent = Armor[i];
            GameObject temp = parent.transform.GetChild(parent.transform.childCount - 1).gameObject;
            if (temp != null)
            {
                string name = "Armor/";
                if(currArmor == null)
                {
                    name += tribePath+ "Shirt/";
                }
                else
                {
                    name += currArmor + "/";
                }
                if (currGender == 1 && temp.name == "Torso")
                    name += "Female";
                name += temp.name;
                GameObject newGear = Resources.Load<GameObject>(name);
                GameObject spawnGear = Replace(temp, newGear);
                if(currGender == 1 && (spawnGear.name  == "LeftShoulder" || spawnGear.name == "RightShoulder"))
                {

                    spawnGear.transform.localPosition = new Vector3(spawnGear.transform.localPosition.x / 5, spawnGear.transform.localPosition.y, spawnGear.transform.localPosition.z);
                }
                else
                {
                    spawnGear.transform.localPosition = new Vector3(spawnGear.transform.localPosition.x * 5, spawnGear.transform.localPosition.y, spawnGear.transform.localPosition.z);
                }
            }

        }
    }
    public void SetThrownWepInfo(EquippedItemData data)
    {
        Weapons[1].GetComponentInChildren<ThrownWeapon>().SetMyData(data);
    }
    public void SwitchWeapon(int wepHand,  Item item)
    {
        GameObject parent = Weapons[wepHand];
        GameObject temp = parent.transform.GetChild(0).gameObject;
        if (item != null)
        {
            string name = "";
            if (item.Eqp.Wep != null)
                name += "Weapons/" + item.Name;
            else if (item.Eqp.Tool != null)
            {
                name += "Tools/";
                switch (item.Eqp.Tool.Type)
                {
                    case (int)Tools.FishingRod:
                        {
                            name += "FishingRods/";
                            break;
                        }
                    case (int)Tools.Axe:
                        {
                            name += "Axes/";
                            break;
                        }
                    case (int)Tools.Pickaxe:
                        {
                            name += "Pickaxes/";
                            break;
                        }
                    case (int)Tools.ButchersKnife:
                        {
                            name += "HuntersKnife/";
                            break;
                        }
                    case (int)Tools.Bolas:
                        {
                            name += "Bolas/";
                            break;
                        }
                }
                name += item.Name;

            }
            else if(item.Eqp.Type == 10)
            {
                name += "Weapons/" + item.Name;
            }
            Debug.Log(name);
            GameObject newWep = Resources.Load<GameObject>(name);
            if (newWep != null)
            {
                GameObject spawnWep = (GameObject)Instantiate(newWep, temp.transform.position, Quaternion.identity);
                if(item.Eqp.Type == 0 || item.Eqp.Type == 1)
                {
                    Weapon tempWep = spawnWep.GetComponent<Weapon>();
                    switch (item.Eqp.Wep.AttackSpeed)
                    {
                        case 0:
                        case 1:
                            {
                                tempWep.attackAnimationDuration = .65f;
                                break;
                            }
                        case 2:
                            {
                                tempWep.attackAnimationDuration = .8f;
                                break;
                            }
                        case 3:
                            {
                                tempWep.attackAnimationDuration = 1.0f;
                                break;
                            }
                        case 4:
                            {
                                tempWep.attackAnimationDuration = 1.2f;
                                break;
                            }
                        case 5:
                            {
                                tempWep.attackAnimationDuration = 1.35f;
                                break;
                            }
                    }
                    tempWep.type = item.Eqp.Type;
                    tempWep.weaponSpeed = item.Eqp.Wep.AttackSpeed;
                    tempWep.SetDamage(item.Eqp.Wep.Damage);
                    tempWep.magicPower = item.Eqp.Wep.MagicPower;
                }
                else if (item.Eqp.Type == 2)
                {
                    RangedWeapon tempWep = spawnWep.GetComponent<RangedWeapon>();
                    switch (item.Eqp.Wep.AttackSpeed)
                    {
                        case 1:
                            {
                                tempWep.attackAnimationDuration = .65f;
                                break;
                            }
                        case 2:
                            {
                                tempWep.attackAnimationDuration = .8f;
                                break;
                            }
                        case 3:
                            {
                                tempWep.attackAnimationDuration = 1.0f;
                                break;
                            }
                        case 4:
                            {
                                tempWep.attackAnimationDuration = 1.2f;
                                break;
                            }
                        case 5:
                            {
                                tempWep.attackAnimationDuration = 1.35f;
                                break;
                            }
                    }
                    tempWep.type = item.Eqp.Type;
                    tempWep.weaponSpeed = item.Eqp.Wep.AttackSpeed;
                    tempWep.SetDamage(item.Eqp.Wep.Damage);
                    tempWep.magicPower = item.Eqp.Wep.MagicPower;
                }
                else if (item.Eqp.Type == 11)
                {
                    ThrownWeapon tempWep = spawnWep.GetComponent<ThrownWeapon>();
                    switch (item.Eqp.Wep.AttackSpeed)
                    {
                        case 1:
                            {
                                tempWep.attackAnimationDuration = .65f;
                                break;
                            }
                        case 2:
                            {
                                tempWep.attackAnimationDuration = .8f;
                                break;
                            }
                        case 3:
                            {
                                tempWep.attackAnimationDuration = 1.0f;
                                break;
                            }
                        case 4:
                            {
                                tempWep.attackAnimationDuration = 1.2f;
                                break;
                            }
                        case 5:
                            {
                                tempWep.attackAnimationDuration = 1.35f;
                                break;
                            }
                    }
                    tempWep.type = item.Eqp.Type;
                    tempWep.weaponSpeed = item.Eqp.Wep.AttackSpeed;
                    tempWep.SetDamage(item.Eqp.Wep.Damage);
                    tempWep.magicPower = item.Eqp.Wep.MagicPower;
                }


                if(item.Eqp.Tool != null)
                {
                    m_collector.SetTool(spawnWep.GetComponent<CollectionTool>());
                }
                spawnWep.transform.SetParent(temp.transform.parent.transform);
                spawnWep.transform.rotation = temp.transform.rotation;
                spawnWep.transform.localScale = temp.transform.localScale;
                spawnWep.name = temp.name;
                Destroy(temp);
                spawnWep.name = item.Title;
            }
        }
        else
        {
            temp.SetActive(false);
        }
    }
    public void SwitchGloves(string path)
    {
        if (path == null)
            path = tribePath+ "Hands";
        for (int i = 0; i < 2; i++)
        {
            GameObject parent = Hands[i].Hand;
            GameObject temp = parent.transform.GetChild(parent.transform.childCount - 1).gameObject;
            for (int j = 0; j < 4; j++)
            {
                Finger Knuckle = Hands[i].fingers[j];
                GameObject tempKnuckle = Knuckle.Knuckle.transform.GetChild(Knuckle.Knuckle.transform.childCount - 1).gameObject;
                if (temp != null)
                {
                    string knuckleName = "Gloves/" + path + "/" + tempKnuckle.name;
                    GameObject newKnuckle = Resources.Load<GameObject>(knuckleName);
                    Replace(tempKnuckle, newKnuckle);
                }
                GameObject tempFingerTip = Knuckle.Tip.transform.GetChild(Knuckle.Tip.transform.childCount - 1).gameObject;
                if (temp != null)
                {
                    string fingerName = "Gloves/" + path + "/" + tempFingerTip.name;
                    GameObject newTip = Resources.Load<GameObject>(fingerName);
                    Replace(tempFingerTip, newTip);
                }

            }
            if (temp != null)
            {
                string name = "Gloves/" + path + "/" + temp.name;
                GameObject newGear = Resources.Load<GameObject>(name);
                Replace(temp, newGear);
            }
        }
    }
    public void SwitchHatHair(int type)
    {
        if(type == 0)
        {
            Hair.SetActive(true);
            SwitchHair(false);
        }
        else if(type ==1)
        {
            Hair.SetActive(true);
            SwitchHair(true);
        }
        else
        {
            Hair.SetActive(false);
        }
    }
    public void SwitchHat(string path, Item item)
    {
        
        if (path == null)
        {
            Helmet[0].SetActive(false);
            Hat.SetActive(false);
            SwitchHatHair(0);
            return;
        }
        else
        {
            currHat = path;
        }
        if(item.Eqp.Type == 4)
        {
            Helmet[0].SetActive(false);
            Hat.SetActive(true);
            SwitchHatHair(1);
            GameObject parent = Hat;
            GameObject temp = parent.transform.GetChild(parent.transform.childCount - 1).gameObject;
            if (temp != null)
            {
                string name = "Hats/" + path;
                GameObject newGear = Resources.Load<GameObject>(name);
                Replace(temp, newGear);
            }
        }
        else if(item.Eqp.Type ==9)
        {
            SwitchHatHair(2);
            Hat.SetActive(false);
            Helmet[0].SetActive(true);
            for (int i = 0; i < 2; i++)
            {
                GameObject parent = Helmet[i];
                GameObject temp = parent.transform.GetChild(parent.transform.childCount - 1).gameObject;
                if (temp != null)
                {
                    string name = "Helmets/" + path + "/";
                    name += temp.name;
                    GameObject newGear = Resources.Load<GameObject>(name);
                    Replace(temp, newGear);
                }
            }

        }
    }
    public void SwitchBoots(string path)
    {
        if (path == null)
            path = tribePath+"Boots";
        currBoots = path;
        for (int i = 0; i < 2; i++)
        {
            GameObject parent = Boots[i];
            GameObject temp = parent.transform.GetChild(parent.transform.childCount - 1).gameObject;
            if (temp != null)
            {
                string name = "Boots/" + path + "/";
                name += temp.name;
                Debug.Log(name);    
                GameObject newGear = Resources.Load<GameObject>(name);
                Replace(temp, newGear);
            }

        }
    }
    public void SwitchCape(string path)
    {
        currCape = path;
        for (int i = 0; i < 2; i++)
        {
            GameObject parent = Cape[i];
            GameObject temp = parent.transform.GetChild(parent.transform.childCount - 1).gameObject;
            if (temp != null)
            {
                if (path == null)
                    temp.SetActive(false);
                else
                {
                    string name = "Capes/" + path + "/";
                    name += temp.name;
                    GameObject newGear = Resources.Load<GameObject>(name);
                    Replace(temp, newGear);
                }
            }

        }
    }
    public void SwitchPants(string path)
    {
        currPants = path;
        if (path == null)
        {
            path = tribePath;
            if (currGender == 1)
            {

                GameObject testGear = Resources.Load<GameObject>("Pants/" + path + "Skirt/Waist");
                if (testGear != null)
                    path += "Skirt";
                else
                {
                    path += "Pants";
                }
            }
            else
            {
                path += "Pants";
            }

        }

        string name = "Pants/" + path + "/";

        for (int i = 0; i < 3; i++)
        {
            GameObject parent = Pants[i];
            GameObject temp = parent.transform.GetChild(parent.transform.childCount - 1).gameObject;

            if (temp != null)
            {
                string partName = name;
                if (i == 0)
                {
                    partName += "Waist";
                }
                else if (i == 1)
                {
                    partName += "LeftLeg";
                }
                else if (i == 2)
                {
                    partName += "RightLeg";
                }
                Debug.Log(name);
                Debug.Log(partName);
                GameObject newGear = Resources.Load<GameObject>(partName);
                if (newGear != null)
                {
                    Replace(temp, newGear);
                }
                else
                {
                    TurnOffRenderer(temp);
                }
            }
        }
    }
    void TurnOffRenderer(GameObject temp)
    {
        temp.GetComponent<Renderer>().enabled = false;
    }
    public void UnequipAll()
    {
        SwitchArmor(null);
        SwitchHat(null, null);
        SwitchCape(null);
        SwitchPants(null);
        SwitchBoots(null);
        SwitchGloves(null);
        SwitchWeapon(0, null);
        SwitchWeapon(1, null);
    }
    GameObject Replace(GameObject oldObj, GameObject spawnObj)
    {
        if (spawnObj != null)
        {
            GameObject newObj = (GameObject)Instantiate(spawnObj, oldObj.transform.position,Quaternion.identity);
            newObj.transform.SetParent(oldObj.transform.parent.transform);
            newObj.transform.rotation = oldObj.transform.rotation;
            newObj.transform.localScale = oldObj.transform.localScale;
            newObj.name = oldObj.name;
            Destroy(oldObj);
            return newObj;
        }
        else
        {
            TurnOffRenderer(oldObj);
            return null;
        }
    }
    //Change to specified hair
    public void SwitchHair(bool hat)
    {
        GameObject tHair = null;
        string hairName;
        if (currGender == 0)
        {
            hairName = "MaleHair";
        }
        else
        {
            hairName = "FemaleHair";
        }
        hairName += (currHairstyle + 1).ToString();
        if (hat)
            hairName += "Hat";
        tHair = Hair.transform.GetChild(0).gameObject;
        if (tHair != null)
        {
            GameObject newHair = Resources.Load<GameObject>("Hairstyles/" + hairName);
            GameObject spawnhair = (GameObject)Instantiate(newHair, tHair.transform.position, Quaternion.identity);
            spawnhair.transform.SetParent(Hair.transform);
            spawnhair.transform.rotation = tHair.transform.rotation;
            spawnhair.transform.localScale = tHair.transform.localScale;
            spawnhair.name = "hair";
            Destroy(tHair);
            spawnhair.GetComponent<Renderer>().material.color = hairColor;
        }

    }

    public void SetSideWeapon(int i, string path)
    {
        string name = "WeaponsOffhand/" + path;
        GameObject newOffhand = Resources.Load<GameObject>(name);
        if (newOffhand != null)
        {
            GameObject spawnOffhand = (GameObject)Instantiate(newOffhand);
            Debug.Log(i);
            spawnOffhand.transform.parent = sideWeapons[i].transform;
            Vector3 localPos = spawnOffhand.transform.position;
            spawnOffhand.transform.position = sideWeapons[i].transform.position;
            spawnOffhand.transform.localPosition = localPos;
            Quaternion localRot = spawnOffhand.transform.rotation;
            spawnOffhand.transform.localRotation = Quaternion.identity;
            spawnOffhand.transform.localRotation = localRot;
        }
    }
    //Switch out hairs, and skeleton for genders, armor variants need to be changed
    public void SwitchGender()
    {
        if (currGender >= 2)
        {
            currGender = 0;
        }
        if (currHairstyle >= hairstyles[currGender])
        {
            currHairstyle = 0;
        }
        SwitchArmorVarient();
        SwitchPants(currPants);

    }
    public void SetGender(int i)
    {
        if(i != currGender)
        {
            currGender = i;
            skinManager.UpdateTone(currSkinTone, currGender);
            SwitchGender();
        }
    }
    public void IncrementHairstyle()
    {
        currHairstyle++;
        if (currHairstyle >= hairstyles[currGender])
        {
            currHairstyle = 0;
        }
        SwitchHair(false);
    }
    public void DecrementHairstyle()
    {
        currHairstyle--;
        if (currHairstyle < 0)
        {
            currHairstyle = hairstyles[currGender]-1;
        }
        SwitchHair(false);
    }
    public void SetHairstyle(int i)
    {
        currHairstyle = i;
        if (currHairstyle < 0 || currHairstyle >= hairstyles[currGender])
        {
            currHairstyle = 0;
        }
        SwitchHair(false);
    }
    public void SetEyeColor(Color color)
    {
        eyeColor = color;
        Eyes.transform.GetComponent<Renderer>().material.color = eyeColor;
    }
    public void SetHairColor(Color color)
    {
        hairColor = color;
        Hair.transform.GetChild(0).GetComponent<Renderer>().material.color = hairColor;
    }
}
