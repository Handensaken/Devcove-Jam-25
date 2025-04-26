using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //facingDir will be used to save the last known looking direction to orient the player sprite after moving
    // private Vector2 facingDir;
    //Denna bool moggar din vector2
    [HideInInspector]
    public bool isFacingRight;

    //Reads the value from the input asset
    private Vector2 movementInput;

    [SerializeField]
    Slider manaSlider;

    [SerializeField]
    private float movementSpeed = 5;

    //Reference to the in-game resolution manager
    [SerializeField]
    private ResolutionManager resolutionManager;

    private SpriteRenderer playerSpriteRenderer;

    [Header("Brawler stuff")]
    [SerializeField]
    GameObject fist;

    [SerializeField]
    GameObject fistHolder;

    [SerializeField]
    float timeBetweenPunches;

    [Header("Mage stuff")]
    [SerializeField]
    int MaxMana = 100;

    [Tooltip("The minimum amount of mana needed to switch to mage")]
    [SerializeField]
    int manaThreshhold = 10;

    [SerializeField]
    GameObject fireball;

    [Tooltip("The minimum time it will take to shoot a spell")]
    [SerializeField]
    float minChargeTime = 0.1f;

    [Tooltip("The max time the spell will scale with")]
    [SerializeField]
    float maxChargeTime = 1f;

    [Tooltip("The damage the spell will deal if shot at minimum charge")]
    [SerializeField]
    float minDamage = 10;

    [Tooltip("The damage the spell will deal if the shot is fully charged")]
    [SerializeField]
    float maxDamage = 100f;

    [SerializeField]
    float manaCost = 25f;

    [SerializeField]
    float timeBetweenSpells;

    private float timeCharged = 0;
    private float timePassed = 0;
    private float mana;

    //Sets up the 2 states for the player to have
    private enum PlayerClass
    {
        Brawler,
        Mage
    }

    private Animator anim;
    private PlayerClass playerClass;

    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        isFacingRight = true;
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        //Initiates the player with as a mage with max mana
        playerClass = PlayerClass.Mage;
        mana = MaxMana;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        //        Debug.Log(GameEventManager.instance);
        GameEventManager.instance.OnForcedSwitch += SwitchPlayerClass;
        GameEventManager.instance.OnActivatePlayerInput += ActivatePlayerInput;
        GameEventManager.instance.OnStopPlayerInput += DisablePlayerInput;
        GameEventManager.instance.OnPlayerHurt += HurtPlayer;
    }

    //Set up player switch event. This could be done natively as is now, but later down the line we want effects and shit to be able to
    //hijack the event without needing 4 billion references here (cringe)
    void OnEnable()
    {
        /*Debug.Log(GameEventManager.instance);
        GameEventManager.instance.OnForcedSwitch += SwitchPlayerClass;
        GameEventManager.instance.OnActivatePlayerInput += ActivatePlayerInput;
        GameEventManager.instance.OnStopPlayerInput += DisablePlayerInput;*/
    }

    public void ActivatePlayerInput()
    {
        Debug.Log("fu");
        playerInput.actions.FindActionMap("Player").Enable();
    }

    public void Fagagagaga()
    {
        Debug.Log("Fat whore");
    }

    void DisablePlayerInput()
    {
        playerInput.actions.FindActionMap("Player").Disable();

        anim.SetTrigger("John Anime");
    }

    void OnDisable()
    {
        GameEventManager.instance.OnForcedSwitch -= SwitchPlayerClass;
        GameEventManager.instance.OnActivatePlayerInput -= ActivatePlayerInput;
        GameEventManager.instance.OnStopPlayerInput -= DisablePlayerInput;
        GameEventManager.instance.OnPlayerHurt -= HurtPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        timePassed += Time.deltaTime;
    }

    //player bounds
    [SerializeField]
    [Tooltip(
        "simple bounds defining the area the player is allowed to move in. x = upper | y = lower"
    )]
    private Vector2 bounds = new Vector2(-1.7f, -5f);

    private void MovePlayer()
    {
        if (anim != null)
            anim.SetFloat("MovDir", movementInput.x);
        //avoid bounds
        if (transform.position.y >= bounds.x && movementInput.y > 0)
        {
            movementInput.y = 0;
        }
        if (transform.position.y <= bounds.y && movementInput.y < 0)
        {
            movementInput.y = 0;
        }
        transform.Translate(movementInput * movementSpeed * Time.deltaTime);
    }

    //reads and assings movement vector
    public void MovementInput(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        if (ctx.action.inProgress)
        {
            if (movementInput.x >= 0)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
            }
        }
        if (ctx.canceled)
        {
            isFacingRight = true;
        }
    }

    public bool getfacingright()
    {
        if (isFacingRight)
            return true;
        else
            return false;
    }

    [SerializeField]
    private Transform SpellSpawnPos;

    private void HurtPlayer(float f)
    {
        anim.SetTrigger("Hurt");
    }

    public void Yaya()
    {
        GameObject pog = Instantiate(fireball, SpellSpawnPos.position, Quaternion.identity);
        if (isFacingRight)
            //activate normal mage animation
            pog.GetComponent<spellBehaviour>().ShootRight();
        else
            //activate flipped mage animation
            pog.GetComponent<spellBehaviour>().ShootLeft();
    }

    //Reads attack input
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.inProgress)
        {
            //Debug.Log("HIT THAT CHILD");


            //playerClass.Attack();

            if (playerClass == PlayerClass.Mage)
            {
                if (timeBetweenSpells < timePassed)
                {
                    mana -= manaCost;
                    //TODO:Add support for charging attacks
                    //Here you could play an animation

                    //I want to spawn the object from an animation event
                    //But animation event's can't send gameobject references, so I need to think
                    //GameObject pog = Instantiate(
                    //     fireball,
                    //    fistHolder.transform.position,
                    //    Quaternion.identity
                    //);
                    anim.SetTrigger("Pew");
                    //if (isFacingRight)
                    //activate normal mage animation
                    //  pog.GetComponent<spellBehaviour>().ShootRight();
                    // else
                    //activate flipped mage animation
                    //  pog.GetComponent<spellBehaviour>().ShootLeft();
                    Debug.Log("Mage Attack");
                    timePassed = 0;
                }
            }
            else
            {
                if (timeBetweenPunches < timePassed)
                {
                    Debug.Log("Punch");
                    anim.SetTrigger("Punch");
                    //Instantiate(fist, fistHolder.transform.position, Quaternion.identity);
                    timePassed = 0;
                }
            }
        }

        //When the input is released and the player doesn't have any more mana, switches
        //Probably needs to be restructured later to fit animations
        if (ctx.canceled)
        {
            if (playerClass == PlayerClass.Mage)
            {
                if (mana < 1)
                {
                    //Send event to switch
                    GameEventManager.instance.ForcedSwitch();
                }
            }
        }
    }

    //Reads switch input
    public void GetSwitchInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.inProgress)
        {
            SwitchPlayerClass();
        }
    }

    public void SwitchPlayerClass()
    {
        if (mana < manaThreshhold && playerClass == PlayerClass.Brawler)
        {
            Debug.Log("Preventing switch due to insufficient mana");
            return;
        }
        else
        {
            //tells the resolution manager to change the resolution of the game
            resolutionManager.ChangeResolution();
            GameEventManager.instance.ScreenShake();

            //checks and changes player sprite and updates player class state
            if (playerClass == PlayerClass.Mage)
            {
                //                playerSpriteRenderer.color = Color.magenta;
                playerClass = PlayerClass.Brawler;
            }
            else if (playerClass == PlayerClass.Brawler)
            {
                // playerSpriteRenderer.color = Color.red;
                playerClass = PlayerClass.Mage;
            }
            else
            {
                Debug.LogError(
                    "Some-fucking-how the player has aquired a third, unidentified class"
                );
            }
            Debug.Log($"Mana is {mana} | new player class is {playerClass}");
        }
    }

    public void ActivateAttackbox(int i)
    {
        if (i == 1)
        {
            fist.SetActive(true);
        }
        else
        {
            fist.SetActive(false);
        }
    }

    public void RecieveMana(float manaRecieved)
    {
        mana += manaRecieved;
        if (mana > MaxMana)
            mana = MaxMana;
    }
}
