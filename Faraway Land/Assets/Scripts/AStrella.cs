using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStrella : MonoBehaviour
{
   public static AStrella instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Node> GeneratePath(Node start, Node end)
    {
        List<Node> openSet = new List<Node>();


        foreach(Node n in FindObjectsOfType<Node>())
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0) 
        { 
            int lowestF = default;

            for (int i = 0; i < openSet.Count; i++) 
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore()) 
                { 
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);

            if (currentNode == end) 
            { 
                List<Node> path = new List<Node>();

                path.Insert(0, end);

                while (currentNode != start) 
                {
                    currentNode = currentNode.cameFrom;
                    path.Add(currentNode);
                }
                path.Reverse();
                return path;
            }

            foreach (Node connectedNode in currentNode.connections)
            {
                float heldScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);

                if (heldScore < connectedNode.gScore) 
                { 
                    connectedNode.cameFrom = currentNode;
                    connectedNode.gScore = heldScore;
                    connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);

                    if(!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }
        
        
        
        
        return null;
    }

    public Node FindNearestNode(Vector2 position)
    {
        Node foundNode = null;
        float minDistance = float.MaxValue;

        foreach(Node node in NodesInScene())
        {
            float currentDistance = Vector2.Distance(position, node.transform.position);
            if(currentDistance < minDistance)
            {
                minDistance = currentDistance;
                foundNode = node;
            }
        }
        return foundNode;
    }

    public Node FindNFurthestNode(Vector2 position)
    {
        Node foundNode = null;
        float maxDistance = 0;

        foreach(Node node in NodesInScene())
        {
            float currentDistance = Vector2.Distance(position, node.transform.position);
            if(currentDistance > maxDistance)
            {
                maxDistance = currentDistance;
                foundNode = node;
            }
        }
        return foundNode;
    }

    public Node[] NodesInScene()
    {
        return FindObjectsOfType<Node>();
    }




}
