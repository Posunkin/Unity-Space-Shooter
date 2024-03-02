using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [SerializeField] private float offSet;
    private float _radius = 1;
    private float _camWidth, _camHeight;
    private Vector3 _pos;

    private void Awake()
    {
        _camHeight = Camera.main.orthographicSize - offSet;
        _camWidth = (_camHeight + offSet) * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        _pos = transform.position;
        if (_pos.x > _camWidth - _radius) 
        {
            _pos.x = _camWidth - _radius;
        }
        if (_pos.x < -_camWidth + _radius) 
        {
            _pos.x = -_camWidth + _radius;
        }
        if (_pos.y > _camHeight - _radius) 
        {
            _pos.y = _camHeight - _radius;
        }
        if (_pos.y < -_camHeight + _radius) 
        {
            _pos.y = -_camHeight + _radius;
        }
        transform.position = _pos;
    }

    private void OnDrawGizmos()
    {
        Vector3 boundSize = new Vector3(_camWidth * 2, _camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
