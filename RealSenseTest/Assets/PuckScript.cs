using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UIElements;


public class PuckScript : MonoBehaviour
{
    public int currentValue = 0;
    public GameObject sparkEffectObj;
    [SerializeField] private float _timerValue = 5;
    public PuckCoreScript coreScript;
    private Rigidbody rb;
    public float movingSpeed = 15f;
    public Vector3 targetPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(transform.position != targetPos)
        {
            Vector3 direction = targetPos - transform.position;
            rb.velocity = direction * (movingSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Entered trigger");
        if (other.gameObject.layer != 3) return;
        ValueArea valueArea = other.gameObject.GetComponent<ValueArea>();
        
        if (other.gameObject.name != coreScript.currentLayerName) return;
        currentValue = valueArea.value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Hit! - {collision.gameObject.name}");
        if (collision.gameObject.tag != "Puck") return;
        Debug.Log("Pucks collided!");
        SpawnSpark(collision.GetContact(0).point);
    }

    private void SpawnSpark(Vector3 pos)
    {
        GameObject sparkObj = Instantiate(sparkEffectObj, pos, new Quaternion(0,0,0,0));
        ParticleSystem particles = sparkObj.GetComponent<ParticleSystem>();
        particles.Play();
        StartCoroutine(SparkTimer(sparkObj));
    }

    private IEnumerator SparkTimer(GameObject obj)
    {
        yield return new WaitForSeconds(_timerValue);
        Destroy(obj);
    }

}

