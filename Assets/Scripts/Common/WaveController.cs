using UnityEngine;

public class WaveController : MonoBehaviour {
	[Header("Setup")]
	[Range(1, 50)]
	public int segments = 10;
	public bool onlyTopEdge = true; //TODO refactor to be able to select side

	[Header("General")]
	public float scale = 0.1f;
	public float waveScale = 1.0f;
	public float speed = 2.0f;

	[Header("Noise")]
	public float noiseStrength = 1f;
	public float noiseWalk = 1f;

	Vector3[] baseHeight;

	Mesh mesh;
	
	void Start () {
		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh m = new Mesh();
		mf.mesh = m;

		m.vertices = GenerateVertices(segments, -0.5f);
		m.triangles = GenerateTriangles(segments);

		Vector2[] uv = new Vector2[m.vertices.Length];
		for (int i = 0; i < uv.Length; i++) {
			uv[i] = m.vertices[i];
		}

		Vector3[] normals = new Vector3[m.vertices.Length];
		for (int i = 0; i < normals.Length; i++) {
			normals[i] = -Vector3.forward;
		}
		
		m.normals = normals;

		m.RecalculateBounds();
		m.RecalculateNormals();

		mesh = m;
	}

	void Update () {
		if (baseHeight == null) {
			baseHeight = mesh.vertices;
		}

		Vector3[] vertices = new Vector3[baseHeight.Length];

		for (int i = 0; i < baseHeight.Length; i++) {
			if (i < baseHeight.Length / 2 || !onlyTopEdge) {
				Vector3 vertex = baseHeight[i];

				vertex.y += Mathf.Sin (Time.time * speed + vertex.y + (vertex.x * waveScale)) * scale;

				vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;

				vertices[i] = vertex;
			} else {
				vertices[i] = mesh.vertices[i];
			}
		}

		mesh.vertices = vertices;
		mesh.RecalculateNormals();
	}

	Vector3[] GenerateVertices(int segmentCount, float modifier) {
		float min = 0.0f + modifier;
		float max = 1.0f + modifier;

		int count = 2 + (2 * segmentCount);

		Vector3[] vertices = new Vector3[count];
		float halfLength = vertices.Length / 2.0f;
		float mod = (halfLength - 1) / ((halfLength - 1) * (halfLength - 1));
		for (int i = 0; i < halfLength; i++) {

			float x = mod * i + modifier;

			vertices[i] = new Vector3(x, max, 0.0f);
			vertices[i + vertices.Length / 2] = new Vector3(x, min, 0.0f);
		}

		return vertices;
	}

	int[] GenerateTriangles(int segmentCount) {
		int verticeCount = 2 + (2 * segmentCount);

		int a,b,c,d;
		a = verticeCount / 2;
		b = verticeCount / 2 + 1;
		c = 0;
		d = 1;

		int[] tris = new int[segmentCount * 6];
		for (int i = 0; i < segmentCount * 6; i += 6) {
			tris[i] = a;
			tris[i+1] = c;
			tris[i+2] = b;

			tris[i+3] = c;
			tris[i+4] = d;
			tris[i+5] = b;

			a++;
			b++;
			c++;
			d++;
		}

		return tris;
	}
}
