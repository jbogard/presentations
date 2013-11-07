namespace CodeCampServerLite.Helpers
{	
	public interface IFormHandler<T>
	{
		void Handle(T form);
	}
}