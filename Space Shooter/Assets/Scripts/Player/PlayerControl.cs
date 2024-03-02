using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float rollMult;
    [SerializeField] private float pitchMult;

    private float _xAxis, _yAxis;
    private Vector3 _pos;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }    

    private void Update()
    {
        GetInputs();
        Move();
    }

    private void GetInputs()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _yAxis = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        _pos = transform.position;
        _pos.x += _xAxis * speed * Time.deltaTime;
        _pos.y += _yAxis * speed * Time.deltaTime;
        transform.position = _pos;

        transform.rotation = Quaternion.Euler(_yAxis * pitchMult, _xAxis * rollMult, 0);
    }
}
