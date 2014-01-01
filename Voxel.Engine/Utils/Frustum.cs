using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;
using OpenTK;

namespace Voxel.Engine.Utils
{
    public class Frustum : IEquatable<Frustum>
    {
        #region Private Fields

        private Matrix4 _matrix;
        private readonly Vector3[] _corners = new Vector3[CornerCount];
        private readonly Plane[] _planes = new Plane[PlaneCount];

        private const int PlaneCount = 6;

        #endregion Private Fields

        #region Public Fields
        public const int CornerCount = 8;
        #endregion

        #region Public Constructors

        public Frustum(Matrix4 value)
        {
            this._matrix = value;
            this.CreatePlanes();
            this.CreateCorners();
        }

        #endregion Public Constructors

        #region Public Properties

        public Matrix4 Matrix
        {
            get { return this._matrix; }
            set
            {
                this._matrix = value;
                this.CreatePlanes();    // FIXME: The odds are the planes will be used a lot more often than the matrix
                this.CreateCorners();   // is updated, so this should help performance. I hope ;)
            }
        }

        public Plane Near
        {
            get { return this._planes[0]; }
        }
        
        public Plane Far
        {
            get { return this._planes[1]; }
        }
        
        public Plane Left
        {
            get { return this._planes[2]; }
        }

        public Plane Right
        {
            get { return this._planes[3]; }
        }

        public Plane Top
        {
            get { return this._planes[4]; }
        }

        public Plane Bottom
        {
            get { return this._planes[5]; }
        }

        #endregion Public Properties
       
        #region IEquatable Methods
        public static bool operator ==(Frustum a, Frustum b)
        {
            if (object.Equals(a, null))
                return (object.Equals(b, null));

            if (object.Equals(b, null))
                return (object.Equals(a, null));

            return a._matrix == (b._matrix);
        }

        public static bool operator !=(Frustum a, Frustum b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return this._matrix.GetHashCode();
        }

        public bool Equals(Frustum other)
        {
            return (this == other);
        }

        public override bool Equals(object obj)
        {
            Frustum f = obj as Frustum;
            return (object.Equals(f, null)) ? false : (this == f);
        }

        #endregion

        #region Public Methods

        public Vector3[] GetCorners()
        {
            return (Vector3[])this._corners.Clone();
        }
        
        public void GetCorners(Vector3[] corners)
        {
            if (corners == null) throw new ArgumentNullException("corners");
            if (corners.Length < CornerCount) throw new ArgumentOutOfRangeException("corners");

            this._corners.CopyTo(corners, 0);
        }

        public override string ToString()
        {
            var sb = new StringBuilder(256);
            sb.Append("{Near:");
            sb.Append(this._planes[0].ToString());
            sb.Append(" Far:");
            sb.Append(this._planes[1].ToString());
            sb.Append(" Left:");
            sb.Append(this._planes[2].ToString());
            sb.Append(" Right:");
            sb.Append(this._planes[3].ToString());
            sb.Append(" Top:");
            sb.Append(this._planes[4].ToString());
            sb.Append(" Bottom:");
            sb.Append(this._planes[5].ToString());
            sb.Append("}");
            return sb.ToString();
        }

        #endregion Public Methods

        #region Private Methods

        private void CreateCorners()
        {
            IntersectionPoint(ref this._planes[0], ref this._planes[2], ref this._planes[4], out this._corners[0]);
            IntersectionPoint(ref this._planes[0], ref this._planes[3], ref this._planes[4], out this._corners[1]);
            IntersectionPoint(ref this._planes[0], ref this._planes[3], ref this._planes[5], out this._corners[2]);
            IntersectionPoint(ref this._planes[0], ref this._planes[2], ref this._planes[5], out this._corners[3]);
            IntersectionPoint(ref this._planes[1], ref this._planes[2], ref this._planes[4], out this._corners[4]);
            IntersectionPoint(ref this._planes[1], ref this._planes[3], ref this._planes[4], out this._corners[5]);
            IntersectionPoint(ref this._planes[1], ref this._planes[3], ref this._planes[5], out this._corners[6]);
            IntersectionPoint(ref this._planes[1], ref this._planes[2], ref this._planes[5], out this._corners[7]);
        }

