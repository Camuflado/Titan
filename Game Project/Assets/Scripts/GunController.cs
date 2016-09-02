using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    public Transform weaponHold;
    public Gun[] allGuns;
    Gun equippedGun;

    void Start()
    {
        if (allGuns[0] != null)
        {
            /* Guns
             *  0 - Pistol
             *  1 - MachineGun Burst
             *  2 - Shotgun
             *  3 - MachineGun 
             *  */
            EquipGun(allGuns[0]);
        }
    }

    public void EquipGun(Gun gunToEquip)
    {
        if (equippedGun != null) //destruir a gun antes de apanharmos uma nova
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }

    public void EquipGun(int weaponIndex)
    {
        EquipGun(allGuns[weaponIndex]);
    }

    public void OnTriggerHold() //Shoot
    {
        if (equippedGun != null) //Se tiver weapon equipada
        {
            equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null) //Se tiver weapon equipada
        {
            equippedGun.OnTriggerRelease();
        }
    }
}
