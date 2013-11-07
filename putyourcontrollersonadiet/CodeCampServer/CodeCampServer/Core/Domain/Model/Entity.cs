using System;

namespace CodeCampServerLite.Core.Domain.Model
{
    public abstract class Entity
    {
        public virtual Guid Id { get; protected set; }

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
            // When this instance is transient, we use the base GetHashCode()
            // and remember it, so an instance can NEVER change its hash code.
            if (IsTransient())
            {
                return base.GetHashCode();
            }

            return Id.GetHashCode();
        }

        private bool IsTransient()
        {
            return Equals(Id, Guid.Empty);
        }
    }
}