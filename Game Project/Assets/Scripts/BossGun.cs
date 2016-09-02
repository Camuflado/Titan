using UnityEngine;
using System.Collections;

public class BossGun : MonoBehaviour
{
    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;

    public Transform[] projectileSpawn; //Onde Vai spawnar o tiro
    public FireBallProjectile projectile; //Adicionar o tipo de tiro (Neste caso prefab que tenha o script FireBallProjectile
    public float msBetweenShots = 100; //Delay
    public float muzzleVelocity; //Velocidade tiro?
    public int burstCount; //Quantidade de tiros

    bool triggerReleaseSinceLastShot;
    int shootRemainingInBurst;
    float nextShotTime;


    void Start()
    {
        shootRemainingInBurst = burstCount;
    }

    public void OnTriggerHold() //Quando carregarmos na tecla
    {
        Shoot();
        triggerReleaseSinceLastShot = false;
    }

    public void OntriggerRelease() //Quando largarmos a tecla
    {
        triggerReleaseSinceLastShot = true;
        shootRemainingInBurst = burstCount; //reset nos tiros
    }

    void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shootRemainingInBurst == 0) // Se não houver mais tiros, não dispara
                    return;
                shootRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleaseSinceLastShot)
                    return;
            }

            for (int i = 0; i < projectileSpawn.Length; i++) //Disparar nos vários pontos?
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                //Criar um novo Tiro, na posição do "projectileSpawn" sem rotação
                FireBallProjectile newFireBallProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as FireBallProjectile;
                newFireBallProjectile.SetSpeed(muzzleVelocity); //Colocar velocidade no tiro
            }
        }
    }
}
