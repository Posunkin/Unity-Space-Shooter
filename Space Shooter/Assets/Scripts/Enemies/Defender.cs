using System;
using UnityEngine;

namespace SpaceShooter.Enemies
{
    public class Defender : Enemy
    {
        public Action onDefenderDeath;
        private EnemyWeaponControl weaponControl;

        private void Start()
        {
            weaponControl = GetComponent<EnemyWeaponControl>();
        }

        protected override void Death(bool fromPlayer)
        {
            onDefenderDeath?.Invoke();
            GameObject go = Instantiate(explosionEffect);
            go.transform.position = transform.position;
            gameObject.SetActive(false);
        }

        public void Alive(float sundiverHp)
        {
            currentHealth = sundiverHp * 0.1f;
            weaponControl.Init();
        }

        public void Init(float sundiverHp)
        {
            currentHealth = sundiverHp * 0.1f;
        }
    }
}

