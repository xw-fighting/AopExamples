using AspectOrientedOop.Shared;

namespace AspectOrientedOop.Version2
{
   public class CustomerTrace : ICustomerRepository
   {
      private readonly ICustomerRepository _fallback;
      private readonly ILogger _logger;

      public CustomerTrace(ICustomerRepository fallback, ILogger logger)
      {
         _fallback = fallback;
         _logger = logger;
      }

      public Customer Load(int customerId)
      {
         _logger.Log("trace/enter: CustomerRepository.Load(customerId={0})", customerId);

         var result =_fallback.Load(customerId);

         _logger.Log("trace/exit: CustomerRepository.Load(customerId={0})", customerId);

         return result;
      }
   }
}
