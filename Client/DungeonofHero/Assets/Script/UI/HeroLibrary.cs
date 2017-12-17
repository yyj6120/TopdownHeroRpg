
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ClassDefinition
{
    public string id;
    public string name;
    public string description;
    public Sprite symbol;
}

[Serializable]
public struct HeroTypeDefinition
{
    public ClassDefinition Classdefinition;
    //Unique stats to customize tank handling and fire rate
    public float speed;
    public float hp;
    public float mana;
    //The display prefab to be instantiated to represent this tank in the menu and in-game
    public GameObject displayPrefab;
    //How many stars are displayed per stat for this tank in the customization screen.
    public float exp;
    public int level;
}
/**
 * 
 */
public class HeroLibrary : PersistentSingleton<HeroLibrary>
{
    /**
     * 
     */
    [SerializeField]
    private HeroTypeDefinition[] heroDefinitions;

    /**
     * @return
     */
    protected override void Awake()
    {
        base.Awake();

        if (heroDefinitions.Length == 0)
        {
            Debug.Log("<color=red>WARNING: 영웅을 정의하셔야합니다. 영웅정보가 없습니다.!</color>");
        }
    }
    /**
     * @param index 
     * @return
     */
    public HeroTypeDefinition GetheroDataForIndex(int index)
    {
        if ((index < 0) || ((index + 1) > heroDefinitions.Length))
        {
            Debug.Log("<color=red>WARNING: 정의된인덱스가 없거나 or 정의된 영웅의 인덱스를 초과합니다.");
        }
        return heroDefinitions[index];
    }
    /**
     * @return
     */
    public int GetNumberOfDefinitions()
    {
        return heroDefinitions.Length;
    }

}