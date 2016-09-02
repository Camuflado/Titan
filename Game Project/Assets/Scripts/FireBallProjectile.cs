using UnityEngine;
using System.Collections;

public class FireBallProjectile : MonoBehaviour
{

    public LayerMask collisionMask; //Layer do collision mask
    public Color trailColour;
    float speed = 10;
    float damage = 1;

    float lifetime = 3;
    float skinWidth = 0.1f; 

    void Start()
    {
        Destroy(gameObject, lifetime);

        //Se o projectile estiver dentro do inimigo, acertar nele
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0]);
        }

        GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColour); //Adicionar no Script Projectile no Unity a cor de escolha do trail
    }

    public void SetSpeed(float newSpeed) //Random speed
    {
        speed = newSpeed;
    }

    void Update ()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance); //Collision
        transform.Translate(Vector3.forward * Time.deltaTime * speed); //Tiro andar
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) //Se está a colidir com o objecto
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>(); //Damage
        if(damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }

        GameObject.Destroy(gameObject); //Destroy projectile
    }

    void OnHitObject(Collider c) //Se o projectile estiver dentro do inimigo, acertar nele 
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>(); //Damage
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }

        GameObject.Destroy(gameObject); //Destroy projectile
    }
}
