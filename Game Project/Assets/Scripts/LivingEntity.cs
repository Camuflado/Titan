using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    //protected float health;   -------- Mudei de procted para public para conseguir usar no script HUD
    public float health { get; protected set; }
    protected bool dead;

    public event System.Action OnDeath; //Para contar mortes para proxima wave do spawn

    protected virtual void Start()
    {
        health = startingHealth; 
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        //Do some stuff here with hit var
        TakeDamage(damage);
        AudioManager.instance.PlaySound("Impact", transform.position); //Som de hit
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        if (OnDeath != null) //Para contar mortes para proxima wave do spawn
        {
            OnDeath();
            AudioManager.instance.PlaySound("Enemy Death", transform.position); //Som de kill
        }
        GameObject.Destroy(gameObject); //destruir o Objecto
    }
    
}
