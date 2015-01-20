using System;

namespace AspectOrientedOop.Shared
{
   public interface INotifier
   {
      event EventHandler<CustomerChangedNotification> CustomerChanged;
   }
}