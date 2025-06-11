using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void FixedUpdate()
    {
        SnowTrailController.Instance.DrawSnowTrail(transform.position);
    }
}
