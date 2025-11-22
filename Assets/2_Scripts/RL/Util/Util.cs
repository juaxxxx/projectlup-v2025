using System.Text;
using System.Collections;
using UnityEngine;

namespace Roguelike.Util
{
    public static class RoguelikeUtil
    {
        //엔진 에러 로그창에서, 오류클릭하면 오히려 여기로 와서 별로임
        //private static StringBuilder sb = new StringBuilder();
        //public static void PrintError(string msg, GameObject caller = null)
        //{
        //    sb.Clear();

        //    if (caller != null)
        //    {
        //        sb.Append('[');
        //        sb.Append(caller.name);
        //        sb.Append(']');
        //    }

        //    sb.Append(msg);

        //    Debug.LogError(msg);
        //}

        //public static void PrintWarning(string msg, GameObject caller = null)
        //{
        //    sb.Clear();

        //    if (caller != null)
        //    {
        //        sb.Append('[');
        //        sb.Append(caller.name);
        //        sb.Append(']');
        //    }

        //    Debug.LogWarning(msg);
        //}

        public static IEnumerator DelayOneFrame(System.Action callback)
        {
            yield return null;
            callback?.Invoke();
        }

        public static IEnumerator DelayForSeconds(float seconds, System.Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}
