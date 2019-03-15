using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterName { ElectroidTheCrusher, BurningBruiser, SubAbsoluteZero, DoctorScorpio }
public class Character
{
    #region Const
    const float NORMAL_VELOCITY_MOVEMENT = 200.0f;
    const float VELOCITY_KNOCKBACK = 400.0f;
    const float VELOCITY_JUMP = 5.0f;
    const float ENERGY_GAIN = 0.5f;
    const int DEFAULT_DAMAGE = 1;
    const float ATTACK_ENERGY_COST = 0.5f;
    const float SKILL_ENERGY_COST = 5.1f;
    const float SKILL_ENERGY_COST_OVER_TIME = 0.01f;
    const float STUN_COOLDOWN = 3;
    const float JUMP_COOLDOWN = .3f;
    const float RAYCAST_SIZE = 0.4f;
    const float RAYCAST_POSITION = 1.2f;
    const float TIME_INVINCIBLE = 1f;
    const float TIME_BETWEEN_SKILLS = 1f;
    #endregion

    public static float MAX_HEALTH = 100;
    public static float MAX_ENERGY = 10;
    public static int MAX_NB_JUMP = 2;
    public static int CHARACTER_LAYER = 9;
    public static int LEVEL_LAYER = 11;

    Color colorCharacter;
    public GameObject character;
    GameObject sprite;
    GameObject shield;
    Vector3 lastPosition;
    float energy;
    float health;
    int lives;
    int damage = DEFAULT_DAMAGE;
    int direction;
    float skillOverPower;

    float healthDamageOverTime;
    float energieDamageOverTime;

    public bool gotSkillDamage = false;

    bool gotItemSpecial = false;
    bool stunned = false;
    bool slowed = false;
    bool stun = false;
    bool canJump = true;
    bool invincible;
    bool canSkill;

    float timeStunned;
    float timeStun;
    float timeSlowed;
    float timeItemDamage;
    float timeSkillHealthDamage;
    float timeSkillEnergieDamage;
    float timeItemSpecial;
    float timeInvincible;
    float timeSkills;

    float timeNextJump;

    public Animator stateMachine, blood;
    public SpriteRenderer sr;
    Rigidbody2D rb2D;
    ClampName cn;
    GameObject aura;

    FactorySkill skillType;
    public int id;

    float velocityMovement = NORMAL_VELOCITY_MOVEMENT;

    int cptJump = 1;
    bool isCharging = false;
    int layerMask;
    int groundLayerMask;

    SpriteRenderer renderer;

