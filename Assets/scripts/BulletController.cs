using System;
using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet info")]
    [SerializeField] private float activeTime;

    private float shootTime;
    private int damage;

    public int Damage { get => damage; set => damage = value; }

    private void OnEnable()
    {
        StartCoroutine(DeactiveAftertime());
    }

    private IEnumerator DeactiveAftertime()
    {
        yield return new WaitForSeconds(activeTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);


    }
}
