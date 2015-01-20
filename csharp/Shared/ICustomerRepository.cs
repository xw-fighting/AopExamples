namespace AspectOrientedOop.Shared
{
   public interface ICustomerRepository
   {
      Customer Load(int customerId);
   }
}