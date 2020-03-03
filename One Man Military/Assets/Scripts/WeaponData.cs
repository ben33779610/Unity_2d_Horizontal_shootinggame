using UnityEngine;

    [CreateAssetMenu(fileName = "武器資料",menuName = "Data/Weapon")]
public class WeaponData : ScriptableObject
{

    [Header("傷害")]
    public float damage;
    [Header("射速")]
    public float shootspeed;
    [Header("冷卻時間")]
    public float cdtime;


}
