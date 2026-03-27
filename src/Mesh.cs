using System;

namespace obj_voxelizer
{
    public class Mesh
    {
        public List<Vectorz> vertices = new List<Vectorz>();
        public List<(int, int, int)> faces = new List<(int, int, int)>();

        public Mesh(List<Vectorz> vertices, List<(int, int, int)> faces)
        {
            this.vertices = vertices;
            this.faces = faces;
        }

        public AABB getBounds()
        {
            Vectorz min = vertices[0];
            Vectorz max = vertices[0];

            foreach (var v in vertices)
            {
                min = Vectorz.Min(min, v);
                max = Vectorz.Max(max, v);
            }

            return new AABB(min, max);
        }
    }
}