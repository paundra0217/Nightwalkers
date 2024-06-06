using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conjure : MonoBehaviour
{
    [SerializeField] ConjureSO[] ConjureWeapon;
    PlayerCombat playerCombat;

    private void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        if(playerCombat.IsConjure == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Shoot();
        }

    }

    public void Shoot()
    {

    }
}
