using System;
using Microsoft.EntityFrameworkCore;

namespace back_end.Data
{
	public class TripContext:DbContext
	{
		public TripContext(DbContextOptions options) :base(options)
		{

		}
	}
}

