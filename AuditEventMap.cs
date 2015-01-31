using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Audits.Db {
   class AuditEventMap : ClassMap<AuditEvent> {
      AuditEventMap() {
         Id();
         Map(x => x.EventType).CustomType(typeof(AuditEventType));
         Map(x => x.EventKey);
         Map(x => x.EventMessage);
         Map(x => x.EventData);
      }
   }
}
