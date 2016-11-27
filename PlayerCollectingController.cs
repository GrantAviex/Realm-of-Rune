using UnityEngine;
using System.Collections;

public class PlayerCollectingController : MonoBehaviour 
{
    
    public CollectionTool tool;

    Animator m_Animator;
    GameObject Target;

    PlayerData data;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void SetTool(CollectionTool newTool)
    {
        tool = newTool;
    }

    public void Collect(ResourceObject resource)
    {
        if (tool == null || resource.Type != tool.type)
        {
            Debug.Log("Tool is null or don't have correct tool for collection");
            return;
        }
        if(resource != null)
        {
            switch (tool.type)
            {
                case (int)Tools.FishingRod:
                    {
                        Fish();
                        break;
                    }
                case (int)Tools.Axe:
                    {
                        Chop();
                        break;
                    }
                case (int)Tools.Pickaxe:
                    {
                        Mine();
                        break;
                    }
                case (int)Tools.ButchersKnife:
                    {
                        Skin();
                        break;
                    }
                case (int)Tools.Bolas:
                    {
                        Capture();
                        break;
                    }
            }
        }
    }
    public void Fish()
    {

    }
    public void Mine()
    {

    }
    public void Skin()
    {

    }
    public void Chop()
    {

    }
    public void Capture()
    {

    }

    public void SpawnBola()
    {
        GameObject projectile = Resources.Load<GameObject>("Tools/Bolas/" + tool.name + "Projectile");
        GameObject liveProjectile = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        Rigidbody arrowBody = liveProjectile.GetComponent<Rigidbody>();
        Vector3 fireDir;
        if (Target != null)
        {
            float dist = (Target.transform.position - transform.parent.position).magnitude;
            fireDir = ((Target.transform.position - transform.parent.position) + (Vector3.up * (dist / 2.75f))).normalized;
        }
        else
        {
            fireDir = (Camera.main.transform.forward + (Vector3.up * .75f)).normalized;
        }
        arrowBody.AddForce(fireDir * arrowBody.mass * 300);
        liveProjectile.GetComponent<Projectile>().lifeTime = 2.0f;
        liveProjectile.GetComponentInChildren<Bola>().tier = tool.tier;
    }
}
