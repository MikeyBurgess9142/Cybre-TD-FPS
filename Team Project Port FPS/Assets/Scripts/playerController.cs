using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("---Components---")]
    [SerializeField] CharacterController controller;

    [Header("---Player Stats---")]
    [Range(10, 1000)] [SerializeField] int HP;
    [Range(5, 30)] [SerializeField] float playerSpd;
    [Range(1, 10)] [SerializeField] int jumpMax;
    [Range(1,25)] [SerializeField] int jumpSpd;
    [Range(10,10)] [SerializeField] int gravity;

    [Header("---Gun Stats---")]
    [Range(0, 10)] [SerializeField] float shtRate;
    [Range(10, 500)] [SerializeField] int shtDist;
    [Range(5, 250)] [SerializeField] int shtDmg;
    //[SerializeField] GameObject cube;

    int hpOrigin;
    int jumpsCurr;
    Vector3 move;
    Vector3 playerVeloc;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        hpOrigin = HP;
        updateHP();
        respawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();

            if (!isShooting && Input.GetButton("Shoot"))
            {
                StartCoroutine(shoot());
            }
        }
    }

    void movement()
    {

        if (controller.isGrounded && playerVeloc.y < 0)
        {
            playerVeloc.y = 0;
            jumpsCurr = 0;
        }

        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move = move.normalized;
        controller.Move(move * Time.deltaTime * playerSpd);

        if (Input.GetButtonDown("Jump") && jumpsCurr < jumpMax)
        {
            jumpsCurr++;
            playerVeloc.y = jumpSpd;
        }

        playerVeloc.y -= gravity * Time.deltaTime;
        controller.Move(playerVeloc * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shtDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDmg(shtDmg);
            }
        }

        yield return new WaitForSeconds(shtRate);
        isShooting = false;
    }

    public void respawnPlayer()
    {
        HP = hpOrigin;
        updateHP();
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void takeDmg(int dmg)
    {
        HP -= dmg;
        updateHP();
        StartCoroutine(gameManager.instance.playerHit());

        if (HP <= 0)
        {
            gameManager.instance.playerDead();
        }
    }

    public void updateHP()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)hpOrigin;
    }
}
