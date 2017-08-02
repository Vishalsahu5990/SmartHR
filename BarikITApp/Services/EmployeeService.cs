
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BarikITApp;
using SQLite.Net.Async;
using Xamarin.Forms;


namespace BarikITApp
{
	public class EmployeeService
	{
		private static readonly AsyncLock Locker = new AsyncLock();

		private SQLiteAsyncConnection Database { get; } = DependencyService.Get<ISQLite>().GetAsyncConnection();

		public async Task AddEmployees(List<tblEmployee> movies)
		{
			using (await Locker.LockAsync())
			{
				await Database.InsertAllAsync(movies);
			}
		}

		public async Task<List<tblEmployee>> GetEmployees()
		{
			using (await Locker.LockAsync())
			{
				return await Database.Table<tblEmployee>().Where(x => x.Id > 0).ToListAsync();
			}
		}
		public async Task DeleteEmployee(tblEmployee emp)
		{
			try
			{
using (await Locker.LockAsync())
			{

				//var entity = await Database.FindAsync<tblEmployee>(id);

				var res=await Database.DeleteAsync(emp);
			}
			}
			catch (Exception ex)
			{

			}
		}
		public async Task<int> UpdateEmployees(List<tblEmployee>  entity)
		{
			using (await Locker.LockAsync())
			{
				return await Database.UpdateAsync(entity);
			}
		}
	}
}
