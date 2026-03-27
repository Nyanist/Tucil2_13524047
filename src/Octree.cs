using System;

namespace obj_voxelizer
{
    public class Octree
    {
        public AABB bounds;
        public bool 
        isLeaf = true; // buat ngecek 'fit' ke mesh, tapi gajadi di implement hehe
        public Octree[] child = new Octree[8];
        public int depth;
        // public Mesh mesh;
        public static double EPSILON = 1e-6;

        public Octree(AABB bounds, int depth = 0)
        {
            this.bounds = bounds;
            this.depth = depth;
        }

        public void GetChildren()
        {
            Vectorz center = (bounds.min + bounds.max) / 2;
            Vectorz min = bounds.min;
            Vectorz max = bounds.max;

            // min -> center -> max
            // prev (minX, minY, minZ) -> (maxX, maxY, maxZ)
            // prev (minX, minY, minZ) -> (centerX, centerY, centerZ)
            // prev (centerX, centerY, centerZ) -> (maxX, maxY, maxZ)
            // prev's combination

            child[0] = new Octree(new AABB(new Vectorz(min.X   , min.Y   , min.Z   ), new Vectorz(center.X, center.Y, center.Z)), depth + 1);
            child[1] = new Octree(new AABB(new Vectorz(center.X, min.Y   , min.Z   ), new Vectorz(max.X   , center.Y, center.Z)), depth + 1);
            child[2] = new Octree(new AABB(new Vectorz(min.X   , center.Y, min.Z   ), new Vectorz(center.X, max.Y   , center.Z)), depth + 1);
            child[3] = new Octree(new AABB(new Vectorz(center.X, center.Y, min.Z   ), new Vectorz(max.X   , max.Y   , center.Z)), depth + 1);
            child[4] = new Octree(new AABB(new Vectorz(min.X   , min.Y   , center.Z), new Vectorz(center.X, center.Y, max.Z   )), depth + 1);
            child[5] = new Octree(new AABB(new Vectorz(center.X, min.Y   , center.Z), new Vectorz(max.X   , center.Y, max.Z   )), depth + 1);
            child[6] = new Octree(new AABB(new Vectorz(min.X   , center.Y, center.Z), new Vectorz(center.X, max.Y   , max.Z   )), depth + 1);
            child[7] = new Octree(new AABB(new Vectorz(center.X, center.Y, center.Z), new Vectorz(max.X   , max.Y   , max.Z   )), depth + 1);

            isLeaf = false;
        }

        public bool isAdjacent(Octree other)
        {
            return ((Math.Abs(bounds.min.X - other.bounds.max.X) < EPSILON || Math.Abs(bounds.max.X - other.bounds.min.X) < EPSILON) &&
                   bounds.min.Y < other.bounds.max.Y && bounds.max.Y > other.bounds.min.Y &&
                   bounds.min.Z < other.bounds.max.Z && bounds.max.Z > other.bounds.min.Z) ||
                   ((Math.Abs(bounds.min.Y - other.bounds.max.Y) < EPSILON || Math.Abs(bounds.max.Y - other.bounds.min.Y) < EPSILON) &&
                   bounds.min.X < other.bounds.max.X && bounds.max.X > other.bounds.min.X &&
                   bounds.min.Z < other.bounds.max.Z && bounds.max.Z > other.bounds.min.Z) ||
                   ((Math.Abs(bounds.min.Z - other.bounds.max.Z) < EPSILON || Math.Abs(bounds.max.Z - other.bounds.min.Z) < EPSILON) &&
                   bounds.min.X < other.bounds.max.X && bounds.max.X > other.bounds.min.X &&
                   bounds.min.Y < other.bounds.max.Y && bounds.max.Y > other.bounds.min.Y);
        }

        // public void Subdivide(int maxDepth)
        // {
        //     if (depth > maxDepth || depth < 0) 
        //         return;

        //     GetChildren();

        //     for (int i = 0; i < 8; i++)
        //     {
        //         child[i].depth = this.depth + 1;
        //         if (child[i].IntersectsMesh())
        //             child[i].Subdivide(maxDepth);
        //         else
        //             child[i].isLeaf = false;
        //     }
        // }

        // public bool IntersectsMesh()
        // {
        //     // Check if mesh intersects with octree
        //     foreach (var face in mesh.faces)
        //     {
        //         AABB triangleBounds = new AABB(
        //             mesh.vertices[face.Item1],
        //             mesh.vertices[face.Item2],
        //             mesh.vertices[face.Item3]
        //         );

        //         if (Collision.IntersectAABB(triangleBounds, bounds))
        //             return true;
        //     }
        //     return false;
        // }

    }
}