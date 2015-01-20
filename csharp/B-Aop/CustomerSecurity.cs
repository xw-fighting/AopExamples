using System;
using AspectOrientedOop.Shared;

namespace AspectOrientedOop.Version2
{
   public class CustomerSecurity : ICustomerRepository
   {
      private readonly ICustomerRepository _fallback;
      private readonly ISecurity _security;

      public CustomerSecurity(ICustomerRepository fallback, ISecurity security)
      {
         _fallback = fallback;
         _security = security;
      }

      public Customer Load(int customerId)
      {
         if (!_security.CanLoadCustomer(customerId))
         {
            throw new Exception("Access denied");
         }

         var result = _fallback.Load(customerId);

         return result;
      }
   }
}
