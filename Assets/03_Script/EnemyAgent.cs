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
        // 새로운 에피소드 시작시 초기화
        playerInRange = false;
        transform.position = new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f));
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 관찰 정보 수집 (예: 적과 플레이어의 거리)
        sensor.AddObservation(Vector3.Distance(transform.position, player.position));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // 행동 받음 (예: 이동 및 회전)
        var continuousActions = actionBuffers.ContinuousActions;
        float moveInput = continuousActions[0];
        float rotateInput = continuousActions[1];

        rb.velocity = transform.forward * moveInput * moveSpeed;
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.fixedDeltaTime);

        // 플레이어와 충돌시 벌을 주고 에피소드 종료
        if (playerInRange)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 플레이어가 직접 제어할 때 사용할 휴리스틱 정책 (예: 키보드 입력)
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

    public void PawnPlayer()
    {
        playerInRange = true;
    }
}