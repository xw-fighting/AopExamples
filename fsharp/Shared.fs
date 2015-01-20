[<AutoOpen>]
module Shared

type CustomerId = 
   | CustomerId of int
 
type Customer() = 
   member this.Id : CustomerId = 
      CustomerId(1)
   

module Accessor =

   let ReadEntity<'a when 'a : (new : unit -> 'a)> (query : string) (id : CustomerId)  =      
      new 'a()

module Security =

   let CanLoadCustomer (id : CustomerId) = 
      true