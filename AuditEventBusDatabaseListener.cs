using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;

namespace Dargon.Audits.Db {
   public class AuditEventBusDatabaseListener {
      private Dictionary<IPersistenceConfigurer, FluentNHibernateDatabase> databases;
      private FluentNHibernateDatabase database;

      public AuditEventBusDatabaseListener(AuditEventBus auditEventBus, IPersistenceConfigurer configurer) {
         database = new FluentNHibernateDatabase(configurer);
         databases = new Dictionary<IPersistenceConfigurer, FluentNHibernateDatabase> { { configurer, database } };

         auditEventBus.EventPosted += (sender, auditEvent) => database.Add(auditEvent);
      }

      public void SetDatabase(IPersistenceConfigurer configurer) {
         if (databases.TryGetValue(configurer, out database))
            return;
         database = new FluentNHibernateDatabase(configurer);
         databases.Add(configurer, database);
      }
      
   }
}
