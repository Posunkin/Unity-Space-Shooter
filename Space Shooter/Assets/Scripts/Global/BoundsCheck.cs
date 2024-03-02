using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [SerializeField] private float offSet;
    [SerializeField] private bool keepOnScreen = true;
    [SerializeField] private float _radius;
    internal bool isOnScreen = true;
    internal bool offRight, offLeft, offUp, offDown;
    private float _camWidth, _camHeight;
    private Vector3 pos;

    internal float Radius { get => _radius; private set => _radius = value; }
    internal float CamWidth { get => _camWidth; private set => _camWidth = value; }
    internal float CamHeight { get => _camHeight; private set => _camHeight = value; }

    private void Awake()
    {
        CamHeight = Camera.main.orthographicSize - offSet;
        CamWidth = (CamHeight + offSet) * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if (pos.x > CamWidth - Radius) 
        {
            pos.x = CamWidth - Radius;
            offRight = true;
        }
        if (pos.x < -CamWidth + Radius) 
        {
            pos.x = -CamWidth + Radius;
            offLeft = true;
        }
        if (pos.y > CamHeight - Radius) 
        {
            pos.y = CamHeight - Radius;
            offUp = true;
        }
        if (pos.y < -CamHeight + Radius) 
        {
            pos.y = -CamHeight + Radius;
            offDown = true;
        }

        isOnScreen = !(offRight || offLeft || offDown || offUp);

        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
        }
        
    }

    private void OnDrawGizmos()
    {
        Vector3 boundSize = new Vector3(CamWidth * 2, CamHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
