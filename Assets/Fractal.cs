using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material mat;
    public int maxDepth;
    public float childScale;

    private int depth;
    private static Vector3[] childDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back,
        //special consideration since only added to root
        Vector3.down
    };

    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f),
        //down only on root
        Quaternion.Euler(0f, 0f, 180f)
    };

    private void Start()
    {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = mat;
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length - 1; i++)
        {
            yield return new WaitForSeconds(0.5f);
            new GameObject("FractalChild").AddComponent<Fractal>().Initialize(this, i);
        }

        if (this.depth == 0)
        {
            yield return new WaitForSeconds(0.5f);
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, childDirections.Length - 1);
        }
    }

    private void Initialize (Fractal parent, int i)
    {
        mesh = parent.mesh;
        mat = parent.mat;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[i] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[i];
    }
}
