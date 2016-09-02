using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BossOgre))]
public class BossGunController : MonoBehaviour
{
    BossOgre bossOgre;
    public Transform weaponBossHold; //Onde está a arma
    public BossGun[] allBossGun;
    BossGun equippedBossGun;

    void Start()
    {
        bossOgre = GetComponent<BossOgre>();
        if (allBossGun[0] != null)
        {
            EquipBossGun(allBossGun[0]);
        }
    }

    public void EquipBossGun(BossGun bossGunToEquip)
    {
        if (equippedBossGun != null)
        {
            Destroy(equippedBossGun.gameObject);
        }
        equippedBossGun = Instantiate(bossGunToEquip, weaponBossHold.position, weaponBossHold.rotation) as BossGun;
        equippedBossGun.transform.parent = weaponBossHold;
    }

    public void EquipBossGun(int bossWeaponIndex)
    {
        EquipBossGun(allBossGun[bossWeaponIndex]);
    }

    public void OnTriggerHold() //Shoot
    {
        if (equippedBossGun != null) //Se tiver weapon equipada
        {
            StartCoroutine(Attack2Animation()); //Animação Attack_01 
            //equippedBossGun.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedBossGun != null) //Se tiver weapon equipada
        {
            equippedBossGun.OntriggerRelease();
        }
    }


    IEnumerator Attack2Animation()
    {
        bossOgre.pathfinder.speed = 0;
        GetComponent<Animation>().Play("Attack_02");
        yield return new WaitForSeconds(GetComponent<Animation>()["Attack_02"].length);
        equippedBossGun.OnTriggerHold();
        GetComponent<Animation>().Play("Walk");
        bossOgre.pathfinder.speed = 3;
    
    }
}
