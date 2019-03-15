using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager
{
    const float BLEND_RUN = 1;
    const float BLEND_IDLE = 0.5f;
    const int LEFT = 1;
    const int RIGHT = -1;

    private static AnimationManager instance = null;

    private AnimationManager()
    {

    }

    public static AnimationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AnimationManager();
            }
            return instance;
        }
    }

    public void Update(AnimationDatas datas)
    {
        Vector3 position = datas.stateMachine.gameObject.transform.position;
        datas.blood.gameObject.transform.position = position;

        if (datas.pkg.A) //Actions Button A
        {
            datas.stateMachine.SetTrigger("Punch");
        }

         if (datas.pkg.B && datas.energy > 0) //Actions Button B
        {
            datas.stateMachine.SetTrigger("Kick");
        }

        if (datas.pkg.Y && !datas.stun && datas.energy > 0 ) //Actions Button Y
        {
           
              datas.stateMachine.SetTrigger("Stun");
        }

        if ((datas.pkg.RB && !datas.isCharging && (datas.energy > datas.skillEnergyCost || datas.gotItemSpecial)) && datas.canSkill) //Actions Button RB holded
        {
              datas.stateMachine.SetTrigger("ChargeSkill");
        }


        if (datas.pkg.X) //Actions Button X
        {
            if (datas.nbJump < Character.MAX_NB_JUMP)
            {
                datas.stateMachine.SetTrigger("Jump");
            }
        }
        
            if (datas.pkg.LeftStick.x > 0) //left
            {
                if (!datas.sr.flipX)
                    datas.sr.flipX = true;
                if (datas.stateMachine.GetFloat("Blend") < BLEND_IDLE)
                    datas.stateMachine.SetFloat("Blend", BLEND_RUN);
            }
            else if (datas.pkg.LeftStick.x < 0) //right
            {
                if (datas.sr.flipX)
                    datas.sr.flipX = false;
                if (datas.stateMachine.GetFloat("Blend") < BLEND_IDLE)
                    datas.stateMachine.SetFloat("Blend", BLEND_RUN);
            }
            else
            {
                if (datas.stateMachine.GetFloat("Blend") > BLEND_IDLE)
                    datas.stateMachine.SetFloat("Blend", 0);
            }
        
    }

    public void LaunchUseSkill(Animator animator)
    {
        animator.SetTrigger("UseSkill");
    }

    public void LaunchHurtAnimation(Animator animator)
    {
        animator.SetTrigger("GetHurt");
    }

    public void LaunchStunAnimation(Animator animator)
    {
         animator.SetTrigger("GetStun");
    }

    public void LaunchKnockbackAnimation(Animator animator, int direction)
    {
        
        if (direction == LEFT)
            animator.SetTrigger("KnockBackLeft");
        else if (direction == RIGHT)
            animator.SetTrigger("KnockBackRight");


    }

    public void ResetStateMachine(Animator animator)
    {
        animator.Rebind();
    }

    public void QuitStunAnimation(Animator animator)
    {
        animator.SetTrigger("NotStunAnymore");
    }

    public class AnimationDatas
    {
        public InputManager.InputPkg pkg;

        public Animator stateMachine;
        public Animator blood;

        public SpriteRenderer sr;

        public FactorySkill skillType;

        public bool isCharging;
        public bool gotItemSpecial;
        public bool stun;

        public float energy;
        public float skillEnergyCost;
        public float skillOverpower;
        public float nbJump;
        public bool canJump;
        public bool canSkill;

        public AnimationDatas(InputManager.InputPkg _pkg, Animator _stateMachine, SpriteRenderer _sr, bool _isCharging, Animator _blood, FactorySkill _skillType, float _energy, float _skillEnergyCost, bool _gotItemSpecial, bool _stun, float _skillOverpower, float _nbJump, bool _canJump,bool canSkill)
        {
            pkg = _pkg;
            stateMachine = _stateMachine;
            sr = _sr;
            isCharging = _isCharging;
            blood = _blood;
            skillType = _skillType;
            energy = _energy;
            skillEnergyCost = _skillEnergyCost;
            gotItemSpecial = _gotItemSpecial;
            stun = _stun;
            skillOverpower = _skillOverpower;
            nbJump = _nbJump;
            canJump = _canJump;
            this.canSkill = canSkill;
        }

    }
}
