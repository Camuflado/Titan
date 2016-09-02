using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State {Idle, Chasing, Attacking};
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity; //Player Entity

    Material skinMaterial;
    Color originalColour;

    float attackDistanceThreshold = 0.5f;
    float timeBetweenAttacks = 1;
    float damage = 1; 
    float nextAttackTime;

    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

	protected override void Start () //Assim para o Start do LivingEntity correr tambem
    {
        base.Start(); //Start do LivingEntity correr
        pathfinder = GetComponent<NavMeshAgent>();

        skinMaterial = GetComponent<Renderer>().material;
        originalColour = skinMaterial.color;

        if(GameObject.FindGameObjectWithTag("Player") != null) //Se o player existir
        {
            currentState = State.Chasing;
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform; //Ir atrás do Player Tag ///// Criar um novo para ir para sitios especificos?
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius; //radius do enemy
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius; //radius do player

            StartCoroutine(UpdatePath());
        }
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }
	
	void Update ()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime) //Cooldown attacks
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude; //distancia

                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) //Se estiver no range
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    AudioManager.instance.PlaySound("Enemy Attack", transform.position); //Som de hit
                    StartCoroutine(Attack());
                }
            }
        }
	}

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false; //desactivar andar

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);
        //Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius); // O ataque não entra dentro do player

        float attackSpeed = 3; //quanto mais alto mais rápido vai ser a animação
        float percent = 0; //saber a percentagem da animação

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            //y = 4(-x^2+x)
            float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null;
        }

        skinMaterial.color = originalColour;
        currentState = State.Chasing;
        pathfinder.enabled = true; //activar andar

    }

    IEnumerator UpdatePath() //Para melhor performance
    {
        float refreshRate = 0.25f; //Refresh 0.25 segundos

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    //Dentro do public override void TakeHit
    // AudioManager.instance.PlaySound("Impact", transform.position); //Som de hit
    // AudioManager.instance.PlaySound("Enemy Death", transform.position); //Som de kill

    //Som 2D, tipo Level Completo
    // AudioManager.instance.PlaySound2D("Level Complete"); //Som de kill
}
