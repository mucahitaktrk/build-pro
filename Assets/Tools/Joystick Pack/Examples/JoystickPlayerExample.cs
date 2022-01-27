using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class JoystickPlayerExample : MonoBehaviour
{
    //--PlayerScript---------------------------------------------
    private float speed = 1f;
    private float rotationSpeed = 720f;
    private FloatingJoystick floatingJoystick;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    //-----------------------------------------------------------
    [SerializeField] private GameObject point = null;
    [SerializeField] private GameObject startPoint = null; 
    [SerializeField] private GameObject[] constructionMaterials = null;
    //----------CONCRETE-------------------------------------
    [Header("Concrete")]
    private int concreteBrickCount;
    private GameObject concreteMachine;
    [SerializeField] private GameObject concreteBrick = null;
    [SerializeField] private List<GameObject> concreteBrickObject;
    private GameObject[] concreteBrickPostion;
    [SerializeField] private List<GameObject> concreteBricks;
    private bool concreteBrickController = false;
    private bool concreteActiveController = false;
    //----------CAMERA--------------------------------------
    private GameObject cameraObject;
    //----------CONCRETE CRUSH------------------------------
    [Header("Iron")]
    private int ironCount;
    [SerializeField] private GameObject iron = null;
    [SerializeField] private List<GameObject> ironObject;
    private GameObject[] ironPostion;
    [SerializeField] private List<GameObject> irons;
    private bool concreteCrushController = false;
    //----------HOUSE----------------------------------------
    private GameObject houseObject;
    private bool houseController;
    //-----------BRICK----------------------------------------
    [Header("Brick")]
    private int brickCount;
    private GameObject brickMachine;
    [SerializeField] private GameObject brick = null;
    [SerializeField] private List<GameObject> brickObject;
    private GameObject[] brickPostion;
    [SerializeField] private List<GameObject> bricks;
    private bool brickController = false;
    private bool brickActiveController = false;

    [SerializeField] private TextMeshProUGUI[] materialText = null;
    private int materialCount = 0;

    private float timer = 0;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        TagSystem();
        houseObject.transform.GetChild(0).gameObject.SetActive(true);
        houseObject.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void Update()
    {
        materialText[0].text = "IRON = " + irons.Count;
        materialText[1].text = "BRICK = " + bricks.Count;
        materialText[2].text = "CONCRETE = " + concreteBricks.Count;
        CameraSystem();
        MoveController();
        HouseSystem();
        PlusSystem();
        if (Time.time > timer)
        {
            ConcreteCrushMachineSystem();
            BrickMachineSystem();
            ConcreteMachineSystem();
            timer = Time.time + 1;
        }
    }
    private void MoveController()
    {
        playerRigidbody.velocity = new Vector3(floatingJoystick.Horizontal * 10, 0, floatingJoystick.Vertical * 10);
        playerAnimator.SetFloat("CharacterSpeed", Mathf.Abs(playerRigidbody.velocity.z));
        playerAnimator.SetFloat("CharacterSpeed", Mathf.Abs(playerRigidbody.velocity.x));
        Vector3 movementDirection = new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical);
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void TagSystem()
    {
        brickMachine = GameObject.FindGameObjectWithTag("BrickMachineTag");
        concreteMachine = GameObject.FindGameObjectWithTag("ConcreteMachineTag");
        floatingJoystick = GameObject.FindGameObjectWithTag("FloatingJoystick").GetComponent<FloatingJoystick>();
        houseObject = GameObject.FindGameObjectWithTag("HouseTag");
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        ironPostion = GameObject.FindGameObjectsWithTag("IronTag");
        brickPostion = GameObject.FindGameObjectsWithTag("BrickTag");
        concreteBrickPostion = GameObject.FindGameObjectsWithTag("ConcreteBrickTag");

    }

    private void CameraSystem()
    {
        cameraObject.transform.position = new Vector3(transform.position.x, transform.position.y + 30, transform.position.z - 20.05f);
    }
    private void BrickMachineSystem()
    {
        if (brickActiveController)
        {
            if (brickCount < 12)
            {
                brickObject.Add(Instantiate(brick, brickPostion[brickCount].transform.position, Quaternion.Euler(0, 90, 0)));
                brickCount++;
            }
        }

    }
    private void ConcreteCrushMachineSystem()
    {
        if (ironCount < 18)
        {
            ironObject.Add(Instantiate(iron, ironPostion[ironCount].transform.position, Quaternion.identity));
            ironCount++;
        }
    }

    private void ConcreteMachineSystem()
    {
        if (concreteActiveController)
        {
            if (concreteBrickCount < 21)
            {
                concreteBrickObject.Add(Instantiate(concreteBrick, concreteBrickPostion[concreteBrickCount].transform.position, Quaternion.Euler(0, 90, 0)));
                concreteBrickCount++;
            }
        }

    }

    private void PlusSystem()
    {
        if (materialCount < 40)
        {
            if (concreteCrushController)
            {
                ironCount--;
                point.gameObject.transform.position += point.transform.up * constructionMaterials[0]
                .transform.localScale.y - new Vector3(0, 0.5f, 0);
                ironObject[ironCount].transform.DOMove(point.transform.position, 0.2f);
                ironObject[ironCount].transform.DORotate(point.transform.eulerAngles, 0.0f);
                ironObject[ironCount].transform.parent = transform.GetChild(2);
                irons.Add(ironObject[ironCount]);
                ironObject.RemoveAt(ironCount);
                materialCount++;
                return;
            }
            if (brickController)
            {
                brickCount--;
                point.gameObject.transform.position += point.transform.up * constructionMaterials[0]
                .transform.localScale.y - new Vector3(0, 0.5f, 0);
                brickObject[brickCount].transform.DOMove(point.transform.position, 0.5f);
                brickObject[brickCount].transform.DORotate(point.transform.eulerAngles, 0.0f);
                brickObject[brickCount].transform.parent = transform.GetChild(2);
                bricks.Add(brickObject[brickCount]);
                brickObject.RemoveAt(brickCount);
                materialCount++;
                return;
            }
            if (concreteBrickController)
            {
                concreteBrickCount--;
                point.gameObject.transform.position += point.transform.up * constructionMaterials[0]
                .transform.localScale.y - new Vector3(0, 0.5f, 0);
                concreteBrickObject[concreteBrickCount].transform.DOMove(point.transform.position, 0.05f);
                concreteBrickObject[concreteBrickCount].transform.DORotate(point.transform.eulerAngles, 0.0f);
                concreteBrickObject[concreteBrickCount].transform.parent = transform.GetChild(2);
                concreteBricks.Add(concreteBrickObject[concreteBrickCount]);
                concreteBrickObject.RemoveAt(concreteBrickCount);
                materialCount++;
                return;
            }
        }

    }

    private void HouseSystem()
    {
        if (materialCount > 2)
        {
            if (houseController)
            {
                houseObject.transform.GetChild(0).gameObject.SetActive(false);
                houseObject.transform.GetChild(1).gameObject.SetActive(true);
                Destroy(gameObject.transform.GetChild(2).gameObject);
                point.transform.position = startPoint.transform.position;
                materialCount = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 11)
        {
            houseController = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (materialCount > 15)
            {
                brickMachine.transform.GetChild(0).gameObject.SetActive(false);
                brickMachine.transform.GetChild(1).gameObject.SetActive(true);
                brickActiveController = true;
                Destroy(gameObject.transform.GetChild(2).gameObject);
                point.transform.position = startPoint.transform.position;
                materialCount = 0;
            }
            return;
        }
        if (collision.gameObject.layer == 7)
        {
            if (materialCount > 15)
            {
                concreteMachine.transform.GetChild(0).gameObject.SetActive(false);
                concreteMachine.transform.GetChild(1).gameObject.SetActive(true);
                concreteActiveController = true;
                Destroy(gameObject.transform.GetChild(2).gameObject);
                point.transform.position = startPoint.transform.position;
                materialCount = 0;
            }
            return;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (ironCount > 1)
                concreteCrushController = true;
            else
                concreteCrushController = false;
            return;
        }
        if (collision.gameObject.layer == 12)
        {
            if (brickCount > 1)
                brickController = true;
            else
                brickController = false;
            return;
        }
        if (collision.gameObject.layer == 13)
        {
            if (concreteBrickCount > 1)
                concreteBrickController = true;
            else
                concreteBrickController = false;
            return;
        }
    }
}