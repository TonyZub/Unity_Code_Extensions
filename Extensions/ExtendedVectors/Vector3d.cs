using System.Globalization;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.Internal;

namespace Extensions
{
    public struct Vector3d : IEquatable<Vector3d>, IFormattable
    {
        public const double kEpsilon = 1E-05d;

        public const double kEpsilonNormalSqrt = 1E-15d;

        //
        // Сводка:
        //     X component of the vector.
        public double x;

        //
        // Сводка:
        //     Y component of the vector.
        public double y;

        //
        // Сводка:
        //     Z component of the vector.
        public double z;

        private static readonly Vector3d zeroVector = new Vector3d(0d, 0d, 0d);

        private static readonly Vector3d oneVector = new Vector3d(1d, 1d, 1d);

        private static readonly Vector3d upVector = new Vector3d(0d, 1d, 0d);

        private static readonly Vector3d downVector = new Vector3d(0d, -1d, 0d);

        private static readonly Vector3d leftVector = new Vector3d(-1d, 0d, 0d);

        private static readonly Vector3d rightVector = new Vector3d(1d, 0d, 0d);

        private static readonly Vector3d forwardVector = new Vector3d(0d, 0d, 1d);

        private static readonly Vector3d backVector = new Vector3d(0d, 0d, -1d);

        private static readonly Vector3d positiveInfinityVector = new Vector3d(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

        private static readonly Vector3d negativeInfinityVector = new Vector3d(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    2 => z,
                    _ => throw new IndexOutOfRangeException("Invalid Vector3d index!"),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3d index!");
                }
            }
        }

        public Vector3 AsVector3 => new Vector3((float)x, (float)y, (float)z);

        //
        // Сводка:
        //     Returns a normalized vector based on the current vector. The normalized vector
        //     has a magnitude of 1 and is in the same direction as the current vector. Returns
        //     a zero vector If the current vector is too small to be normalized.
        public Vector3d normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        //
        // Сводка:
        //     Returns the length of this vector (Read Only).
        public double magnitude
        {
            get
            {
                return (double)Math.Sqrt(x * x + y * y + z * z);
            }
        }

