using System;
using Microsoft.EntityFrameworkCore;

namespace SteamBigData.Data
{
	public class SteamBigDataDbContext : DbContext
	{
		public SteamBigDataDbContext(DbContextOptions<SteamBigDataDbContext> options) : base(options)
		{

		}

		public DbSet<SoldInfo> SoldInfos { get; set; }
	}
}

