module Customers

type CustomerCache = Map<CustomerId, Customer>
 
type CallContext = {
   UserId : int
   Cache : CustomerCache 
   }
   
module CustomerRepositoryServices =
 
   type CustomerReader = CallContext * CustomerId -> (CallContext * Customer option)
 
   let DbCall (context : CallContext, id : CustomerId) : (CallContext * Customer option) =
      let customer = Accessor.ReadEntity<Customer> "SELECT * FROM Customer WHERE Id = @0" id

      (context, Some(customer))
 
   let CacheCall (fallback : CustomerReader) (context : CallContext, id : CustomerId) : (CallContext * Customer option) =
      match (context.Cache.TryFind id) with
      | Some found -> (context, Some(found))
      | None ->  
         match (fallback (context, id)) with
         | (context, None) -> (context, None)
         | (context, Some customer) ->
            let updated = {context with Cache = context.Cache.Add (id, customer)}
            (updated, Some(customer))
 
   let SecureCall (fallback : CustomerReader) (context : CallContext, id : CustomerId) : (CallContext * Customer option) =
      if not (Security.CanLoadCustomer id) then
         failwith "Access denied."

      fallback (context, id)
 
   let TraceCall (fallback : CustomerReader) (context : CallContext, id : CustomerId) : (CallContext * Customer option) =
      printf "Trace/in"
      let result = fallback (context, id)
      printf "Trace/out"
      result
 
module CustomerRepository =
 
   open CustomerRepositoryServices
 
   let LoadCustomer (context : CallContext) (id : CustomerId) : (CallContext * Customer option) =
      let result = (TraceCall << SecureCall << CacheCall) DbCall (context, id)

      result
