using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundedQuadMesh : MonoBehaviour
{
    public float RoundEdges = 0.5f;
    public float RoundTopLeft = 0.0f;
    public float RoundTopRight = 0.0f;
    public float RoundBottomLeft = 0.0f;
    public float RoundBottomRight = 0.0f;
    public float Size = 1f;
    public int CornerVertexCount = 8;
    public bool CreateUV = true;
    public bool DoubleSided = false;
    public bool AutoUpdate = true;

    private MeshFilter m_MeshFilter;
    private Mesh m_Mesh;
    private Vector3[] m_Vertices;
    private Vector3[] m_Normals;
    private Vector2[] m_UV;
    private int[] m_Triangles;

    void Start ()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
        if (m_MeshFilter == null)
            m_MeshFilter = gameObject.AddComponent<MeshFilter>();
        if (GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        m_Mesh = new Mesh();
        m_MeshFilter.sharedMesh = m_Mesh;
        UpdateMesh();
    }

    public Mesh UpdateMesh()
    {
        if (CornerVertexCount<2)
            CornerVertexCount = 2;
        int sides = DoubleSided ? 2 : 1;
        int vCount = CornerVertexCount * 4 * sides + sides;
        int triCount = (CornerVertexCount * 4) * sides;
        if (m_Vertices == null || m_Vertices.Length != vCount)
        {
            m_Vertices = new Vector3[vCount];
            m_Normals = new Vector3[vCount];
        }
        if (m_Triangles == null || m_Triangles.Length != triCount * 3)
            m_Triangles = new int[triCount * 3];
        if (CreateUV && (m_UV == null || m_UV.Length != vCount))
        { 
            m_UV = new Vector2[vCount];
        }
        float f = 1f / (CornerVertexCount-1);
        m_Vertices[0] = Vector3.zero;
        int count = CornerVertexCount * 4;
        if (CreateUV)
        {
            m_UV[0] = Vector2.one *0.5f;
            if (DoubleSided)
                m_UV[count + 1] = m_UV[0];
        }
        
        for (int i = 0; i < CornerVertexCount; i++ )
        {
            float s = Mathf.Sin((float)i * Mathf.PI * 0.5f*f);
            float c = Mathf.Cos((float)i * Mathf.PI * 0.5f*f);
            float tl = Mathf.Clamp01(RoundTopLeft + RoundEdges);
            float tr = Mathf.Clamp01(RoundTopRight + RoundEdges);
            float bl = Mathf.Clamp01(RoundBottomLeft + RoundEdges);
            float br = Mathf.Clamp01(RoundBottomRight + RoundEdges);
            Vector2 v1 = new Vector3(-1f + tl - c * tl, 1 - tl + s * tl);
            Vector2 v2 = new Vector3(1f - tr + s * tr, 1f - tr + c * tr);
            Vector2 v3 = new Vector3(1f - br + c * br, -1f + br - s * br);
            Vector2 v4 = new Vector3(-1f + bl - s * bl, -1f + bl - c * bl);

            m_Vertices[1 + i] = v1 * Size;
            m_Vertices[1 + CornerVertexCount + i] = v2 * Size;
            m_Vertices[1 + CornerVertexCount * 2 + i] = v3 * Size;
            m_Vertices[1 + CornerVertexCount * 3 + i] = v4 * Size;
            if (CreateUV)
            {
                m_UV[1 + i] = v1 * 0.5f + Vector2.one * 0.5f;
                m_UV[1 + CornerVertexCount * 1 + i] = v2 * 0.5f + Vector2.one * 0.5f;
                m_UV[1 + CornerVertexCount * 2 + i] = v3 * 0.5f + Vector2.one * 0.5f;
                m_UV[1 + CornerVertexCount * 3 + i] = v4 * 0.5f + Vector2.one * 0.5f;
            }
            if (DoubleSided)
            {
                // The backside vertices are in reverse order
                m_Vertices[1 + CornerVertexCount * 7 + CornerVertexCount - i] = v1 * Size;
                m_Vertices[1 + CornerVertexCount * 6 + CornerVertexCount - i] = v2 * Size;
                m_Vertices[1 + CornerVertexCount * 5 + CornerVertexCount - i] = v3 * Size;
                m_Vertices[1 + CornerVertexCount * 4 + CornerVertexCount - i] = v4 * Size;
                if (CreateUV)
                {
                    m_UV[1 + CornerVertexCount * 7 + CornerVertexCount - i] = v1 * 0.5f + Vector2.one * 0.5f;
                    m_UV[1 + CornerVertexCount * 6 + CornerVertexCount - i] = v2 * 0.5f + Vector2.one * 0.5f;
                    m_UV[1 + CornerVertexCount * 5 + CornerVertexCount - i] = v3 * 0.5f + Vector2.one * 0.5f;
                    m_UV[1 + CornerVertexCount * 4 + CornerVertexCount - i] = v4 * 0.5f + Vector2.one * 0.5f;
                }
            }
        }
        for (int i = 0; i < count + 1;i++ )
        {
            m_Normals[i] = -Vector3.forward;
            if (DoubleSided)
                m_Normals[count + 1 + i] = Vector3.forward;
        }
        
        for (int i = 0; i < count; i++)
        {
            m_Triangles[i*3    ] = 0;
            m_Triangles[i*3 + 1] = i + 1;
            m_Triangles[i*3 + 2] = i + 2;
            if (DoubleSided)
            {
                m_Triangles[(count + i) * 3] = count+1;
                m_Triangles[(count + i) * 3 + 1] = count+1 +i + 1;
                m_Triangles[(count + i) * 3 + 2] = count+1 +i + 2;
            }
        }
        m_Triangles[count * 3 - 1] = 1;
        if (DoubleSided)
            m_Triangles[m_Triangles.Length - 1] = count + 1 + 1;

        m_Mesh.Clear();
        m_Mesh.vertices = m_Vertices;
        m_Mesh.normals = m_Normals;
        if (CreateUV)
            m_Mesh.uv = m_UV;
        m_Mesh.triangles = m_Triangles;
        
        return m_Mesh;
    }
    void Update ()
    {
        if (AutoUpdate)
            UpdateMesh();
    }
}
