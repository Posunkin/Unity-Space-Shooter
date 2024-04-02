using System;
using System.Collections;
using UnityEngine;

namespace SpaceShooter.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        public Action<Enemy, bool> OnDeath;
        [Header("Enemy stats:")]
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float speed;
        [SerializeField] protected int _score;
        [SerializeField] protected float _chanceToSpawnPowerUp;
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float Speed {get => speed; set => speed = value;}
        public float ChanceToSpawnPowerUp {get => _chanceToSpawnPowerUp; set => _chanceToSpawnPowerUp = value; }
        protected float maxHealth;

        [Header("Effects:")]
        [SerializeField] protected GameObject explosionEffect;
        [SerializeField] protected ParticleSystem smokeEffect;

        protected float damageOnTrigger = 10;
        protected Vector3 pos {get => this.transform.position; set => this.transform.position = value; }
        public int score { get => _score; }
        public float chanceToSpawnPowerUp  { get => _chanceToSpawnPowerUp; }

        protected Vector3 tempPos;
        protected BoundsCheck bndCheck;

        // Realization of changing color on damage
        protected Color[] originalColors;
        protected Material[] materials;

        protected virtual void Awake()
        {
            bndCheck = GetComponent<BoundsCheck>();
            GetAllMaterials();
            maxHealth = currentHealth;
        }

        protected virtual void Move()
        {
            tempPos = pos;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }

        public virtual void Init()
        {
            currentHealth = maxHealth;
        }

        protected virtual void GetAllMaterials()
        {
            materials = Utils.GetAllMaterials(gameObject);
            originalColors = new Color[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                originalColors[i] = materials[i].color;
            }
        }

        protected virtual void Death(bool fromPlayer)
        {
            OnDeath?.Invoke(this, fromPlayer);
            if (bndCheck.isOnScreen)
            {
                GameObject go = Instantiate(explosionEffect);
                go.transform.position = this.transform.position;
            }
            Destroy(this.gameObject);
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            Transform root = other.gameObject.transform.root;
            GameObject go = root.gameObject;
            
            if (go.GetComponent<PlayerStats>() != null && !go.GetComponent<PlayerStats>().isInvulnerable)
            {
                TakeDamage();
            }
            else
            {
                Debug.Log("Triggered with " + other.gameObject.name);
            }
        }

        public virtual void TakeDamage()
        {
            currentHealth -= damageOnTrigger;
            StartCoroutine(nameof(ShowDamage));
            if (currentHealth <= 0)
            {
                GameManager.Instance.UpdateScore(score);
                Death(false);
            }
            else if (currentHealth <= maxHealth / 2)
            {
                smokeEffect.Play();
            }
        }
        
        public virtual void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (gameObject.activeSelf) StartCoroutine(nameof(ShowDamage));
            if (currentHealth <= 0)
            {
                GameManager.Instance.UpdateScore(score);
                Death(true);
            }
            else if (currentHealth <= maxHealth / 2)
            {
                smokeEffect.Play();
            }
        }

        protected virtual IEnumerator ShowDamage()
        {
            foreach (Material m in materials)
            {
                m.color = Color.red;
            }
            yield return new WaitForSeconds(0.15f);
            ReturnColors();
        }

        protected void ReturnColors()
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = originalColors[i];
            }
        }
    }
}
