using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 facingDir;
    private Vector2 movementInput;

    [SerializeField]
    private float movementSpeed = 5;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    [SerializeField]
    private float upperBounds = -1.7f;

    [SerializeField]
    private float lowerBounds = -5f;

    private void MovePlayer()
    {
        //avoidBounds
        if (transform.position.y >= upperBounds - transform.localScale.y / 2 && movementInput.y > 0)
        {
            movementInput.y = 0;
        }
        if (transform.position.y <= lowerBounds + transform.localScale.y / 2 && movementInput.y < 0)
        {
            movementInput.y = 0;
        }
        transform.Translate(movementInput * movementSpeed * Time.deltaTime);
    }

    public void MovementInput(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        if (ctx.action.inProgress)
        {
            facingDir = movementInput;
        }
    }
}
