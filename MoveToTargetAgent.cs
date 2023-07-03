using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToTargetAgent : Agent
{

    [SerializeField] private Transform target;


    public override void OnEpisodeBegin()
    {
        target.localPosition = new Vector3(Random.Range(-4, 4), 1f, Random.Range(-2.55f, 2.55f));
        transform.localPosition = new Vector3(Random.Range(-4, 4), 1f, Random.Range(-2.55f, 2.55f));

    }


    public override void CollectObservations(VectorSensor sensor){
        
        sensor.AddObservation((Vector3)transform.localPosition);
        sensor.AddObservation((Vector3)target.localPosition);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

    }


    public override void OnActionReceived(ActionBuffers actions){
    float moveX = actions.ContinuousActions[0];
    float moveZ = actions.ContinuousActions[1];
    float movementSpeed = 10f;

    transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * movementSpeed; 
}


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Target target))
        {

            AddReward(10f);
            EndEpisode();

        }else if (collision.TryGetComponent(out Wall wall)){

            AddReward(-2f);
            EndEpisode();

        }


    }

}
