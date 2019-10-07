using UnityEngine;

public class Modifier
{
    public class ModifierProperty
    {
        public virtual void Modify(Transform obj, float value1){}
        public virtual void Modify(Transform obj, float value1, float value2){}
    }

    public class ChangeRotationModifier:ModifierProperty
    {
        public override void Modify(Transform objectRotation, float changeValue)
        {
            objectRotation.localRotation = Quaternion.Euler(0,0,changeValue);
        }
    }
    public class ChangeRotationClampedModifier:ModifierProperty
    {
        public override void Modify(Transform objectRotation, float targetRotation, float interpolation)
        {
            objectRotation.localRotation = Quaternion.SlerpUnclamped(objectRotation.localRotation, Quaternion.Euler(0,0,targetRotation), interpolation);
        }
        
    }

    public ModifierProperty _changeRotation = new ChangeRotationModifier();
    public ModifierProperty _changeRotationClamped = new ChangeRotationClampedModifier(); 
}