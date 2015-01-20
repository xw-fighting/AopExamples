using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AspectOrientedOop.Shared;

namespace AspectOrientedOop.Version1
{
   public class CustomerRepository : ICustomerRepository
   {
      private readonly IDictionary<int, Customer> _cache = new ConcurrentDictionary<int, Customer>();
      private readonly IDbAccessor _accessor;
      private readonly ILogger _logger;
      private readonly ISecurity _security;

      public CustomerRepository(
         IDbAccessor accessor, 
         INotifier notifier, 
         ILogger logger, 
         ISecurity security
         )
      {
         _accessor = accessor;
         _logger = logger;
         _security = security;

         notifier.CustomerChanged += NotifierCustomerChanged;
      }

      public Customer Load(int customerId)
      {
         _logger.Log("trace/enter: CustomerRepository.Load(customerId={0})", customerId);

         if (!_security.CanLoadCustomer(customerId))
         {
            throw new Exception("Access denied");
         }

         Customer result;

         if (!_cache.TryGetValue(customerId, out result))
         {
            _cache[customerId] = result = _accessor.ReadEntity<Customer>(
               "SELECT * FROM Customer WHERE Id = @0"
               , customerId
               );
         }

         _logger.Log("trace/exit: CustomerRepository.Load(customerId={0})", customerId);

         return result;
      }

      private void NotifierCustomerChanged(object sender, CustomerChangedNotification e)
      {
         _cache.Remove(e.CustomerId);
      }
   }
}
