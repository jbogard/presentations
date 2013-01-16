using System;
using NHibernate;

namespace CodeCampServerLite.Infrastructure.DataAccess
{
    public interface ITransactionBoundary : IDisposable
    {
        ISession CurrentSession { get; }
        void Begin();
        void Commit();
        void RollBack();
    }

    public class NHibernateTransactionBoundary : ITransactionBoundary
    {
		private readonly ISessionSource _sessionSource;
		private ITransaction _transaction;
		private bool _begun;
		private bool _disposed;
		private bool _rolledBack;

        public NHibernateTransactionBoundary(ISessionSource sessionSource)
		{
			_sessionSource = sessionSource;
		}

		public void Begin()
		{
			CheckIsDisposed();
		
			CurrentSession = _sessionSource.CreateSession();

			BeginNewTransaction();
			_begun = true;
		}

		public void Commit()
		{
			CheckIsDisposed();
			CheckHasBegun();

			if (_transaction.IsActive && !_rolledBack)
			{
				_transaction.Commit();
			}

			BeginNewTransaction();
		}

		public void RollBack()
		{
			CheckIsDisposed();
			CheckHasBegun();

			if (_transaction.IsActive)
			{
				_transaction.Rollback();
				_rolledBack = true;
			}

			BeginNewTransaction();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public ISession CurrentSession
		{
			get; private set;
		}

		private void BeginNewTransaction()
		{
			if (_transaction != null)
			{
				_transaction.Dispose();
			}

			_transaction = CurrentSession.BeginTransaction();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_begun || _disposed)
				return;

			if (disposing)
			{
				_transaction.Dispose();
				CurrentSession.Dispose();
			}

			_disposed = true;
		}

		private void CheckHasBegun()
		{
			if (!_begun)
				throw new InvalidOperationException("Must call Begin() on the unit of work before committing");
		}

		private void CheckIsDisposed()
		{
			if (_disposed) 
				throw new ObjectDisposedException(GetType().Name);
		}
    }
}