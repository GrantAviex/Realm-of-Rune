using UnityEngine;

public class Draggable : MonoBehaviour
{
	public bool fixX;
	public bool fixY;
	public Transform thumb;	
	bool dragging;
    Vector3 oldThumbPos;
    void Start()
    {
        oldThumbPos = thumb.localPosition;
    }
	void FixedUpdate()
	{
        if(oldThumbPos != thumb.localPosition)
        {
            SendMessage("OnDrag", Vector3.one - (thumb.position - GetComponent<Collider>().bounds.min) / GetComponent<Collider>().bounds.size.x);
        }
        oldThumbPos = thumb.localPosition;
	}

	void SetDragPoint(Vector3 point)
	{
		point = (Vector3.one - point) * GetComponent<Collider>().bounds.size.x + GetComponent<Collider>().bounds.min;
		//SetThumbPosition(point);
	}

	void SetThumbPosition(Vector3 point)
	{
		thumb.position = new Vector3(fixX ? thumb.position.x : point.x, fixY ? thumb.position.y : point.y, thumb.position.z);
	}
}
