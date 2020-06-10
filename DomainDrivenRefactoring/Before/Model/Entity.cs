using System;

namespace Before.Model
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
    }
}