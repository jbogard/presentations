using System;

namespace After.Model
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
    }
}