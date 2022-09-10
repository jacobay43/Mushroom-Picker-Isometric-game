using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class IsometricPlayer : MonoBehaviour
{
    public float speed = .75f;
    private Rigidbody2D body;
    private Animator anim;
    private string direction;// = "down";
    private bool canHarvest = false;
    public int mushroomCounter = 0;
    private float scrollPos;
    private float deltaX;
    private float deltaY;
    private bool menuSwitch;
    Collider2D collisionObject;
    /// <summary>
    /// Reference to uiController in Game
    /// </summary>
    [SerializeField] UIController uiController;
    private Vector2 mousePosition;
    private float mouseX;
    private float mouseY;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        menuSwitch = false;
        deltaX = 0;
        deltaY = 0;
    }

  
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuSwitch = !menuSwitch;
            uiController.MenuImage.gameObject.SetActive(menuSwitch);
        }
        //TODO: Fix movement logic
        if (Managers.Player.isSleeping)
            return;
        
        deltaX = Input.GetAxis("Horizontal") * speed;
        deltaY = Input.GetAxis("Vertical") *speed;
    
        if (deltaX == 0 && deltaY == 0)
        {
            direction = "none";

        }

        if (deltaY < 0)
        {
           //direction = "down";
        }
        else if (deltaY > 0)
        {
            direction = "up";
        }
        if (deltaX < 0)// && deltaY == 0)
        {
            direction = "left";
        }
        else if (deltaX > 0)// && deltaY == 0)
        {
            direction = "right";
        } 


        if (direction == "left")// || Input.GetKey(KeyCode.A))
        {
            anim.SetInteger("Direction", 2);
        }
        else if (direction == "right")// ||Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("Direction", 3);
        }
        else if (direction == "up")//Input.GetKey(KeyCode.W))
        {
            anim.SetInteger("Direction", 1);
        }
        else if (direction == "down" && Input.GetKey(KeyCode.S))
        {
            anim.SetInteger("Direction", 4);
        }
        if (deltaX == 0 && deltaY == 0)
        {
            anim.SetInteger("Direction", 0);
        }


        if (Input.GetMouseButtonDown(0) && (Managers.Inventory.GetItemCount("hoe") > 0) && Managers.Inventory.IsEquipped("hoe"))
        { 
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseX = mousePosition.x - this.gameObject.transform.position.x;
                mouseY = mousePosition.y - this.gameObject.transform.position.y;
                if (mouseX < 0 && mouseY <= 0)
                {
                    direction = "left";
                }
                else if (mouseX < 0 && mouseY > 0)
                {
                    direction = "left";
                }
                else if (mouseX < 0 && mouseY < 0)
                {
                    direction = "left";
                }
                else if ((mouseX == 0) && mouseY > 0)
                {
                    direction = "up";
                }

                else if (mouseX > 0 && mouseY >= 0)
                {
                    direction = "right";
                }
                else if (mouseX > 0 && mouseY <= 0)
                {
                    direction = "right";
                }
                else if ((mouseX == 0) && mouseY < 0)
                {
                    direction = "down";
                }
                StartCoroutine(HoeAnimation());
                   
            }
        }

        if (Input.GetMouseButtonDown(0) && (Managers.Inventory.GetItemCount("kettle") > 0) && Managers.Inventory.IsEquipped("kettle"))
        {
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseX = mousePosition.x - this.gameObject.transform.position.x;
                mouseY = mousePosition.y - this.gameObject.transform.position.y;
                if (mouseX < 0 && mouseY <= 0)
                {
                    direction = "left";
                }
                else if (mouseX < 0 && mouseY > 0)
                {
                    direction = "left";
                }
                else if (mouseX < 0 && mouseY < 0)
                {
                    direction = "left";
                }
                else if ((mouseX == 0) && mouseY > 0)
                {
                    direction = "up";
                }

                else if (mouseX > 0 && mouseY >= 0)
                {
                    direction = "right";
                }
                else if (mouseX > 0 && mouseY <= 0)
                {
                    direction = "right";
                }
                else if ((mouseX == 0) && mouseY < 0)
                {
                    direction = "down";
                }
                StartCoroutine(KettleAnimation());

            }
        }

        //Mouse Scrolling
        scrollPos = Input.mouseScrollDelta.y;
        if (scrollPos > 0)
            uiController.Scroll('l');
        else if (scrollPos < 0)
            uiController.Scroll('r');
            

        Vector2 movement = new Vector2(deltaX, deltaY);
        body.velocity = movement;
    }

    private IEnumerator KettleAnimation()
    {
        if (direction == "left")
            anim.SetInteger("WaterDirection", 1);
        else
            anim.SetInteger("WaterDirection", 2);
        Debug.Log($"Watered in {anim.GetInteger("WaterDirection")}");

        yield return new WaitForSeconds(.55f);
 
        anim.SetInteger("WaterDirection", 0);
    }
    private IEnumerator HoeAnimation()
    {
        if (direction == "down")
            anim.SetInteger("HoeDirection", 1);
        else if (direction == "up")
            anim.SetInteger("HoeDirection", 2);
        else if (direction == "left")
            anim.SetInteger("HoeDirection", 3);
        else
            anim.SetInteger("HoeDirection", 4);
        Debug.Log($"Axed in {anim.GetInteger("HoeDirection")}");

        yield return new WaitForSeconds(.55f);
        if (canHarvest && collisionObject != null)
        {
            canHarvest = false;
            StartCoroutine(RemoveMushroom());
        }
        anim.SetInteger("HoeDirection", 0);
    }

    private IEnumerator RemoveMushroom()
    {
        yield return new WaitForSeconds(.2f);
        try
        {
            Destroy(collisionObject.gameObject, .5f);
            
            Managers.Inventory.AddItem("mushroom");
            mushroomCounter += 1;
            uiController.MushroomText.text = $"x{mushroomCounter}";
        }
        catch(NullReferenceException e)
        {
            Debug.Log($"Lost Reference to Mushroom. Try again");
            Debug.Log(e);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.StartsWith("Mushroom"))
        {
            collisionObject = collision;
            canHarvest = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.StartsWith("Mushroom"))
        {
            collisionObject = null;
            canHarvest = false;
        }
    }
}
