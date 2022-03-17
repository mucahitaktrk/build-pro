using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class JoystickPlayerExample : MonoBehaviour
{
    //--PlayerScript---------------------------------------------
    [SerializeField] private float speed = 10f;
    private float rotationSpeed = 720f;
    private FloatingJoystick floatingJoystick;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private int ironCount = 0;
    private List<GameObject> _materialList = new List<GameObject>();
    private List<GameObject> _ironList = new List<GameObject>();


    [SerializeField] private Transform _collectedPos = null;

    [SerializeField] private GameObject _ironPrefab = null;

    private void Awake()
    {
        Components();
    }

    private void Start()
    {
        Tags();
    }

    public void Update()
    {
        MoveController();
    }

    private void MoveController()
    {
        playerRigidbody.velocity = new Vector3(floatingJoystick.Horizontal * speed , 0, floatingJoystick.Vertical * speed);
        playerAnimator.SetFloat("CharacterSpeed", Mathf.Abs(playerRigidbody.velocity.z));
        playerAnimator.SetFloat("CharacterSpeed", Mathf.Abs(playerRigidbody.velocity.x));
        Vector3 movementDirection = new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical);
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Tags()
    {
        floatingJoystick = GameObject.FindGameObjectWithTag("FloatingJoystick").GetComponent<FloatingJoystick>();
    }

    private void Components()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void IronPlusSystem()
    {
        if (_materialList.Count <= 20)
        {
            _ironList.Add(_materialList[_materialList.Count - 1]);
            _materialList[_materialList.Count - 1].transform.DOMove(_collectedPos.position, 2f);
            _collectedPos.position += new Vector3(0, 0.5f, 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

}