module Customers

type CustomerCache = Map<CustomerId, Customer>
 
type DynamicCallContext = {
      UserId : int
      }
 
type CachedCallContext = {
   UserId : int
   Cache : CustomerCache 
   }
      
type CallContext =
   | Dynamic of DynamicCallContext
   | Cached of CachedCallContext
 
type CustomerReader = CallContext * CustomerId -> (CallContext * Customer option)
 
 
module CustomerRepositoryServices =
 
   let DbCall (context : CallContext, id : CustomerId) : (CallContext * Customer option) =
      let customer = Accessor.ReadEntity<Customer> "SELECT * FROM Customer WHERE Id = @0" id
 
      (context, Some(customer))
 
 
   let CacheCall (reader : CustomerReader) (context : CallContext, id : CustomerId) : (CallContext * Customer option) =
      match context with
      | Cached c -> 
         let found = c.Cache.TryFind id
 
         if found.IsNone then
            let (result, customer) = reader (context, id)
         
            if (customer.IsSome) then
               let updated = {c with Cache = c.Cache.Add (id, customer.Value)}
 
               (Cached(updated), customer)
            else
               (context, found)
         else      
            (context, found)
      | Dynamic d ->
         let result = reader (context, id)
 
         result
 
 
   let SecureCall (reader : CustomerReader) (context : CallContext, id : CustomerId) : (CallContext * Customer option) =
      let result = reader (context, id)
 
      result
 
 
   let TraceCall (reader : CustomerReader) (context : CallContext, id : CustomerId) : (CallContext * Customer option) =
      printf "Trace/in"
   
      let result = reader (context, id)
 
      printf "Trace/out"
 
      result
 
module CustomerRepository =
 
   open CustomerRepositoryServices
 
   let LoadCustomer (context : CallContext) (id : CustomerId) : (CallContext * Customer option) =
      let result = (TraceCall << SecureCall << CacheCall) DbCall (context, id)
 
      result
