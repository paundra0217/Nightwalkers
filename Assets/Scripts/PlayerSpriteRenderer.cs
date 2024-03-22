/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDCT;

public class PlayerSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerController movement;

    public AnimatedSprite idle;
    public AnimatedSprite jump;
    public AnimatedSprite run;

    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerController>();
    }

    private void OnEnable(){
        spriteRenderer.enabled = true;
    }

    private void OnDisable(){
        spriteRenderer.enabled = false;
        run.enabled = false;
    }

    private void LateUpdate(){
        run.enabled = movement.running;
        if(movement.jumping){
            jump.enabled = true;
            idle.enabled = false;
            run.enabled = false;
        }
    }
*/
