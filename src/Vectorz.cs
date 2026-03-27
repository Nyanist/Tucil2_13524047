using System;

namespace obj_voxelizer
{
    public struct Vectorz
    {
        public float X, Y, Z;

        public Vectorz(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vectorz operator +(Vectorz a, Vectorz b)
        {
            return new Vectorz(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vectorz operator -(Vectorz a, Vectorz b)
        {
            return new Vectorz(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vectorz operator *(Vectorz a, float s)
        {
            return new Vectorz(a.X * s, a.Y * s, a.Z * s);
        }

        public static Vectorz operator /(Vectorz a, float s)
        {
            return new Vectorz(a.X / s, a.Y / s, a.Z / s);
        }

        public float Length()
        {
            return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }

        public float LengthSquared() {
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
        }

        public Vectorz Normalized()
        {
            float len = Length();
            return len > 0 ? this / len : this;
        }

        public static float Dot(Vectorz a, Vectorz b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Vectorz Cross(Vectorz a, Vectorz b)
        {
            return new Vectorz(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );
        }

        public static Vectorz Min(Vectorz a, Vectorz b)
        {
            return new Vectorz(
                Math.Min(a.X, b.X),
                Math.Min(a.Y, b.Y),
                Math.Min(a.Z, b.Z)
            );
        }

        public static Vectorz Max(Vectorz a, Vectorz b)
        {
            return new Vectorz(
                Math.Max(a.X, b.X),
                Math.Max(a.Y, b.Y),
                Math.Max(a.Z, b.Z)
            );
        }

    }
}
