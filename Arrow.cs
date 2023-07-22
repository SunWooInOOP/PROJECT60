using System.Linq;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviourPun
{
    public GameObject player;
    public GameObject arrow;

    public void Awake()
    {
        if (photonView.IsMine) arrow.SetActive(true);
        else
        {
            arrow.SetActive(false);
        }
    }

    public void OnEnable()
    {
        if (photonView.IsMine) arrow.SetActive(true);
        else
        {
            arrow.SetActive(false);
        }
    }

    void Update()
    {
        GameObject closestRunner = FindClosestRunner();

        if (closestRunner != null)
        {
            PointArrowAtRunner(closestRunner);
        }
    }

    GameObject FindClosestRunner()
    {
        GameObject[] runners = GameObject.FindGameObjectsWithTag("RunPlayer");

        if (runners.Length == 0)
        {
            return null;
        }

        return runners.OrderBy(runner => Vector3.Distance(player.transform.position, runner.transform.position)).First();
    }

    void PointArrowAtRunner(GameObject runner)
    {
        // Calculate the direction vector from the arrow to the runner.
        Vector3 directionToRunner = runner.transform.position - arrow.transform.position;

        // Project the direction vector onto the xz plane (set y coordinate to 0).
        directionToRunner.y = 0f;

        // Normalize the direction vector.
        directionToRunner.Normalize();

        // Calculate the rotation to look at the runner.
        Quaternion lookRotation = Quaternion.LookRotation(directionToRunner, Vector3.up);

        // Apply the rotation to the arrow.
        arrow.transform.rotation = Quaternion.Euler(90f, lookRotation.eulerAngles.y, 0f);
    }



}

