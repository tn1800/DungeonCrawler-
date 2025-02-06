using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//player movement 
public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] new Rigidbody2D rigidbody;

    Entity entity;

    Vector2 movementDir = Vector2.zero;

    void Start(){
        entity = GetComponent<Entity>();
    }

    
    public void OnMove(InputValue inputValue)
    {
        movementDir = inputValue.Get<Vector2>();
    }

    void FixedUpdate(){
        rigidbody.velocity = movementDir * entity.stats.MovementSpeed;
    }
}
