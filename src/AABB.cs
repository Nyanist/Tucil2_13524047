using System;

namespace obj_voxelizer
{
    public class AABB
    {
        public Vectorz min;
        public Vectorz max;

        public AABB(Vectorz min, Vectorz max)
        {
            this.min = min;
            this.max = max;
        }

        public AABB(Vectorz vertices1, Vectorz vertices2, Vectorz vertices3)
        {
            this.min = Vectorz.Min(Vectorz.Min(vertices1, vertices2), vertices3);
            this.max = Vectorz.Max(Vectorz.Max(vertices1, vertices2), vertices3);
        }

    }

    public class Collision
    {
        // A intersects B
        public static bool IntersectAABB(AABB a, AABB b)
        {
            return a.min.X <= b.max.X && a.max.X >= b.min.X &&
                   a.min.Y <= b.max.Y && a.max.Y >= b.min.Y &&
                   a.min.Z <= b.max.Z && a.max.Z >= b.min.Z;
        }

        // A contains B
        public static bool ContainsAABB(AABB a, AABB b)
        {
            return a.min.X <= b.min.X && a.max.X >= b.max.X &&
                   a.min.Y <= b.min.Y && a.max.Y >= b.max.Y &&
                   a.min.Z <= b.min.Z && a.max.Z >= b.max.Z;
        }
    }
}