using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WonderingAI : MonoBehaviour {
	public float speed = 3.0f;
	public float obstacleRange = 2.5f;

	public float HorSight = 120f;
	public float VerSight = 60f;
	public float sightDis = 10f;

	private NavMeshAgent agent;

	public Transform[] points;
	private int destPoint = 0;

	[SerializeField] private Transform target;
	private Vector3 leftDir, rightDir, upDir, downDir;

	private enum State {
		Wondering, Seeking, Alerted, Stopped
	};
	private State state;

	void Start() {
		state = State.Wondering;
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update() {
		bool inSight = InSight ();
		if (state == State.Wondering && inSight) {
			// Debug.Log ("player discovered!");
			state = State.Seeking;
		} else if (!inSight && state == State.Seeking){
			// Debug.Log ("Setting state to wondering");
			state = State.Wondering;
		}

		// AI behaves according to its current state
		switch (state) {
		case State.Wondering:
			//Debug.Log (lastKnownPos);
			if (!agent.pathPending && agent.remainingDistance < 0.5f)
				GotoNextPoint();
			break;
		case State.Seeking:
			agent.destination = target.position;
			break;
		case State.Alerted:
			
			break;
		case State.Stopped:
			
			break;
		default:
			break;
		}

	}
		

	void GotoNextPoint() {
		// Returns if no points have been set up
		if (points.Length == 0)
			return;

		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = Random.Range(0, points.Length - 1);
	}

	// Determine whether the player is in sight or not
	// some code adjusted from https://www.jianshu.com/p/43148b88765f
	private bool InSight() {
		Vector3 tempForward = transform.forward;

		Quaternion left = Quaternion.AngleAxis (-HorSight / 2, transform.up);
		Quaternion right = Quaternion.AngleAxis (HorSight / 2, transform.up);
		Quaternion up = Quaternion.AngleAxis (-VerSight / 2, transform.right);
		Quaternion down = Quaternion.AngleAxis (VerSight / 2, transform.right);

		leftDir = left * tempForward;
		rightDir = right * tempForward;
		upDir = up * tempForward;
		downDir = down * tempForward;

		Debug.DrawLine (transform.position, transform.position + leftDir * sightDis, Color.blue);
		Debug.DrawLine (transform.position, transform.position + rightDir * sightDis, Color.red);
		Debug.DrawLine (transform.position, transform.position + upDir * sightDis, Color.white);
		Debug.DrawLine (transform.position, transform.position + downDir * sightDis, Color.green);

		// In the sight distance, then determine whether it's really in the sight
		if (Vector3.Distance(transform.position,target.position) < sightDis) {
			Vector3 dir = target.transform.position - transform.position;

			// Judge whether there are obstacles between player and AI
			Ray ray = new Ray(transform.position, dir);
			RaycastHit hit;
			if (Physics.SphereCast (ray, 1.0f, out hit)) {
				GameObject hitObject = hit.transform.gameObject;
				if (hitObject.name != "Player") {
					// Debug.Log ("Obstacles between player and this AI");
					return false;
				}
			}

			Vector3 targetHorPos = new Vector3(dir.x,leftDir.y, dir.z);
			Vector3 targetVerPos = new Vector3(upDir.x, dir.y, dir.z);

			Vector3 leftCross = Vector3.Cross(targetHorPos,leftDir);
			Vector3 rightCross = Vector3.Cross (targetHorPos,rightDir);

			Vector3 upCross = Vector3.Cross (targetVerPos, upDir);
			Vector3 downCross = Vector3.Cross (targetVerPos, downDir);

			if (Vector3.Dot(dir, transform.forward) > 0) {

				if (Vector3.Dot(leftCross, rightCross) < 0) {
					// Debug.Log("In horizontal sight");
					return true;
				}
				if (Vector3.Dot(upCross, downCross) < 0) {
					// Debug.Log("In vertical sight");
					return true;
				}
			}
		}
		return false;
	}
}
