using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //facingDir will be used to save the last known looking direction to orient the player sprite after moving
    private Vector2 facingDir;

    //Denna bool moggar din vector2
    [HideInInspector]
    public bool isFacingRight;

    //Reads the value from the input asset
    private Vector2 movementInput;

    [SerializeField]
    Image manaBar;

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
    float timeBetweenPunches;

    [Header("Mage stuff")]
    [SerializeField]
    int MaxMana = 100;

    [Tooltip("The minimum amount of mana needed to switch to mage")]
    [SerializeField]
    int manaThreshhold = 10;

    [SerializeField]
    GameObject fireball;

    [SerializeField]
    float manaCost = 25f;

    [SerializeField]
    float timeBetweenSpells;

    private float timePassed = 0;
    private float actualManaLmao = 0;
    private float mana
    {
        get { return actualManaLmao; }
        set
        {
            actualManaLmao = value;
            manaBar.fillAmount = value / MaxMana;
        }
    }

    //Sets up the 2 states for the player to have
    private enum PlayerClass
    {
        Brawler,
        Mage
    }

    private Animator anim;
    private PlayerClass playerClass;

    private PlayerInput playerInput;

    private PlayerHealth playerHealth;
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

        anim.SetBool("Mage", true);
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
        // Debug.Log("fu");
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
        {
            if (movementInput.sqrMagnitude != 0)
            {
                anim.SetBool("move", true);
            }
            else
            {
                anim.SetBool("move", false);
            }
            anim.SetFloat("MovDir", movementInput.x);
        }
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

    void LateUpdate()
    {
        if (facingDir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (facingDir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //    transform.localScale = new Vector3(-1, 1, 1);
    }

    //reads and assings movement vector
    public void MovementInput(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        if (ctx.action.inProgress)
        {
            facingDir = movementInput;
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
            // isFacingRight = true;
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

    public void Death()
    {
        anim.SetBool("Dead", true);
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0.6f;
        StartCoroutine(slowTimeScale());
    }

    public void EndOfDeathAnimation()
    {
        Time.timeScale = 1f;
        StopAllCoroutines();
        SceneManager.LoadScene("Main");
    }

    IEnumerator slowTimeScale()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale -= 0.01f;
    }

    public void Yaya(string amogus)
    {
        //   Debug.Log(amogus);

        GameObject pog = Instantiate(fireball, SpellSpawnPos.position, Quaternion.identity);
        if (!isFacingRight)
            pog.transform.rotation = new Quaternion(0, 0, 180, 0);
        pog.GetComponent<spellBehaviour>().ShootRight();
    }

    //Reads attack input
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.inProgress)
        {
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
                    // Debug.Log("Mage Attack");
                    timePassed = 0;
                }
            }
            else
            {
                Debug.Log(timePassed);

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

    //referenced in animation
    public void ResCha()
    {
        resolutionManager.ChangeResolution();
        GameEventManager.instance.ScreenShake(0.3f);
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
            anim.SetTrigger("switch");
            //tells the resolution manager to change the resolution of the game

            //checks and changes player sprite and updates player class state
            if (playerClass == PlayerClass.Mage)
            {
                anim.SetBool("Mage", false);
                //                playerSpriteRenderer.color = Color.magenta;
                playerClass = PlayerClass.Brawler;
            }
            else if (playerClass == PlayerClass.Brawler)
            {
                anim.SetBool("Mage", true);

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
