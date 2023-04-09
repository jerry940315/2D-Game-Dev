using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Animator animator;
    public Collider2D playerCollider;
    PlayerController playerController;
    SpriteRenderer spriteRenderer;

    public int health;
    public int flashingTimes;
    public float flashingSeconds;
    private float lastTime = -1f;

    public float Health{
        get{
            return health;
        }
        set{
            if(Time.time > lastTime + 1f){
                lastTime = Time.time;
                health = (int)value;
                HealthBar.currentHealth = health;
                if(health <= 0){
                    health = 0;
                    Died();
                }
                else{
                    Damaged();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.maxHealth = health;
        HealthBar.currentHealth = health;
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Died (){
        animator.SetTrigger("PlayerDied");
        playerController.LockMovement();
    }

    public void Damaged (){
        PlayerFlashing(flashingTimes, flashingSeconds);
    }

    void PlayerFlashing(int times, float seconds){
        StartCoroutine(DoFlash(times, seconds));
    }

    IEnumerator DoFlash(int times, float seconds){
        for(int i = 0; i < times * 2; i++){
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(seconds);
        }
        spriteRenderer.enabled = true;
    }
}
