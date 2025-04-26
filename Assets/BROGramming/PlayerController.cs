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

    //creates player classes
    private readonly Brawler brawler = new Brawler();
    private readonly Mage mage = new Mage();

    private playerClass playerClass;

    // Start is called before the first frame update
    void Start()
    {
        playerClass = mage;
        playerClass.Mana = 1;
        //THIS IS FUCKING NIGHTMARE CODE
        //I HATE THESE FUCKING CLASSES
        //THEY ARE CUNTS
        //THEY CREATE THEIR OWN VALUES BECAUSE I AM BAD AT CODE
        //FUCK YOU
        //THEY WOULD WORK AS FUCKING ENUM STATES 
        //BUT I THOUGHT ID BE CLEVER AND USE CLASS OVERRIDES BUT THAT IS LITERALLY JUST STUFF OF NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE 


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
            playerClass.Attack();
        }

        //When the input is released and the player doesn't have any more mana, switches
        //Probably needs to be restructured later to fit animations
        if (ctx.canceled)
        {
            if (playerClass is Mage)
            {
                if (playerClass.Mana < 1)
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
        if (playerClass.Mana < 1 && playerClass is Brawler)
        {
            Debug.Log("Preventing switch due to insufficient mana");
            return;
        }
        else
        {
            Debug.Log($"Mana is {playerClass.Mana} | player class is {playerClass}");
            //  resolutionManager.ChangeResolution();

            //Debug.Log("Switch");
            //  if (resolutionManager != null)
            //{
            if (playerClass is Mage)
            {
                playerSpriteRenderer.color = Color.magenta;
                playerClass = brawler;
            }
            else if (playerClass is Brawler)
            {
                playerSpriteRenderer.color = Color.red;
                playerClass = mage;
            }
            else
            {
                Debug.LogError(
                    "Some-fucking-how the player has aquired a third, unidentified class"
                );
            }
            /* }
             else
             {
                 Debug.LogWarning("resolutionManager is null");
             }*/
        }
    }
}

public class playerClass
{
    public float Mana;

    public playerClass()
    {
      //  this.Mana = 1;
    }

    public virtual void Attack() { }
}

public class Brawler : playerClass
{
    public override void Attack()
    {
        Debug.Log("Brawler Attack");
    }
}

public class Mage : playerClass
{
    public override void Attack()
    {
        Mana--;

        Debug.Log($"Mage Attack! new mana {Mana}");
    }
}
