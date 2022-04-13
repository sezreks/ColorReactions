using UnityEngine;

namespace Extentions
{
    public static class TransformExtentions
    {
        public static void ResetTransformation(this Transform trans)
        {
            trans.position = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = new Vector3(1, 1, 1);
        }
        public static void ResetTransformation(this Transform trans, Vector3 Position)
        {
            trans.position = Position;
            trans.localRotation = Quaternion.identity;
            trans.localScale = new Vector3(1, 1, 1);
        }
        public static void ResetTransformation(this Transform trans, Vector3 Position, Quaternion Rotation, Vector3 Scale)
        {
            trans.position = Position;
            trans.localRotation = Rotation;
            trans.localScale = Scale;
        }

        public static Transform Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            return transform;
        }
        public static Transform ClearImmediate(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
            return transform;
        }


    }
}
