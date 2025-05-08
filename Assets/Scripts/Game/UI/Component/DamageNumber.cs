using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text damageTxt;
    [SerializeField] private Vector2 randomXSpeed;
    [SerializeField] private Vector2 randomYSpeed;
    [SerializeField] private float fallAcceleration;
    private float xSpeed;
    private float ySpeed;

    public void Init(int damage)
    {
        if(damage < 0)
        {
            damage = -damage;
            damageTxt.color = Color.red;
        }
        else
        {
            damageTxt.color = Color.green;
        }
        damageTxt.text = damage.ToString();
        transform.localScale = Vector3.zero;
        xSpeed = Random.Range(randomXSpeed.x, randomXSpeed.y);
        ySpeed = Random.Range(randomYSpeed.x, randomYSpeed.y);
        StartCoroutine(AnimCoroutine());
    }

    private IEnumerator AnimCoroutine()
    {
        float time = 4;
        while (time > 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.1f);
            transform.Translate(new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime);
            ySpeed -= fallAcceleration * Time.deltaTime;
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

}
