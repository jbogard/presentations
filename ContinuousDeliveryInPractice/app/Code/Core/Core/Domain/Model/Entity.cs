using System;

namespace CodeCampServerLite.Core.Domain.Model
{
    public abstract class Entity
    {
        private int? _oldHashCode;

        public virtual Guid Id { get; private set; }

        public virtual bool IsPersistent
        {
            get { return !IsTransient(); }
        }

        public override bool Equals(object obj)
        {
            if (!IsTransient())
            {
                var persistentObject = obj as Entity;
                return (persistentObject != null) && (Id == persistentObject.Id);
            }

            return base.Equals(obj);
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            // Once we have a hash code we'll never change it
            if (_oldHashCode.HasValue)
                return _oldHashCode.Value;

            // When this instance is transient, we use the base GetHashCode()
            // and remember it, so an instance can NEVER change its hash code.
            if (IsTransient())
            {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }
            return Id.GetHashCode();
        }

        private bool IsTransient()
        {
            return Equals(Id, Guid.Empty);
        }
    }
}