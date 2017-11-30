using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * 
 */
[Serializable]
public class WeaponDefinition
{
    //Unique ID to reference projectile internally
    public string id;

    //Display name for projectile
    public string name;

    //The icon used to represent the projectile on the HUD
    public Sprite weaponIcon;

    //The projectile prefab to be fired
    public Rigidbody projectilePrefab;

    //The colour of the HUD overlay when this projectile is active
    public Color weaponColor;

    //Sound pool to play from when this projectile is fired
    public AudioClip[] fireSound;
}


public class HeroWeaponLibrary : PersistentSingleton<HeroWeaponLibrary>
{
    /**
     * 
     */
    [SerializeField]
    private WeaponDefinition[] weaponDefinitions;

    /**
     * @return
     */
    protected void Awake()
    {
        // TODO implement here
    }

    /**
     * @param index 
     * @return
     */
    public WeaponDefinition GetProjectileDataForIndex(int index)
    {
        if ((index < 0) || ((index + 1) > weaponDefinitions.Length))
        {
            Debug.Log("<color=red>WARNING: 정의된무기가 없습니다 or 정의된 무기의 인덱스를 초과하였습니다.</color>");
        }
        return weaponDefinitions[index];
    }

    /**
     * @param index 
     * @return
     */
    //public AudioClip GetFireSoundForIndex(int index)
    //{
    //    // TODO implement here
    //    return null;
    //}

    /**
     * @return
     */
    public int GetNumberOfDefinitions()
    {
        return weaponDefinitions.Length;
    }

}