        private void CreatePlanes()
        {            
            this._planes[0] = new Plane(-this._matrix.M13, -this._matrix.M23, -this._matrix.M33, -this._matrix.M43);
            this._planes[1] = new Plane(this._matrix.M13 - this._matrix.M14, this._matrix.M23 - this._matrix.M24, this._matrix.M33 - this._matrix.M34, this._matrix.M43 - this._matrix.M44);
            this._planes[2] = new Plane(-this._matrix.M14 - this._matrix.M11, -this._matrix.M24 - this._matrix.M21, -this._matrix.M34 - this._matrix.M31, -this._matrix.M44 - this._matrix.M41);
            this._planes[3] = new Plane(this._matrix.M11 - this._matrix.M14, this._matrix.M21 - this._matrix.M24, this._matrix.M31 - this._matrix.M34, this._matrix.M41 - this._matrix.M44);
            this._planes[4] = new Plane(this._matrix.M12 - this._matrix.M14, this._matrix.M22 - this._matrix.M24, this._matrix.M32 - this._matrix.M34, this._matrix.M42 - this._matrix.M44);
            this._planes[5] = new Plane(-this._matrix.M14 - this._matrix.M12, -this._matrix.M24 - this._matrix.M22, -this._matrix.M34 - this._matrix.M32, -this._matrix.M44 - this._matrix.M42);
            
            this.NormalizePlane(ref this._planes[0]);
            this.NormalizePlane(ref this._planes[1]);
            this.NormalizePlane(ref this._planes[2]);
            this.NormalizePlane(ref this._planes[3]);
            this.NormalizePlane(ref this._planes[4]);
            this.NormalizePlane(ref this._planes[5]);
        }

        private static void IntersectionPoint(ref Plane a, ref Plane b, ref Plane c, out Vector3 result)
        {
            // Formula used
            //                d1 ( N2 * N3 ) + d2 ( N3 * N1 ) + d3 ( N1 * N2 )
            //P =   -------------------------------------------------------------------------
            //                             N1 . ( N2 * N3 )
            //
            // Note: N refers to the normal, d refers to the displacement. '.' means dot product. '*' means cross product
            
            Vector3 v1, v2, v3;
            Vector3 cross;
            
            Vector3.Cross(ref b.Normal, ref c.Normal, out cross);
            
            float f;
            Vector3.Dot(ref a.Normal, ref cross, out f);
            f *= -1.0f;
            
            Vector3.Cross(ref b.Normal, ref c.Normal, out cross);
            Vector3.Multiply(ref cross, a.D, out v1);
            //v1 = (a.D * (Vector3.Cross(b.Normal, c.Normal)));
            
            
            Vector3.Cross(ref c.Normal, ref a.Normal, out cross);
            Vector3.Multiply(ref cross, b.D, out v2);
            //v2 = (b.D * (Vector3.Cross(c.Normal, a.Normal)));
            
            
            Vector3.Cross(ref a.Normal, ref b.Normal, out cross);
            Vector3.Multiply(ref cross, c.D, out v3);
            //v3 = (c.D * (Vector3.Cross(a.Normal, b.Normal)));
            
            result.X = (v1.X + v2.X + v3.X) / f;
            result.Y = (v1.Y + v2.Y + v3.Y) / f;
            result.Z = (v1.Z + v2.Z + v3.Z) / f;
        }
        
        private void NormalizePlane(ref Plane p)
        {
            float factor = 1f / p.Normal.Length;
            p.Normal.X *= factor;
            p.Normal.Y *= factor;
            p.Normal.Z *= factor;
            p.D *= factor;
        }

        #endregion
    }
}
