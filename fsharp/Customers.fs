module Customers

open Shared

type CustomerLoader = int -> Customer
   
let LoadDatabaseCustomer (customerId : int) =
   printfn "Database;"
   let customer = Accessor.ReadEntity<Customer> "SELECT * FROM Customer WHERE Id = @0" customerId

   customer

let LoadCachedCustomer (fallback : CustomerLoader) (customerId : int) =
   printf "Cache;"
   fallback customerId

let TraceLoadCustomer (fallback : CustomerLoader) (customerId : int) =
   printf "Trace/in;"
   let result = fallback customerId   
   printf "Trace/out;"
   result

let SecureLoadCustomer (fallback : CustomerLoader) (customerId : int) =
   printf "Secure;"
   fallback customerId   

module CustomerRepository =

   let LoadCustomer (customerId : int) =
      let customer =       
         (TraceLoadCustomer
         << SecureLoadCustomer 
         << LoadCachedCustomer)
            LoadDatabaseCustomer customerId
         
      customer 