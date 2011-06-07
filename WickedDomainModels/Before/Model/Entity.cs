using System;

namespace Before.Model
{
	public abstract class Entity : IComparable<Entity>
	{
		public const string ID = "Id";

		public Guid Id { get; protected set; }

		public int CompareTo(Entity other)
		{
			if (IsPersistent())
			{
				return Id.CompareTo(other.Id);
			}
			return 0;
		}

		public override bool Equals(object obj)
		{
			if (IsPersistent())
			{
				var Entity = obj as Entity;
				return (Entity != null) && (Id == Entity.Id);
			}

			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return IsPersistent() ? Id.GetHashCode() : base.GetHashCode();
		}

		private bool IsPersistent()
		{
			return (Id != Guid.Empty);
		}
	}
}