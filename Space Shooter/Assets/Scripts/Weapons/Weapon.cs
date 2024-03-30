using UnityEngine;

namespace SpaceShooter.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Weapon definition:")]
        [SerializeField] protected GameObject playerProjectilePrefab;
        [SerializeField] protected GameObject enemyProjectilePrefab;
        [SerializeField] protected float _defDamage;
        [SerializeField] protected float _currentDamage;
        [SerializeField] protected float _projectileSpeed;
        [SerializeField] protected float _delayBetweenShots;
        [SerializeField] internal WeaponType type;
        [SerializeField] protected bool isPlayer;
        protected float _lastShootTime;
        internal float defDamage { get => _defDamage; }
        internal float lastShootTime { get => _lastShootTime; set => _lastShootTime = value; }
        internal float delayBetweenShots { get => _delayBetweenShots; set => _delayBetweenShots = value;}
        internal float projectileSpeed { get => _projectileSpeed; set => _projectileSpeed = value; }
        internal float currentDamage { get => _currentDamage; set => _currentDamage = value; }

        // Audio
        protected AudioSource audioSource => GetComponent<AudioSource>();
        [SerializeField] protected AudioClip shotSound;
        protected Vector2 pitchRange;
        
        [Header("Weapon parameters for WeaponPowerUp:")]
        [SerializeField] protected Color _color;
        [SerializeField] protected string _letter;
        internal Color color { get => _color; }
        internal string letter { get => _letter; }

        protected Transform projectileAnchor;
        protected Transform parent;
        
        protected WeaponControl weaponControl;
        protected GameObject currentProjectile;

        protected void Awake()
        {
            weaponControl = GetComponentInParent<WeaponControl>();
            parent = this.gameObject.transform.root;
            projectileAnchor = GameObject.Find("PROJECTILE ANCHOR").transform;
            if (parent.tag == "Player") 
            {
                isPlayer = true;
                currentProjectile = playerProjectilePrefab;
            }
            else 
            {
                isPlayer = false;
                currentProjectile = enemyProjectilePrefab;
            }
        }
        
        protected virtual void Start()
        {
            weaponControl.OnWeaponShoot += TempFire;
        }
        
        protected virtual void OnDisable()
        {
            weaponControl.OnWeaponShoot -= TempFire;
        }

        protected abstract void TempFire();
        protected abstract GameObject Shoot();
    }
}

