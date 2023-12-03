using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Sprite;
// using WeaponTypeEnum;

[CreateAssetMenu]
[SerializeField]
public class SaveData : ScriptableObject
{
    public string playerName;
    public string selectedLocale;
    public int hp;
    public int mp;

    // public Vector3 PlayerPosition;
    public float posX;
    public float posY;
    public float posZ;

		// public Transform PlayerTransform;
    public float directionX;
    public float directionZ;

		public Weapon equippedWeapon;

    public int attackPower;
    public int defensePower;
}