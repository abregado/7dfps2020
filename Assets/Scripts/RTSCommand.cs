using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class RTSCommand : MonoBehaviour {
    public Camera cam;
    public NavMeshSurface navMesh;
    public int mouseButtonIndex = 0;
    private bool _instantTravel = false;
    private ThirdPersonCharacter _character;
    public NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) {
            _instantTravel = true;
        }

        _character = GetComponent<ThirdPersonCharacter>();
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (agent.remainingDistance > agent.stoppingDistance) {
            _character.Move(agent.desiredVelocity, false,false);
        }
        else {
            agent.ResetPath();
            _character.Move(Vector3.zero, false,false);
        }
        
        if (Input.GetMouseButtonDown(mouseButtonIndex)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (_instantTravel) {
                    transform.SetPositionAndRotation(hit.point, Quaternion.identity);
                }
                else {
                    Debug.Log("destination set");
                    agent.SetDestination(hit.point);
                }
            }
            navMesh.BuildNavMesh();
        }

        
        
            
    }
}
