using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.Contact.Components;
using VRC.SDK3.Dynamics.PhysBone.Components;
using System;

namespace AvatarAnalyzer
{
    public enum InfoType { Warn, Bad, Normal };
    public class ObjectItem
    {
        public GameObject obj;
        public Dictionary<string, Component> component = new Dictionary<string, Component>();
        public Dictionary<KeyValuePair<InformationCode, Component>, List<Component>> attribute = new Dictionary<KeyValuePair<InformationCode, Component>, List<Component>>();
        public Dictionary<KeyValuePair<InformationCode, Component>, List<Component>> badAttribute = new Dictionary<KeyValuePair<InformationCode, Component>, List<Component>>();
        public Dictionary<KeyValuePair<InformationCode, Component>, List<Component>> warnAttribute = new Dictionary<KeyValuePair<InformationCode, Component>, List<Component>>();

        public ObjectItem(GameObject obj)
        {
            this.obj = obj;
            componentInit<ParentConstraint>();
            componentInit<PositionConstraint>();
            componentInit<RotationConstraint>();
            componentInit<ScaleConstraint>();
            componentInit<AimConstraint>();
            componentInit<LookAtConstraint>();
            componentInit<SkinnedMeshRenderer>();
            componentInit<MeshRenderer>();
            componentInit<MeshFilter>();
            componentInit<Light>();
            componentInit<ParticleSystem>();
            componentInit<VRCPhysBone>();
            componentInit<VRCPhysBoneColliderBase>();
            componentInit<VRCContactReceiver>();
            componentInit<VRCContactSender>();
            if (this.obj.tag == "EditorOnly")
                AddAttribute(InfoType.Normal, QuickCreateKey(InformationCode.IsEditorOnly));
        }

        public bool hasComponent<T>()
        {
            return component.ContainsKey(typeof(T).Name);
        }
        public T getComponent<T>() where T : Component
        {
            return component[typeof(T).Name] as T;
        }
        private void componentInit<T>() where T : Component
        {
            if (obj.GetComponent<T>() == null)
                return;
            component.Add(typeof(T).Name, obj.GetComponent<T>());
        }

        public static KeyValuePair<InformationCode, Component> QuickCreateKey(InformationCode IT, Component CMP)
        {
            return new KeyValuePair<InformationCode, Component>(IT, CMP);
        }
        public static KeyValuePair<InformationCode, Component> QuickCreateKey(InformationCode IT)
        {
            return new KeyValuePair<InformationCode, Component>(IT, null);
        }
        public Dictionary<KeyValuePair<InformationCode, Component>, List<Component>> GetAttribute(InfoType IT)
        {
            switch (IT)
            {
                case InfoType.Warn:
                    return warnAttribute;
                case InfoType.Bad:
                    return badAttribute;
                case InfoType.Normal:
                    return attribute;
            }
            throw new Exception();
        }

        public List<Component> AddAttribute(InfoType IT, KeyValuePair<InformationCode, Component> CodeKV)
        {
            var attr = GetAttribute(IT);
            if (!attr.ContainsKey(CodeKV))
                attr.Add(CodeKV, new List<Component>());
            return attr[CodeKV];
        }
        public void AddAttribute(InfoType IT, KeyValuePair<InformationCode, Component> CodeKV, Component cmp)
        {
            AddAttribute(IT, CodeKV).Add(cmp);
        }

        public void AddAttribute(InfoType IT, KeyValuePair<InformationCode, Component> CodeKV, List<Component> cmp)
        {
            AddAttribute(IT, CodeKV).AddRange(cmp);

        }

        public bool HasAttribute(InfoType IT, KeyValuePair<InformationCode, Component> CodeKV)
        {
            return GetAttribute(IT).ContainsKey(CodeKV);
        }

        public bool hasDepend()
        {
            return 0 < component.Count + attribute.Count;
        }
    }
}