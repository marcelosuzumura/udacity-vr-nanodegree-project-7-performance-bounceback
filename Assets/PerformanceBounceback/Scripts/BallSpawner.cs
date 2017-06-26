using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour {

    public static BallSpawner current;

    public GameObject pooledBall; //the prefab of the object in the object pool
    public int ballsAmount = 200; //the number of objects you want in the object pool. marcelo: raised from 20 to 200
    public List<GameObject> pooledBalls; //the object pool
    public static int ballPoolNum = 0; //a number used to cycle through the pooled objects

    private float cooldown;
    private float cooldownLength = 0.5f;

    void Awake() {
        current = this; //makes it so the functions in ObjectPool can be accessed easily anywhere
    }

    void Start() {
        // Create Pool
        pooledBalls = new List<GameObject>();
        for (int i = 0; i < ballsAmount; i++) {
            GameObject obj = Instantiate(pooledBall);
            obj.SetActive(false);
            pooledBalls.Add(obj);
        }
    }

	public GameObject GetPooledBall() {
	    ballPoolNum++;
	    if (ballPoolNum > (ballsAmount - 1)) {
	        ballPoolNum = 0;
	    }

		// marcelo: the code below instantiates balls indefinitely, which is bad because the balls are never discarded and accumulates over time
		// so at some point, the game WILL crash

//		// if we’ve run out of objects in the pool too quickly, create a new one
//	    if (pooledBalls[ballPoolNum].activeInHierarchy) {
//	        // create a new bullet and add it to the bulletList
//	        GameObject obj = Instantiate(pooledBall);
//	        pooledBalls.Add(obj);
//	        ballsAmount++;
//	        ballPoolNum = ballsAmount - 1;
//	    }

		// marcelo: instead, I'll just raise the number of balls instantiated initially and keep recycling them
		// we just have to be careful not to recycle a ball while they are still bouncing, so keeping a high initial count must do the trick
		// and because the player will throw mainly the newly spawned balls, the older ones are (almost) guaranteed
		// to be just rolling on the ground, unused and ready to be recycled

//        Debug.Log(ballPoolNum);
        return pooledBalls[ballPoolNum];
	}
   	
	void Update () {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0) {
            cooldown = cooldownLength;
            SpawnBall();
        }		
	}

    void SpawnBall() {
        GameObject selectedBall = BallSpawner.current.GetPooledBall();
        selectedBall.transform.position = transform.position;

		// this GetComponent only occurs every cooldownLength seconds, so it doesn't need to be cached.
        Rigidbody selectedRigidbody = selectedBall.GetComponent<Rigidbody>();
        selectedRigidbody.velocity = Vector3.zero;
        selectedRigidbody.angularVelocity = Vector3.zero;
        selectedBall.SetActive(true);
    }

}
