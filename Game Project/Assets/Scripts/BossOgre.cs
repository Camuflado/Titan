using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BossGunController))]
public class BossOgre : LivingEntity
{
    BossGunController bossGunController;

    //public static event System.Action OnHitStatic; //Para chamar no HUD

    public enum BossActionType
    {
        Idle,
        Moving,
        Attacking
    }
    BossActionType currentState;

    public NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity; //Player Entity

    float attackDistanceThreshold = 2f;
    float timeBetweenAttacks = 1;
    float damage = 1;
    float nextAttackTime;

    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

    protected override void Start()
    {
        bossGunController = GetComponent<BossGunController>();
        base.Start(); //Start do LivingEntity correr
        pathfinder = GetComponent<NavMeshAgent>();

        //skinMaterial = GetComponent<Renderer>().material;
        //originalColour = skinMaterial.color;

        if (GameObject.FindGameObjectWithTag("Player") != null) //Se o player existir
        {
            currentState = BossActionType.Moving;
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform; //Ir atrás do Player Tag ///// Criar um novo para ir para sitios especificos?
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius; //radius do enemy
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius; //radius do player

            StartCoroutine(UpdatePath());
        }
    }

    void OnTargetDeath() //QUando Player Morrer
    {
        hasTarget = false;
        currentState = BossActionType.Idle;
        GetComponent<Animation>().Play("Idle_02");
    }

    public void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime) //Cooldown attacks
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude; //distancia

                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) //Se estiver no range
                {
                    StartCoroutine(Attack1Animation()); //Animação Attack_01 
                                     
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    AudioManager.instance.PlaySound("Enemy Attack", transform.position); //Som de hit
                    StartCoroutine(Attack());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(RandomAttack());
            //bossGunController.OnTriggerHold();
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            bossGunController.OnTriggerRelease();
        }

    }

    IEnumerator Attack1Animation()
    {
        GetComponent<Animation>().Play("Attack_01");
        yield return new WaitForSeconds(GetComponent<Animation>()["Attack_01"].length);
        if (hasTarget == true)
            GetComponent<Animation>().Play("Walk");
    }

    IEnumerator Attack()
    {
        currentState = BossActionType.Attacking;
        pathfinder.enabled = false; //desactivar andar

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);
        //Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius); // O ataque não entra dentro do player

        float attackSpeed = 3; //quanto mais alto mais rápido vai ser a animação
        float percent = 0; //saber a percentagem da animação

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
                /* *** PARA HUD ***
                 if (OnHitStatic != null)
                    OnHitStatic();
                    */

            }
            percent += Time.deltaTime * attackSpeed;
            //y = 4(-x^2+x)
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null;
        }

        currentState = BossActionType.Moving;
        pathfinder.enabled = true; //activar andar

    }

    IEnumerator UpdatePath() //Para melhor performance
    {
        float refreshRate = 0.25f; //Refresh 0.25 segundos

        while (hasTarget)
        {
            if (currentState == BossActionType.Moving)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }


    // RANDOM ATTACK
    IEnumerator RandomAttack()
    {
        int randomAttack;
        randomAttack = Random.Range(1,3);
        if (randomAttack == 1) //Attack 1
        {
            print("Random: 1");
            StartCoroutine(Attack1Animation());
        }
        else if (randomAttack == 2) //Attack2
        {
            bossGunController.OnTriggerHold();
            print("Random: 2");
        }
        yield return null;
    }

}