    public void Start(int lives, CharacterName chName, int id)
    {
        character = GameObject.Instantiate(Resources.Load("Prefabs/Character/Character")) as GameObject;
        this.lives = lives;
        this.id = id;
        direction = 1;
        stateMachine = character.GetComponentInChildren<Animator>();
        sr = character.GetComponentInChildren<SpriteRenderer>();
        rb2D = character.GetComponentInChildren<Rigidbody2D>();
        blood = character.transform.Find("Blood").GetComponent<Animator>();
        
        cn = character.GetComponentInChildren<ClampName>();


        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).tag == "Player")
            {
                renderer = character.transform.GetChild(i).GetComponent<SpriteRenderer>();
            }
            if (character.transform.GetChild(i).tag == "aura")
            {
                aura = character.transform.GetChild(i).gameObject;
            }
            if (character.transform.GetChild(i).tag == "shield")
            {
                shield = character.transform.GetChild(i).gameObject;
            }
        }

        aura.SetActive(false);
        shield.SetActive(false);
        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).tag == "Player")
            {
                sprite = character.transform.GetChild(i).gameObject;
            }
        }

        switch (chName)
        {
            case CharacterName.BurningBruiser:
                sr.color = Color.red;
                skillType = FactorySkill.InfernalSpark;
                break;
            case CharacterName.SubAbsoluteZero:
                sr.color = Color.blue;
                skillType = FactorySkill.ArticBlast;
                break;
            case CharacterName.ElectroidTheCrusher:
                sr.color = Color.yellow;
                skillType = FactorySkill.LightningStrike;
                break;
            case CharacterName.DoctorScorpio:
                sr.color = Color.green;
                skillType = FactorySkill.ToxicFlash;
                break;
            default:
                sr.color = Color.white;
                break;
        }
        layerMask = 1 << CHARACTER_LAYER;
        groundLayerMask = 1 << LEVEL_LAYER;

        lastPosition = Vector3.zero;
        init();
    }

    public void SetUpClampName(Text txt)
    {
        cn.nameLabel = txt;
    }

    public void InitUIColor()
    {
        UiManager.Instance.ChangeColorUI(sr.color, id);
    }

    public void UpdateUIPlayer()
    {
        UiManager.Instance.UpdateUIGame(id, health, energy, lives);
    }

    public void Update(InputManager.InputPkg pkg)
    {
        if (!renderer.IsVisibleFrom(Camera.main) || health <= 0)
        {
            if (stunned)
            {
                //     AnimationManager.Instance.QuitStunAnimation(stateMachine);
            }
            Dead();
            UpdateUI();
        }
        if (!stunned)
        {
            AnimationManager.AnimationDatas animDatas = new AnimationManager.AnimationDatas(pkg, stateMachine, sr, isCharging, blood, skillType, energy, SKILL_ENERGY_COST, gotItemSpecial, stun, skillOverPower, cptJump, canJump, canSkill);

            AnimationManager.Instance.Update(animDatas);

            if (pkg.B && energy > 0)
            {
                KickAction();
            }
            else if (pkg.Y && energy > 0)
            {
                StunAction();
            }
            else if (pkg.RB)
            {
                if (!isCharging && (energy > SKILL_ENERGY_COST || gotItemSpecial) && canSkill)
                {
                    Debug.Log("SSSS");
                    StartSkillAction();
                }

                if (isCharging)
                {
                    ChargeSkillAction();
                }
            }

            if (isCharging && (!pkg.RB || (energy <= 0 && !gotItemSpecial)))
            {
                LaunchSkillAction();
            }
        }
        aura.transform.position = sprite.transform.position;
        shield.transform.position = sprite.transform.position;
        CheckTimeOfItem();
        CheckTimeOfDamage();
        CheckTimeOfEffects();

        if (PositionHasChange())
        {
            cn.SetTextPosition();
        }

    }

    public void FixedUpdate(InputManager.InputPkg pkg)
    {
        if (!stunned)
        {

            if (pkg.A)
                PunchAction();

            direction = (pkg.LeftStick.x > 0 ? -1 : (pkg.LeftStick.x < 0 ? 1 : direction));

            if (pkg.LeftStick.x != 0)
            {

                rb2D.AddForce((Mathf.Sign(direction) * character.transform.right) * velocityMovement);
            }

            //Jumping
            if (pkg.X)
            {

                if (cptJump < MAX_NB_JUMP && canJump)
                {
                    rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
                    cptJump++;
                    rb2D.AddForce(character.transform.up * VELOCITY_JUMP, ForceMode2D.Impulse);
                    SoundManager.Instance.Play("Jump", SoundManager.SoundType.Character);

                    timeNextJump = Time.time + JUMP_COOLDOWN;
                    canJump = false;
                }
            }
        }

        if (cptJump >= MAX_NB_JUMP)
        {
            Debug.Log("Not supposed to jump");
        }

        if (Time.time >= timeNextJump)
        {
            canJump = true;
            timeNextJump = 0;
        }

        if (IsGrounded())
        {
            cptJump = 1;
        }
    }

    void CheckTimeOfItem()
    {
        if (timeItemDamage < Time.time)
        {
            damage = DEFAULT_DAMAGE;
            if (aura.activeSelf)
            {
                aura.SetActive(false);
            }
        }

        if (timeItemSpecial < Time.time)
        {
            gotItemSpecial = false;
            UiManager.Instance.ActiveDesactiveSpecialBar(id, false);
        }
    }

    public void GotItemDamage()
    {
        damage += DEFAULT_DAMAGE;
        timeItemDamage = Time.time + Attack.TIME_ITEM_DAMAGE;
        aura.SetActive(true);
    }

    public void GotItemHealth()
    {
        health += Health.HEALTH_ITEM_VALUE;
        if (health >= MAX_HEALTH)
            health = MAX_HEALTH;

        UpdateUI();

    }

    public void GotItemEnergy()
    {
        FillEnergy(Energy.ENERGY_ITEM_VALUE);
    }

    public void GotItemSpecial()
    {
        gotItemSpecial = true;
        UiManager.Instance.ActiveDesactiveSpecialBar(id, true);
        timeItemSpecial = Time.time + Special.TIME_ITEM_SPECIAL;
    }

    public void CheckTimeOfDamage()
    {
        if (timeSkillHealthDamage > Time.time)
            TakeDirectHealthDamage(healthDamageOverTime * Time.deltaTime);

        if (timeSkillEnergieDamage > Time.time)
            TakeDirectEnergieDamage(energieDamageOverTime * Time.deltaTime);

    }

    public void CheckTimeOfEffects()
    {
        if (timeStunned < Time.time)
            if (stunned)
            {
                stunned = false;
                AnimationManager.Instance.QuitStunAnimation(stateMachine);
            }

        if (timeSlowed < Time.time)
            if (slowed)
            {
                slowed = false;
                velocityMovement = NORMAL_VELOCITY_MOVEMENT;
            }

        if (timeStun < Time.time)
        {
            if (stun)
            {
                stun = false;
            }
        }

        if (timeInvincible < Time.time)
        {
            shield.SetActive(false);
            invincible = false;
        }
        if (timeSkills < Time.time)
        {
            canSkill = true;
        }

    }

    public void TakeDirectHealthDamage(float damage)
    {
        if (!invincible)
        {
            health -= damage;
            if (health < 0)
                health = 0;
            AnimationManager.Instance.LaunchHurtAnimation(blood);
        }
        UpdateUI();
    }

    public void Dead()
    {

        AnimationManager.Instance.ResetStateMachine(stateMachine);
        if (lives > 1)
        {
            lives--;
            ChangePosition(GameManager.Instance.getRandomPosSpawn());

            Debug.Log(character.transform.position);
            init();
        }
        else
        {
            if (lives > 0)
                lives--;
            character.SetActive(false);
            cn.nameLabel.gameObject.SetActive(false);
        }
    }

    void ChangePosition(Vector3 pos)
    {
        character.transform.position = pos;
        for (int i = 0; i < character.transform.childCount; i++)
        {
            character.transform.GetChild(i).position = pos;
        }
    }

    void init()
    {
        health = MAX_HEALTH;
        energy = MAX_ENERGY;
        canSkill = true;
        rb2D.velocity = Vector2.zero;

        timeStunned = 0;
        timeSlowed = 0;

        timeSkillEnergieDamage = 0;
        timeSkillHealthDamage = 0;
        timeStun = 0;
        isCharging = false;
        stun = false;
        stunned = false;
        slowed = false;
        gotItemSpecial = false;
        gotSkillDamage = false;
        canJump = true;
        timeItemDamage = 0;
        timeItemSpecial = 0;
        timeInvincible = TIME_INVINCIBLE + Time.time;
        shield.SetActive(true);
        invincible = true;
        skillOverPower = 0;
        timeSkills = 0;
        velocityMovement = NORMAL_VELOCITY_MOVEMENT;


    }

    void UpdateUI()
    {
        UiManager.Instance.UpdateUIGame(id, health, energy, lives);
    }

    void FillEnergy(float nb)
    {

        energy += nb;

        if (energy > MAX_ENERGY)
            energy = MAX_ENERGY;

        UpdateUI();
    }

    public void TakeDirectEnergieDamage(float damage)
    {
        if (!invincible)
        {
            energy -= damage;
            if (energy < 0)
                energy = 0;
        }
        UpdateUI();
    }

    public void TakeHealthDamageOverTime(float damage, float time)
    {
        if (!invincible)
        {
            timeSkillHealthDamage = Time.time + time;
            healthDamageOverTime = damage;
        }
    }

    public void TakeEnergieDamageOverTime(float damage, float time)
    {
        if (!invincible)
        {
            timeSkillEnergieDamage = Time.time + time;
            energieDamageOverTime = damage;
        }
    }

    public void GetSlowEffect(float velocitySlowQuantity, float time)
    {
        if (!invincible)
        {
            timeSlowed = Time.time + time;
            if (!slowed)
            {
                slowed = true;
                velocityMovement -= velocitySlowQuantity;
            }
        }
    }

    public void GetStun(float time)
    {
        if (!invincible)
        {
            timeStunned = Time.time + time;
             AnimationManager.Instance.ResetStateMachine(stateMachine);
            stunned = false;
            isCharging = false;
            if (!stunned)
            {
                stunned = true;
                AnimationManager.Instance.LaunchStunAnimation(stateMachine);
            }
        }
    }

    public bool isAlive()
    {
        return lives > 0 ? true : false;
    }

    Player TouchPlayer()
    {
        RaycastHit2D r = (direction == 1 ? Physics2D.Raycast(sprite.transform.position + new Vector3(0.5f, 0, 0), Vector2.right, RAYCAST_SIZE, layerMask) : Physics2D.Raycast(sprite.transform.position - new Vector3(0.5f, 0, 0), -Vector2.right, RAYCAST_SIZE, layerMask));

        if (r)
        {
            Player p = PlayerManager.Instance.getPlayerFromGameObject(r.transform.parent.gameObject);
            return p;
        }

        return null;
    }

    bool IsGrounded()
    {
        RaycastHit2D r = Physics2D.Raycast(sprite.transform.position - new Vector3(0, RAYCAST_POSITION, 0), Vector2.down, RAYCAST_SIZE, groundLayerMask);

        return (r ? true : false);
    }

    void PunchAction()
    {
        SoundManager.Instance.Play("Punch", SoundManager.SoundType.Character);
        Player p = TouchPlayer();

        if (p != null)
        {
            SoundManager.Instance.Play("Hit", SoundManager.SoundType.Character);
            p.character.TakeDirectHealthDamage(damage);
            FillEnergy(ENERGY_GAIN);
            p.character.GetPunchKnockback(direction);
        }
    }

    void KickAction()
    {
        SoundManager.Instance.Play("Punch", SoundManager.SoundType.Character);
        Player p = TouchPlayer();

        if (p != null)
        {
            SoundManager.Instance.Play("Hit", SoundManager.SoundType.Character);
            p.character.TakeDirectHealthDamage(damage);
            AnimationManager.Instance.ResetStateMachine(p.character.stateMachine);
            p.character.stunned = false;
            AnimationManager.Instance.LaunchKnockbackAnimation(p.character.stateMachine, -direction);
        }
        if (!gotItemSpecial)
            TakeDirectEnergieDamage(ATTACK_ENERGY_COST);
    }

    void StunAction()
    {
        SoundManager.Instance.Play("Punch", SoundManager.SoundType.Character);

        if (!stun)
        {
            Player p = TouchPlayer();
            if (p != null)
            {
                SoundManager.Instance.Play("Hit", SoundManager.SoundType.Character);
                p.character.TakeDirectHealthDamage(damage);
                p.character.GetStun(1);
                stun = true;
                timeStun = Time.time + STUN_COOLDOWN;
            }

            if (!gotItemSpecial)
                TakeDirectEnergieDamage(ATTACK_ENERGY_COST);
        }
    }

    void StartSkillAction()
    {
        isCharging = true;
        canSkill = false;
        skillOverPower = 0;
        timeSkills = Time.time + TIME_BETWEEN_SKILLS;
        //consume x direct energy
        if (!gotItemSpecial)
        {
            Debug.Log("TAKE MOITIE");
            energy -= SKILL_ENERGY_COST;

        }
    }

    void ChargeSkillAction()
    {
        // consume energy over time
        if (!gotItemSpecial)
            TakeDirectEnergieDamage(SKILL_ENERGY_COST_OVER_TIME);

        skillOverPower += SKILL_ENERGY_COST_OVER_TIME;
        if (skillOverPower > 1)
            skillOverPower = 1;
    }

    void LaunchSkillAction()
    {
        isCharging = false;

        Vector2 position = stateMachine.gameObject.transform.position;

        position.x += direction * 2;
        AnimationManager.Instance.LaunchUseSkill(stateMachine);
        SkillManager.Instance.spawnSkill(skillType, position, skillOverPower, id);
    }

    void GetPunchKnockback(int direction)
    {
        rb2D.AddForce((Mathf.Sign(direction) * character.transform.right) * VELOCITY_KNOCKBACK);
    }

    bool PositionHasChange()
    {
        if (lastPosition != sprite.transform.position)
        {
            lastPosition = sprite.transform.position;
            return true;
        }

        return false;
    }
}


