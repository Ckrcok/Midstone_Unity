
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10.0f;
    public float range = 100.0f;
    public float impactForce = 30f;
    public float impactRate = 15f;
    public Camera fpsCam;
    public ParticleSystem shotEffect;
    public GameObject ImpactEffect;
    private float nextTimeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f/impactRate;
            Shoot();
        }
    }


    void Shoot()
    {
        shotEffect.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("RAYCAST WORKED++");

            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
                Debug.Log( "TAKE DAMAGE++");

            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);

            }

            GameObject impatGO = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impatGO, 2f);
            Debug.Log(hit.transform.name + "---");
        }
    }
}
