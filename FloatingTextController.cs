using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour {
    private static FloatingText popupText;

    public static void Initialize()
    {
        if (!popupText)
            popupText = Resources.Load<FloatingText>("Prefabs/PopupTextParent");
    }

    public static void CreateFloatingText(string text, Vector3 location)
    {
        FloatingText instance = Instantiate(popupText);
        //Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-.2f, .2f), location.position.y + Random.Range(-.2f, .2f)));

        instance.transform.position = location;
        instance.SetText(text);
    }
}
