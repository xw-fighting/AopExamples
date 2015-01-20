using System.Collections.Concurrent;
using System.Collections.Generic;
using AspectOrientedOop.Shared;

namespace AspectOrientedOop.Version2
{
   public class CustomerCache : ICustomerRepository
   {
      private readonly IDictionary<int, Customer> _cache = new ConcurrentDictionary<int, Customer>();
      private readonly ICustomerRepository _fallback;
      
      public CustomerCache(ICustomerRepository fallback, INotifier notifier)
      {
         _fallback = fallback;
         
         notifier.CustomerChanged += NotifierCustomerChanged;
      }

      public Customer Load(int customerId)
      {
         Customer result;

         if (!_cache.TryGetValue(customerId, out result))
         {
            _cache[customerId] = _fallback.Load(customerId);
         }

         return result;
      }

      private void NotifierCustomerChanged(object sender, CustomerChangedNotification e)
      {
         _cache.Remove(e.CustomerId);
      }
   }
}
