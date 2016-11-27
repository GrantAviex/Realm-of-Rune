using UnityEngine;
using System.Collections;

public class CharacterModifier : MonoBehaviour 
{
    [SerializeField]
    GearSystem playerSystem;

    int currSkinTone = 0;
    int currGender = 0;
    float changeTimer = 0;
	void Start () 
    {
        currSkinTone = 1;
	}
	
	void FixedUpdate () 
    {
        changeTimer -= Time.deltaTime;
	    if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            currSkinTone++;
            if (currSkinTone > 5)
            {
                currSkinTone = 5;
            }
            else
            {
                playerSystem.SwitchSkinTone(currSkinTone);
            }
        }
        if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            currSkinTone--;
            if (currSkinTone < 1)
            {
                currSkinTone = 1;
            }
            else
            {
                playerSystem.SwitchSkinTone(currSkinTone);
            }
        }

        if (changeTimer <= 0)
        {

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                playerSystem.SwitchTribe(0);
                changeTimer = 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                playerSystem.SwitchTribe(1);
                changeTimer = 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                playerSystem.SwitchTribe(2);
                changeTimer = 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                playerSystem.SwitchTribe(3);
                changeTimer = 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                playerSystem.SwitchTribe(4);
                changeTimer = 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                currGender++;
                if (currGender == 2)
                    currGender = 0;
                playerSystem.SetGender(currGender);
                playerSystem.SwitchHair(false);
                changeTimer = 0.5f;
            }
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            playerSystem.IncrementHairstyle();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            playerSystem.SetEyeColor(playerSystem.eyeColor);
        }
	}
}
