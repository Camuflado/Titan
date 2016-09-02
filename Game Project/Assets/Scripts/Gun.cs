using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;

    public Transform[] projectileSpawn; // posição da gun
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVelocity;
    public int burstCount;

    public AudioClip shootAudio;
    //public AudioClip reloadAudio;

    MuzzleFlash muzzleflash;

    bool triggerReleaseSinceLastShot;
    int shootRemainingInBurst;

    void Start()
    {
        muzzleflash = GetComponent<MuzzleFlash>();
        shootRemainingInBurst = burstCount;
    }

    float nextShotTime;

    void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            if(fireMode == FireMode.Burst) //Ver se o Burst ainda tem tiros
            {
                if (shootRemainingInBurst == 0)
                {
                    return;
                }
                shootRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single) //Single Shoot
            {
                if (!triggerReleaseSinceLastShot)
                {
                    return;
                }
            }

            for (int i = 0; i < projectileSpawn.Length; i++)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                newProjectile.SetSpeed(muzzleVelocity);
            }
            muzzleflash.Activate(); //Activar o efeito de tiro

            //Shoot Sound
            AudioManager.instance.PlaySound(shootAudio, transform.position);
        }
    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleaseSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        triggerReleaseSinceLastShot = true;
        shootRemainingInBurst = burstCount;
    }

}
