using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 3;
    Vector2 rightAttackOffset;
    public Collider2D swordCollider;

    public void AttackRight(){
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(0.1f, -0.095f);
    }

    public void AttackLeft(){
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(-0.1f, -0.095f);
    }

    public void AttackFront(){
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(0f, -0.13f);
    }

    public void AttackBack(){
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(0f, -0.05f);
    }

    public void StopAttack(){
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy" && other != null){
            other.GetComponent<Enemy>().Health -= damage;
        }
    }

}