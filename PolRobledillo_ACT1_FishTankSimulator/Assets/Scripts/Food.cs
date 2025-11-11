using UnityEngine;

public class Food : MonoBehaviour
{
    public bool floorArrived = false;
    void Start()
    {
        ObjectsManager.instance.RegisterFood(this);
    }

    void Update()
    {
        if (!floorArrived)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f) && hit.transform.gameObject.tag == "Obstacle")
            {
                floorArrived = true;
            }
            else
            {
                transform.position += Vector3.down * Time.deltaTime;
            }
        }
    }
}
