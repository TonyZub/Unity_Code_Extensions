using System.Globalization;
using System;
using UnityEngine;
using UnityEngine.Internal;

namespace Extensions
{
    public struct Vector2d : IEquatable<Vector2d>, IFormattable
    {
        //
        // Сводка:
        //     X component of the vector.
        public double x;

        //
        // Сводка:
        //     Y component of the vector.
        public double y;

        private static readonly Vector2d zeroVector = new Vector2d(0d, 0d);

        private static readonly Vector2d oneVector = new Vector2d(1d, 1d);

        private static readonly Vector2d upVector = new Vector2d(0d, 1d);

        private static readonly Vector2d downVector = new Vector2d(0d, -1d);

        private static readonly Vector2d leftVector = new Vector2d(-1d, 0d);

        private static readonly Vector2d rightVector = new Vector2d(1d, 0d);

        private static readonly Vector2d positiveInfinityVector = new Vector2d(double.PositiveInfinity, double.PositiveInfinity);

        private static readonly Vector2d negativeInfinityVector = new Vector2d(double.NegativeInfinity, double.NegativeInfinity);

        public const double kEpsilon = 1E-05d;

        public const double kEpsilonNormalSqrt = 1E-15d;

        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    _ => throw new IndexOutOfRangeException("Invalid Vector2d index!"),
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
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2d index!");
                }
            }
        }

        public Vector2 AsVector2 => new Vector2((float)x, (float)y);

        //
        // Сводка:
        //     Returns a normalized vector based on the current vector. The normalized vector
        //     has a magnitude of 1 and is in the same direction as the current vector. Returns
        //     a zero vector If the current vector is too small to be normalized.
        public Vector2d normalized
        {
            get
            {
                Vector2d result = new Vector2d(x, y);
                result.Normalize();
                return result;
            }
        }

        //
        // Сводка:
        //     Returns the length of this vector (Read Only).
        public double magnitude
        {
            get
            {
                return (double)Math.Sqrt(x * x + y * y);
            }
        }

        //
        // Сводка:
        //     Returns the squared length of this vector (Read Only).
        public double sqrMagnitude
        {
            get
            {
                return x * x + y * y;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector2d(0, 0).
        public static Vector2d zero
        {
            get
            {
                return zeroVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector2d(1, 1).
        public static Vector2d one
        {
            get
            {
                return oneVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector2d(0, 1).
        public static Vector2d up
        {
            get
            {
                return upVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector2d(0, -1).
        public static Vector2d down
        {
            get
            {
                return downVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector2d(-1, 0).
        public static Vector2d left
        {
            get
            {
                return leftVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector2d(1, 0).
        public static Vector2d right
        {
            get
            {
                return rightVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector2d(double.PositiveInfinity, double.PositiveInfinity).
        public static Vector2d positiveInfinity
        {
            get
            {
                return positiveInfinityVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector2d(double.NegativeInfinity, double.NegativeInfinity).
        public static Vector2d negativeInfinity
        {
            get
            {
                return negativeInfinityVector;
            }
        }

        //
        // Сводка:
        //     Constructs a new vector with given x, y components.
        //
        // Параметры:
        //   x:
        //
        //   y:
        public Vector2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        //
        // Сводка:
        //     Set x and y components of an existing Vector2d.
        //
        // Параметры:
        //   newX:
        //
        //   newY:
        public void Set(double newX, double newY)
        {
            x = newX;
            y = newY;
        }

        //
        // Сводка:
        //     Linearly interpolates between vectors a and b by t.
        //
        // Параметры:
        //   a:
        //
        //   b:
        //
        //   t:
        public static Vector2d Lerp(Vector2d a, Vector2d b, double t)
        {
            t = Mathf.Clamp01((float)t);
            return new Vector2d(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        //
        // Сводка:
        //     Linearly interpolates between vectors a and b by t.
        //
        // Параметры:
        //   a:
        //
        //   b:
        //
        //   t:
        public static Vector2d LerpUnclamped(Vector2d a, Vector2d b, double t)
        {
            return new Vector2d(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        //
        // Сводка:
        //     Moves a point current towards target.
        //
        // Параметры:
        //   current:
        //
        //   target:
        //
        //   maxDistanceDelta:
        public static Vector2d MoveTowards(Vector2d current, Vector2d target, double maxDistanceDelta)
        {
            double num = target.x - current.x;
            double num2 = target.y - current.y;
            double num3 = num * num + num2 * num2;
            if (num3 == 0d || (maxDistanceDelta >= 0d && num3 <= maxDistanceDelta * maxDistanceDelta))
            {
                return target;
            }

            double num4 = (double)Math.Sqrt(num3);
            return new Vector2d(current.x + num / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
        }

        //
        // Сводка:
        //     Multiplies two vectors component-wise.
        //
        // Параметры:
        //   a:
        //
        //   b:
        public static Vector2d Scale(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x * b.x, a.y * b.y);
        }

        //
        // Сводка:
        //     Multiplies every component of this vector by the same component of scale.
        //
        // Параметры:
        //   scale:
        public void Scale(Vector2d scale)
        {
            x *= scale.x;
            y *= scale.y;
        }

        //
        // Сводка:
        //     Makes this vector have a magnitude of 1.
        public void Normalize()
        {
            double num = magnitude;
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

            return Format("({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }

        private string Format(string fmt, params object[] args) => string.Format(CultureInfo.InvariantCulture.NumberFormat, fmt, args);

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        //
        // Сводка:
        //     Returns true if the given vector is exactly equal to this vector.
        //
        // Параметры:
        //   other:
        public override bool Equals(object other)
        {
            if (other is Vector2d other2)
            {
                return Equals(other2);
            }

            return false;
        }

        public bool Equals(Vector2d other)
        {
            return x == other.x && y == other.y;
        }

        //
        // Сводка:
        //     Reflects a vector off the surface defined by a normal.
        //
        // Параметры:
        //   inDirection:
        //     The direction vector towards the surface.
        //
        //   inNormal:
        //     The normal vector that defines the surface.
        public static Vector2d Reflect(Vector2d inDirection, Vector2d inNormal)
        {
            double num = -2d * Dot(inNormal, inDirection);
            return new Vector2d(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
        }

        //
        // Сводка:
        //     Returns the 2D vector perpendicular to this 2D vector. The result is always rotated
        //     90-degrees in a counter-clockwise direction for a 2D coordinate system where
        //     the positive Y axis goes up.
        //
        // Параметры:
        //   inDirection:
        //     The input direction.
        //
        // Возврат:
        //     The perpendicular direction.
        public static Vector2d Perpendicular(Vector2d inDirection)
        {
            return new Vector2d(0d - inDirection.y, inDirection.x);
        }

        //
        // Сводка:
        //     Dot Product of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
        public static double Dot(Vector2d lhs, Vector2d rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }

        //
        // Сводка:
        //     Gets the unsigned angle in degrees between from and to.
        //
        // Параметры:
        //   from:
        //     The vector from which the angular difference is measured.
        //
        //   to:
        //     The vector to which the angular difference is measured.
        //
        // Возврат:
        //     The unsigned angle in degrees between the two vectors.
        public static double Angle(Vector2d from, Vector2d to)
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
        //     Gets the signed angle in degrees between from and to.
        //
        // Параметры:
        //   from:
        //     The vector from which the angular difference is measured.
        //
        //   to:
        //     The vector to which the angular difference is measured.
        //
        // Возврат:
        //     The signed angle in degrees between the two vectors.
        public static double SignedAngle(Vector2d from, Vector2d to)
        {
            double num = Angle(from, to);
            double num2 = Mathf.Sign((float)(from.x * to.y - from.y * to.x));
            return num * num2;
        }

        //
        // Сводка:
        //     Returns the distance between a and b.
        //
        // Параметры:
        //   a:
        //
        //   b:
        public static double Distance(Vector2d a, Vector2d b)
        {
            double num = a.x - b.x;
            double num2 = a.y - b.y;
            return (double)Math.Sqrt(num * num + num2 * num2);
        }

        //
        // Сводка:
        //     Returns a copy of vector with its magnitude clamped to maxLength.
        //
        // Параметры:
        //   vector:
        //
        //   maxLength:
        public static Vector2d ClampMagnitude(Vector2d vector, double maxLength)
        {
            double num = vector.sqrMagnitude;
            if (num > maxLength * maxLength)
            {
                double num2 = (double)Math.Sqrt(num);
                double num3 = vector.x / num2;
                double num4 = vector.y / num2;
                return new Vector2d(num3 * maxLength, num4 * maxLength);
            }

            return vector;
        }

        public static double SqrMagnitude(Vector2d a)
        {
            return a.x * a.x + a.y * a.y;
        }

        public double SqrMagnitude()
        {
            return x * x + y * y;
        }

        //
        // Сводка:
        //     Returns a vector that is made from the smallest components of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
        public static Vector2d Min(Vector2d lhs, Vector2d rhs)
        {
            return new Vector2d(Mathf.Min((float)lhs.x, (float)rhs.x), Mathf.Min((float)lhs.y, (float)rhs.y));
        }

        //
        // Сводка:
        //     Returns a vector that is made from the largest components of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
        public static Vector2d Max(Vector2d lhs, Vector2d rhs)
        {
            return new Vector2d(Mathf.Max((float)lhs.x, (float)rhs.x), Mathf.Max((float)lhs.y, (float)rhs.y));
        }

        public static Vector2d SmoothDamp(Vector2d current, Vector2d target, ref Vector2d currentVelocity, float smoothTime, double maxSpeed)
        {
            double deltaTime = Time.deltaTime;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static Vector2d SmoothDamp(Vector2d current, Vector2d target, ref Vector2d currentVelocity, float smoothTime)
        {
            double deltaTime = Time.deltaTime;
            double maxSpeed = double.PositiveInfinity;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static Vector2d SmoothDamp(Vector2d current, Vector2d target, ref Vector2d currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] double maxSpeed, [DefaultValue("Time.deltaTime")] double deltaTime)
        {
            smoothTime = Mathf.Max(0.0001f, smoothTime);
            double num = 2d / smoothTime;
            double num2 = num * deltaTime;
            double num3 = 1d / (1d + num2 + 0.48d * num2 * num2 + 0.235d * num2 * num2 * num2);
            double num4 = current.x - target.x;
            double num5 = current.y - target.y;
            Vector2d vector = target;
            double num6 = maxSpeed * smoothTime;
            double num7 = num6 * num6;
            double num8 = num4 * num4 + num5 * num5;
            if (num8 > num7)
            {
                double num9 = (double)Math.Sqrt(num8);
                num4 = num4 / num9 * num6;
                num5 = num5 / num9 * num6;
            }

            target.x = current.x - num4;
            target.y = current.y - num5;
            double num10 = (currentVelocity.x + num * num4) * deltaTime;
            double num11 = (currentVelocity.y + num * num5) * deltaTime;
            currentVelocity.x = (currentVelocity.x - num * num10) * num3;
            currentVelocity.y = (currentVelocity.y - num * num11) * num3;
            double num12 = target.x + (num4 + num10) * num3;
            double num13 = target.y + (num5 + num11) * num3;
            double num14 = vector.x - current.x;
            double num15 = vector.y - current.y;
            double num16 = num12 - vector.x;
            double num17 = num13 - vector.y;
            if (num14 * num16 + num15 * num17 > 0f)
            {
                num12 = vector.x;
                num13 = vector.y;
                currentVelocity.x = (num12 - vector.x) / deltaTime;
                currentVelocity.y = (num13 - vector.y) / deltaTime;
            }

            return new Vector2d(num12, num13);
        }

        public static Vector2d operator +(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x + b.x, a.y + b.y);
        }

        public static Vector2d operator -(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x - b.x, a.y - b.y);
        }

        public static Vector2d operator *(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x * b.x, a.y * b.y);
        }

        public static Vector2d operator /(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x / b.x, a.y / b.y);
        }

        public static Vector2d operator -(Vector2d a)
        {
            return new Vector2d(0d - a.x, 0d - a.y);
        }

        public static Vector2d operator *(Vector2d a, double d)
        {
            return new Vector2d(a.x * d, a.y * d);
        }

        public static Vector2d operator *(double d, Vector2d a)
        {
            return new Vector2d(a.x * d, a.y * d);
        }

        public static Vector2d operator /(Vector2d a, double d)
        {
            return new Vector2d(a.x / d, a.y / d);
        }

        public static bool operator ==(Vector2d lhs, Vector2d rhs)
        {
            double num = lhs.x - rhs.x;
            double num2 = lhs.y - rhs.y;
            return num * num + num2 * num2 < 9.99999944E-11d;
        }

        public static bool operator !=(Vector2d lhs, Vector2d rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator Vector2d(Vector3 v)
        {
            return new Vector2d(v.x, v.y);
        }

        public static implicit operator Vector3d(Vector2d v)
        {
            return new Vector3d(v.x, v.y, 0d);
        }
    }
}