        //
        // Сводка:
        //     Returns the squared length of this vector (Read Only).
        public double sqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(0, 0, 0).
        public static Vector3d zero
        {
            get
            {
                return zeroVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(1, 1, 1).
        public static Vector3d one
        {
            get
            {
                return oneVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(0, 0, 1).
        public static Vector3d forward
        {
            get
            {
                return forwardVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(0, 0, -1).
        public static Vector3d back
        {
            get
            {
                return backVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(0, 1, 0).
        public static Vector3d up
        {
            get
            {
                return upVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(0, -1, 0).
        public static Vector3d down
        {
            get
            {
                return downVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(-1, 0, 0).
        public static Vector3d left
        {
            get
            {
                return leftVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(1, 0, 0).
        public static Vector3d right
        {
            get
            {
                return rightVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(double.PositiveInfinity, double.PositiveInfinity,
        //     double.PositiveInfinity).
        public static Vector3d positiveInfinity
        {
            get
            {
                return positiveInfinityVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3d(double.NegativeInfinity, double.NegativeInfinity,
        //     double.NegativeInfinity).
        public static Vector3d negativeInfinity
        {
            get
            {
                return negativeInfinityVector;
            }
        }

        [Obsolete("Use Vector3d.forward instead.")]
        public static Vector3d fwd => new Vector3d(0d, 0d, 1d);

        //
        // Сводка:
        //     Spherically interpolates between two vectors.
        //
        // Параметры:
        //   a:
        //
        //   b:
        //
        //   t:
        public static Vector3d Slerp(Vector3d a, Vector3d b, double t)
        {
            Slerp_Injected(ref a, ref b, t, out var ret);
            return ret;
        }

        //
        // Сводка:
        //     Spherically interpolates between two vectors.
        //
        // Параметры:
        //   a:
        //
        //   b:
        //
        //   t:
        public static Vector3d SlerpUnclamped(Vector3d a, Vector3d b, double t)
        {
            SlerpUnclamped_Injected(ref a, ref b, t, out var ret);
            return ret;
        }

        private static extern void OrthoNormalize2(ref Vector3d a, ref Vector3d b);

        public static void OrthoNormalize(ref Vector3d normal, ref Vector3d tangent)
        {
            OrthoNormalize2(ref normal, ref tangent);
        }

        private static extern void OrthoNormalize3(ref Vector3d a, ref Vector3d b, ref Vector3d c);

        public static void OrthoNormalize(ref Vector3d normal, ref Vector3d tangent, ref Vector3d binormal)
        {
            OrthoNormalize3(ref normal, ref tangent, ref binormal);
        }

        //
        // Сводка:
        //     Rotates a vector current towards target.
        //
        // Параметры:
        //   current:
        //     The vector being managed.
        //
        //   target:
        //     The vector.
        //
        //   maxRadiansDelta:
        //     The maximum angle in radians allowed for this rotation.
        //
        //   maxMagnitudeDelta:
        //     The maximum allowed change in vector magnitude for this rotation.
        //
        // Возврат:
        //     The location that RotateTowards generates.
        public static Vector3d RotateTowards(Vector3d current, Vector3d target, double maxRadiansDelta, double maxMagnitudeDelta)
        {
            RotateTowards_Injected(ref current, ref target, maxRadiansDelta, maxMagnitudeDelta, out var ret);
            return ret;
        }

        //
        // Сводка:
        //     Linearly interpolates between two points.
        //
        // Параметры:
        //   a:
        //     Start value, returned when t = 0.
        //
        //   b:
        //     End value, returned when t = 1.
        //
        //   t:
        //     Value used to interpolate between a and b.
        //
        // Возврат:
        //     Interpolated value, equals to a + (b - a) * t.
        public static Vector3d Lerp(Vector3d a, Vector3d b, double t)
        {
            t = Mathf.Clamp01((float)t);
            return new Vector3d(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        //
        // Сводка:
        //     Linearly interpolates between two vectors.
        //
        // Параметры:
        //   a:
        //
        //   b:
        //
        //   t:
        public static Vector3d LerpUnclamped(Vector3d a, Vector3d b, double t)
        {
            return new Vector3d(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        //
        // Сводка:
        //     Calculate a position between the points specified by current and target, moving
        //     no farther than the distance specified by maxDistanceDelta.
        //
        // Параметры:
        //   current:
        //     The position to move from.
        //
        //   target:
        //     The position to move towards.
        //
        //   maxDistanceDelta:
        //     Distance to move current per call.
        //
        // Возврат:
        //     The new position.
        public static Vector3d MoveTowards(Vector3d current, Vector3d target, double maxDistanceDelta)
        {
            double num = target.x - current.x;
            double num2 = target.y - current.y;
            double num3 = target.z - current.z;
            double num4 = num * num + num2 * num2 + num3 * num3;
            if (num4 == 0d || (maxDistanceDelta >= 0d && num4 <= maxDistanceDelta * maxDistanceDelta))
            {
                return target;
            }

            double num5 = (double)Math.Sqrt(num4);
            return new Vector3d(current.x + num / num5 * maxDistanceDelta, current.y + num2 / num5 * maxDistanceDelta, current.z + num3 / num5 * maxDistanceDelta);
        }

        public static Vector3d SmoothDamp(Vector3d current, Vector3d target, ref Vector3d currentVelocity, float smoothTime, double maxSpeed)
        {
            double deltaTime = Time.deltaTime;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static Vector3d SmoothDamp(Vector3d current, Vector3d target, ref Vector3d currentVelocity, float smoothTime)
        {
            double deltaTime = Time.deltaTime;
            double maxSpeed = double.PositiveInfinity;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static Vector3d SmoothDamp(Vector3d current, Vector3d target, ref Vector3d currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] double maxSpeed, [DefaultValue("Time.deltaTime")] double deltaTime)
        {
            double num = 0d;
            double num2 = 0d;
            double num3 = 0d;
            smoothTime = Mathf.Max(0.0001f, smoothTime);
            double num4 = 2d / smoothTime;
            double num5 = num4 * deltaTime;
            double num6 = 1d / (1d + num5 + 0.48d * num5 * num5 + 0.235d * num5 * num5 * num5);
            double num7 = current.x - target.x;
            double num8 = current.y - target.y;
            double num9 = current.z - target.z;
            Vector3d vector = target;
            double num10 = maxSpeed * smoothTime;
            double num11 = num10 * num10;
            double num12 = num7 * num7 + num8 * num8 + num9 * num9;
            if (num12 > num11)
            {
                double num13 = (double)Math.Sqrt(num12);
                num7 = num7 / num13 * num10;
                num8 = num8 / num13 * num10;
                num9 = num9 / num13 * num10;
            }

            target.x = current.x - num7;
            target.y = current.y - num8;
            target.z = current.z - num9;
            double num14 = (currentVelocity.x + num4 * num7) * deltaTime;
            double num15 = (currentVelocity.y + num4 * num8) * deltaTime;
            double num16 = (currentVelocity.z + num4 * num9) * deltaTime;
            currentVelocity.x = (currentVelocity.x - num4 * num14) * num6;
            currentVelocity.y = (currentVelocity.y - num4 * num15) * num6;
            currentVelocity.z = (currentVelocity.z - num4 * num16) * num6;
            num = target.x + (num7 + num14) * num6;
            num2 = target.y + (num8 + num15) * num6;
            num3 = target.z + (num9 + num16) * num6;
            double num17 = vector.x - current.x;
            double num18 = vector.y - current.y;
            double num19 = vector.z - current.z;
            double num20 = num - vector.x;
            double num21 = num2 - vector.y;
            double num22 = num3 - vector.z;
            if (num17 * num20 + num18 * num21 + num19 * num22 > 0f)
            {
                num = vector.x;
                num2 = vector.y;
                num3 = vector.z;
                currentVelocity.x = (num - vector.x) / deltaTime;
                currentVelocity.y = (num2 - vector.y) / deltaTime;
                currentVelocity.z = (num3 - vector.z) / deltaTime;
            }

            return new Vector3d(num, num2, num3);
        }

        //
        // Сводка:
        //     Creates a new vector with given x, y, z components.
        //
        // Параметры:
        //   x:
        //
        //   y:
        //
        //   z:
        public Vector3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        //
        // Сводка:
        //     Creates a new vector with given x, y components and sets z to zero.
        //
        // Параметры:
        //   x:
        //
        //   y:
        public Vector3d(double x, double y)
        {
            this.x = x;
            this.y = y;
            z = 0d;
        }

        //
        // Сводка:
        //     Set x, y and z components of an existing Vector3d.
        //
        // Параметры:
        //   newX:
        //
        //   newY:
        //
        //   newZ:
        public void Set(double newX, double newY, double newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        //
        // Сводка:
        //     Multiplies two vectors component-wise.
        //
        // Параметры:
        //   a:
        //
        //   b:
        public static Vector3d Scale(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        //
        // Сводка:
        //     Multiplies every component of this vector by the same component of scale.
        //
        // Параметры:
        //   scale:
        public void Scale(Vector3d scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        //
        // Сводка:
        //     Cross Product of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
        public static Vector3d Cross(Vector3d lhs, Vector3d rhs)
        {
            return new Vector3d(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }

        //
        // Сводка:
        //     Returns true if the given vector is exactly equal to this vector.
        //
        // Параметры:
        //   other:
        public override bool Equals(object other)
        {
            if (other is Vector3d other2)
            {
                return Equals(other2);
            }

            return false;
        }

        public bool Equals(Vector3d other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        //
        // Сводка:
        //     Reflects a vector off the plane defined by a normal.
        //
        // Параметры:
        //   inDirection:
        //     The direction vector towards the plane.
        //
        //   inNormal:
        //     The normal vector that defines the plane.
        public static Vector3d Reflect(Vector3d inDirection, Vector3d inNormal)
        {
            double num = -2d * Dot(inNormal, inDirection);
            return new Vector3d(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y, num * inNormal.z + inDirection.z);
        }

        //
        // Сводка:
        //     Returns a normalized vector based on the given vector. The normalized vector
        //     has a magnitude of 1 and is in the same direction as the given vector. Returns
        //     a zero vector If the given vector is too small to be normalized.
        //
        // Параметры:
        //   value:
        //     The vector to be normalized.
        //
        // Возврат:
        //     A new vector with the same direction as the original vector but with a magnitude
        //     of 1.0.
        public static Vector3d Normalize(Vector3d value)
        {
            double num = Magnitude(value);
            if (num > 1E-05d)
            {
                return value / num;
            }

            return zero;
        }

        //
        // Сводка:
        //     Makes this vector have a magnitude of 1.
        public void Normalize()
        {
            double num = Magnitude(this);
            if (num > 1E-05d)
            {
                this /= num;
            }
            else
            {
                this = zero;
            }
        }

        //
        // Сводка:
        //     Dot Product of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
        public static double Dot(Vector3d lhs, Vector3d rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        //
        // Сводка:
        //     Projects a vector onto another vector.
        //
        // Параметры:
        //   vector:
        //
        //   onNormal:
        public static Vector3d Project(Vector3d vector, Vector3d onNormal)
        {
            double num = Dot(onNormal, onNormal);
            if (num < Mathf.Epsilon)
            {
                return zero;
            }

            double num2 = Dot(vector, onNormal);
            return new Vector3d(onNormal.x * num2 / num, onNormal.y * num2 / num, onNormal.z * num2 / num);
        }

        //
        // Сводка:
        //     Projects a vector onto a plane.
        //
        // Параметры:
        //   vector:
        //     The vector to project on the plane.
        //
        //   planeNormal:
        //     The normal which defines the plane to project on.
        //
        // Возврат:
        //     The orthogonal projection of vector on the plane.
        public static Vector3d ProjectOnPlane(Vector3d vector, Vector3d planeNormal)
        {
            double num = Dot(planeNormal, planeNormal);
            if (num < Mathf.Epsilon)
            {
                return vector;
            }

            double num2 = Dot(vector, planeNormal);
            return new Vector3d(vector.x - planeNormal.x * num2 / num, vector.y - planeNormal.y * num2 / num, vector.z - planeNormal.z * num2 / num);
        }

        //
        // Сводка:
        //     Calculates the angle between two vectors.
        //
        // Параметры:
        //   from:
        //     The vector from which the angular difference is measured.
        //
        //   to:
        //     The vector to which the angular difference is measured.
        //
        // Возврат:
        //     The angle in degrees between the two vectors.
        public static double Angle(Vector3d from, Vector3d to)
        {
            double num = (double)Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            if (num < 1E-15d)
            {
                return 0d;
            }

            double num2 = Mathf.Clamp((float)(Dot(from, to) / num), -1f, 1f);
            return (double)Math.Acos(num2) * 57.29578d;
        }

        //
        // Сводка:
        //     Calculates the signed angle between vectors from and to in relation to axis.
        //
        //
        // Параметры:
        //   from:
        //     The vector from which the angular difference is measured.
        //
        //   to:
        //     The vector to which the angular difference is measured.
        //
        //   axis:
        //     A vector around which the other vectors are rotated.
        //
        // Возврат:
        //     Returns the signed angle between from and to in degrees.
        public static double SignedAngle(Vector3d from, Vector3d to, Vector3d axis)
        {
            double num = Angle(from, to);
            double num2 = from.y * to.z - from.z * to.y;
            double num3 = from.z * to.x - from.x * to.z;
            double num4 = from.x * to.y - from.y * to.x;
            double num5 = Mathf.Sign((float)(axis.x * num2 + axis.y * num3 + axis.z * num4));
            return num * num5;
        }

        //
        // Сводка:
        //     Returns the distance between a and b.
        //
        // Параметры:
        //   a:
        //
        //   b:
        public static double Distance(Vector3d a, Vector3d b)
        {
            double num = a.x - b.x;
            double num2 = a.y - b.y;
            double num3 = a.z - b.z;
            return (double)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
        }

        //
        // Сводка:
        //     Returns a copy of vector with its magnitude clamped to maxLength.
        //
        // Параметры:
        //   vector:
        //
        //   maxLength:
        public static Vector3d ClampMagnitude(Vector3d vector, double maxLength)
        {
            double num = vector.sqrMagnitude;
            if (num > maxLength * maxLength)
            {
                double num2 = (double)Math.Sqrt(num);
                double num3 = vector.x / num2;
                double num4 = vector.y / num2;
                double num5 = vector.z / num2;
                return new Vector3d(num3 * maxLength, num4 * maxLength, num5 * maxLength);
            }

            return vector;
        }

        public static double Magnitude(Vector3d vector)
        {
            return (double)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        public static double SqrMagnitude(Vector3d vector)
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        }

        //
        // Сводка:
        //     Returns a vector that is made from the smallest components of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
        public static Vector3d Min(Vector3d lhs, Vector3d rhs)
        {
            return new Vector3d(Mathf.Min((float)lhs.x, (float)rhs.x), Mathf.Min((float)lhs.y, (float)rhs.y), Mathf.Min((float)lhs.z, (float)rhs.z));
        }

        //
        // Сводка:
        //     Returns a vector that is made from the largest components of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
        public static Vector3d Max(Vector3d lhs, Vector3d rhs)
        {
            return new Vector3d(Mathf.Max((float)lhs.x, (float)rhs.x), Mathf.Max((float)lhs.y, (float)rhs.y), Mathf.Max((float)lhs.z, (float)rhs.z));
        }

        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3d operator -(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3d operator -(Vector3d a)
        {
            return new Vector3d(0f - a.x, 0f - a.y, 0f - a.z);
        }

        public static Vector3d operator *(Vector3d a, double d)
        {
            return new Vector3d(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3d operator *(double d, Vector3d a)
        {
            return new Vector3d(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3d operator /(Vector3d a, double d)
        {
            return new Vector3d(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Vector3d lhs, Vector3d rhs)
        {
            double num = lhs.x - rhs.x;
            double num2 = lhs.y - rhs.y;
            double num3 = lhs.z - rhs.z;
            double num4 = num * num + num2 * num2 + num3 * num3;
            return num4 < 9.99999944E-11d;
        }

        public static bool operator !=(Vector3d lhs, Vector3d rhs)
        {
            return !(lhs == rhs);
        }

        //
        // Сводка:
        //     Returns a formatted string for this vector.
        //
        // Параметры:
        //   format:
        //     A numeric format string.
        //
        //   formatProvider:
        //     An object that specifies culture-specific formatting.
        public override string ToString()
        {
            return ToString(null, null);
        }

        //
        // Сводка:
        //     Returns a formatted string for this vector.
        //
        // Параметры:
        //   format:
        //     A numeric format string.
        //
        //   formatProvider:
        //     An object that specifies culture-specific formatting.
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        //
        // Сводка:
        //     Returns a formatted string for this vector.
        //
        // Параметры:
        //   format:
        //     A numeric format string.
        //
        //   formatProvider:
        //     An object that specifies culture-specific formatting.
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "F2";
            }

            if (formatProvider == null)
            {
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            }

            return Format("({0}, {1}, {2})", x.ToString(format, formatProvider), y.ToString(format, formatProvider), z.ToString(format, formatProvider));
        }

        private string Format(string fmt, params object[] args) => string.Format(CultureInfo.InvariantCulture.NumberFormat, fmt, args);

        [Obsolete("Use Vector3d.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
        public static double AngleBetween(Vector3d from, Vector3d to)
        {
            return (double)Math.Acos(Mathf.Clamp((float)Dot(from.normalized, to.normalized), -1f, 1f));
        }

        [Obsolete("Use Vector3d.ProjectOnPlane instead.")]
        public static Vector3d Exclude(Vector3d excludeThis, Vector3d fromThat)
        {
            return ProjectOnPlane(fromThat, excludeThis);
        }

        private static extern void Slerp_Injected([In] ref Vector3d a, [In] ref Vector3d b, double t, out Vector3d ret);

        private static extern void SlerpUnclamped_Injected([In] ref Vector3d a, [In] ref Vector3d b, double t, out Vector3d ret);

        private static extern void RotateTowards_Injected([In] ref Vector3d current, [In] ref Vector3d target, double maxRadiansDelta, double maxMagnitudeDelta, out Vector3d ret);
    }
}

