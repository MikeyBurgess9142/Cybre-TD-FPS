using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController_Old : MonoBehaviour
{
    [Header("---Components---")]
    [SerializeField] CharacterController controller;

    [Header("---Player Stats---")]
    [Range(10, 1000)][SerializeField] int HP;
    [Range(5, 30)][SerializeField] float playerSpd;
    [Range(1, 10)][SerializeField] int jumpMax;
    [Range(1, 25)][SerializeField] int jumpSpd;
    [Range(10, 40)][SerializeField] int gravity;
    [Range(1, 5)][SerializeField] int sprintMod;

    [Header("---Gun Stats---")]
    public List<gunStats> gunList = new List<gunStats>();
    [Range(0, 10)][SerializeField] float shtRate;
    [Range(10, 500)][SerializeField] int shtDist;
    [Range(5, 250)][SerializeField] int shtDmg;
    [SerializeField] MeshFilter gunModel;
    [SerializeField] MeshRenderer gunMaterial;

    [Header("---Gun Transformations---")]
    public Transform gunPivot;
    public Transform gunModelADS;
    public Transform gunModelDefaultPos;
    public int adsSpd;
    public int notADSSpd;

    [Header("----- Camera Stats -----")]
    public float zoomMax;
    public int zoomInSpd;
    public int zoomOutSpd;

    [Header("---Sway Settings---")]
    [SerializeField] float smooth;
    [SerializeField] float swayMultiplier;

    Quaternion rotationX, rotationY, targetRotation;

    int hpOrigin;
    int jumpsCurr;
    int selectedGun;
    Vector3 move;
    Vector3 playerVeloc;
    bool isShooting;
    bool isSprinting;
    bool zooming;
    float playerSpdOrig;
    float zoomOrig;

    // Start is called before the first frame update
    void Start()
    {
        hpOrigin = HP;
        playerSpdOrig = playerSpd;
        zoomOrig = Camera.main.fieldOfView;

        respawnPlayer();
        updateHP();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
            selectGun();
            zoomCamera();
            gunSway();

            if (!isShooting && Input.GetButton("Shoot"))
            {
                StartCoroutine(shoot());
            }
        }
    }

    void movement()
    {
        sprint();

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

    void sprintInput()
    {
        if (Input.GetButton("Sprint") && !zooming)
        {
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
        }
    }

    void sprint()
    {
        sprintInput();

        if (isSprinting)
        {
            if (playerSpd < (playerSpdOrig * sprintMod))
            {
                playerSpd *= sprintMod;
            }
        }
        else
        {
            if (playerSpd != playerSpdOrig)
            {
                playerSpd /= sprintMod;
            }
        }
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
            zooming = false;
            gameManager.instance.playerDead();
        }
    }

    public void updateHP()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)hpOrigin;
    }

    public void resetGun()
    {
        Camera.main.fieldOfView = zoomOrig;

        gunPivot.localPosition = new Vector3(0, 0, 0);
        gunPivot.localEulerAngles = new Vector3(0, 0, 0);
        gunPivot.localScale = new Vector3(1, 1, 1);
        if (gunModel != null)
        {
            gunModel.transform.localPosition = new Vector3(0, 0, 0);
            gunModel.transform.eulerAngles = new Vector3(0, 0, 0);
            gunModel.transform.localScale = new Vector3(0, 0, 0);
        }
        gunModelADS.localPosition = new Vector3(0, 0, 0);
        gunModelDefaultPos.localPosition = new Vector3(0, 0, 0);
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        resetGun();

        shtRate = gunList[selectedGun].shtRate;
        shtDist = gunList[selectedGun].shtDist;
        shtDmg = gunList[selectedGun].shtDmg;
        gunModel.transform.localEulerAngles = gunList[selectedGun].gunRotation;
        gunModel.transform.localScale = gunList[selectedGun].gunScale;
        zoomMax = gunList[selectedGun].zoomMaxFov;
        zoomInSpd = gunList[selectedGun].zoomInSpd;
        zoomOutSpd = gunList[selectedGun].zoomOutSpd;
        adsSpd = gunList[selectedGun].adsSpd;
        notADSSpd = gunList[selectedGun].notADSSpd;
        gunModelADS.localPosition = gunList[selectedGun].gunModelADS;
        gunPivot.localPosition = gunList[selectedGun].gunPosition;
        gunModelDefaultPos.localPosition = gunList[selectedGun].gunModelDefaultPos;

        gunModel.sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunMaterial.sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void gunPickup(gunStats gunStat)
    {
        resetGun();
        gunList.Add(gunStat);

        shtRate = gunStat.shtRate;
        shtDist = gunStat.shtDist;
        shtDmg = gunStat.shtDmg;
        gunModel.transform.localEulerAngles = gunStat.gunRotation;
        gunModel.transform.localScale = gunStat.gunScale;
        zoomMax = gunStat.zoomMaxFov;
        zoomInSpd = gunStat.zoomInSpd;
        zoomOutSpd = gunStat.zoomOutSpd;
        adsSpd = gunStat.adsSpd;
        notADSSpd = gunStat.notADSSpd;
        gunModelADS.localPosition = gunStat.gunModelADS;
        gunPivot.localPosition = gunStat.gunPosition;
        gunModelDefaultPos.localPosition = gunStat.gunModelDefaultPos;

        gunModel.sharedMesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunMaterial.sharedMaterial = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
    }

    void zoomInput()
    {
        if (Input.GetButtonDown("Zoom"))
        {
            zooming = true;
        }
        else if (Input.GetButtonUp("Zoom"))
        {
            zooming = false;
        }
    }

    void zoomCamera()
    {
        zoomInput();

        if (zooming)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomMax, Time.deltaTime * zoomInSpd);

            if (isSprinting)
            {
                isSprinting = false;
            }
            if (gunList.Count > 0)
            {
                gunPivot.transform.localPosition = Vector3.Lerp(gunPivot.transform.localPosition, gunModelADS.localPosition, Time.deltaTime * adsSpd);
            }
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomOrig, Time.deltaTime * zoomOutSpd);

            if (gunList.Count > 0)
            {
                gunPivot.transform.localPosition = Vector3.Lerp(gunPivot.transform.localPosition, gunModelDefaultPos.localPosition, Time.deltaTime * notADSSpd);
            }
        }
    }

    void gunSway()
    {
        if (gameManager.instance.playerScript.gunList.Count > 0)
        {
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            if (gameManager.instance.playerScript.zooming)
            {
                rotationX = Quaternion.AngleAxis(-mouseY, Vector3.left);
                rotationY = Quaternion.AngleAxis(mouseX, Vector3.down);
            }
            else
            {
                rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
                rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
            }

            targetRotation = rotationX * rotationY;

            //gunPivot.localRotation = Quaternion.Slerp(gunPivot.localRotation, targetRotation, smooth * Time.deltaTime);
        }
    }
}
