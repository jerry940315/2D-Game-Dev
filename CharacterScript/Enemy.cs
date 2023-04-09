using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    Rigidbody2D rb;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private PlayerHealth playerHealth;

    public float health;
    public float moveSpeed;
    public float damage;
    public float radius;
    public float flashTime;
    public float collisionOffset = 0.005f;

    bool canMove = true;

    protected Vector3 playerPos;
    private Transform playerTransform;

    public float Health{
        get{
            return health;
        }
        set{
            health = value;
            if(health <= 0){
                health = 0;
                Defeated();
            }
            else{
                Damaged();
            }
        }
    }

    public void Start(){
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void FixedUpdate(){
        if(canMove){
            playerPos = new Vector2(playerTransform.position.x, playerTransform.position.y - 0.12f);
            float distance = (playerPos - transform.position).sqrMagnitude;

            if(distance < radius && distance != 0){
                bool success = TryMove(playerPos - transform.position);

                if(!success){
                    success = TryMove(new Vector2((playerPos - transform.position).x, 0));

                    if(!success){
                        success = TryMove(new Vector2(0,(playerPos - transform.position).y));
                    }
                }
                
                animator.SetBool("IsMoving", success);
            }
            else{
                animator.SetBool("IsMoving", false);
            }
        }
    }

    private bool TryMove(Vector2 direction){
        if(direction != Vector2.zero){
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);
            
            direction.Normalize();

            if(count == 0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else{
                return false;
            }
        }
        else{
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D"){
            playerHealth.Health -= damage;
        }
    }

    void FlashColor(float time){
        spriteRenderer.color = Color.red;
        Invoke("ResetColor",time);
    }

    void ResetColor(){
        spriteRenderer.color = originalColor;
    }

    public void Damaged (){
        animator.SetTrigger("Damaged");
        FlashColor(flashTime);
    }

    public void Defeated (){
        animator.SetTrigger("Defeated");
        FlashColor(0.07f);
    }

    void LockMovement(){
        canMove = false;
    }

    public void RemoveEnemy (){
        Destroy(gameObject);
    }

}