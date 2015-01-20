namespace AspectOrientedOop.Shared
{
   public interface ILogger
   {
      void Log(string message, params object[] parameters);
   }
}