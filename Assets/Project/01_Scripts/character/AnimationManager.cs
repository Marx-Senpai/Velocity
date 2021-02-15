using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.Animations;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    
    private float timer;
    [SerializeField] private float timerLimitMin;
    [SerializeField] private float timerLimitMax;
    private float timerAdd = 1f;
    private float timerLimit;

    private bool inputPressed = false;
    private float speedMultiplier = 1f;

    private CharacterInputDetector player_inputDetector;
    private CustomCharacterController player_controller;
    
    void Start()
    {
        animator = this.GetComponent<Animator>();
        player_inputDetector = GameObject.Find("Player").GetComponent<CharacterInputDetector>();
        player_controller = GameObject.Find("Player").GetComponent<CustomCharacterController>();
        timerLimit = Random.Range(timerLimitMin, timerLimitMax);
    }

    void Update()
    {
        timer += timerAdd * Time.deltaTime;
        
        // ----------------------------------------------------------------------------------------------------
        // Variations de l'idle
        if (timer > timerLimit && this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetInteger("changeIdle", Random.Range(0, 100));
            timer = 0;
        }
        
        // ----------------------------------------------------------------------------------------------------
        // Animations lorsque le joueur se déplace
        
        if (player_inputDetector.axisXGauche != 0 || player_inputDetector.axisYGauche != 0)
        {
            animator.SetBool("isWalking", true);
            
            if (Mathf.Abs(player_inputDetector.axisXGauche) > Mathf.Abs(player_inputDetector.axisYGauche))
            {
                speedMultiplier = Mathf.Abs(player_inputDetector.axisXGauche); 
            }
            else
            {
                speedMultiplier = Mathf.Abs(player_inputDetector.axisYGauche); 
            }
            
            animator.SetFloat("walkSpeedMultiplier", speedMultiplier*2);
        }
        else if (player_inputDetector.axisXGauche == 0 && player_inputDetector.axisYGauche == 0)
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void PlayDeathAnimation()
    {
        animator.SetBool("isDying", true);
    }
    
    private bool AnimatorIsPlaying(){
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }
    
    private bool AnimatorIsPlaying(string stateName){
        return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}