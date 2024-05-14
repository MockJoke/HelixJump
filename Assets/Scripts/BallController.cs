using UnityEngine;

public class BallController : MonoBehaviour 
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float impulseForce = 5f;

    private Vector3 startPos;
    [HideInInspector] public int perfectPass = 0;
    private bool ignoreNextCollision;
    public bool isSuperSpeedActive;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        
        startPos = transform.position;
    }
    
    private void OnCollisionEnter(Collision CollidedObj)
    {
        if(ignoreNextCollision)
            return;

        if(isSuperSpeedActive)
        {
            if(!CollidedObj.transform.GetComponent<Goal>())
            {
                /*foreach (Transform t in other.transform.parent)
                {
                    gameObject.AddComponent<TriangleExplosion>();

                    StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
                    //Destroy(other.gameObject);
                    Debug.Log("exploding - exploding - exploding - exploding");
                }*/
                Destroy(CollidedObj.transform.parent.gameObject);           //destroying parent because the ball will hit a part so we dont want to destroy only a part, we want to destroy the whole stage 
            }

        }
        //if super speed is not active and a death part gets hit -> restart game
        else
        {   
            //adding reset level functionality via death part --> initialized when deathpart gets hit 
            DeathPart deathPart = CollidedObj.transform.GetComponent<DeathPart>();
            
            if(deathPart)
                deathPart.HitDeathPart();
        }

        rb.velocity = Vector3.zero;     //remove velocity to not make the ball jump higher after falling done a greater distance
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);      //pushes the ball up

        //safety check
        ignoreNextCollision = true;
        Invoke(nameof(AllowCollision), .2f);      //not to allow 2 forces to act within a short period of time

        //deactivating super speed
        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void Update()
    {
        //activate super speed
        if(perfectPass >= 3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);          //adding extra force to make ball move faster to get super speed
        }
    }

    public void ResetBall()     //to set the pos of ball when level gets restarted
    {
        transform.position = startPos;
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }
}
