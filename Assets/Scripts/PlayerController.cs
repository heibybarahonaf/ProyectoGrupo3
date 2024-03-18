using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;

    private Vector3 playerInput;

    public CharacterController player;
    public float playerspeed;   //velocidad del jugador
    private Vector3 movePlayer;
    public float gravity = 9.8f;
    public float fallvelocity;
    public float jumpForce;


    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;

    //Variables Animacion
    public Animator playerAnimatorController;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnimatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    // se utiliza para las fisicas
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        playerAnimatorController.SetFloat("PlayerWalkVelocity", playerInput.magnitude * playerspeed);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward; // permitira que el jugador siempre se mueva respecto a la cam

        movePlayer = movePlayer * playerspeed;

        player.transform.LookAt(player.transform.position + movePlayer);  // gire hacia donde se encuentre la camara

        setGravity();

        playerSkills();

        player.Move(movePlayer * Time.deltaTime);



    }


    //funcion para las habilidades del jugador

    public void playerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallvelocity = jumpForce;
            movePlayer.y = fallvelocity;
            playerAnimatorController.SetTrigger("PlayerJump");
        }
        else
        {
            fallvelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallvelocity;
        }

    }
    // funci�n para deternminar la direcci�n a la que mira la c�mara
    public void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

    }

    // funci�n para la gravedad
    public void setGravity()
    {

        if (player.isGrounded)
        {

            fallvelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallvelocity;
        }
        else
        {
            fallvelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallvelocity;
            playerAnimatorController.SetFloat("PlayerVerticalVelocity", player.velocity.y);
        }
        playerAnimatorController.SetBool("IsGrounded", player.isGrounded);
        sliderDown();

    }

    public void sliderDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;

        if (isOnSlope)
        {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;

            movePlayer.y += slopeForceDown;

        }
    }

    // se ejecuta cuando el player choca con algo
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        hitNormal = hit.normal;

    }

    void OnAnimatorMove(){
        
    }

}
