﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Collections
{
    public class TypeList : TypeList<object>, ITypeList
    {
    }
    public class TypeList<TBaseType> : ITypeList<TBaseType>
    {
        public int Count { get { return _typeList.Count; } }
        public bool IsReadOnly { get { return false; } }
        public Type this[int index]
        {
            get { return _typeList[index]; }
            set
            {
                CheckType(value);
                _typeList[index] = value;
            }
        }
        private readonly List<Type> _typeList;
        public TypeList()
        {
            _typeList = new List<Type>();
        }
        public void Add<T>() where T : TBaseType
        {
            _typeList.Add(typeof(T));
        }
        public void Add(Type item)
        {
            CheckType(item);
            _typeList.Add(item);
        }
        public void Insert(int index, Type item)
        {
            _typeList.Insert(index, item);
        }
        public int IndexOf(Type item)
        {
            return _typeList.IndexOf(item);
        }
        public bool Contains<T>() where T : TBaseType
        {
            return Contains(typeof(T));
        }
        public bool Contains(Type item)
        {
            return _typeList.Contains(item);
        }

        /// <inheritdoc/>
        public void Remove<T>() where T : TBaseType
        {
            _typeList.Remove(typeof(T));
        }

        /// <inheritdoc/>
        public bool Remove(Type item)
        {
            return _typeList.Remove(item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            _typeList.RemoveAt(index);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _typeList.Clear();
        }

        /// <inheritdoc/>
        public void CopyTo(Type[] array, int arrayIndex)
        {
            _typeList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<Type> GetEnumerator()
        {
            return _typeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _typeList.GetEnumerator();
        }

        private static void CheckType(Type item)
        {
            if (!typeof(TBaseType).IsAssignableFrom(item))
            {
                throw new ArgumentException("Given item is not type of " + typeof(TBaseType).AssemblyQualifiedName, "item");
            }
        }
    }
}
