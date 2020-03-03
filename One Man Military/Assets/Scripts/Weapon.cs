using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("武器資料")]
    public WeaponData data;
    [Header("參考位置")]
    public Transform reftrans;
    [Header("射擊位置")]
    public Transform shoottrans;

    public GameObject bullet;

    public void shot()
    {
        GameObject temp =  Instantiate(bullet, shoottrans);
        temp.GetComponent<Rigidbody2D>().AddForce(transform.forward);

    }
}
