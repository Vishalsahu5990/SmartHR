using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarikITApp
{
	public interface IEmployeeService
	{
		Task AddEmployees(IList<tblEmployee> movies);

		Task<IList<tblEmployee>> GetEmployees();
	}
}
