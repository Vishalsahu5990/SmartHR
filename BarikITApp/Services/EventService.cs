using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BarikITApp;
using SQLite.Net.Async;
using Xamarin.Forms;


namespace BarikITApp
{
	public class EventService
	{
private static readonly AsyncLock Locker = new AsyncLock();

private SQLiteAsyncConnection Database { get; } = DependencyService.Get<ISQLite>().GetAsyncConnection();

		public async Task AddEvents(List<tblEvent> movies)
{
	using (await Locker.LockAsync())
	{
		await Database.InsertAllAsync(movies);
	}
}

public async Task<List<tblEvent>> GetEvents()
{
	using (await Locker.LockAsync())
	{
		return await Database.Table<tblEvent>().Where(x => x.Id > 0).ToListAsync();
	}
}

	}
}
