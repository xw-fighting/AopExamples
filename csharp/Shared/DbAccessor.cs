namespace AspectOrientedOop.Shared
{
   public interface IDbAccessor
   {
      T ReadEntity<T>(string query, params object[] parameters)
         where T : new();

      T WriteEntity<T>(T entity);
   }

   public class DbAccessor : IDbAccessor
   {
      public T ReadEntity<T>(string query, params object[] parameters)
         where T : new()
      {
            return new T();
      }

      public T WriteEntity<T>(T entity)
      {
         return entity;
      }
   }
}
