using TMPro;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected GameObject obj;
    [SerializeField] protected TextMesh letter;
    [SerializeField] protected Vector2 rotMinMax = new Vector2(15, 90);
    [SerializeField] protected Vector2 driftMinMax = new Vector2(0.25f, 2);
    protected float lifeTime = 6f;
    protected float fadeTime = 4f;
    
    protected Vector3 rotPerSecod;
    protected float birthTime;

    protected Rigidbody rb;
    protected BoundsCheck bnd;
    protected Renderer rend;
    
    protected virtual void Awake()
    {
        // Initialize all required components
        rb = GetComponent<Rigidbody>();
        bnd = GetComponent<BoundsCheck>();
        rend = obj.GetComponent<Renderer>();
        // Get random speed
        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rb.velocity = vel;
        // Set rotation to [0, 0, 0]
        transform.rotation = Quaternion.identity;

        // Set the random rotation speed
        rotPerSecod = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
        Random.Range(rotMinMax.x, rotMinMax.y),
        Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
    }

    protected virtual void Update()
    {
        obj.transform.rotation = Quaternion.Euler(rotPerSecod * Time.time);
        // Time to fadeout
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        if (u >= 1) 
        {
            Destroy(this.gameObject);
        }
        // Color fadeout
        if (u > 0)
        {
            Color c = rend.material.color;
            c.a = 1f - u;
            rend.material.color = c;

            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bnd.isOnScreen)
        {
            Destroy(this.gameObject);
        }
    }

    public abstract void SetType(WeaponType type);
}
