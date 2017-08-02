using System;
using SQLite.Net;
using SQLite.Net.Async;

namespace BarikITApp
{
	public interface ISQLite
	{
		void CloseConnection();
		SQLiteConnection GetConnection();
		SQLiteAsyncConnection GetAsyncConnection();
		void DeleteDatabase(); 
	}
}

