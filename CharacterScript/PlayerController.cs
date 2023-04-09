using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.005f;

    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack; 

    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;
    bool IsFaceFront = true;
    bool IsFaceSides = false;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate(){
        if(canMove){
            if(movementInput != Vector2.zero){
                bool success = TryMove(movementInput);

                if(!success){
                    success = TryMove(new Vector2(movementInput.x,0));

                    if(!success){
                        success = TryMove(new Vector2(0,movementInput.y));
                    }
                }
                
                animator.SetBool("IsMoving", success);
            }
            else{
                animator.SetBool("IsMoving", false);
            }

            if(movementInput.x < 0){
                spriteRenderer.flipX = true;
                animator.SetBool("FaceSides",true);
                IsFaceSides = true;
            }
            else if(movementInput.x > 0){
                spriteRenderer.flipX = false;
                animator.SetBool("FaceSides",true);
                IsFaceSides = true;
            }
            else if(movementInput.x == 0){
                animator.SetBool("FaceSides",false);
                if(movementInput.y < 0){
                    animator.SetBool("FaceFront",true);
                    IsFaceFront = true;
                    IsFaceSides = false;
                }
                else if(movementInput.y > 0){
                    animator.SetBool("FaceFront",false);
                    IsFaceFront = false;
                    IsFaceSides = false;
                }
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

    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire(){
        animator.SetTrigger("SwordAttack");
    }

    void SwordAttack(){
        LockMovement();
        if(IsFaceSides == true){
            if(spriteRenderer.flipX == true){
                swordAttack.AttackLeft();
            }
            else{
                swordAttack.AttackRight();
            }
        }
        else if(IsFaceFront == true){
            swordAttack.AttackFront();
        }
        else{
            swordAttack.AttackBack();
        }
    }

    void EndSwordAttack(){
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement(){
        canMove = false;
    }

    void UnlockMovement(){
        canMove = true;
    }

}