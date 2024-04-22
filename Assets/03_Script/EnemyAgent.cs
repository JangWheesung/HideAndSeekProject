using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EnemyAgent : Agent
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 120f;
    public Transform player;
    public LayerMask playerLayer;

    private Rigidbody rb;
    private bool playerInRange;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // ���ο� ���Ǽҵ� ���۽� �ʱ�ȭ
        playerInRange = false;
        transform.position = new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f));
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // ���� ���� ���� (��: ���� �÷��̾��� �Ÿ�)
        sensor.AddObservation(Vector3.Distance(transform.position, player.position));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // �ൿ ���� (��: �̵� �� ȸ��)
        var continuousActions = actionBuffers.ContinuousActions;
        float moveInput = continuousActions[0];
        float rotateInput = continuousActions[1];

        rb.velocity = transform.forward * moveInput * moveSpeed;
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.fixedDeltaTime);

        // �÷��̾�� �浹�� ���� �ְ� ���Ǽҵ� ����
        if (playerInRange)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // �÷��̾ ���� ������ �� ����� �޸���ƽ ��å (��: Ű���� �Է�)
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

    public void PawnPlayer()
    {
        playerInRange = true;
    }
}