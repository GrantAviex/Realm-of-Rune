using UnityEngine;
using UnityEngine.UI;

public class ColorIndicator : MonoBehaviour {

	HSBColor color;

	void Start() 
    {
        color = HSBColor.FromColor(GetComponent<RawImage>().material.GetColor("_Color"));
        transform.parent.BroadcastMessage("SetColor", color);
	}

	void ApplyColor ()
	{
        GetComponent<RawImage>().material.SetColor ("_Color", color.ToColor());
        transform.parent.BroadcastMessage("OnColorChange", color, SendMessageOptions.DontRequireReceiver);
	}

	void SetHue(float hue)
    {
		color.h = 1-hue;
		ApplyColor();
    }	

	void SetSaturationBrightness(Vector2 sb)
    {
		color.s = 1-sb.x;
		color.b = 1-sb.y;
		ApplyColor();
	}
}
