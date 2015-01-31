using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;

namespace Dargon.Audits.Db {
   internal class FluentNHibernateDatabase : Database {
      private ISessionFactory sessionFactory;

      public FluentNHibernateDatabase(IPersistenceConfigurer configurer) { sessionFactory = CreateSessionFactory(configurer); }

      private static ISessionFactory CreateSessionFactory(IPersistenceConfigurer configurer) {
         return Fluently.Configure()
            .Database(configurer)
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<AuditEventMap>())
            .BuildSessionFactory();
      }

      public void Add(AuditEvent auditEvent) {
         using (ISession session = sessionFactory.OpenSession()) {
            using (ITransaction transaction = session.BeginTransaction()) {
               session.Save(auditEvent);
               transaction.Commit();
            }
         }
      }

      public IList<AuditEvent> Get() {
         using (ISession session = sessionFactory.OpenSession()) {
            using (ITransaction transaction = session.BeginTransaction()) {
               List<AuditEvent> list = session.Query<AuditEvent>().ToList();
               transaction.Commit();

               return list;
            }
         }
      }

      public IList<AuditEvent> Get(int index) {
         using (ISession session = sessionFactory.OpenSession()) {
            using (ITransaction transaction = session.BeginTransaction()) {
               IQuery query = session.CreateQuery("FROM AuditEvent entity WHERE id > :eventId");
               query.SetParameter("eventId", index);
               transaction.Commit();

               return query.List<AuditEvent>();
            }
         }
      }

   }

}
