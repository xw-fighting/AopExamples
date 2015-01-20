using AspectOrientedOop.Shared;

namespace AspectOrientedOop.Version2
{
   public class CustomerDatabase : ICustomerRepository
   {
      private readonly IDbAccessor _accessor;

      public CustomerDatabase(IDbAccessor accessor)
      {
         _accessor = accessor;
      }

      public Customer Load(int customerId)
      {
         var result = _accessor.ReadEntity<Customer>(
            "SELECT * FROM Customer WHERE Id = @0"
            , customerId
            );

         return result;
      }
   }
}
