using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Path : MonoBehaviour
{
    public List<GameObject> nodes;
    List<PathSegment> segments;

    void Start()
    {
        segments = GetSegments();
    }

    public List<PathSegment> GetSegments()
    {
        List<PathSegment> segments = new List<PathSegment>();
        int i;
        for (i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 src = nodes[i].transform.position;
            Vector3 dst = nodes[i + 1].transform.position;
            PathSegment segment = new PathSegment(src, dst);
            segments.Add(segment);
        }
        return segments;
    }

    public float GetParam(Vector3 position, float lastParam)
    {
        // find closest segment to agent
        float param = 0f;
        PathSegment currentSegment = null;
        float tempParam = 0f;
        foreach (PathSegment ps in segments)
        {
            tempParam += Vector3.Distance(ps.a, ps.b);
            if (lastParam <= tempParam)
            {
                currentSegment = ps;
                break;
            }
        }
        if (currentSegment == null)
            return 0f;

        Vector3 currPos = position - currentSegment.a;
        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();
        Vector3 pointInSegment = Vector3.Project(currPos, segmentDirection);

        param = tempParam - Vector3.Distance(currentSegment.a, currentSegment.b);
        param += pointInSegment.magnitude;
        return param;
    }

    public Vector3 GetPosition(float param)
    {
        // Given current location along a path, find the corresponding segment
        Vector3 position = Vector3.zero;
        PathSegment currentSegment = null;
        float tempParam = 0f;
        foreach (PathSegment ps in segments)
        {
            tempParam += Vector3.Distance(ps.a, ps.b);
            if (param <= tempParam)
            {
                currentSegment = ps;
                break;
            }
        }
        if (currentSegment == null)
            return Vector3.zero;

        // Converts parameter as a spatial point and returns it
        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();
        tempParam -= Vector3.Distance(currentSegment.a, currentSegment.b);
        tempParam = param - tempParam;
        position = currentSegment.a + segmentDirection * tempParam;
        return position;
    }

    void OnDrawGizmos()
    {
        Vector3 direction;
        Color tmp = Gizmos.color;
        Gizmos.color = Color.magenta;//example color
        int i;
        for (i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 src = nodes[i].transform.position;
            Vector3 dst = nodes[i + 1].transform.position;
            direction = dst - src;
            Gizmos.DrawRay(src, direction);
        }
        Gizmos.color = tmp;
    }
}