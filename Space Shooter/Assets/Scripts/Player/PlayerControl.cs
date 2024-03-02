using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float rollMult;
    [SerializeField] private float pitchMult;

    private float xAxis, yAxis;
    private Vector3 pos;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }    

    private void Update()
    {
        GetInputs();
        Move();
    }

    private void GetInputs()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }
}
