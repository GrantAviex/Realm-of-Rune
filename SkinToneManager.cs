using UnityEngine;
using System.Collections;

public class SkinToneManager : MonoBehaviour
{

    public GameObject[] Chest;
    public GameObject[] Legs;
    public GameObject[] Wrists;
    public Glove[] Hands;
    public GameObject[] Feet;
    public GameObject Head;
    public bool FemaleShoulders = false;
    public  int currGender = 0;

    void Start()
    {
        Hands = new Glove[2];
        for (int i = 0; i < 2; i++)
        {
            Hands[i] = new Glove(Wrists[i]);
        }
    }

    public void UpdateTone(int tone, int gender)
    {
        string path = "";
        if (tone > 0 && tone <= 5)
        {
            path = "SkinTones/" + tone.ToString() + "/Materials/";
            string meshPath = "SkinTones/" + tone.ToString() + "/";
            bool genderChange = (gender != currGender);
            currGender = gender;
            SwitchTorso(path, meshPath, genderChange);
            SwitchHands(path);
            SwitchFeet(path);
            SwitchLegs(path);
            SwitchHead(path);
        }

    }
    void CopyTexture(GameObject oldObj, Texture newTex)
    {
        Renderer rend = oldObj.GetComponent<Renderer>();
        rend.material.mainTexture = newTex;
    }
    public void SwitchTorso(string path, string meshPath, bool genderChange)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject temp = Chest[i];
            if (temp != null)
            {
                string name = path;
                if (currGender == 1 && temp.name == "Torso")
                    name += "Female";
                name += temp.name;
                Texture newTexture = Resources.Load<Texture>(name);
                if(currGender == 1 && temp.name == "Torso" && genderChange)
                {
                    GameObject newMesh = Resources.Load<GameObject>(meshPath + "Female" + temp.name);
                    temp.GetComponent<MeshFilter>().sharedMesh = newMesh.GetComponent<MeshFilter>().sharedMesh;
                }
                else if(currGender == 0 && temp.name == "Torso" && genderChange)
                {
                    GameObject newMesh = Resources.Load<GameObject>(meshPath + temp.name);
                    temp.GetComponent<MeshFilter>().sharedMesh = newMesh.GetComponent<MeshFilter>().sharedMesh;
                }
                if ((temp.name == "LeftShoulder" || temp.name == "RightShoulder") && genderChange)
                {
                    float move = 0;
                    if(temp.transform.localPosition.x < 0)
                    {
                        move = 0.02f;
                    }
                    else
                    {
                        move = -0.02f;
                    }
                    temp.transform.localPosition = new Vector3(temp.transform.localPosition.x  + move, temp.transform.localPosition.y, temp.transform.localPosition.z);
                }
                CopyTexture(temp, newTexture);
            }
        }
        FemaleShoulders = (currGender == 1);
    }
    public void SwitchHands(string path)
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject parent = Hands[i].Hand;
            GameObject temp = parent.transform.GetChild(parent.transform.childCount - 2).gameObject;
            for (int j = 0; j < 4; j++)
            {
                Finger Knuckle = Hands[i].fingers[j];
                GameObject tempKnuckle = Knuckle.Knuckle.transform.GetChild(Knuckle.Knuckle.transform.childCount - 2).gameObject;
                if (temp != null)
                {
                    string knuckleName = path + tempKnuckle.name;
                    Texture newTexture = Resources.Load<Texture>(knuckleName);
                    CopyTexture(tempKnuckle, newTexture);
                }
                GameObject tempFingerTip = Knuckle.Tip.transform.GetChild(Knuckle.Tip.transform.childCount - 2).gameObject;
                if (temp != null)
                {
                    string fingerName = path + tempFingerTip.name;
                    Texture newTexture = Resources.Load<Texture>(fingerName);
                    CopyTexture(tempFingerTip, newTexture);
                }

            }
            if (temp != null)
            {
                string name = path + temp.name;
                Texture newTexture = Resources.Load<Texture>(name);
                CopyTexture(temp, newTexture);
            }
        }
    }
    public void SwitchFeet(string path)
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject temp = Feet[i];
            if (temp != null)
            {
                string name = path + temp.name;
                Texture newTexture = Resources.Load<Texture>(name);
                CopyTexture(temp, newTexture);
            }

        }
    }
    public void SwitchLegs(string path)
    {
        for (int i =0;i < 3; i++)
        {
            GameObject temp = Legs[i];
            if (temp != null)
            {
                string name = path + temp.name;
                Texture newTexture = Resources.Load<Texture>(name);
                CopyTexture(temp, newTexture);
            }
        }
    }
    public void SwitchHead(string path)
    {
        GameObject temp = Head;
        if (temp != null)
        {
            string name = path + temp.name;
            Texture newTexture = Resources.Load<Texture>(name);
            CopyTexture(temp, newTexture);
        }
    }

}
