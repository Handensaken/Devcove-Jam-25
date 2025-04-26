using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //facingDir will be used to save the last known looking direction to orient the player sprite after moving
    private Vector2 facingDir;

    //Reads the value from the input asset
    private Vector2 movementInput;

    [SerializeField]
    private float movementSpeed = 5;

    //Reference to the in-game resolution manager
    [SerializeField]
    private ResolutionManager resolutionManager;

    private SpriteRenderer playerSpriteRenderer;

    //Sets up the 2 states for the player to have
    private enum PlayerClasses
    {
        Brawler,
        Mage
    }

    private PlayerClasses playerClasses;
    private float mana;

    // Start is called before the first frame update
    void Start()
    {
        //Initiates the player with as a mage with 1 mana
        playerClasses = PlayerClasses.Mage;
        mana = 1;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        Debug.Log(GameEventManager.instance);
        GameEventManager.instance.OnForcedSwitch += SwitchPlayerClass;
    }

    void OnDisable()
    {
        GameEventManager.instance.OnForcedSwitch -= SwitchPlayerClass;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    //player bounds
    [SerializeField]
    [Tooltip(
        "simple bounds defining the area the player is allowed to move in. x = upper | y = lower"
    )]
    private Vector2 bounds = new Vector2(-1.7f, -5f);

    private void MovePlayer()
    {
        //avoid bounds
        if (transform.position.y >= bounds.x - transform.localScale.y / 2 && movementInput.y > 0)
        {
            movementInput.y = 0;
        }
        if (transform.position.y <= bounds.y + transform.localScale.y / 2 && movementInput.y < 0)
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
            facingDir = movementInput;
        }
    }

    //Reads attack input
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.inProgress)
        {
            //Debug.Log("HIT THAT CHILD");


            //playerClass.Attack();

            if (playerClasses == PlayerClasses.Mage)
            {
                mana--;
                Debug.Log("Mage Attack");
            }
            else
            {
                Debug.Log("Punch");
            }
        }

        //When the input is released and the player doesn't have any more mana, switches
        //Probably needs to be restructured later to fit animations
        if (ctx.canceled)
        {
            /*if (playerClass is Mage)
            {
                if (playerClass.Mana < 1)
                {
                    //Send event to switch
                    GameEventManager.instance.ForcedSwitch();
                    //    Debug.Log("Recognizing Mage");
                }
            }*/
            if (playerClasses == PlayerClasses.Mage)
            {
                if (mana < 1)
                {
                    //Send event to switch
                    GameEventManager.instance.ForcedSwitch();
                    //    Debug.Log("Recognizing Mage");
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
        /* if (playerClass.Mana < 1 && playerClass is Brawler)
         {
             Debug.Log("Preventing switch due to insufficient mana");
             return;
         }*/
        if (mana < 1 && playerClasses == PlayerClasses.Brawler)
        {
            Debug.Log("Preventing switch due to insufficient mana");
            return;
        }
        else
        {
            resolutionManager.ChangeResolution();
            if (playerClasses == PlayerClasses.Mage)
            {
                playerSpriteRenderer.color = Color.magenta;
                playerClasses = PlayerClasses.Brawler;
            }
            else if (playerClasses == PlayerClasses.Brawler)
            {
                playerSpriteRenderer.color = Color.red;
                playerClasses = PlayerClasses.Mage;
            }
            else
            {
                Debug.LogError(
                    "Some-fucking-how the player has aquired a third, unidentified class"
                );
            }
            Debug.Log($"Mana is {mana} | new player class is {playerClasses}");
        }
    }
}
