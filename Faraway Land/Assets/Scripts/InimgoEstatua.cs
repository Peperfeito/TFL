using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimgoEstatua : MonoBehaviour
{
    public Node currentNode;
    public List<Node> path;

    public Player player;
    private float speed = 3;
    [SerializeField] GameObject telaMorte;

    public enum StateMachine
    {
        Patrol,
        Engage,
        Stop
    }

    public StateMachine currentState;

    private void Start()
    {
        currentState = StateMachine.Patrol;
    }

    private void Update()
    {
        switch(currentState)
        {
            case StateMachine.Patrol:
                Patrol();
                break;
            case StateMachine.Engage:
                Engage();
                break;
            case StateMachine.Stop:
                Stop();
                break;
        }

        bool playerSeen = Vector2.Distance(transform.position, player.transform.position) < 5.0f;

        if(playerSeen == false && currentState != StateMachine.Patrol)
        {
            currentState = StateMachine.Patrol;
            path.Clear();
        }
        else if(playerSeen == true && currentState != StateMachine.Engage)
        {
            currentState = StateMachine.Engage;
            path.Clear();
        }
        else if (currentState != StateMachine.Engage)
        {
            currentState = StateMachine.Engage;
            path.Clear();
        }

        CreatePath();
    }

    void Patrol()
    {
        if(path.Count == 0)
        {
            path = AStrella.instance.GeneratePath(currentNode, AStrella.instance.NodesInScene()[Random.Range(0, AStrella.instance.NodesInScene().Length)]);
        }

    }

    void Engage()
    {
        if (path.Count == 0)
        {
            path = AStrella.instance.GeneratePath(currentNode, AStrella.instance.FindNearestNode(player.transform.position));
        }
    }

    void Stop()
    {
        if (path.Count == 0)
        {
            path = null;
        }
    }

    void CreatePath()
    {
        if (path.Count > 0)
        {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[x].transform.position.x, path[x].transform.position.y, -2), speed * Time.deltaTime);
            
            if(Vector2.Distance(transform.position, path[x].transform.position) < 0.1f )
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entrou?");

        if(collision.CompareTag("Player"))
        {
            telaMorte.SetActive(true);
            //Destroy(collision.gameObject);
        }
    }



}
