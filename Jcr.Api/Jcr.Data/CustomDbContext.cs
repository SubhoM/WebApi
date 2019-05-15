using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Data
{
    public class CustomDbContext : DbContext
    {
        protected CustomDbContext(string nameOrConnectionString)
                   : base(nameOrConnectionString)
        {
        }

        protected CustomDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }

        protected CustomDbContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }

        protected CustomDbContext(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {

        }
    }
    public partial class DBAMPContext
    {
        private static void DBAMPContextStaticPartial() { }
    }
    public partial class DBMEdition01Context
    {
        private static void DBMEdition01ContextStaticPartial() { }
    }
    public partial class DBFileTableContext
    {
        private static void DBFileTableContextStaticPartial() { }
    }

    
}
