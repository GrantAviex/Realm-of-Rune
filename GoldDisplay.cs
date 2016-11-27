using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PixelCrushers.DialogueSystem;
public class GoldDisplay : MonoBehaviour 
{
    Text goldText;
	// Use this for initialization
	void Start () 
    {
        goldText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        int gold = GameManager.Instance.gameData.data.gold;
        goldText.text = gold.ToString();
        DialogueLua.SetVariable("Player Gold", gold);        
	}
}
