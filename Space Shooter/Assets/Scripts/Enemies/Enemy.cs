using System;
using System.Collections;
using UnityEngine;

namespace SpaceShooter.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        public Action<Enemy> OnDeath;
        [SerializeField] protected float health;
        [SerializeField] protected float speed;
        [SerializeField] protected int _score;
        [SerializeField] protected float _chanceToSpawnPowerUp;
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
        }

        protected virtual void Move()
        {
            tempPos = pos;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
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

        protected virtual void Death()
        {
            OnDeath?.Invoke(this);
            Destroy(this.gameObject);
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            Transform root = other.gameObject.transform.root;
            GameObject go = root.gameObject;
            
            if (go.GetComponent<PlayerStats>() != null)
            {
                TakeDamage(damageOnTrigger);
            }
            else
            {
                Debug.Log("Triggered with " + other.gameObject.name);
            }
        }
        
        public virtual void TakeDamage(float damage)
        {
            health -= damage;
            StartCoroutine(nameof(ShowDamage));
            if (health <= 0)
            {
                GameManager.Instance.UpdateScore(score);
                Death();
            }
        }

        protected virtual IEnumerator ShowDamage()
        {
            foreach (Material m in materials)
            {
                m.color = Color.red;
            }
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = originalColors[i];
            }
        }
    }
}
