using Microsoft.EntityFrameworkCore;
using SisbolApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SisbolApi.Services
{
    public class ApiLogService
    {
        private readonly LogDbContext _db;

        public ApiLogService(LogDbContext db)
        {
            _db = db;
        }

        public async Task Log(ApiLogItem apiLogItem)
        {
            _db.ApiLogItem.Add(apiLogItem);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApiLogItem>> Get()
        {
            var items  = from i in _db.ApiLogItem
                         orderby i.Id descending
                         select i;

            return await items.ToListAsync();
        }
    }
}
