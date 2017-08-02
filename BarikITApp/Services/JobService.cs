using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BarikITApp;
using SQLite.Net.Async;
using Xamarin.Forms;


namespace BarikITApp
{
	public class JobService
	{
private static readonly AsyncLock Locker = new AsyncLock();

private SQLiteAsyncConnection Database { get; } = DependencyService.Get<ISQLite>().GetAsyncConnection();

public async Task AddJobs(List<tblJob> movies)
{
	using (await Locker.LockAsync())
	{
		await Database.InsertAllAsync(movies);
	}
}

public async Task<List<tblJob>> GetJobs()
{
	using (await Locker.LockAsync())
	{
		return await Database.Table<tblJob>().Where(x => x.Id > 0).ToListAsync();
	}
}

	}
}
