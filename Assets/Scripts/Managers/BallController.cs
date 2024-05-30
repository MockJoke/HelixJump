using UnityEngine;

public class BallController : MonoBehaviour 
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    
    [Header("Movement Fields")]
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private GameObject splashPrefab;
    [SerializeField] private float splashOffsetY = 0.19f;
    
    // [HideInInspector] public int perfectPass = 0;
    private Vector3 startPos;
    // private bool ignoreNextCollision;
    // private bool isSuperSpeedActive;

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        
        startPos = transform.position;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        AddSplash(collision.transform);
        
        rb.velocity = new Vector3(rb.velocity.x, bounceForce * Time.deltaTime, rb.velocity.z);

        if (collision.transform.GetComponent<Section>())
        {
            SectionType sectionType = collision.transform.GetComponent<Section>().GetSectionType();

            switch (sectionType)
            {
                case SectionType.danger:
                {
                    DangerSection dangerSection = collision.transform.GetComponent<DangerSection>();
                    if (dangerSection)
                        dangerSection.OnHitDangerSection();
                    
                    break;
                }
                case SectionType.drop:
                {
                    DroppingSection dangerSection = collision.transform.GetComponent<DroppingSection>();
                    if (dangerSection)
                        dangerSection.Drop();
                    
                    break;
                }
            }
        }

        #region unused code
        //         if (ignoreNextCollision)
//             return;
//
//         if (isSuperSpeedActive)
//         {
//             if (!collision.transform.GetComponent<GoalSection>())
//             {
//                 /*foreach (Transform t in other.transform.parent)
//                 {
//                     gameObject.AddComponent<TriangleExplosion>();
//
//                     StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
//                     //Destroy(other.gameObject);
//                     Debug.Log("exploding - exploding - exploding - exploding");
//                 }*/
//                 Destroy(collision.transform.parent.gameObject);           //destroying parent because the ball will hit a part so we dont want to destroy only a part, we want to destroy the whole stage 
//             }
//         }
//         //if super speed is not active and a death part gets hit -> restart game
//         else
//         {   
//             //adding reset level functionality via death part --> initialized when deathpart gets hit 
//             DangerSection dangerSection = collision.transform.GetComponent<DangerSection>();
//             
//             if (dangerSection)
//                 dangerSection.OnHitDangerSection();
//         }

        // rb.velocity = Vector3.zero;     //remove velocity to not make the ball jump higher after falling done a greater distance
        // rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);      //pushes the ball up

        //safety check
        // ignoreNextCollision = true;
        // Invoke(nameof(AllowCollision), .2f);      //not to allow 2 forces to act within a short period of time

        //deactivating super speed
        // perfectPass = 0;
        // isSuperSpeedActive = false;
        #endregion
    }

    private void Update()
    {
        //activate super speed
        // if (perfectPass >= 3 && !isSuperSpeedActive)
        // {
        //     isSuperSpeedActive = true;
        //     rb.AddForce(Vector3.down * 10, ForceMode.Impulse);          //adding extra force to make ball move faster to get super speed
        // }
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }

    // private void AllowCollision()
    // {
    //     ignoreNextCollision = false;
    // }

    private void AddSplash(Transform section)
    {
        GameObject splash = Instantiate(splashPrefab,
                    new Vector3(transform.position.x, section.position.y + splashOffsetY, transform.position.z),
                            transform.rotation, section);
        splash.transform.localScale = Vector3.one * Random.Range(0.175f, 0.25f);
        
        Destroy(splash, 5f);
    }
}
