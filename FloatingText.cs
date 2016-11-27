using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
    public Animator animator;
    private Text damageText;

    void OnEnable()
    {
        Destroy(gameObject, 2.0f);
        damageText = animator.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        damageText.text = text;
    }
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
