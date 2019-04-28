using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public const float EPSILON = 0.0001f;
        public const float TWO_PI = Mathf.PI * 2.0f;
        public const float HALF_PI = Mathf.PI * 0.5f;
        public const float QUARTER_PI = Mathf.PI * 0.25f;
        public static readonly float SQRT2 = Mathf.Sqrt(2f);
        public static readonly float SQRT3 = Mathf.Sqrt(3f);
        
        /// <summary>
        /// Wrap an angle between 0 and 2 PI
        /// </summary>
        /// <returns>
        /// The angle, wrapped.
        /// </returns>
        /// <param name='angle'>
        /// The value to wrap.
        /// </param>
        public static float WrapAngle(float angle)
        {
            return Normalize(angle, 0.0f, TWO_PI);
        }
        
        /// <summary>
        /// Normalize the specified value between start and end. Normalize assumes that
        /// values outside of this range are wrapped.
        /// 
        /// Examples: Normalize(angle, 0, 360) will normalize angle between 0 and 360. If angle
        /// is above 360, it will wrap. Some sample cases:
        /// 
        /// Normalize(300, 0, 360) = 300
        /// Normalize(500, 0, 360) = 140
        /// Normalize(-50, 0, 360) = 310
        /// Normalize(360, 0, 360) = 0
        /// 
        /// </summary>
        /// <param name='value'>
        /// The value to normalize.
        /// </param>
        /// <param name='start'>
        /// The start of the range to normalize within. Must be less than end, otherwise a
        /// division by zero will occur, resulting in undefined behavior.
        /// </param>
        /// <param name='end'>
        /// The end of the range to normalize within. Must be greater than start, otherwise a 
        /// dicision by zero will occur, resulting in undefined behavior.
        /// </param>
        public static float Normalize(float value, float start, float end)
        {
            float range = end - start;
            float offset = value - start;
            
            return value - Mathf.Floor(offset / range) * range;
        }
        
        // Same as Normalize(value, 0f, end).
        public static float FloatMod(float value, float end)
        {
            return value - Mathf.Floor(value / end) * end;
        }
        
        /// Normalize a value that is in the range of min .. max to the new range 0.0 .. 1.0
        public static float NormalizeRange(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }
        
        /// Normalize a value that is in the range of min .. max to the new range newMin .. newMax
        public static float NormalizeRange(float value, float min, float max,
                                           float newMin, float newMax)
        {
            return newMin + NormalizeRange(value, min, max) * (newMax - newMin);
        }
        
        /// <summary>
        /// Return a target angle whose angular distance to current is minimal.
        /// </summary>
        /// <returns>
        /// A target angle that whose distance to current is minimal.
        /// </returns>
        /// <param name='current'>
        /// The angle that will be rotated from.
        /// </param>
        /// <param name='target'>
        /// The angle that will be rotated to.
        /// </param>
        public static float MinAngle(float current, float target)
        {
            float diff = WrapAngle(target - current);
            
            if (diff > Mathf.PI)
            {
                diff -= MathUtils.TWO_PI;
            }
            else if (diff < -Mathf.PI)
            {
                diff += MathUtils.TWO_PI;
            }
            
            return current + diff;
        }
        
        /// <summary>
        /// Gives an integer approximation of multiplying by sqrt(2).
        /// Only effective on large enough numbers, ideally greater than 100.
        /// </summary>
        /// <returns>
        /// Integer approximation of val * sqrt(2)
        /// </returns>
        /// <param name='val'>
        /// Number to multiply
        /// </param>
        public static int IntMultSqrt2(int val)
        {
            return ((val * 1414) / 1000);
        }
        
        public static float GetQuatLength(Quaternion q)
        {
            return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        }
        
        public static Quaternion GetQuatConjugate(Quaternion q)
        {
            return new Quaternion(-q.x, -q.y, -q.z, q.w);
        }
        
        /// <summary>
        /// Logarithm of a unit quaternion. The result is not necessary a unit quaternion.
        /// </summary>
        public static Quaternion GetQuatLog(Quaternion q)
        {
            Quaternion res = q;
            res.w = 0;
            
            if (Mathf.Abs(q.w) < 1.0f)
            {
                float theta = Mathf.Acos(q.w);
                float sin_theta = Mathf.Sin(theta);
                
                if (Mathf.Abs(sin_theta) > 0.0001)
                {
                    float coef = theta / sin_theta;
                    res.x = q.x * coef;
                    res.y = q.y * coef;
                    res.z = q.z * coef;
                }
            }
            
            return res;
        }
        
        public static Quaternion GetQuatExp(Quaternion q)
        {
            Quaternion res = q;
            
            float fAngle = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z);
            float fSin = Mathf.Sin(fAngle);
            
            res.w = Mathf.Cos(fAngle);
            
            if (Mathf.Abs(fSin) > 0.0001)
            {
                float coef = fSin / fAngle;
                res.x = coef * q.x;
                res.y = coef * q.y;
                res.z = coef * q.z;
            }
            
            return res;
        }
        
        /// <summary>
        /// SQUAD Spherical Quadrangle interpolation [Shoe87]
        /// </summary>
        public static Quaternion GetQuatSquad(float t, Quaternion q0, Quaternion q1, Quaternion a0, Quaternion a1)
        {
            float slerpT = 2.0f * t * (1.0f - t);
            
            Quaternion slerpP = Slerp(q0, q1, t);
            Quaternion slerpQ = Slerp(a0, a1, t);
            
            return Slerp(slerpP, slerpQ, slerpT);
        }
        
        public static Quaternion GetSquadIntermediate(Quaternion q0, Quaternion q1, Quaternion q2)
        {
            Quaternion q1Inv = GetQuatConjugate(q1);
            Quaternion p0 = GetQuatLog(q1Inv * q0);
            Quaternion p2 = GetQuatLog(q1Inv * q2);
            Quaternion sum = 
                new Quaternion(-0.25f * (p0.x + p2.x), 
                               -0.25f * (p0.y + p2.y), 
                               -0.25f * (p0.z + p2.z), 
                               -0.25f * (p0.w + p2.w));
            
            return q1 * GetQuatExp(sum);
        }
        
        /// <summary>
        /// Smooths the input parameter t.
        /// If less than k1 ir greater than k2, it uses a sin.
        /// Between k1 and k2 it uses linear interp.
        /// </summary>
        public static float Ease(float t, float k1, float k2)
        {
            float f;
            float s;
            
            f = k1 * 2 / Mathf.PI + k2 - k1 + (1.0f - k2) * 2 / Mathf.PI;
            
            if (t < k1)
            {
                s = k1 * (2 / Mathf.PI) * (Mathf.Sin((t / k1) * Mathf.PI / 2 - Mathf.PI / 2) + 1);
            }
            else if (t < k2)
            {
                s = (2 * k1 / Mathf.PI + t - k1);
            }
            else
            {
                s = 2 * k1 / Mathf.PI + k2 - k1 + ((1 - k2) * (2 / Mathf.PI)) * 
                    Mathf.Sin(((t - k2) / (1.0f - k2)) * Mathf.PI / 2);
            }
            
            return (s / f);
        }
        
        /// <summary>
        /// We need this because Quaternion.Slerp always uses the shortest arc.
        /// </summary>
        public static Quaternion Slerp(Quaternion p, Quaternion q, float t)
        {
            Quaternion ret;
            
            float fCos = Quaternion.Dot(p, q);
            
            if ((1.0f + fCos) > 0.00001)
            {
                float fCoeff0, fCoeff1;
                
                if ((1.0f - fCos) > 0.00001)
                {
                    float omega = Mathf.Acos(fCos);
                    float invSin = 1.0f / Mathf.Sin(omega);
                    fCoeff0 = Mathf.Sin((1.0f - t) * omega) * invSin;
                    fCoeff1 = Mathf.Sin(t * omega) * invSin;
                }
                else
                {
                    fCoeff0 = 1.0f - t;
                    fCoeff1 = t;
                }
                
                ret.x = fCoeff0 * p.x + fCoeff1 * q.x;
                ret.y = fCoeff0 * p.y + fCoeff1 * q.y;
                ret.z = fCoeff0 * p.z + fCoeff1 * q.z;
                ret.w = fCoeff0 * p.w + fCoeff1 * q.w;
            }
            else
            {
                float fCoeff0 = Mathf.Sin((1.0f - t) * Mathf.PI * 0.5f);
                float fCoeff1 = Mathf.Sin(t * Mathf.PI * 0.5f);
                
                ret.x = fCoeff0 * p.x - fCoeff1 * p.y;
                ret.y = fCoeff0 * p.y + fCoeff1 * p.x;
                ret.z = fCoeff0 * p.z - fCoeff1 * p.w;
                ret.w = p.z;
            }
            
            return ret;
        }
        
        public static float SafeAngle(float input)
        {
            if (input < 0f)
            {
                input += 360f;
            }
            if (input > 360f)
            {
                input -= 360f;
            }
            return input;
        }
        
        public static float EaseToSinInOut(float x)
        {
            return (1f - Mathf.Cos(Mathf.PI * Mathf.Clamp(x, 0f, 1f))) / 2f;
        }
        
        public static float EaseToSinOut(float x)
        {
            return Mathf.Sin((Mathf.PI * Mathf.Clamp(x, 0f, 1f)) / 2f);
        }
        
        public static float EaseToPower(float x, int power)
        {
            float returnValue = 0f;
            if (power % 2 == 0)
            {
                returnValue = -Mathf.Pow((Mathf.Clamp(x, 0f, 1f) - 1f), Convert.ToSingle(power)) + 1f;
            }
            else
            {
                returnValue = Mathf.Pow((Mathf.Clamp(x, 0f, 1f) - 1f), Convert.ToSingle(power)) + 1f;
            }
            return returnValue;
        }
        
        // http://wiki.unity3d.com/index.php/3d_Math_functions
        // Get the intersection between a line and a plane. 
        // If the line and plane are not parallel, the function outputs true, otherwise false.
        public static bool LinePlaneIntersection(out Vector3 intersection, 
                                                 Vector3 linePoint, 
                                                 Vector3 lineVec, 
                                                 Vector3 planeNormal, 
                                                 Vector3 planePoint)
        {
            float length;
            float dotNumerator;
            float dotDenominator;
            Vector3 vector;
            intersection = Vector3.zero;
            
            // calculate the distance between the linePoint and the line-plane intersection point
            dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
            dotDenominator = Vector3.Dot(lineVec, planeNormal);
            
            // line and plane are not parallel
            if (dotDenominator != 0.0f)
            {
                length =  dotNumerator / dotDenominator;
                
                // create a vector from the linePoint to the intersection point
                vector = SetVectorLength(lineVec, length);
                
                // get the coordinates of the line-plane intersection point
                intersection = linePoint + vector;  

                return true;    
            }
            
            // output not valid
            else
            {
                return false;
            }
        }
        
        //create a vector of direction "vector" with length "size"
        public static Vector3 SetVectorLength(Vector3 vector, float size)
        {
            //normalize the vector
            Vector3 vectorNormalized = Vector3.Normalize(vector);
            
            //scale the vector
            return vectorNormalized *= size;
        }

        // Round the component values of a Vector3 to integers. Useful for snapping.
        public static Vector3 Vector3Round(Vector3 input)
        {
            Vector3 result = input;
            result.x = Mathf.Round(result.x);
            result.y = Mathf.Round(result.y);
            result.z = Mathf.Round(result.z);
            return result;
        }

        /// <summary>
        /// Calculates the intersection line segment between 2 lines (not segments).
        /// Returns false if no solution can be found.
        /// Source: http://paulbourke.net/geometry/pointlineplane/calclineline.cs
        /// </summary>
        /// <returns></returns>
        public static bool CalculateLineLineIntersection(Vector3 line1Point1, Vector3 line1Point2, 
            Vector3 line2Point1, Vector3 line2Point2, out Vector3 resultSegmentPoint1, out Vector3 resultSegmentPoint2)
        {
            // Algorithm is ported from the C algorithm of 
            // Paul Bourke at http://local.wasp.uwa.edu.au/~pbourke/geometry/lineline3d/
            resultSegmentPoint1 = Vector3.zero;
            resultSegmentPoint2 = Vector3.zero;

            Vector3 p1 = line1Point1;
            Vector3 p2 = line1Point2;
            Vector3 p3 = line2Point1;
            Vector3 p4 = line2Point2;
            Vector3 p13 = p1 - p3;
            Vector3 p43 = p4 - p3;

            if (p43.sqrMagnitude < Mathf.Epsilon) {
                return false;
            }
            Vector3 p21 = p2 - p1;
            if (p21.sqrMagnitude < Mathf.Epsilon) {
                return false;
            }

            float d1343 = p13.x * p43.x + p13.y * p43.y + p13.z * p43.z;
            float d4321 = p43.x * p21.x + p43.y * p21.y + p43.z * p21.z;
            float d1321 = p13.x * p21.x + p13.y * p21.y + p13.z * p21.z;
            float d4343 = p43.x * p43.x + p43.y * p43.y + p43.z * p43.z;
            float d2121 = p21.x * p21.x + p21.y * p21.y + p21.z * p21.z;

            float denom = d2121 * d4343 - d4321 * d4321;
            if (Math.Abs(denom) < Mathf.Epsilon) {
                return false;
            }
            float numer = d1343 * d4321 - d1321 * d4343;

            float mua = numer / denom;
            float mub = (d1343 + d4321 * (mua)) / d4343;

            resultSegmentPoint1.x = (p1.x + mua * p21.x);
            resultSegmentPoint1.y = (p1.y + mua * p21.y);
            resultSegmentPoint1.z = (p1.z + mua * p21.z);
            resultSegmentPoint2.x = (p3.x + mub * p43.x);
            resultSegmentPoint2.y = (p3.y + mub * p43.y);
            resultSegmentPoint2.z = (p3.z + mub * p43.z);

            return true;
        }

        /// <summary>
        /// Determines whether the point is inside the GameObject defined polygon
        /// </summary>
        public static bool IsPointInside(Vector3 point, List<GameObject> polyPoints)
        {
            List<Vector3> newPositions = new List<Vector3>();
            for (int i = 0, c = polyPoints.Count; i < c; i++)
            {
                newPositions.Add(polyPoints[i].transform.position);
            }
            return IsPointInside(point, newPositions);
        }

        /// <summary>
        /// Determines whether the point is inside the polygon
        /// </summary>
        public static bool IsPointInside(Vector3 point, List<Vector3> polyPoints)
        {
            bool isInside = false;

            // If the z value of our character's position intersects with an odd number of
            // lines on one side of a boundary, we know we're inside it.
            int numVerts = polyPoints.Count;
            for (int j = 0; j < numVerts; j++)
            {
                Vector3 currentPoint = polyPoints[j];
                Vector3 prevPoint = j == 0 ?
                    polyPoints[numVerts - 1] :
                    polyPoints[j - 1];

                // Does our shape intersect on the Z axis?
                if ((currentPoint.x >= point.x && prevPoint.x < point.x) ||
                    (currentPoint.x < point.x && prevPoint.x >= point.x))
                {
                    // If we intersect on Z, figure out the intersection point.
                    float rise = 0f;
                    float run = 0f;
                    if (currentPoint.x < prevPoint.x)
                    {
                        rise = prevPoint.y - currentPoint.y;
                        run = prevPoint.x - currentPoint.x;
                    }
                    else
                    {
                        rise = currentPoint.y - prevPoint.y;
                        run = currentPoint.x - prevPoint.x;
                    }
                    float slope = rise / run;
                    float offset = prevPoint.y - (prevPoint.x * slope);
                    float intercept = point.x * slope + offset;

                    // Count how many intersects we have on one side of the entity.
                    if (point.y > intercept)
                    {
                        // If there's an odd number of intersects, we know we're inside the zone.
                        isInside = !isInside;
                    }
                }
            }

            return isInside;
        }

        /// <summary>
        /// Given a line segment and a list of GameObjects defining a poly hull, returns a list of all intersections
        /// between that line and poly.
        /// </summary>
        public static List<Vector3> LineSegmentPolyIntersects(Vector3 startPoint, 
                                                              Vector3 endPoint, 
                                                              List<GameObject> poly)
        {
            List<Vector3> newPositions = new List<Vector3>();
            for (int i = 0, c = poly.Count; i < c; i++)
            {
                newPositions.Add(poly[i].transform.position);
            }
            return LineSegmentPolyIntersects(startPoint, endPoint, newPositions);
        }

        /// <summary>
        /// Given a line segment and a list of Vector3 points defining a poly hull, returns a list of all intersections
        /// between that line and poly.
        /// </summary>
        public static List<Vector3> LineSegmentPolyIntersects(Vector3 startPoint, Vector3 endPoint, List<Vector3> poly)
        {
            List<Vector3> intersections = new List<Vector3>();

            for (int i = 0, c = poly.Count; i < c; i++)
            {
                Vector3 testFromNode = Vector3.zero;
                Vector3 testToNode = Vector3.zero;
                testFromNode = poly[i];
                if ((i + 1) == c)
                {
                    testToNode = poly[0];
                }
                else
                {
                    testToNode = poly[i + 1];
                }

                Vector3 resultVecNear = Vector3.zero;
                Vector3 resultVecFar = Vector3.zero;
                CalculateLineLineIntersection(startPoint, 
                                              endPoint, 
                                              testFromNode, 
                                              testToNode, 
                                              out resultVecNear, 
                                              out resultVecFar);

                float fromDot = Vector3.Dot((resultVecNear - testFromNode), (testToNode - testFromNode));
                float toDot = Vector3.Dot((resultVecNear - testToNode), (testFromNode - testToNode));
                float startDot = Vector3.Dot((resultVecNear - startPoint), (endPoint - startPoint));
                float endDot = Vector3.Dot((resultVecNear - endPoint), (startPoint - endPoint));
                if (fromDot > 0 && 
                    toDot > 0 && 
                    startDot > 0 && 
                    endDot > 0 && 
                    resultVecNear != startPoint && 
                    resultVecNear != endPoint &&
                    (resultVecFar - resultVecNear).magnitude <= EPSILON)
                {
                    intersections.Add(resultVecNear);
                }
            }

            return intersections;
        }
    }
}
