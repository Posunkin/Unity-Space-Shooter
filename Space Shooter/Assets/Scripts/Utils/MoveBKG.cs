using UnityEngine;

public class MoveBKG : MonoBehaviour
{
    private BoundsCheck bnd;
    [SerializeField] bool isLeft;

    private void Start()
    {
        bnd = GetComponent<BoundsCheck>();
        transform.position = isLeft ? new Vector3(-bnd.CamWidth, 0, -10) : new Vector3(bnd.CamWidth, 0, -10);
    }    
}
