using UnityEngine;
using System.Text;
using System;

namespace HealingJam
{
    public static class Utility
    {
        public static double SystemTimeInMilliseconds { get { return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds; } }

        public static Vector2 MousePosition()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            return (Vector2)Input.mousePosition;
#else
			if (Input.touchCount > 0)
			{
				return Input.touches[0].position;
			}

			return Vector2.zero;
#endif
        }
        public static bool MouseDown()
        {
            return Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began);
        }

        public static bool MouseUp()
        {
            return (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended));
        }

        /// <summary>
        /// Returns the world width of the orthographic camera.
        /// </summary>
        public static float WorldWidth(Camera cam)
        {
            return 2f * cam.orthographicSize * cam.aspect;
        }

        /// <summary>
        /// Returns the world height of the orthographic camera.
        /// </summary>
        public static float WorldHeight(Camera cam)
        {
            return 2f * cam.orthographicSize;
        }

        public static T RandomValueFrom<T>(params T[] values)
        {
            return values[UnityEngine.Random.Range(0, values.Length)];

        }
        public static int RandomChoose(float[] probs)
        {
            float total = 0;

            foreach (float elem in probs)
            {
                total += elem;
            }

            float randomPoint = UnityEngine.Random.value * total;

            for (int i = 0; i < probs.Length; i++)
            {
                if (randomPoint < probs[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }
            return probs.Length - 1;
        }


        //public static byte[] ByteArrayOrCalculation(byte[] temp, byte[] temp2)
        //{
        //    if (temp == null && temp2 == null)
        //        return null;
        //    if (temp == null)
        //        temp = new byte[0];
        //    if (temp2 == null)
        //        temp2 = new byte[0];

        //    byte[] result = new byte[Mathf.Max(temp.Length, temp2.Length)];

        //    for (int i = 0; i < temp.Length; ++i)
        //    {
        //        result[i] |= temp[i];
        //    }

        //    for (int i = 0; i < temp2.Length; ++i)
        //    {
        //        result[i] |= temp2[i];
        //    }

        //    return result;
        //}

        /// <summary>
        /// 벡터 사이각을 0 ~ 180도로 출력 한다.
        /// </summary>
        public static float Vector3AngleWith180(Vector3 v1, Vector3 v2)
        {
            return Vector3.Angle(v1, v2);
        }

        /// <summary>
        /// 벡터 사이각을 0 ~ 360도로 출력 한다.
        /// </summary>
        public static float Vector3AngleWith360(Vector3 v1, Vector3 v2)
        {
            float angle = Vector3.Angle(v1, v2);
            if (AngleDir(v1, v2, Vector3.up) == -1)
            {
                angle = 360.0f - angle;
                if (angle > 359.9999f)
                {
                    angle -= 360.0f;
                }
                return angle;
            }
            else
            {
                return angle;
            }
        }

        /// <summary>
        /// 두 벡터의 사이각이 0 - 180 도 일경우(오른쪽에 위치) 1을 아닐 경우(왼쪽에 위치) -1
        /// </summary>
        public static int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);
            if (dir > 0.0)
            {
                return 1;
            }
            else if (dir < 0.0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// p1 벡터가 p2 벡터 앞에 위치 하는지 비교 한다.
        /// </summary>
        public static bool IsFront(Vector3 p1, Vector3 p2)
        {
            float deg = Vector3AngleWith180(p1, p2);
            return deg <= 90;
        }

        /// <summary>
        /// 클래스의 멤버 변수들을 모두 복사 한다.
        /// </summary>
        public static void CopyAllTo<T>(T source, T target)
        {
            var type = typeof(T);
            foreach (var sourceProperty in type.GetProperties())
            {
                try
                {
                    var targetProperty = type.GetProperty(sourceProperty.Name);
                    targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
                }
                catch (Exception e) { Debug.LogError(e); }
            }
            foreach (var sourceField in type.GetFields())
            {
                try
                {
                    var targetField = type.GetField(sourceField.Name);
                    targetField.SetValue(target, sourceField.GetValue(source));
                }
                catch (Exception e) { Debug.LogError(e); }
            }
        }
    }
}