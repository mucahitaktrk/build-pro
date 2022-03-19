using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _ironPrefab;
    [SerializeField] private Transform ironTransform;
    public List<GameObject> ironList = new List<GameObject>();
    private Vector3 _ironSpawnVector;
    [SerializeField] private float ironLeftDistance = 0.0f;
    [SerializeField] private float ironBackDistance = 0.0f;

    private void Start()
    {
        _ironSpawnVector = ironTransform.position;
        StartCoroutine(nameof(IronSpawn));
    }

    private void Update()
    {
        if (ironList.Count == 0)
        {
            ironTransform.position = _ironSpawnVector;
        }
    }

    IEnumerator IronSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (ironList.Count < 18)
            {
                ironList.Add(Instantiate(_ironPrefab, ironTransform.position, Quaternion.identity));
                if (ironTransform.position.x > 20)
                {
                    ironTransform.position += Vector3.left * ironLeftDistance;
                }
                else
                {
                    ironTransform.position += Vector3.back * ironBackDistance;
                    ironTransform.position -= Vector3.left * 20;
                }

            }
        }

    }
}
