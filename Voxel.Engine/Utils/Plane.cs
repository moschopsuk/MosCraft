﻿using System;
using System.Runtime.Serialization;
using OpenTK;

namespace Voxel.Engine.Utils
{
    [DataContract]
    public struct Plane : IEquatable<Plane>
    {
        #region Public Fields

        [DataMember]
        public float D;

        [DataMember]
        public Vector3 Normal;

        #endregion Public Fields

        #region Constructors

        public Plane(Vector4 value)
            : this(new Vector3(value.X, value.Y, value.Z), value.W)
        {

        }

        public Plane(Vector3 normal, float d)
        {
            Normal = normal;
            D = d;
        }

        public Plane(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 ab = b - a;
            Vector3 ac = c - a;

            Vector3 cross = Vector3.Cross(ab, ac);
            Normal = Vector3.Normalize(cross);
            D = -(Vector3.Dot(Normal, a));
        }

        public Plane(float a, float b, float c, float d)
            : this(new Vector3(a, b, c), d)
        {

        }

        #endregion Constructors

        #region Public Methods

        public float Dot(Vector4 value)
        {
            return ((((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + (this.D * value.W));
        }

        public void Dot(ref Vector4 value, out float result)
        {
            result = (((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + (this.D * value.W);
        }

        public float DotCoordinate(Vector3 value)
        {
            return ((((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + this.D);
        }

        public void DotCoordinate(ref Vector3 value, out float result)
        {
            result = (((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + this.D;
        }

        public float DotNormal(Vector3 value)
        {
            return (((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z));
        }

        public void DotNormal(ref Vector3 value, out float result)
        {
            result = ((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z);
        }

        public void Normalize()
        {
            float factor;
            Vector3 normal = Normal;
            Normal = Vector3.Normalize(Normal);
            factor = (float)Math.Sqrt(Normal.X * Normal.X + Normal.Y * Normal.Y + Normal.Z * Normal.Z) /
                    (float)Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);

            D = D * factor;
        }

        public static Plane Normalize(Plane value)
        {
            Plane ret;
            Normalize(ref value, out ret);
            return ret;
        }

        public static void Normalize(ref Plane value, out Plane result)
        {
            float factor;
            result.Normal = Vector3.Normalize(value.Normal);
            factor = (float)Math.Sqrt(result.Normal.X * result.Normal.X + result.Normal.Y * result.Normal.Y + result.Normal.Z * result.Normal.Z) /
                    (float)Math.Sqrt(value.Normal.X * value.Normal.X + value.Normal.Y * value.Normal.Y + value.Normal.Z * value.Normal.Z);
            result.D = value.D * factor;
        }

        public static bool operator !=(Plane plane1, Plane plane2)
        {
            return !plane1.Equals(plane2);
        }

        public static bool operator ==(Plane plane1, Plane plane2)
        {
            return plane1.Equals(plane2);
        }

        public override bool Equals(object other)
        {
            return (other is Plane) ? this.Equals((Plane)other) : false;
        }

        public bool Equals(Plane other)
        {
            return ((Normal == other.Normal) && (D == other.D));
        }

        public override int GetHashCode()
        {
            return Normal.GetHashCode() ^ D.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{{Normal:{0} D:{1}}}", Normal, D);
        }

        #endregion
    }
}
