using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common
{
    class PoolObject
    {
        public bool IsActive { get; set; }
        public MessageBox MessageBox { get; set; }
        public PoolObject(MessageBox messageBox, bool isActive)
        {
            MessageBox = messageBox;
            IsActive = isActive;
        }

        public override int GetHashCode()
        {
            return MessageBox.GetHashCode();
        }
    }

    public class MessageBoxManager : IDisposable
    {
        private List<PoolObject> _objects;

        public MessageBoxManager(MessageBox prefab) : this(prefab, 0) { }

        private Canvas _canvas;
        private MessageBox _msgPrefab;

        public MessageBoxManager(MessageBox prefab, int capacity)
        {
            var resOBJ = Resources.Load<GameObject>("Prefabs/Common/MsgCanvas");
            _canvas = GameObject.Instantiate(resOBJ).GetComponent<Canvas>();
            _msgPrefab = prefab;

            _objects = new List<PoolObject>();
            for (int i = 0; i < capacity; i++)
            {
                var msg = GameObject.Instantiate(_msgPrefab, _canvas.transform);
                msg.gameObject.SetActive(false);

                var poolObj = new PoolObject(msg, false);
                _objects.Add(poolObj);
            }
        }

        public MessageBox Rental()
        {
            PoolObject obj = null;
            for (int i = 0; i < _objects.Count; i++)
            {
                if (!_objects[i].IsActive)
                {
                    obj = _objects[i];
                    break;
                }
            }

            // 存在してたらそれを返す、なければ作る
            if (obj != null)
            {
                obj.IsActive = true;
                obj.MessageBox.gameObject.SetActive(true);
                return obj.MessageBox;
            }
            else
            {
                var msg = GameObject.Instantiate(_msgPrefab, _canvas.transform);
                msg.gameObject.SetActive(true);

                var poolObj = new PoolObject(msg, true);
                _objects.Add(poolObj);
                return msg;
            }
        }
        public void Release(MessageBox msgBox)
        {
            foreach (var poolObj in _objects)
            {
                if (poolObj.MessageBox == msgBox)
                {
                    poolObj.IsActive = false;
                    poolObj.MessageBox.gameObject.SetActive(false);
                    return;
                }
            }
        }

        public void Dispose()
        {
            foreach (var poolObj in _objects)
            {
                if (poolObj.MessageBox != null)
                {
                    GameObject.Destroy(poolObj.MessageBox.gameObject);
                }
            }
            _objects.Clear();

            if (_canvas != null)
            {
                GameObject.Destroy(_canvas.gameObject);
            }
        }
    }
}